using System;
using System.Collections.Generic;
using System.Text;

namespace QR.API.Mapper.Std
{
  public  enum LicenseMode
    {
        None,
        MachineRpaServer=11,
        MachineRpaBot,
        Application=101,
        NopCommerce,
        QuickReach,
        TokenOnly=201
    }

  public enum MSG
    {      
        DomainName,   
        GlobalMachineId,
        Mode,
        ReferenceId,
       

        fx_setting_SendUrl,
        fx_setting_Code,

        fx_setting_SendUrl_Steer,
        fx_setting_Code_Steer,
        fx_QR_AppId = 9,

        SvcBusConnString,
        Topic,
    }

    #region ERROR CODES
    public enum ErrorCodes
    { 

        None =0,

        HandleReciever=101,
        ErrorOnKatalon=201,

        TimedOut = 600,
        MaxTryLimit,

        Unhandled = 1000,


    }

    #endregion
}
