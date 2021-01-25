using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using OpenBots.Commands.Variable;
using OpenBots.Core.Script;
//using OpenBots.Core.Script;
using Steeroid.Business.Services;
using Steeroid.Models.Interfaces.Plugins;
using Steeroid.Models.Interfaces.Scripting;
using Steeroid.Models.Scripting;

namespace Steeroid.Infra2._0.Areas.Scripting
{
    public class ScriptManager : IScriptManager
    {
        public ScriptManager()
        {
            TasktVars = new Dictionary<string, string>();
            MyCustomVars = new Dictionary<string, string>();
        }

        public static Dictionary<string, string> TasktVars
        {
            get
            {
                return BizService.TasktVars;
            }
            set
            {
               BizService.TasktVars = value;
            }
        }


        public Dictionary<string, string> ExtVars { get; set; }

        public string SourceXmlFile { get; set; }
        public static Dictionary<string, string> MyCustomVars { get; set; }
        List<ScriptAction> myAddVars { get; set; }
        public List<ScriptVariable> ScriptVariables { get; set; }

        Script scriptItems;

        public Script ScriptItems
        {

            get
            {
                return scriptItems;
            }
        }

        public Dictionary<string, string> ReadWriteXml(string xml, Dictionary<string, string> extParam)
        {

            SourceXmlFile = xml;
            ExtVars = extParam;

            //get deserialized script
            scriptItems = Script.DeserializeFile(SourceXmlFile);




            //#1 script variables
            ScriptVariables = scriptItems.Variables ?? new List<ScriptVariable>();



            //#2 addcommandVariables
            myAddVars = new List<ScriptAction>();
            var CommandItems = scriptItems.Commands;
            foreach (var cmd in CommandItems)
            {
                GetAddVariable(cmd);
            }

            //#3 Katalon
            MyCustomVars = new Dictionary<string, string>();
            foreach (var cmd in CommandItems)
            {
                GetKatVariable(cmd);
            }

            GetFinalDictionary();

            //#4 Crossbreed with external
            //STEP_.PARAM #4 Crossbreed with external
            TasktVars = CrossBreed(TasktVars, extParam);

            //Console.WriteLine("Total vars: " +  MyAddVars.Count().ToString());         

            foreach (var item in TasktVars)
            {
                Console.WriteLine(item.Key + " = " + item.Value);

            }

            writeXml();

            SerializeScript(ScriptItems);

            return TasktVars;

        }

        Script writeXml()
        {

            //SourceXmlFile = xml;
            //ExtVars = extParam;

            //get deserialized script
            //Script scriptItems = Script.DeserializeFile(SourceXmlFile);

            if (scriptItems == null)
                return new Script();

            //#1 script variables          
            foreach (var item in scriptItems.Variables)
            {
                item.VariableValue = TasktVars[item.VariableName];
            }


            //#2 addcommandVariables          
            foreach (var cmd in scriptItems.Commands)
            {
                SetAddVariable(cmd);

            }

            //#3 Katalon do these after all add variables is set        
            foreach (var cmd in scriptItems.Commands)
            {

                setKatVars(cmd);
            }



            return scriptItems;

        }



        void GetFinalDictionary()
        {
            TasktVars = new Dictionary<string, string>();

            //#1 
            foreach (var item in ScriptVariables)
            {
                if (!string.IsNullOrEmpty(item.VariableName))
                    TasktVars.Add(item.VariableName, item.VariableValue.ToString());

            }

            //#2
            foreach (var item in myAddVars)
            {
                var cmd = item.ScriptCommand as NewVariableCommand;// AddVariableCommand;

                if (!TasktVars.ContainsKey(cmd.v_VariableName))
                    TasktVars.Add(cmd.v_VariableName, cmd.v_Input);
            }

            //#3 katalon
            foreach (var item in MyCustomVars)
            {

                if (!TasktVars.ContainsKey(item.Key))
                    TasktVars.Add(item.Key, item.Value);
            }


        }

        List<ScriptAction> GetAddVariable(ScriptAction action)
        {
            if (action.AdditionalScriptCommands.Count > 0)
            {
                foreach (var item in action.AdditionalScriptCommands)
                {
                    GetAddVariable(item);
                }
            }
            else if (action.ScriptCommand is NewVariableCommand)
            {
                myAddVars.Add(action);
            }
            //else if (action.ScriptCommand is AddToVariableCommand)
            //{
            //    myAddVars.Add(action);
            //}

            return myAddVars;
        }

