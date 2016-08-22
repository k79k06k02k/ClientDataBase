/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseConfig.cs
**********************************************************/
public static class ClientDataBaseConfig
{
    /// <summary>
    /// GameTable 識別字串，判斷方式是與資料一開始的字串做比對
    /// </summary>
    public const string GameTableCheck = "##";


    const string ROOT = "Assets/KKTools/ClientDataBase/";

    /// <summary>
    /// Script Templates 存放路徑
    /// </summary>
    public const string ScriptTemplatesPath = ROOT + "Templates/";

    /// <summary>
    /// 資料表(txt) 存放路徑
    /// </summary>
    public const string GameTablePath = ROOT + "GameTable/";

    /// <summary>
    /// 資料類別 存放路徑
    /// </summary>
    public const string TableClassPath = ROOT + "Generate/TableClass/";

    /// <summary>
    /// Scriptable Asset 存放路徑
    /// </summary>
    public const string ScriptableAssetPath = ROOT + "Generate/Resources/ClientDataBase/";

    /// <summary>
    /// Scriptable Script 存放路徑
    /// </summary>
    public const string ScriptableScriptsPath = ROOT + "Generate/Scriptable/Script/";

    /// <summary>
    /// Scriptable Editor Script 存放路徑
    /// </summary>
    public const string ScriptableEditorPath = ROOT + "Generate/Scriptable/Editor/";


    /// <summary>
    /// 類別名稱 前綴
    /// EX: Table[FileName]
    /// </summary>
    public const string ClassNamePrefix = "Table";

    /// <summary>
    /// Scriptable Script 後綴
    /// EX: Table[FileName]Scriptable
    /// </summary>
    public const string ScripTableScriptSuffix = "Scriptable";

    /// <summary>
    /// Scriptable Asset 後綴
    /// EX: Table[FileName]Asset
    /// </summary>
    public const string ScripTableAssetSuffix = "Asset";

    /// <summary>
    /// Scriptable Editor 後綴
    /// EX: Table[FileName]ScriptableEditor
    /// </summary>
    public const string ScripTableEditorSuffix = "ScriptableEditor";


    /// <summary>
    /// 附檔名 .cs
    /// </summary>
    public const string FILE_EXTENSION_TXT = ".txt";

    /// <summary>
    /// 附檔名 .cs
    /// </summary>
    public const string FILE_EXTENSION_CS = ".cs";

    /// <summary>
    /// 附檔名 .asset
    /// </summary>
    public const string FILE_EXTENSION_ASSET = ".asset";


    public static string GetTableClassScriptName(string fileName, bool isExtension = false)
    {
        return ClassNamePrefix + fileName + (isExtension ? FILE_EXTENSION_CS : "");
    }

    public static string GetScriptableScriptName(string fileName, bool isExtension = false)
    {
        return ClassNamePrefix + fileName + ScripTableScriptSuffix + (isExtension ? FILE_EXTENSION_CS : "");
    }

    public static string GetScriptableScriptEditorName(string fileName, bool isExtension = false)
    {
        return ClassNamePrefix + fileName + ScripTableEditorSuffix + (isExtension ? FILE_EXTENSION_CS : "");
    }

    public static string GetScriptableAssetName(string fileName, bool isExtension = false)
    {
        return ClassNamePrefix + fileName + ScripTableAssetSuffix + (isExtension ? FILE_EXTENSION_ASSET : "");
    }
}
