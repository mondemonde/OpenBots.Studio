using Microsoft.EntityFrameworkCore;
using Steeroid.Models;
using Steeroid.Models.Models;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steeroid.Infra2._0.DAL
{
    public class MyDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var dir = Agent.GetCurrentDir();
            var sqlite = @"C:\BlastAsia\Steeroid\Common\MyDBContext.sqlite";
            optionsBuilder.UseSqlite(@"Data Source=" + sqlite);
        }


        public DbSet<WFProfile> WFProfiles { get; set; }

        public DbSet<WFProfileParameter> WFProfileParameters { get; set; }

        public DbSet<TableConfig> TableConfigs { get; set; }



        public DbSet<MachineServer> MachineServers { get; set; }

        public DbSet<BusMessage> BusMessages { get; set; }

        //public DbSet<OutputBusMessage> OutputBusMessages { get; set; }




    }

}