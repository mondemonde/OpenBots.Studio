using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Steeroid.Models;
using Steeroid.Models.BaseInterfaces;

namespace Steeroid.Business.Areas.Communication
{
   public interface IBizQueueClient
    {
        IBizQueueClient CreateMe(MachineServer thisMac);       
        Task<ServiceResponse<bool>> SendMessage(string body);
       
    }
}
