using MediatR;
using Steeroid.Business.Validation;
using Steeroid.Models.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Steeroid.Business.License.Commands
{
  public  class RegisterCmd :IRequest<ServiceResponse<LicensedMachine>>
    {
        public string EnteredToken { get; set; }
    }

    public class RegisterCmdHandler : IRequestHandler<RegisterCmd, ServiceResponse<LicensedMachine>>
    {

        ILicenseManager _dbAccess;
        public RegisterCmdHandler(ILicenseManager dbaccess)
        {
            _dbAccess = dbaccess;
        }
   
        public Task<ServiceResponse<LicensedMachine>> Handle(RegisterCmd request, CancellationToken cancellationToken)
        {
            var model = new LicensedMachine();
            var result = new ServiceResponse<LicensedMachine>();
            

            //manager runs here    
            var token =request.EnteredToken;
            try
            {
                model =  _dbAccess.Register(token).Result;

                //validate
                result.AddValidation( EntityValidator<LicensedMachine>.ValidatePropertyAttribute(model));

            }
            catch (Exception ex)
            {
                result.AddValidationMessage(ex.Message);
                
            }

            result.AddModel(model);

            return Task.FromResult(result);
       
            
        }
    }

}
