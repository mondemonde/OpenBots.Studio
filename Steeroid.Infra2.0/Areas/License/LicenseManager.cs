using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
///using Flurl.Http;//
//using Flurl;
using AutoMapper;
using System.IO;
//using DevNote.Interface.Models;
using Newtonsoft.Json;
using Steeroid.Models.Interfaces;
using Steeroid.Models;
using Steeroid.Business.License;
using Steeroid.Models.Helpers;
using Steeroid.Infra2._0.DAL;
using Autofac.Core;
using DevNoteHub;

namespace Steeroid.Infra2._0.Areas.License
{
    public class LicenseManager :ILicenseManager
    {
        private readonly IRepository _repository;

        public LicenseManager(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<LicensedMachine> GetMachine()
        {
            var macs = await _repository.ListAsync<MachineServer>();
            var mac = macs.FirstOrDefault();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<MachineServer, LicensedMachine>());
            var mapper = config.CreateMapper();
            // Perform mapping
            LicensedMachine dto = mapper.Map<MachineServer, LicensedMachine>(mac);

            if (dto == null)
                dto = new LicensedMachine();

            return dto;

        }

        public async Task<LicensedMachine> Register(string token)
        {
            try
            {

                var frontMachine = new MachineServer();
                string jsonResultToken = string.Empty;
                // hdjsonResultToken.Value = string.Empty;

                if (!string.IsNullOrEmpty(token))
                {
                  JwtAuthentication jwtAuth = new JwtAuthentication();
                  TableConfig setting = new TableConfig()
                   {
                        Enable_JWTExpiration = false,
                        IsJWTPrivateKeyValidate = false,
                        Id = 1,
                        MaxTry = 3
                    };
                    var mac = jwtAuth.AuthMacToken(token, setting);
                    if (mac != null)
                    {
                        jsonResultToken = mac.ToString();
                        frontMachine = JsonConvert.DeserializeObject<MachineServer>(jsonResultToken);
                    }

                }

                if (frontMachine.Id > 0)
                {
                    //clear record first..
                    await _repository.ClearAsync<MachineServer>();

                    frontMachine.Id = 0;                   
                    await _repository.AddAsync<MachineServer>(frontMachine);

                    //TIP# Configure AutoMapper
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<MachineServer, LicensedMachine>());
                    var mapper = config.CreateMapper();
                    // Perform mapping
                    LicensedMachine dto = mapper.Map<MachineServer, LicensedMachine>(frontMachine);
                    dto.JsonResult = jsonResultToken;
                    //  return Request.CreateResponse<RegVM>(HttpStatusCode.OK, dto);
                    return dto;

                }
                else //failed
                {
                    //Agent.LogInfo("Failed to Register License.");
                    throw new ApplicationException("Failed to Register License.");

                    //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid License.");
                    //return new LicensedMachine();

                }



            }
            catch (Exception ex)
            {
                throw (ex);
                //Agent.LogError(ex);
                //return new LicensedMachine();

            }


        }


        /// <summary>
        /// todo: should move to Global helper
        /// </summary>

        public void Reset()
        {
            Console.WriteLine("Install default Db.");

            var dir = @"C:\BlastAsia\Steeroid\Common";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var folder = Path.Combine(Agent.GetCurrentDir(), "Data");

            var source = Path.Combine(folder, "MyDBContext.sqlite");
            File.Copy(source, Path.Combine(dir, "MyDBContext.sqlite"), true);

        }

       
    }
}
