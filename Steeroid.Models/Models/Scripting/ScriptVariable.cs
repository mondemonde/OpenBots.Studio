//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
//using System.Windows.Forms;
using System.Xml;
using System.Data;
using System.Reflection;
using Steeroid.Models.Interfaces.Scripting;
using Steeroid.Models.Interfaces.Plugins;
//using taskt.Core.Automation.Commands;
//using Tasktskie.Common.Contracts;

namespace Steeroid.Models.Scripting
{
    #region Script and Variables


    [Serializable]
    public class ScriptVariable
    {
        /// <summary>
        /// name that will be used to identify the variable
        /// </summary>
        public string VariableName { get; set; }
        /// <summary>
        /// index/position tracking for complex variables (list)
        /// </summary>
        [XmlIgnore]
        public int CurrentPosition = 0;
        /// <summary>
        /// value of the variable or current index
        /// </summary>
        public object VariableValue { get; set; }
        /// <summary>
        /// retrieve value of the variable
        /// </summary>
        public string GetDisplayValue(string requiredProperty = "")
        {
           
            if (VariableValue is string)
            {
                switch (requiredProperty)
                {
                    case "type":
                    case "Type":
                    case "TYPE":
                        return "BASIC";
                    default:
                        return (string)VariableValue;
                }
              
            }
            else if(VariableValue is DataTable)
            {
                DataTable dataTable = (DataTable)VariableValue;              
                var dataRow = dataTable.Rows[CurrentPosition];
                //return Newtonsoft.Json.JsonConvert.SerializeObject(dataRow.ItemArray);   

                var expando = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
                foreach (DataColumn col in dataRow.Table.Columns)
                {
                    expando[col.ColumnName] = dataRow[col];
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(expando);
                return json;
            }
            else
            {
                List<string> requiredValue = (List<string>)VariableValue;
                switch(requiredProperty)
                {
                    case "count":
                    case "Count":
                    case "COUNT":
                        return requiredValue.Count.ToString();
                    case "index":
                    case "Index":
                    case "INDEX":
                        return CurrentPosition.ToString();
                    case "tojson":
                    case "ToJson":                
                    case "toJson":
                    case "TOJSON":
                        return Newtonsoft.Json.JsonConvert.SerializeObject(requiredValue);
                    case "topipe":
                    case "ToPipe":
                    case "toPipe":                
                    case "TOPIPE":
                        return String.Join("|", requiredValue);
                    case "first":
                    case "First":
                    case "FIRST":
                        return requiredValue.FirstOrDefault();
                    case "last":
                    case "Last":
                    case "LAST":
                        return requiredValue.Last();
                    case "type":
                    case "Type":
                    case "TYPE":
                        return "LIST";
                    default:
                        return requiredValue[CurrentPosition];
                }
            }
           
        }
    }

    #endregion Script and Variables

   
}