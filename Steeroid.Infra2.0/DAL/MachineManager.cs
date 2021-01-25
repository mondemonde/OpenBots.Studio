//using BaiCrawler.DAL;
using Steeroid.Infra2._0.DAL;
using Steeroid.Models;
using Steeroid.Models.Models;
//using DevNoteHub.Models;
//using SteeroidUpdate.Main.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.UI;

namespace Steeroid.Infra2._0.DAL
{
    public class MachineManager
    {
      //static MyDbContext _myDb;
       MyDbContext _myDb;


      public MyDbContext MyDb {
            get {
                //if (_myDb == null)
                //    _myDb = new AppDbContext();
                return _myDb;
            } }
 
       public MachineManager(MyDbContext context )
        {
            _myDb = context;
        }

        public List<IGrouping<string, MachineEvent>> GetDomainList()
        {
            var mac = GetFrontMachine();
            int eMachineId = 0;
            if (mac != null)
                eMachineId = mac.Id;
            var view =
                    from s in MyDb.MachineServers
                    join e in MyDb.WFProfiles on s.Id equals eMachineId
                    //where e.Domain == getEntity
                    select new MachineEvent{
                      MachineId = s.Id,
                      ClientId = s.ClientId,
                      Department = e.Department,
                      Descriptions = e.Descriptions,
                      Domain = e.Domain,
                      ExpirationDate = s.ExpirationDate,
                      IsDisabled = s.IsDisabled,
                      Tag = e.Tag,
                      TopicId = s.TopicId
                    };
            var list = view.OrderBy(p => p.Domain).ToLookup(p => p.Domain).ToList();
            return list;

        }

        public List<MachineEvent> GetMachineList(string domain)
        {
            var mac = GetFrontMachine();
            int eMachineId = 0;
            if (mac != null)
                eMachineId = mac.Id;


            if(string.IsNullOrEmpty(domain))
            {

                var view =
                 from s in MyDb.MachineServers
                 join e in MyDb.WFProfiles on s.Id equals eMachineId
                 //where e.Domain == getEntity
                 select new MachineEvent
                 {
                     MachineId = s.Id,
                     ClientId = s.ClientId,
                     Department = e.Department,
                     Descriptions = e.Descriptions,
                     Domain = e.Domain,
                     ExpirationDate = s.ExpirationDate,
                     IsDisabled = s.IsDisabled,
                     Tag = e.Tag,
                     TopicId = s.TopicId
                 };
                var list = view.OrderBy(p => p.Tag).ToList();

                var newList = new List<MachineEvent>();

                var objects = list.GroupBy(o => o.MachineId).Select(g => g.First());

                return objects.ToList();

            }
            else
            {
                var view =
                 from s in MyDb.MachineServers
                 join e in MyDb.WFProfiles on s.Id equals eMachineId
                    //where e.Domain == getEntity
                    select new MachineEvent
                 {
                     MachineId = s.Id,
                     ClientId = s.ClientId,
                     Department = e.Department,
                     Descriptions = e.Descriptions,
                     Domain = e.Domain,
                     ExpirationDate = s.ExpirationDate,
                     IsDisabled = s.IsDisabled,
                     Tag = e.Tag,
                     TopicId = s.TopicId
                 };
                var list = view.Where(e => e.Domain == domain).OrderBy(p => p.Tag).ToList();

                var newList = new List<MachineEvent>();

                var objects = list.GroupBy(o => o.MachineId).Select(g => g.First());

                return objects.ToList();
            }

         
           

        }

        public List<MachineEvent> GetList(string domain)
        {

            var front = GetFrontMachine();
            int eMachineId = 0;
            if (front != null)
                eMachineId = front.Id;

            if (string.IsNullOrEmpty(domain))
            {

              

                var view = from mac in MyDb.MachineServers
                           join evnt in MyDb.WFProfiles
                             on mac.Id equals eMachineId
                             into MatchedEvents
                           from me in MatchedEvents.DefaultIfEmpty()
                           select new MachineEvent
                           {
                               MachineId = mac.Id,
                               ClientId = mac.ClientId,
                               Department = me.Department,
                               Descriptions = me.Descriptions,
                               Domain = me.Domain,
                               ExpirationDate = mac.ExpirationDate,
                               IsDisabled = mac.IsDisabled,
                               Tag = me.Tag,
                               TopicId = mac.TopicId
                           };


                var list = view.OrderBy(p => p.Tag).ToList();

                //var newList = new List<MachineEvent>();

                var objects = list.GroupBy(o => o.MachineId).Select(g => g.First());

                return objects.ToList();

            }
            else
            {
               
                var view = from mac in MyDb.MachineServers
                           join evnt in MyDb.WFProfiles
                             on mac.Id equals eMachineId
                             into MatchedEvents
                           from me in MatchedEvents.DefaultIfEmpty()
                           select new MachineEvent
                           {
                               MachineId = mac.Id,
                               ClientId = mac.ClientId,
                               Department = me.Department,
                               Descriptions = me.Descriptions,
                               Domain = me.Domain,
                               ExpirationDate = mac.ExpirationDate,
                               IsDisabled = mac.IsDisabled,
                               Tag = me.Tag,
                               TopicId = mac.TopicId
                           };

                var list = view.Where(e => e.Domain.Contains(domain)).OrderBy(p => p.Tag).ToList();

               // var newList = new List<MachineEvent>();

                var objects = list.GroupBy(o => o.MachineId).Select(g => g.First());

                return objects.ToList();
            }

          

        }



