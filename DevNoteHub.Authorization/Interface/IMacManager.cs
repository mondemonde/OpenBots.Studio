//using DevNote.Interface.Models;
//using DevNoteHub.Models;
using Steeroid.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevNoteHub.API.Interface
{
  public  interface IMacManager
    {
        MachineServer GetMachineRecord();

        WFProfile GetEventToUpdateFromFront(int localId);
        List<WFProfileParameter> GetEventParamsToUpdateFromFront(int localId);


        //WFProfile GetEventToUpdateFromHub(string globalEventFrontId);
        //List<WFProfileParameter> GetEventParamsToUpdateFromHub(string globalEventFrontId);

    }
}
