using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Steeroid.Models.Helpers;

namespace Steeroid.Infra2._0.Areas.Bot
{
    public class OBRunner
    {

        //private void ExecuteJob(string mainScriptFilePath)
        //{
        //    // Log Event
        //    Agent.LogInfo("Job execution started");
        //    // Log Event
        //    Agent.LogInfo("Attempt to execute process");

        //    //AgentViewModel agent = AgentsAPIManager.GetAgent(AuthAPIManager.Instance, job.AgentId.ToString());
        //    //Credential credential = CredentialsAPIManager.GetCredentials(AuthAPIManager.Instance, agent.CredentialId.ToString());

        //    // Run Automation
        //    RunAutomation(mainScriptFilePath);

        //    // Log Event
        //    Agent.LogInfo("Job execution completed");

        //    // Log Event
        //    Agent.LogInfo("Attempt to update Job Status (Post-execution)");
       

        //    // Delete Automation Files Directory
        //    //Directory.Delete(Path.GetDirectoryName(mainScriptFilePath), true);


        //   // _isSuccessfulExecution = true;
        //}



       public async void RunAutomation(string mainScriptFilePath)
        {
            try
            {
                var executionParams = mainScriptFilePath; //GetExecutionParams(job, automation, mainScriptFilePath, projectDependencies);

                ConfigManager config = new ConfigManager();

                var executorPath = config.GetValue("openbots_exe"); //Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "OpenBots.Executor.exe").FirstOrDefault();
                var cmdLine = $"\"{executorPath}\" \"{executionParams}\"";

                // launch the Executor              
                ProcessLauncher.PROCESS_INFORMATION procInfo;
                ProcessLauncher.LaunchProcess(cmdLine,out procInfo);

                Agent.LogInfo("OpenBot Ended: "+  procInfo.ToString());
            }
            catch (Exception ex)
            {
                Agent.LogError(ex);
            }
        }

     

    }
}
