using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Steeroid.Business.Areas.Machine.Queries;
using Steeroid.Business.License;
using Steeroid.Business.License.Commands;
using Steeroid.Models;
using Steeroid.Models.Helpers;

namespace Steeroid.Services.License
{
    public class MachineService
    {


        private readonly IMediator _mediator;
        // private readonly IRepository _repository;


        public MachineService(IMediator mediator)
        {
            _mediator = mediator;
            //_repository = repository;
        }

        public async Task<LicensedMachine> GetMachine()
        {
            GetDefaultMachineCmd cmd = new GetDefaultMachineCmd();
            var response = await _mediator.Send(cmd);
            var mac = response.Model;


            var config = new MapperConfiguration(cfg => cfg.CreateMap<MachineServer, LicensedMachine>());
            var mapper = config.CreateMapper();
            // Perform mapping
            LicensedMachine dto = mapper.Map<MachineServer, LicensedMachine>(mac);

            if (dto == null)
                dto = new LicensedMachine();

            return dto;

        }



        public async Task<LicensedMachine> Register(string enteredToken)
        {
            /// return RegisterToken(enteredToken);
            RegisterCmd cmd = new RegisterCmd
            {
                EnteredToken = enteredToken
            };

            var response = await _mediator.Send(cmd);
            return response.Model;


        }




        public void Reset()
        {
            //next: move this to biz

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
