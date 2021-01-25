//using DevNoteHub.API.Interface;
//using DevNoteHub.API.Models;
//using IntegrationEvents.Events.DevNote;
//using ServiceBus.Producer.Services;
using Steeroid.Business.Machine;
using Steeroid.Models;
using Steeroid.Models.Enums;
using System.Threading.Tasks;

namespace Steeroid.Business.Areas.Remoting
{
    public interface ISynchManager
    {

        string CreateMessageBody(WFProfile eventRec, EnumSynch mode, ref EventDTO mBody);
        string DecryptSvcBusConnString(IMachineManager mngr);
        string DecryptSvcBusConnString(MachineServer mac);
        string DecryptSvcBusEntityPathTopic(IMachineManager mngr);
        string DecryptSvcBusEntityPathTopic(MachineServer mac);
        MachineServer GetMyGlobalMachineId();
        void PickUpdate();
        Task<bool> SendOutputToHub(DevNoteIntegrationEvent eventRec);
        Task<bool> SendUpdateToHub(WFProfile eventRec, EnumSynch mode);
    }
}