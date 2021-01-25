using Steeroid.Business.Areas.Recordings;
using Steeroid.Business.Areas.Remoting;
using Steeroid.Models.Interfaces;
using Steeroid.Models.Interfaces.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Steeroid.Infra2._0.Areas.Recordings
{
    public class RecordManager : IRecordManager
    {
        IRepository _dbAccess;
        ISynchManager _synchManager;
        IScriptManager _scriptManager;

        public RecordManager(IRepository repo,ISynchManager synch
            ,IScriptManager scriptManager)
        {
            _synchManager = synch;
            _dbAccess =repo;
            _scriptManager = scriptManager;
        }

        public IRepository DBAccess => _dbAccess;

        public ISynchManager SynchManager => _synchManager;

        public IScriptManager ScriptManager => _scriptManager;
    }
}
