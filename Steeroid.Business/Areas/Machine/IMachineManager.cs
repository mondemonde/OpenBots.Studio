//using SteeroidUpdate.Main.Core.Entities;
//using SteeroidUpdate.Main.SharedKernel.Interfaces;
using Steeroid.Models;
using Steeroid.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steeroid.Business.Machine
{
    //public interface IMachineDbAccess
    //{
    //    Task<IQueryable<MachineServer>> GetMachines();
    //}

    public interface IMachineManager
    {
        IRepository DBAccess { get;  }
        MachineServer GetMachineRecord();

        WFProfile GetEventToUpdateFromFront(int localId);
        List<WFProfileParameter> GetEventParamsToUpdateFromFront(int localId);

        Task<List<MachineServer>> GetMachines();


        //WFProfile GetEventToUpdateFromHub(string globalEventFrontId);
        //List<WFProfileParameter> GetEventParamsToUpdateFromHub(string globalEventFrontId);

    }


}
