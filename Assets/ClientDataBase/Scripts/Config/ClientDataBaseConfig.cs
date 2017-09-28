using System;


namespace ClientDataBase
{
    public class ClientDataBaseConfig : UnityEngine.ScriptableObject
    {
        /// <summary>
        /// GameTable 識別字串，判斷方式是與資料一開始的字串做比對
        /// </summary>
        public string gameTableCheck = "##";


        public string root = "Assets/ClientDataBase/";

        /// <summary>
        /// Script Templates 存放路徑
        /// </summary>
        public string pathScriptTemplates = "Templates/";

        /// <summary>
        /// 資料表(txt) 存放路徑
        /// </summary>
        public string pathGameTable = "GameTable/";

        /// <summary>
        /// 資料類別 存放路徑
        /// </summary>
        public string pathTableClass = "Generate/TableClass/";

        /// <summary>
        /// Scriptable Asset 存放路徑
        /// </summary>
        public string pathScriptableAsset = "Generate/Resources/ClientDataBase/";

        /// <summary>
        /// Scriptable Script 存放路徑
        /// </summary>
        public string pathScriptableScripts = "Generate/Scriptable/Script/";

        /// <summary>
        /// Scriptable Editor Script 存放路徑
        /// </summary>
        public string pathScriptableEditor = "Generate/Scriptable/Editor/";


        /// <summary>
        /// 類別名稱 前綴
        /// EX: Table[FileName]
        /// </summary>
        public string nameClassPrefix = "Table";

        /// <summary>
        /// Scriptable Asset 後綴
        /// EX: Table[FileName]Asset
        /// </summary>
        public string nameScriptableAssetSuffix = "Asset";

        /// <summary>
        /// Scriptable Script 後綴
        /// EX: Table[FileName]Scriptable
        /// </summary>
        public string nameScriptableScriptSuffix = "Scriptable";

        /// <summary>
        /// Scriptable Editor 後綴
        /// EX: Table[FileName]ScriptableEditor
        /// </summary>
        public string nameScriptableEditorSuffix = "ScriptableEditor";


        /// <summary>
        /// 附檔名 .tsv
        /// </summary>
        public readonly string extensionTsv = ".tsv";

        /// <summary>
        /// 附檔名 .txt
        /// </summary>
        public readonly string extensionTxt = ".txt";

        /// <summary>
        /// 附檔名 .cs
        /// </summary>
        public readonly string extensionCs = ".cs";

        /// <summary>
        /// 附檔名 .asset
        /// </summary>
        public readonly string extensionAsset = ".asset";


        public string GetTemplatePathName(string name)
        {
            return root + pathScriptTemplates + name + extensionTxt;
        }

        public string GetGameTablePathName(string name)
        {
            return root + pathGameTable + name + extensionTsv;
        }

        public string GetGameTablePath()
        {
            return root + pathGameTable;
        }

        public string GetTableClassPath()
        {
            return root + pathTableClass;
        }

        public string GetScriptableAssetPath()
        {
            return root + pathScriptableAsset;
        }

        public string GetScriptableScriptsPath()
        {
            return root + pathScriptableScripts;
        }

        public string GetScriptableEditorPath()
        {
            return root + pathScriptableEditor;
        }



        public string GetTableClassScriptName(string fileName, bool boolExtension = false)
        {
            return nameClassPrefix + fileName + (boolExtension ? extensionCs : "");
        }

        public string GetScriptableScriptName(string fileName, bool boolExtension = false)
        {
            return nameClassPrefix + fileName + nameScriptableScriptSuffix + (boolExtension ? extensionCs : "");
        }

        public string GetScriptableScriptEditorName(string fileName, bool boolExtension = false)
        {
            return nameClassPrefix + fileName + nameScriptableEditorSuffix + (boolExtension ? extensionCs : "");
        }

        public string GetScriptableAssetName(string fileName, bool boolExtension = false)
        {
            return nameClassPrefix + fileName + nameScriptableAssetSuffix + (boolExtension ? extensionAsset : "");
        }

        public string GetScriptableAssetNameFromType(Type type)
        {
            return type.ToString().Replace(nameScriptableScriptSuffix, nameScriptableAssetSuffix);
        }
    }
}