        void SetAddVariable(ScriptAction action)
        {
            if (action.AdditionalScriptCommands.Count > 0)
            {
                foreach (var item in action.AdditionalScriptCommands)
                {
                    SetAddVariable(item);
                }
            }
            else if (action.ScriptCommand is NewVariableCommand)
            {
                //myAddVars.Add(action);
                var cmd = action.ScriptCommand as NewVariableCommand;
                if (TasktVars.ContainsKey(cmd.v_VariableName))
                {
                    cmd.v_Input = TasktVars[cmd.v_VariableName];
                }

            }
            //else if (action.ScriptCommand is AddToVariableCommand)
            //{
            //    //myAddVars.Add(action);
            //    var cmd = action.ScriptCommand as AddToVariableCommand;
            //    if (TasktVars.ContainsKey(cmd.v_userVariableName))
            //    {
            //        cmd.v_Input = TasktVars[cmd.v_userVariableName];
            //    }
            //}

        }


        List<ScriptAction> GetKatVariable(ScriptAction action)
        {
            if (action.AdditionalScriptCommands.Count > 0)
            {
                foreach (var item in action.AdditionalScriptCommands)
                {
                    GetKatVariable(item);
                }
            }
            else if (action.ScriptCommand is ICustomScript)//StartProcessCommandKatalon)
            {
                //MyAddVars.Add(action);
                //var htmlFile = @"C:\BlastAsia\Common\Temp\latest.html";             


                var cmd = action.ScriptCommand as ICustomScript; //StartProcessCommandKatalon;
                                                                 //STEP_.PLAYER #505 decode and save to default html latest file  

                //var vScript = cmd.v_Script.DecodeBase64();
                // var newKats = DevNoteMod.Vars.ScriptManager.GetKatVars(vScript);
                var newKats =
                    cmd.MyGetCustomVars(action as IScriptAction);


                foreach (var item in newKats)
                {
                    if (!MyCustomVars.ContainsKey(item.Key))
                    {
                        MyCustomVars.Add(item.Key, item.Value);
                    }
                }

            }

            return myAddVars;
            //return MyCustomVars;
        }


        public static Dictionary<string, string> CrossBreed(Dictionary<string, string> original, Dictionary<string, string> externalNew)
        {
            Dictionary<string, string> hybrid = new Dictionary<string, string>();

            foreach (var item in original)
            {

                string key = item.Key;
                string value = item.Value;
                if (externalNew.ContainsKey(item.Key))
                {
                    value = externalNew[item.Key];
                }

                //create new dictionary
                hybrid.Add(key, value);

            }


            return hybrid;
        }

        public static Dictionary<string, string> GetKatVars(string htmlContent)
        {
            Dictionary<string, string> katVars = new Dictionary<string, string>();

            //VarDictionary = new Dictionary<string, string>();
            //var htmlFie = @"C:\BlastAsia\Common\Temp\latest.html";

            //STEP_.Katalon #3 get katalon variables
            var key = @"<tr><td>store</td><td>";
            var html = htmlContent;//File.ReadAllText(htmlFile);



            html = html.Replace("\n", string.Empty);

            //return data.Split(new string[] { "xx" }, StringSplitOptions.None);
            string[] trStores = html.Split(new string[] { key }, StringSplitOptions.None);

            if (trStores.Length > 0)
            {
                var endTag = @"</td></tr>";
                foreach (string tr in trStores)
                {
                    int valueLenght = tr.IndexOf('<');
                    string value = tr.Substring(0, valueLenght);


                    var tdSplit = tr.Split(new string[] { endTag }, StringSplitOptions.None);

                    string tableTag = @"<table";
                    if (!tr.StartsWith(tableTag))
                    {
                        var rawVarName = tdSplit.First();//[1];//<td>UserName</td>

                        var td = "<td>";
                        var tdSplit2 = rawVarName.Split(new string[] { td }, StringSplitOptions.None);
                        string varName = tdSplit2.Last();

                        if (!string.IsNullOrEmpty(varName))
                            katVars.Add(varName, value);

                        Console.WriteLine(tr);
                        Console.WriteLine(varName);
                        Console.WriteLine(value);
                        Console.WriteLine("---------------------------------------------------------------");
                    }
                }

            }

            return katVars;
        }


