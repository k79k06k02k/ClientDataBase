/**********************************************************
// Author   : Arkai (k79k06k02k)
// FileName : ClientDataBaseParse.cs
**********************************************************/
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace ClientDataBase
{
    public class ClientDataBaseParse : Singleton<ClientDataBaseParse>
    {
        public class TableData
        {
            public TableData(string summary, string name, string type, bool isArray, bool isArrayInLine, bool isEnd)
            {
                this.summary = summary;
                this.name = name;
                this.type = type;
                this.isArray = isArray;
                this.isArrayInLine = isArrayInLine;
                this.isEnd = isEnd;
            }

            public string summary;
            public string name;
            public string type;
            public bool isArray;
            public bool isArrayInLine;
            public bool isEnd;
        }

        private string m_tableName;
        private ClientDataBaseConfig m_config;
        private int m_startRow = 2;

        /// <summary>
        /// 讀取GameTable(.txt)
        /// </summary>
        public bool LoadGameTable(Object obj)
        {
            m_config = ClientDataBaseManager.Instance.Config;
            m_tableName = obj.name;

            string strTemp;
            string path = AssetDatabase.GetAssetPath(obj);
            string content = File.ReadAllText(path);
            TextReader reader = null;

            string[] _Summary = null;
            string[] _Variable = null;
            string[] _Type = null;
            int index = 0;


            if (content == null)
            {
                Debug.LogError("GameTable is null.");
                return false;
            }

            if (content == string.Empty)
            {
                Debug.LogError("GameTable is empty.");
                return false;
            }

            reader = new StringReader(content);
            if (reader != null)
            {
                while ((strTemp = reader.ReadLine()) != null)
                {
                    if (index == 0)
                    {
                        _Summary = strTemp.Split("\t"[0]);
                        index++;
                        continue;
                    }

                    if (index == 1)
                    {
                        _Variable = strTemp.Split("\t"[0]);
                        index++;
                        continue;
                    }

                    if (index == 2)
                    {
                        _Type = strTemp.Split("\t"[0]);
                        index++;
                        continue;
                    }

                    if (index == 3)
                    {
                        //1.判斷是否是 GameTable(txt)，檔案的開始字串是否包含 識別字
                        if (_Summary[0].IndexOf(m_config.gameTableCheck) < 0)
                        {
                            Debug.LogError("GameTable is not a table. Please Check txt file start string is [" + m_config.gameTableCheck + "]");
                            break;
                        }

                        //2.判斷欄位數量是否一致
                        int count = _Summary.Length;
                        if (count != _Variable.Length || count != _Type.Length)
                        {
                            Debug.LogError("GameTable column not same.");
                            break;
                        }

                        if (CreateTableScript(_Summary, _Variable, _Type) == false)
                            return false;

                        if (CreateScriptableScript(_Variable, _Type) == false)
                            return false;

                        if (CreateScriptableScriptEditor() == false)
                            return false;

                        break;
                    }
                }

                reader.Close();
            }

            return true;
        }


        /// <summary>
        /// 建立 Table Class
        /// </summary>
        /// <returns>是否成功建立</returns>
        bool CreateTableScript(string[] summary, string[] variable, string[] type)
        {
            Dictionary<string, TableData> dataMap = new Dictionary<string, TableData>();

            string templateDataClass = GetTemplate("TableClass");
            if (string.IsNullOrEmpty(templateDataClass))
                return false;

            templateDataClass = templateDataClass.Replace("$ClassName", m_config.GetTableClassScriptName(m_tableName));

            StringBuilder field = new StringBuilder();

            for (int i = m_startRow; i < variable.Length; i++)
            {
                //透過字元 '[' ']' 判斷是否是Array 
                bool isArray = variable[i].EndsWith("[]");
                bool isArrayInLine = variable[i].EndsWith("{}");
                bool isEnd = i == variable.Length - 1;


                string fieldName = string.Empty;

                //如果是Array，去除括號
                if (isArray)
                {
                    fieldName = variable[i].Replace("[]", "");
                }
                else if (isArrayInLine)
                {
                    fieldName = variable[i].Replace("{}", "");
                }
                else
                {
                    fieldName = variable[i];
                }


                if (dataMap.ContainsKey(fieldName))
                    dataMap[fieldName].summary += ", " + summary[i];
                else
                    dataMap.Add(fieldName, new TableData(summary[i], fieldName, type[i], isArray, isArrayInLine, isEnd));
            }

            foreach (KeyValuePair<string, TableData> item in dataMap)
                field.Append(GetProperty(item.Value.summary, item.Value.name, item.Value.type, item.Value.isArray || item.Value.isArrayInLine, item.Value.isEnd));


            templateDataClass = templateDataClass.Replace("$MemberFields", field.ToString());


            UtilityEditor.CreateFolder(m_config.GetTableClassPath());
            using (var writer = new StreamWriter(m_config.GetTableClassPath() + m_config.GetTableClassScriptName(m_tableName, true)))
            {
                writer.Write(templateDataClass);
                writer.Close();
            }

            AssetDatabase.Refresh();
            Debug.Log(string.Format("[TableClass] is Create.\nFile:[{0}] Path:[{1}]", m_config.GetTableClassScriptName(m_tableName, true), m_config.GetTableClassPath()));

            return true;
        }

        /// <summary>
        /// 建立 Scriptable Script
        /// </summary>
        /// <returns>是否成功建立</returns>
        bool CreateScriptableScript(string[] variable, string[] type)
        {
            string template = GetTemplate("Scriptable");
            if (string.IsNullOrEmpty(template))
                return false;

            template = template.Replace("$ScriptableName", m_config.GetScriptableScriptName(m_tableName));
            template = template.Replace("$GameTableName", m_tableName);
            template = template.Replace("$ClassName", m_config.GetTableClassScriptName(m_tableName));
            template = template.Replace("$GameTablePath", "Config.GameTablePath + GameTableName + Config.FILE_EXTENSION_TXT");


            Dictionary<string, string> variableMap = new Dictionary<string, string>();

            for (int i = m_startRow; i < variable.Length; i++)
            {
                string resultStr = string.Empty;

                //透過字元 '[' ']' 判斷是否是Array 
                bool isArray = variable[i].EndsWith("[]");
                bool isArrayInLine = variable[i].EndsWith("{}");

                string fieldName = string.Empty;
                //如果是Array，去除括號
                if (isArray)
                {
                    fieldName = variable[i].Replace("[]", "");
                }
                else if (isArrayInLine)
                {
                    fieldName = variable[i].Replace("{}", "");
                }
                else
                {
                    fieldName = variable[i];
                }


                resultStr = GetDataClassDetial(i, fieldName, type[i], isArray, isArrayInLine);


                //如果名稱一樣(只會發生在複數Array)
                if (variableMap.ContainsKey(fieldName))
                {
                    string oldStr = variableMap[fieldName];
                    string element = Regex.Match(oldStr, "\\{[^\\}]*\\}").ToString().Trim(new char[] { '{', '}', ' ' });

                    string newStr = element + ", " + GetTypeDataClass(i.ToString(), type[i], "splitStr");

                    variableMap.Remove(fieldName);

                    try
                    {
                        variableMap.Add(fieldName, oldStr.Replace(element, newStr));
                    }
                    catch (ArgumentException e)
                    {
                        Debug.LogError(string.Format("Duplicate variable name and is not Array, table:[{0}], variable name:[{1}].", m_tableName, fieldName));
                        return false;
                    }
                }
                else
                {
                    variableMap.Add(fieldName, resultStr);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> item in variableMap)
            {
                sb.Append(item.Value);
            }

            template = template.Replace("$DataLoad", sb.ToString());


            UtilityEditor.CreateFolder(m_config.GetScriptableScriptsPath());
            using (var writer = new StreamWriter(m_config.GetScriptableScriptsPath() + m_config.GetScriptableScriptName(m_tableName, true)))
            {
                writer.Write(template);
                writer.Close();
            }

            AssetDatabase.Refresh();
            Debug.Log(string.Format("[Scriptable Script] is Create.\nFile:[{0}] Path:[{1}]", m_config.GetScriptableScriptName(m_tableName, true), m_config.GetScriptableScriptsPath()));

            return true;
        }

        /// <summary>
        /// 建立 Scriptable Script Editor
        /// </summary>
        /// <returns>是否成功建立</returns>
        bool CreateScriptableScriptEditor()
        {
            string templateScriptable = GetTemplate("ScriptableEditor");
            if (string.IsNullOrEmpty(templateScriptable))
                return false;

            templateScriptable = templateScriptable.Replace("$ScriptableEditorName", m_config.GetScriptableScriptEditorName(m_tableName));
            templateScriptable = templateScriptable.Replace("$ScriptableName", m_config.GetScriptableScriptName(m_tableName));


            UtilityEditor.CreateFolder(m_config.GetScriptableEditorPath());
            using (var writer = new StreamWriter(m_config.GetScriptableEditorPath() + m_config.GetScriptableScriptEditorName(m_tableName, true)))
            {
                writer.Write(templateScriptable);
                writer.Close();
            }

            AssetDatabase.Refresh();
            Debug.Log(string.Format("[Scriptable Script Editor] is Create.\nFile:[{0}] Path:[{1}]", m_config.GetScriptableScriptEditorName(m_tableName, true), m_config.GetScriptableEditorPath()));

            return true;
        }

        /// <summary>
        /// 建立 Scriptable Asset
        /// </summary>
        /// <returns>是否成功建立</returns>
        public bool CreateScriptableAssets(string scriptableScriptName, string scriptableAssetName)
        {
            m_config = ClientDataBaseManager.Instance.Config;
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(m_config.GetScriptableScriptsPath() + scriptableScriptName);

            if (script == null || script.GetClass() == null)
            {
                Debug.LogError(string.Format("Scriptable Script is Null. [Path:{0}]", m_config.GetScriptableScriptsPath() + scriptableScriptName));
                return false;
            }

            string path = m_config.GetScriptableAssetPath() + scriptableAssetName;
            UtilityEditor.CreateFolder(m_config.GetScriptableAssetPath());

            Object _Object = ScriptableObject.CreateInstance(script.GetClass());
            AssetDatabase.CreateAsset(_Object, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

            Debug.Log(string.Format("[Scriptable Asset] is Create.\nFile:[{0}] Path:[{1}]", scriptableAssetName, m_config.GetScriptableAssetPath()));

            //資料讀取
            ScriptableObjectBase scriptableObjectBase = AssetDatabase.LoadAssetAtPath<ScriptableObjectBase>(path);

            return scriptableObjectBase.LoadGameTable();
        }

        /// <summary>
        ///取得 Script Templates
        /// </summary>
        string GetTemplate(string name)
        {
            string path = m_config.GetTemplatePathName(name);
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            if (textAsset == null)
            {
                Debug.LogError(string.Format("Can't found Script Templates txt file in [Path:{0}]", path));
                return null;
            }

            return textAsset.ToString();
        }

        /// <summary>
        ///設定各屬性
        /// </summary>
        string GetProperty(string summary, string name, string type, bool isArray, bool isEnd = false)
        {
            string templateProperty = GetTemplate("TableClassProperty");
            templateProperty = templateProperty.Replace("$Summary", summary);
            templateProperty = templateProperty.Replace("$Modifier", "public");
            templateProperty = templateProperty.Replace("$Type", type + (isArray ? "[]" : ""));
            templateProperty = templateProperty.Replace("$Name", name);
            templateProperty += isEnd ? "" : "\n\n";
            return templateProperty;
        }

        /// <summary>
        /// 實作 TableClass 細部
        /// </summary>
        string GetDataClassDetial(int index, string name, string type, bool isArray, bool isArrayInLine)
        {
            if (isArray)
                return string.Format("\t\t\t\t\ttable.{0} = new {1}[] {{ {2} }};\n", name, type, GetTypeDataClass(index.ToString(), type, "splitStr"));
            else if (isArrayInLine)
                return GetDataClassDetialArrayInLine(name, index, type);
            else
                return string.Format("\t\t\t\t\ttable.{0} = {1};\n", name, GetTypeDataClass(index.ToString(), type, "splitStr"));
        }

        string GetDataClassDetialArrayInLine(string name, int index, string type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("\n\t\t\t\t\tstring[] old{0} = ({1}).Split(',');\n", name, GetTypeDataClass(index.ToString(), "string", "splitStr")));
            sb.Append(string.Format("\t\t\t\t\ttable.{0} = new {1}[old{0}.Length];\n", name, type));
            sb.Append(string.Format("\t\t\t\t\tfor (int i = 0; i < table.{0}.Length; i++)\n", name));
            sb.Append(string.Format("\t\t\t\t\t\ttable.{0}[i] = {1};\n", name, GetTypeDataClass("i", type, "old" + name)));
            return sb.ToString();
        }


        /// <summary>
        ///取得轉型
        /// </summary>
        string GetTypeDataClass(string index, string type, string value)
        {
            switch (type)
            {
                case "Vector2":
                case "Vector3":
                    return string.Format("Utility.TypeRelate.StringToVector{0}({1}[{2}])", (type == "Vector2" ? "2" : "3"), value, index);

                case "bool":
                    return string.Format("Utility.TypeRelate.StringToBool({0}[{1}])", value, index);

                default:
                    return string.Format("({0})Convert.ChangeType({1}[{2}], typeof({0}))", type, value, index);
            }
        }
    }
}