        public MachineServer GetById(int id)
        {
            var first = MyDb.MachineServers.Where(x => x.Id == id).FirstOrDefault();
            return first;

        }

        public MachineServer GetFrontMachine()
        {
            var list = MyDb.MachineServers.ToList();
            var first = list.FirstOrDefault();

            return first;

        }


        public MachineServer GetByDomain(string domain)
        {

            var front = GetFrontMachine();
            int eMachineId = 0;
            if (front != null)
                eMachineId = front.Id;

            if (string.IsNullOrEmpty(domain))
            {

                return new MachineServer();

            }
            else
            {
                
                var view = from mac in MyDb.MachineServers
                           join evnt in MyDb.WFProfiles
                             on mac.Id equals eMachineId
                             into MatchedEvents
                           from me in MatchedEvents.DefaultIfEmpty()
                           select new MachineEvent
                           {
                               MachineId = mac.Id,
                               ClientId = mac.ClientId,
                               Department = me.Department,
                               Descriptions = me.Descriptions,
                               Domain = me.Domain,
                               ExpirationDate = mac.ExpirationDate,
                               IsDisabled = mac.IsDisabled,
                               Tag = me.Tag,
                               TopicId = mac.TopicId
                           };

                var list = view.Where(e => e.Domain == domain).ToList();

                // var newList = new List<MachineEvent>();
                if (list.Count > 0)
                {
                    var objects = list.GroupBy(o => o.MachineId).Select(g => g.First());

                    return GetById(objects.FirstOrDefault().MachineId);
                }
                else
                    return new MachineServer();
            }


        }

        //public MachineServer Insert(FrontMachine app)
        public MachineServer Insert(MachineServer app)

        {
            try
            {

                if (app.ExpirationDate.HasValue && app.ExpirationDate.Value == DateTime.MinValue)
                    app.ExpirationDate = null;

                app.Created = DateTime.Now.ToUniversalTime();

                //if (string.IsNullOrEmpty(app.DomainName))
                //    throw new ApplicationException("Domain is required.");

                MyDb.MachineServers.Add(app);
                MyDb.SaveChanges();


                return app;
            }

            catch (Exception err)
            {
             
                return app;

            }


        }

        public Models.MachineServer Update(Models.MachineServer app)
        {
            try
            {

                if (app.ExpirationDate.HasValue && app.ExpirationDate.Value == DateTime.MinValue)
                    app.ExpirationDate = null;

                app.Modified = DateTime.Now.ToUniversalTime();

                if (string.IsNullOrEmpty(app.TopicId))
                    throw new ApplicationException("TopicId is required.");

                if (string.IsNullOrEmpty(app.ClientId))
                    throw new ApplicationException("ClientId is required.");



                // var result = MyDb.ApplicationKeys.SingleOrDefault(b => b.Id == app.Id);
                var result = MyDb.MachineServers.Find(app.Id); //.SingleOrDefault(b => b.Id == app.Id);

                if (result != null)
                {
                    //result = Mapper.Map<ApplicationKey>(app);
                    result.TopicId = app.TopicId;
                    result.IsDisabled = app.IsDisabled;
                    result.ClientId = app.ClientId;
                    result.Modified = app.Modified;

                    MyDb.SaveChanges();

                }


                return app;
            }

            catch (Exception err)
            {
               
                return app;

            }


        }


        public  void Clear()
        {
            //MyDb.BusMessages
            foreach (var row in MyDb.MachineServers)
            {
                MyDb.MachineServers.Remove(row);
            }
            MyDb.SaveChanges();
        }

    }
}