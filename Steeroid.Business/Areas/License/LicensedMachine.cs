//using SteeroidUpdate.Main.Core.Entities;
using Steeroid.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Steeroid.Business.License
{
    [NotMapped]
    public class LicensedMachine:MachineServer
    {
        [Required]
        public new string ServiceBusConnectionString { get; set; }


        [NotMapped]
        public string JsonResult { get; set; }

        [NotMapped]
        public string LicenseRemarks
        {
            get
            {
                if (Id > 0 && GlobalMacId > 0 && !string.IsNullOrEmpty(ServiceBusConnectionString))
                {
                    return "License Installed.";
                }
                else
                    return "No License found.";
            }
        }
    }
}