using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steeroid.Business.Machine;
using Steeroid.Infra2._0.DAL;
using Steeroid.Models;
using Steeroid.Models.Interfaces;

namespace Steeroid.Infra2._0.Areas.Machine
{
   
    public class MyMachineManager : IMachineManager
    {
        IRepository _repo;

        public MyMachineManager()
        {
            // _repo = new Ef6Repository();//GlobalService.GetRepository();
        }

        MyDbContext _myDb;

        public MyDbContext MyDb
        {
            get
            {
                if (_myDb == null)
                    _myDb = new MyDbContext();
                return _myDb;
            }
        }

        //public IRepository DBAccess =>throw new NotImplementedException();
        public IRepository DBAccess
        {
            get
            {
                if (_repo == null)
                    _repo = new EfRepository(MyDb);
                return _repo;
            }

        }


        public MachineServer GetMachineRecord()
        {
            //throw new NotImplementedException();
            return MyDb.MachineServers.FirstOrDefault();
        }

        public WFProfile GetEventToUpdateFromFront(int localId)
        {
            // throw new NotImplementedException();
            var @event = MyDb.WFProfiles.Where(e => e.Id == localId).FirstOrDefault();

            return @event = @event ?? new WFProfile();


        }

        public List<WFProfileParameter> GetEventParamsToUpdateFromFront(int localId)
        {
            //throw new NotImplementedException();
            var @event = GetEventToUpdateFromFront(localId);
            var @params = MyDb.WFProfileParameters.Where(p => p.WFProfileId.HasValue && p.WFProfileId.Value == @event.Id).ToList();
            return @params = @params ?? new List<WFProfileParameter>();

        }

        public Task<List<MachineServer>> GetMachines()
        {
            //throw new NotImplementedException();
            return Task.FromResult(MyDb.MachineServers.ToList());

        }
    }

}
