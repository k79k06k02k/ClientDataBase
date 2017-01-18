/**********************************************************
// Author   : Arkai (k79k06k02k)
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


    void Register<T>() where T : ScriptableObjectBase
    {
        m_tableList.Add(typeof(T), LoadTable(typeof(T)));
    }

    ScriptableObjectBase LoadTable(Type type)
    {
        return Utility.AssetRelate.ResourcesLoadCheckNull<ScriptableObjectBase>(c_pathScriptableAssetRoot + m_config.GetScriptableAssetNameFromType(type));
    }


    public T GetTable<T>() where T : ScriptableObjectBase
    {
        if (m_tableList.ContainsKey(typeof(T)))
        {
            return (T)m_tableList[typeof(T)];
        }

        return null;
    }

    public U GetData<T, U>(int index) where T : ScriptableObjectBase
                                      where U : TableClassBase
    {
        T t = GetTable<T>();

        if (t != null)
        {
           return (U)t.GetDataBase(index);
        }

        return null;
    }
}

