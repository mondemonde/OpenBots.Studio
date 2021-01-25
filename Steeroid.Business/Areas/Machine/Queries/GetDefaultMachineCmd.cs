using MediatR;
using Steeroid.Business.Machine;
using Steeroid.Models;
using Steeroid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Steeroid.Models.BaseInterfaces;

namespace Steeroid.Business.Areas.Machine.Queries
{
   public class GetDefaultMachineCmd : IRequest<ServiceResponse<MachineServer>>
    {
    }

    public class GetDefaultMachineCmdHandler : IRequestHandler<GetDefaultMachineCmd
        , ServiceResponse<MachineServer>>
    {

        IRepository _dbAccess;
        public GetDefaultMachineCmdHandler(IRepository dbaccess)
        {
            _dbAccess = dbaccess;
        }


        public async Task<ServiceResponse<MachineServer>> Handle(GetDefaultMachineCmd request, CancellationToken cancellationToken)
        {

            var result = new ServiceResponse<MachineServer>();
            var model = new MachineServer();

            try
            {
                    var macs = await  _dbAccess.ListAsync<MachineServer>();       
                    model = macs.FirstOrDefault();

            }
            catch (Exception err)
            {

                result.AddValidationMessage(err.Message);
            }

            result.AddModel(model);
            return result;
        }
    }



}
