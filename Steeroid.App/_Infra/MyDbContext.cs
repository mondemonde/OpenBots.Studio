
using DevNote.Interface.Models;
//using DevNoteHub.Models;
using Microsoft.EntityFrameworkCore;
using Steeroid.Models;
using Steeroid.Models.Models;
//using SteeroidUpdate.Main.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNoteFront.DAL
{
    public class MacDBContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var dir = Agent.GetCurrentDir();
            var sdf = @"C:\BlastAsia\Steeroid\Common\MyDBContext.sqlite";
            optionsBuilder.UseSqlite(@"Data Source=" + sdf);
        }


        public DbSet<WFProfile> WFProfiles { get; set; }

        public DbSet<WFProfileParameter> WFProfileParameters { get; set; }

        //public DbSet<TableConfig> TableConfigs { get; set; }


       
        public DbSet<MachineServer> MachineServers { get; set; }

        public DbSet<BusMessage> BusMessages { get; set; }

        //public DbSet<OutputBusMessage> OutputBusMessages { get; set; }




    }
}