        string key;//= @"<tr><td>store</td><td>";
        string tail;// = html.Substring(iHead);
        int iBody;// = tail.IndexOf(@"</tr>");
        int iHead;
        string body;// = tail.Substring(0, iBody + 5);
        public string CurrentHead { get; set; }

        public IScript MyScript {get { return scriptItems; } }

        public List<IScriptVariable> MyScriptVariables { get; set; }

        void setKatVars(ScriptAction action)
        {
            string htmlContent = string.Empty;

            if (action.AdditionalScriptCommands.Count > 0)
            {
                foreach (var item in action.AdditionalScriptCommands)
                {
                    setKatVars(item);
                }
            }
            else if (action.ScriptCommand is ICustomScript) //StartProcessCommandKatalon)
            {
                //myAddVars.Add(action);
                var cmd = action.ScriptCommand as ICustomScript;// StartProcessCommandKatalon;

                cmd.MySetCustomVars(action as IScriptAction);

                //htmlContent = cmd.v_Script.DecodeBase64();
                //if (string.IsNullOrEmpty(htmlContent))
                //    return;


                ////var htmlFie = @"C:\BlastAsia\Common\Temp\latest.html";
                //key = @"<tr><td>store</td><td>";
                //var html = htmlContent;//File.ReadAllText(htmlFile);
                //                       //html = html.Replace("\n", string.Empty);

                //iHead = html.IndexOf(key);

                //if (iHead > -1)
                //{
                //    //initial heading
                //    CurrentHead = html.Substring(0, iHead);
                //    tail = html.Substring(iHead);

                //    while (!string.IsNullOrEmpty(tail))
                //    {
                //        tail = buildNewHtml(tail);
                //    }

                //    cmd.v_Script = CurrentHead.EncodeBase64();
                //}
                //else
                //{
                //   // cmd.v_Script = CurrentHead.EncodeBase64();
                //   //do nothing
                //}
            }

        }

        string buildNewHtml(string lastTail)
        {

            int index = tail.IndexOf(key);
            if (index == -1)
            {
                CurrentHead += tail;
                tail = string.Empty;
                return string.Empty;
            }

            //var key = @"<tr><td>store</td><td>";
            //string tail = html.Substring(iHead);

            iBody = lastTail.IndexOf(@"</tr>");
            body = lastTail.Substring(0, iBody + 5);

            Console.WriteLine(body);

            var thisRow = body.XMLParse<tr>();
            if (TasktVars.ContainsKey(thisRow.Key))
            {
                thisRow.Value = TasktVars[thisRow.Key];
            }

            var xmlBody = thisRow.XmlSerialize<tr>();

            var storeLoc = @"<td>store</td>";
            int storeIndex = xmlBody.IndexOf(storeLoc);

            if (storeIndex > -1)
            {
                var template = @"<tr><td>store</td><td>##value##<datalist><option>##value##</option></datalist></td><td>##key##</td></tr>";
                body = template.Replace("##value##", thisRow.Value)
                    .Replace("##key##", thisRow.Key);
            }

            //  int remove = //<? xml version = "1.0" encoding = "utf-16" ?>< tr xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" xmlns: xsd = "http://www.w3.org/2001/XMLSchema" >;

            Console.WriteLine(body);

            CurrentHead += body;

            tail = lastTail.Substring(iBody + 5);

            return tail;
        }


        public static void SerializeScript(Script myScript, string scriptFilePath = "")
        {
            if (string.IsNullOrEmpty(scriptFilePath))
            {
                scriptFilePath = FileEndPointManager.DefaultTempTasktFile;
            }

            //output to xml file
            XmlSerializer serializer = BizService.ScriptDirector.GetSerializer(); //new XmlSerializer(typeof(Script));

            var settings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.Entitize,
                Indent = true
            };

            //write to file
            System.IO.FileStream fs;
            using (fs = System.IO.File.Create(scriptFilePath))
            {
                using (XmlWriter writer = XmlWriter.Create(fs, settings))
                {
                    serializer.Serialize(writer, myScript);
                }
            }

        }




    }
}
