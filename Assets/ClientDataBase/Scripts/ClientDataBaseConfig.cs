/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseConfig.cs
**********************************************************/
using UnityEngine;
public class ClientDataBaseConfig : ScriptableObject
{
    /// <summary>
    /// GameTable 識別字串，判斷方式是與資料一開始的字串做比對
    /// </summary>
    public string GameTableCheck = "##";


    public string ROOT = "Assets/KKTools/ClientDataBase/";

    /// <summary>
    /// Script Templates 存放路徑
    /// </summary>
    public string ScriptTemplatesPath = "Templates/";

    /// <summary>
    /// 資料表(txt) 存放路徑
    /// </summary>
    public string GameTablePath = "GameTable/";

    /// <summary>
    /// 資料類別 存放路徑
    /// </summary>
    public string TableClassPath = "Generate/TableClass/";

    /// <summary>
    /// Scriptable Asset 存放路徑
    /// </summary>
    public string ScriptableAssetPath = "Generate/Resources/ClientDataBase/";

    /// <summary>
    /// Scriptable Script 存放路徑
    /// </summary>
    public string ScriptableScriptsPath = "Generate/Scriptable/Script/";

    /// <summary>
    /// Scriptable Editor Script 存放路徑
    /// </summary>
    public string ScriptableEditorPath = "Generate/Scriptable/Editor/";


    /// <summary>
    /// 類別名稱 前綴
    /// EX: Table[FileName]
    /// </summary>
    public string ClassNamePrefix = "Table";

    /// <summary>
    /// Scriptable Script 後綴
    /// EX: Table[FileName]Scriptable
    /// </summary>
    public string ScriptableScriptSuffix = "Scriptable";

    /// <summary>
    /// Scriptable Asset 後綴
    /// EX: Table[FileName]Asset
    /// </summary>
    public string ScriptableAssetSuffix = "Asset";

    /// <summary>
    /// Scriptable Editor 後綴
    /// EX: Table[FileName]ScriptableEditor
    /// </summary>
    public string ScriptableEditorSuffix = "ScriptableEditor";


    /// <summary>
    /// 附檔名 .cs
    /// </summary>
    public string FileExtensionTXT = ".txt";

    /// <summary>
    /// 附檔名 .cs
    /// </summary>
    public string FileExtensionCS = ".cs";

    /// <summary>
    /// 附檔名 .asset
    /// </summary>
    public string FileExtensionASSET = ".asset";


    public string GetTemplatePathName(string name)
    {
        return ROOT + ScriptTemplatesPath + name + FileExtensionTXT;
    }

    public string GetGameTablePathName(string name)
    {
        return ROOT + GameTablePath + name + FileExtensionTXT;
    }

    public string GetGameTablePath()
    {
        return ROOT + GameTablePath;
    }

    public string GetTableClassPath()
    {
        return ROOT + TableClassPath;
    }

    public string GetScriptableAssetPath()
    {
        return ROOT + ScriptableAssetPath;
    }

    public string GetScriptableScriptsPath()
    {
        return ROOT + ScriptableScriptsPath;
    }

    public string GetScriptableEditorPath()
    {
        return ROOT + ScriptableEditorPath;
    }



    public string GetTableClassScriptName(string fileName, bool isExtension = false)
    {
        return ClassNamePrefix + fileName + (isExtension ? FileExtensionCS : "");
    }

    public string GetScriptableScriptName(string fileName, bool isExtension = false)
    {
        return ClassNamePrefix + fileName + ScriptableScriptSuffix + (isExtension ? FileExtensionCS : "");
    }

    public string GetScriptableScriptEditorName(string fileName, bool isExtension = false)
    {
        return ClassNamePrefix + fileName + ScriptableEditorSuffix + (isExtension ? FileExtensionCS : "");
    }

    public string GetScriptableAssetName(string fileName, bool isExtension = false)
    {
        return ClassNamePrefix + fileName + ScriptableAssetSuffix + (isExtension ? FileExtensionASSET : "");
    }
}
