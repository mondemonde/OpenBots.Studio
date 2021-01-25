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

namespace Steeroid.Business.Recorodings.Query
{
  public  class SearchRecordCmd :IRequest<ServiceResponse<WFProfile>>
    {
        public string EventName { get; set; }
    }

    public class SearchRecordCmdHandler : IRequestHandler<SearchRecordCmd, ServiceResponse<WFProfile>>
    {

        IRepository _dbAccess;
        ISynchManager _synch;
        IRecordManager _mngr;

        public SearchRecordCmdHandler()
        {
            IRecordManager mngr = BizService.Resolve<IRecordManager>();

            _mngr = mngr;
            _dbAccess = _mngr.DBAccess;
            _synch = _mngr.SynchManager;

        }
   
        public async Task<ServiceResponse<WFProfile>> Handle(SearchRecordCmd request, CancellationToken cancellationToken)
        {
            var model = new WFProfile();
            var result = new ServiceResponse<WFProfile>();

            EnumSynch mode = EnumSynch.NONE;

            //?UPSERT-------------------... manager runs here    
            #region #1 
            try
            {
                var tagName = request.EventName;

                var thisDb = _mngr.DBAccess;
                var list = await thisDb.ListAsync<WFProfile>();

                model = list.FirstOrDefault(p => p.Tag.ToLower() == tagName);
                result.AddModel(model);

            }
            catch (Exception ex)
            {
                result.AddValidationMessage(ex.Message);
                
            }

            #endregion


            return result; //Task.FromResult(result);
       
            
        }
    }

}
