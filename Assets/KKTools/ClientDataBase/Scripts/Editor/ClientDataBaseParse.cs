/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseParse.cs
**********************************************************/
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using System.Text;


public class ClientDataBaseParse : Singleton<ClientDataBaseParse>
{
    public class TableData
    {
        public TableData(string summary, string name, string type, bool isArray, bool isEnd)
        {
            this.summary = summary;
            this.name = name;
            this.type = type;
            this.isArray = isArray;
            this.isEnd = isEnd;
        }

        public string summary;
        public string name;
        public string type;
        public bool isArray;
        public bool isEnd;
    }

    string tableName;
    

    /// <summary>
    /// 讀取GameTable(.txt)
    /// </summary>
    public bool LoadGameTable(Object obj)
    {
        tableName = obj.name;
     
        string strTemp;
        TextAsset data = (TextAsset)obj;
        TextReader reader = null;

        string[] _Summary = null;
        string[] _Column = null;
        string[] _Type = null;
        int index = 0;


        if (data == null)
        {
            Debug.LogError("GameTable is null.");
            return false;
        }

        if (data.text == string.Empty)
        {
            Debug.LogError("GameTable is empty.");
            return false;
        }

        reader = new StringReader(data.text);
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
                    _Column = strTemp.Split("\t"[0]);
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
                    if (_Summary[0].IndexOf(ClientDataBaseConfig.GameTableCheck) < 0)
                    {
                        Debug.LogError("GameTable is not a table. Please Check txt file start string is [" + ClientDataBaseConfig.GameTableCheck + "]");
                        break;
                    }

                    //2.判斷欄位數量是否一致
                    int count = _Summary.Length;
                    if (count != _Column.Length || count != _Type.Length)
                    {
                        Debug.LogError("GameTable column not same.");
                        break;
                    }

                    if (CreateTableScript(_Summary, _Column, _Type) == false)
                        return false;

                    if (CreateScriptableScript(_Column, _Type) == false)
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
    bool CreateTableScript(string[] summary, string[] column, string[] type)
    {
        Dictionary<string, TableData> dataMap = new Dictionary<string, TableData>();

        string templateDataClass = GetTemplate("TableClass");
        if (string.IsNullOrEmpty(templateDataClass))
            return false;

        templateDataClass = templateDataClass.Replace("$ClassName", ClientDataBaseConfig.GetTableClassScriptName(tableName));

        StringBuilder field = new StringBuilder();

        for (int i = 0; i < column.Length; i++)
        {
            //透過字元 '[' ']' 判斷是否是Array 
            bool isArray = Regex.Match(column[i], "[\\[\\]]").Success;
            bool isEnd = i == column.Length - 1;

            //如果是Array，去除中括號與中括號內數字
            string fieldName = isArray ? Regex.Replace(column[i], "\\[[0-9]\\]", "") : column[i];

            if (dataMap.ContainsKey(fieldName))
                dataMap[fieldName].summary += ", " + summary[i];
            else
                dataMap.Add(fieldName, new TableData(summary[i], fieldName, type[i], isArray, isEnd));
        }

        foreach (KeyValuePair<string, TableData> item in dataMap)
            field.Append(GetProperty(item.Value.summary, item.Value.name, item.Value.type, item.Value.isArray, item.Value.isEnd));


        templateDataClass = templateDataClass.Replace("$MemberFields", field.ToString());


        UtilityEditor.CreateFolder(ClientDataBaseConfig.TableClassPath);
        using (var writer = new StreamWriter(ClientDataBaseConfig.TableClassPath + ClientDataBaseConfig.GetTableClassScriptName(tableName, true)))
        {
            writer.Write(templateDataClass);
            writer.Close();
        }

        AssetDatabase.Refresh();
        Debug.Log(string.Format("[TableClass] is Create.\nFile:[{0}] Path:[{1}]", ClientDataBaseConfig.GetTableClassScriptName(tableName, true), ClientDataBaseConfig.TableClassPath));

        return true;
    }

    /// <summary>
    /// 建立 Scriptable Script
    /// </summary>
    /// <returns>是否成功建立</returns>
    bool CreateScriptableScript(string[] column, string[] type)
    {
        string template = GetTemplate("Scriptable");
        if (string.IsNullOrEmpty(template))
            return false;

        template = template.Replace("$ScriptableName", ClientDataBaseConfig.GetScriptableScriptName(tableName));
        template = template.Replace("$GameTableName", tableName);
        template = template.Replace("$ClassName", ClientDataBaseConfig.GetTableClassScriptName(tableName));
        template = template.Replace("$GameTablePath", "Config.GameTablePath + GameTableName + Config.FILE_EXTENSION_TXT");


        Dictionary<string, string> columnMap = new Dictionary<string, string>();

        for (int i = 0; i < column.Length; i++)
        {
            string resultStr = string.Empty;

            //透過字元 '[' ']' 判斷是否是Array 
            bool isArray = Regex.Match(column[i], "[\\[\\]]").Success;

            //如果是Array，去除中括號與中括號內數字
            string fieldName = isArray ? Regex.Replace(column[i], "\\[[0-9]\\]", "") : column[i];


            resultStr = GetDataClassDetial(i, fieldName, type[i], isArray);


            //如果名稱一樣(只會發生在複數Array)
            if (columnMap.ContainsKey(fieldName))
            {
                string oldStr = columnMap[fieldName];
                string element = Regex.Match(oldStr, "\\{[^\\}]*\\}").ToString().Trim(new char[] { '{', '}', ' ' });

                string newStr = element + ", " + GetTypeDataClass(i, type[i]);

                columnMap.Remove(fieldName);
                columnMap.Add(fieldName, oldStr.Replace(element, newStr));
            }
            else
            {
                columnMap.Add(fieldName, resultStr);
            }
        }

        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, string> item in columnMap)
        {
            sb.Append(item.Value);
        }

        template = template.Replace("$DataLoad", sb.ToString());


        UtilityEditor.CreateFolder(ClientDataBaseConfig.ScriptableScriptsPath);
        using (var writer = new StreamWriter(ClientDataBaseConfig.ScriptableScriptsPath + ClientDataBaseConfig.GetScriptableScriptName(tableName, true)))
        {
            writer.Write(template);
            writer.Close();
        }

        AssetDatabase.Refresh();
        Debug.Log(string.Format("[Scriptable Script] is Create.\nFile:[{0}] Path:[{1}]", ClientDataBaseConfig.GetScriptableScriptName(tableName, true), ClientDataBaseConfig.ScriptableScriptsPath));

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

        templateScriptable = templateScriptable.Replace("$ScriptableEditorName", ClientDataBaseConfig.GetScriptableScriptEditorName(tableName));
        templateScriptable = templateScriptable.Replace("$ScriptableName", ClientDataBaseConfig.GetScriptableScriptName(tableName));


        UtilityEditor.CreateFolder(ClientDataBaseConfig.ScriptableEditorPath);
        using (var writer = new StreamWriter(ClientDataBaseConfig.ScriptableEditorPath + ClientDataBaseConfig.GetScriptableScriptEditorName(tableName, true)))
        {
            writer.Write(templateScriptable);
            writer.Close();
        }

        AssetDatabase.Refresh();
        Debug.Log(string.Format("[Scriptable Script Editor] is Create.\nFile:[{0}] Path:[{1}]", ClientDataBaseConfig.GetScriptableScriptEditorName(tableName, true), ClientDataBaseConfig.ScriptableEditorPath));

        return true;
    }

