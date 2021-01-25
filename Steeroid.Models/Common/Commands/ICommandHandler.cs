using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steeroid.Models.Common.Commands
{
    public interface ICommandHandler<TCommandParam> where TCommandParam:CmdParam
    {
         void Execute(TCommandParam command);

         //bool IsSuccess {get;set;}

         //string MessageResult { get; set; }

         //EnumCommands Id { get; }


         TCommandParam MyParam { get; set; }


    }


}
