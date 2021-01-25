using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steeroid.Infra2._0.DAL;
using Steeroid.Models;

namespace Steeroid.Infra2._0.Areas.Communication
{
    public class MessageManager
    {
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

        public MessageManager()
        {

        }

        //public List<IGrouping<string, WFProfile>> GetDomainList()
        //{
        //    var list = MyDb.WFProfiles.OrderBy(p=>p.Domain).ToLookup(p=>p.Domain).ToList();
        //    return list;

        //}


        public List<BusMessage> GetMessageList()
        {
            var list = MyDb.BusMessages.OrderByDescending(p => p.Id).Take(10).ToList();
            return list;

        }

        public void ClearMessages()
        {
            //MyDb.BusMessages
            foreach (var row in MyDb.BusMessages)
            {
                MyDb.BusMessages.Remove(row);
            }
            MyDb.SaveChanges();
        }



    }

}
