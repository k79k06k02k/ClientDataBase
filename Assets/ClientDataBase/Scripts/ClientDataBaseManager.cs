/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseManager.cs
**********************************************************/
using System.Collections.Generic;
using System;
using ClientDataBase;

public class ClientDataBaseManager : Singleton<ClientDataBaseManager>
{
    const string c_pathConfig = "Client DataBase Config";
    const string c_pathScriptableAssetRoot = "ClientDataBase/";

    ClientDataBaseConfig _config;
    public ClientDataBaseConfig m_config
    {
        get
        {
            if (_config == null)
                _config = Utility.AssetRelate.ResourcesLoadCheckNull<ClientDataBaseConfig>(c_pathConfig);

            return _config;
        }
    }


    public Dictionary<Type, ScriptableObjectBase> m_tableList = new Dictionary<Type, ScriptableObjectBase>();

    public ClientDataBaseManager()
    {
    }

    ScriptableObjectBase LoadTable(string fileName)
    {
        return Utility.AssetRelate.ResourcesLoadCheckNull<ScriptableObjectBase>(c_pathScriptableAssetRoot + m_config.GetScriptableAssetName(fileName));
    }

    void Register(Type type, ScriptableObjectBase dataBase)
    {
        m_tableList.Add(type, dataBase);
    }

    public T GetTable<T>() where T : ScriptableObjectBase, new()
    {
        if (m_tableList.ContainsKey(typeof(T)))
        {
            return (T)m_tableList[typeof(T)];
        }

        return default(T);
    }
}

