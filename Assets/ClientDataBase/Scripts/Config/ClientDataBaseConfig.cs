/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseConfig.cs
**********************************************************/
public class ClientDataBaseConfig : UnityEngine.ScriptableObject
{
    /// <summary>
    /// GameTable 識別字串，判斷方式是與資料一開始的字串做比對
    /// </summary>
    public string m_gameTableCheck = "##";


    public string m_root = "Assets/ClientDataBase/";

    /// <summary>
    /// Script Templates 存放路徑
    /// </summary>
    public string m_pathScriptTemplates = "Templates/";

    /// <summary>
    /// 資料表(txt) 存放路徑
    /// </summary>
    public string m_pathGameTable = "GameTable/";

    /// <summary>
    /// 資料類別 存放路徑
    /// </summary>
    public string m_pathTableClass = "Generate/TableClass/";

    /// <summary>
    /// Scriptable Asset 存放路徑
    /// </summary>
    public string m_pathScriptableAsset = "Generate/Resources/ClientDataBase/";

    /// <summary>
    /// Scriptable Script 存放路徑
    /// </summary>
    public string m_pathScriptableScripts = "Generate/Scriptable/Script/";

    /// <summary>
    /// Scriptable Editor Script 存放路徑
    /// </summary>
    public string m_pathScriptableEditor = "Generate/Scriptable/Editor/";


    /// <summary>
    /// 類別名稱 前綴
    /// EX: Table[FileName]
    /// </summary>
    public string m_nameClassPrefix = "Table";

    /// <summary>
    /// Scriptable Asset 後綴
    /// EX: Table[FileName]Asset
    /// </summary>
    public string m_nameScriptableAssetSuffix = "Asset";

    /// <summary>
    /// Scriptable Script 後綴
    /// EX: Table[FileName]Scriptable
    /// </summary>
    public string m_nameScriptableScriptSuffix = "Scriptable";
    
    /// <summary>
    /// Scriptable Editor 後綴
    /// EX: Table[FileName]ScriptableEditor
    /// </summary>
    public string m_nameScriptableEditorSuffix = "ScriptableEditor";


    /// <summary>
    /// 附檔名 .cs
    /// </summary>
    public string m_extensionTxt = ".txt";

    /// <summary>
    /// 附檔名 .cs
    /// </summary>
    public string m_extensionCs = ".cs";

    /// <summary>
    /// 附檔名 .asset
    /// </summary>
    public string m_extensionAsset = ".asset";


    public string GetTemplatePathName(string name)
    {
        return m_root + m_pathScriptTemplates + name + m_extensionTxt;
    }

    public string GetGameTablePathName(string name)
    {
        return m_root + m_pathGameTable + name + m_extensionTxt;
    }

    public string GetGameTablePath()
    {
        return m_root + m_pathGameTable;
    }

    public string GetTableClassPath()
    {
        return m_root + m_pathTableClass;
    }

    public string GetScriptableAssetPath()
    {
        return m_root + m_pathScriptableAsset;
    }

    public string GetScriptableScriptsPath()
    {
        return m_root + m_pathScriptableScripts;
    }

    public string GetScriptableEditorPath()
    {
        return m_root + m_pathScriptableEditor;
    }



    public string GetTableClassScriptName(string fileName, bool boolExtension = false)
    {
        return m_nameClassPrefix + fileName + (boolExtension ? m_extensionCs : "");
    }

    public string GetScriptableScriptName(string fileName, bool boolExtension = false)
    {
        return m_nameClassPrefix + fileName + m_nameScriptableScriptSuffix + (boolExtension ? m_extensionCs : "");
    }

    public string GetScriptableScriptEditorName(string fileName, bool boolExtension = false)
    {
        return m_nameClassPrefix + fileName + m_nameScriptableEditorSuffix + (boolExtension ? m_extensionCs : "");
    }

    public string GetScriptableAssetName(string fileName, bool boolExtension = false)
    {
        return m_nameClassPrefix + fileName + m_nameScriptableAssetSuffix + (boolExtension ? m_extensionAsset : "");
    }
}
