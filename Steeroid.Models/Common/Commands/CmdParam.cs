//using LogApplication.INFRA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Steeroid.Models.Common.Commands
{
    public class CmdParam
    {


      





        private string bloodLine;
        private object payload;

        public long Id { get; set; }

        public CmdParam(object obj)
        {
            DateCreated = DateTime.Now;
            CommandName = EnumCommands.Carrier.ToString(); //"Ping";
            RequestDate = DateCreated;
            Id = DateTime.Now.Ticks;
            //Mapper.CreateMap<CmdParam, CmdParam>();
            //this = Mapper.Map<CmdParam>(param);
            //DateCreated = param.DateCreated;
            if (obj is CmdParam)
            {
               var param = obj as CmdParam;
                CommandName = param.CommandName;
                this.DateCreated = param.DateCreated;
                this.CmdIndex = param.CmdIndex;
                //this.IsRespond = param.IsRespond;
                //this.IsSuccess = param.IsSuccess;


                RequestDate = DateTime.Now;
                this.CycleCount = param.CycleCount;
                this.MessageFrom = param.MessageFrom;
                this.MessageTo = param.MessageTo;
                param.AppendBloodline(this);
            }
            //creates new id       

            Payload = null;

            audit();
        }


        public CmdParam()
        {
            DateCreated = DateTime.Now;
            CommandName = EnumCommands.Chat.ToString(); //"Ping";
            RequestDate = DateCreated;
            Id = DateTime.Now.Ticks;
            audit();

        }

        public CmdParam(EnumCommands cmdIndex)
        {
            DateCreated = DateTime.Now;
            CommandName = cmdIndex.ToString(); //"Ping";
            RequestDate = DateCreated;
            Id = DateTime.Now.Ticks;
            audit();

        }



        static int instanceCnt { get; set; }
        DateTime runningTime { get; set; }

        //Step: License
        void audit()
        {
            //see debug original 
            //D:\_MY_PROJECTS\_DEVNOTE\_DevNote4\LogAppModel\LogAppModel.csproj
        }
        public CmdParam(string msg,bool isCarrierOnly = false)
        {
            DateCreated = DateTime.Now;
            CommandName = msg;//EnumCommands.Chat.ToString(); //"Ping";
            RequestDate = DateCreated;
            Id = DateTime.Now.Ticks;
            RawMessage = msg;
            _isCarrier = isCarrierOnly;
            audit();

        }
        [Obsolete("use static CreateCmd")]
        public  void WrapPayload<T> (T ext) where T : CmdParam 
        {
            //CmdParam cmd = ext as CmdParam;
            //CmdParam newCmd = new CmdParam(cmd);
            //cmd.Payload = null;
            //newCmd.Payload = ext;
            //cmd.CommandName = cmdName;
            this.CommandName = ext.CommandName;
            ext.AppendBloodline(this);
            this.payload = ext;           

        }


        bool _isCarrier { get; set; }
        public bool IsCarrierOnly
        {
            get { return _isCarrier; }        }


        public static CmdParam CreateCmdCarrier<T>(T ext,string withCmdName ="" ) where T : CmdParam
        {
            CmdParam newCmd = new CmdParam(ext.CommandName, true);

            if (!string.IsNullOrEmpty(withCmdName))
            {
                newCmd.CommandName = withCmdName;
            }
         
            //cmd.Payload = null;
            //newCmd.Payload = ext;
            //cmd.CommandName = cmdName;
           
            ext.AppendBloodline(newCmd);
            ext.payload = null;
            newCmd.payload = ext;
            
            return newCmd;

        }



        public int LoadingTime { get; set; }

        public string MessageFrom { get; set; }
        public string MessageTo { get; set; }

        public string CommandName { get; set; }

        public DateTime DateCreated { get; set; }

        public string RawMessage { get; set; }

        public bool IsRespond { get; set; }

        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public DateTime RequestDate { get; set; }

        public int CycleCount { get; set; }

        public string BloodLine { get => checkBloodLine(); }

        /// <summary>
        /// assign the parent of this cmd
        /// </summary>
        /// <param name="parent"> make  param the parent of this cmd</param>
        /// <returns></returns>
        public string AppendBloodline(CmdParam parent)
        {
            this.bloodLine = parent.BloodLine + "``\n" + bloodLine;
            return BloodLine;
        }
        string checkBloodLine()
        {
            if (string.IsNullOrEmpty(bloodLine))
                bloodLine = (this.Id.ToString() + "_" + this.CommandName).Trim();

            return bloodLine;
        }


        public long CmdIndex { get; set; }

        public object Payload { get => payload; set => setPayload(value); }

        void setPayload(object obj)
        {           
 
            if (obj is CmdParam)
            {               
                var cmd = obj as CmdParam;
                cmd.AppendBloodline(this);               
            }
            payload = obj;

        }

        public string BookMarkName { get; set; }

      


    }




}
