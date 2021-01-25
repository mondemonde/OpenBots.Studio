using MediatR;
using Steeroid.Business.Areas.Recordings;
using Steeroid.Business.Areas.Remoting;
using Steeroid.Business.Services;
using Steeroid.Business.Validation;
using Steeroid.Models;
using Steeroid.Models.BaseInterfaces;
using Steeroid.Models.Enums;
using Steeroid.Models.Helpers;
using Steeroid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Steeroid.Business.Recorodings.Commands
{
  public  class SaveRecordCmd :IRequest<ServiceResponse<WFProfile>>
    {
        public WFProfile Event { get; set; }
    }

    public class SaveRecordCmdHandler : IRequestHandler<SaveRecordCmd, ServiceResponse<WFProfile>>
    {

        IRepository _dbAccess;
        ISynchManager _synch;
        IRecordManager _mngr;

        public SaveRecordCmdHandler(IRecordManager mngr)
        {
            _mngr = mngr;
            _dbAccess = _mngr.DBAccess;
            _synch = _mngr.SynchManager;

        }
   
        public async Task<ServiceResponse<WFProfile>> Handle(SaveRecordCmd request, CancellationToken cancellationToken)
        {
            var model = request.Event; //new WFProfile();
            var result = new ServiceResponse<WFProfile>();

            EnumSynch mode = EnumSynch.NONE;

            //?UPSERT-------------------... manager runs here    
            #region #1 
            try
            {
                //model =  _dbAccess.Register(token).Result;

                //validate
                result.AddValidation( EntityValidator<WFProfile>.ValidatePropertyAttribute(model));

             
                if(!result.HasError)
                {
                    if (model.Id > 0)
                    {
                          await _dbAccess.UpdateAsync<WFProfile>(model);
                        mode = EnumSynch.DEVHUB_UPDATE;


                       

                        Dictionary<string, string> resultDic = new Dictionary<string, string>();
                        var thisDb = _mngr.DBAccess; 

                        var tagName = model.Tag.ToLower().Trim();
                        var list = await thisDb.ListAsync<WFProfile>();

                        var duplicate = list.FirstOrDefault(p => p.Tag.ToLower() == tagName);

                        if (duplicate != null &&  duplicate.Id>0)
                        {
                            //MessageBox.Show("Event Tag is already existing.","ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                          //UPdate
                            await thisDb.UpdateAsync<WFProfile>(model);

                        }
                        else
                        {
                            //insert
                            await thisDb.AddAsync<WFProfile>(model);
                        }



                        //? #1.1 update wfprofileparameters

                            var profile = list.Where(p => p.Tag.ToLower() == tagName)
                                .ToList().LastOrDefault();

                            if (profile != null && profile.Id > 0)
                            {


                                //ScriptManager mngr = new ScriptManager();


                                _mngr.ScriptManager.ReadWriteXml(profile.SourcePath, resultDic);
                                resultDic =BizService.TasktVars;

                            //save to db 
                            //STEP_.Player #807 get dbparameters namespace Steeroid.Models

                            var listAll = thisDb.ListAsync<WFProfileParameter>().Result;
                            var listParam =listAll.Where(a => a.WFProfileId == profile.Id).ToList();

                                foreach (var arg in listParam)
                                {
                                    //make lower case for propertyname
                                    //resultDic.Add(arg.MappedTo_Input_X, arg.PropertyName.ToLower());
                                   await thisDb.DeleteAsync<WFProfileParameter>(arg);

                                }

                                //add new params
                                foreach (var item in resultDic)
                                {
                                    //	Id	WFProfileId	PropertyName	MappedTo_Input_X	DefaultValue	Created	Modified	Description	GlobalMachineId	WFProfileParameterId
                                    //  5   1           Username         input_1             rgalvez
                                   await thisDb.AddAsync<WFProfileParameter>(new WFProfileParameter
                                    {
                                        WFProfileId = profile.Id,
                                        PropertyName = item.Key,
                                        MappedTo_Input_X = item.Key,
                                        DefaultValue = item.Value,
                                        Description = "auto-generated"

                                    });
                                }

                            }

                        






                    }
                    //! INSERT
                    else
                    {
                        //insert
                       model =  await _dbAccess.AddAsync<WFProfile>(model);
                       mode = EnumSynch.DEVHUB_INSERT;

                    }
                }




            }
            catch (Exception ex)
            {
                result.AddValidationMessage(ex.Message);
                
            }

            result.AddModel(model);
            #endregion

            //?  SYCH with DEVNOTE HUB---------------------------
            #region #2
            if (!result.HasError)
            {
                // publish to azure...
                //STEP_.HUB #1
                // ISynchManager synch = new SynchManager(new DbManager());
                //validate for Hub update

                var current = model;
                try
                {
                    current.GlobalMachineId = _synch.GetMyGlobalMachineId().GlobalMacId;
                    current.WFProfileId = current.Id;

                    var updateResult = await _synch.SendUpdateToHub(current, mode);
                }
                catch (Exception err)
                {
                    Agent.LogWarn(err.Message);
                  
                }

            
            }
            #endregion

            return result; //Task.FromResult(result);
       
            
        }
    }

}
