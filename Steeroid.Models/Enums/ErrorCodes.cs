using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Models.Enums
{
    public enum ErrorCodes
    {
        None = 0,
        HandleReciever = 101,
        ErrorOnKatalon = 201,
        ErrorOnExcel = 301,
        FileNotFound = 404,
        TimedOut = 600,
        MaxTryLimit = 601,
        Unhandled = 1000
    }

    public enum SuccessCodes
    {
        None = 0,
        SuccessOnKatalon = 201,
        SuccessOnExcel = 301,
        Success = 1000

    }



}