    /// <summary>
    /// 建立 Scriptable Asset
    /// </summary>
    /// <returns>是否成功建立</returns>
    public bool CreateScriptableAssets(string scriptableScriptName, string scriptableAssetName)
    {
        MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(ClientDataBaseConfig.ScriptableScriptsPath + scriptableScriptName);

        if (script == null || script.GetClass() == null)
        {
            Debug.LogError(string.Format("Scriptable Script is Null. [Path:{0}]", ClientDataBaseConfig.ScriptableScriptsPath + scriptableScriptName));
            return false;
        }

        string path = ClientDataBaseConfig.ScriptableAssetPath + scriptableAssetName;
        UtilityEditor.CreateFolder(ClientDataBaseConfig.ScriptableAssetPath);

        Object _Object = ScriptableObject.CreateInstance(script.GetClass());
        AssetDatabase.CreateAsset(_Object, path);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

        Debug.Log(string.Format("[Scriptable Asset] is Create.\nFile:[{0}] Path:[{1}]", scriptableAssetName, ClientDataBaseConfig.ScriptableAssetPath));

        //資料讀取
        ScriptableObjectBase scriptableObjectBase = AssetDatabase.LoadAssetAtPath<ScriptableObjectBase>(path);
        scriptableObjectBase.LoadGameTable();

        return true;
    }

    /// <summary>
    ///取得 Script Templates
    /// </summary>
    string GetTemplate(string name)
    {
        string path = ClientDataBaseConfig.ScriptTemplatesPath + name + ClientDataBaseConfig.FILE_EXTENSION_TXT;
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
    string GetDataClassDetial(int index, string name, string type, bool isArray)
    {
        if (isArray)
            return string.Format("\t\t\t\ttable.{0} = new {1}[] {{ {2} }};\n", name, type, GetTypeDataClass(index, type));
        else
            return string.Format("\t\t\t\ttable.{0} = {1};\n", name, GetTypeDataClass(index, type));
    }


    /// <summary>
    ///取得轉型
    /// </summary>
    string GetTypeDataClass(int index, string type)
    {
        switch (type)
        {
            case "Vector2":
            case "Vector3":
                return string.Format("Utility.TypeRelate.StringToVector{0}(splitStr[{1}])", (type == "Vector2" ? "2" : "3"), index);

            case "bool":
                return string.Format("Utility.TypeRelate.StringToBool(splitStr[{0}])", index);

            default:
                return string.Format("({0})Convert.ChangeType(splitStr[{1}], typeof({0}))", type, index);
        }
    }
}

