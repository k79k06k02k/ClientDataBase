/**********************************************************
// Author   : Arkai (k79k06k02k)
// FileName : ClientDataBaseManager.cs
**********************************************************/
using System.Collections.Generic;
using System;
using ClientDataBase;

public class ClientDataBaseManager : Singleton<ClientDataBaseManager>
{
    private const string PATH_CONFIG = "Client DataBase Config";
    private const string PATH_SCRIPTABLE_ASSET_ROOT = "ClientDataBase/";

    private ClientDataBaseConfig m_config;
    public ClientDataBaseConfig Config
    {
        get
        {
            if (m_config == null)
                m_config = Utility.AssetRelate.ResourcesLoadCheckNull<ClientDataBaseConfig>(PATH_CONFIG);

            return m_config;
        }
    }

    private Dictionary<Type, ScriptableObjectBase> m_mapTable = new Dictionary<Type, ScriptableObjectBase>();


    void Register<T>() where T : ScriptableObjectBase
    {
        m_mapTable.Add(typeof(T), LoadTable(typeof(T)));
    }

    ScriptableObjectBase LoadTable(Type type)
    {
        return Utility.AssetRelate.ResourcesLoadCheckNull<ScriptableObjectBase>(PATH_SCRIPTABLE_ASSET_ROOT + Config.GetScriptableAssetNameFromType(type));
    }


    public T GetTable<T>() where T : ScriptableObjectBase
    {
        if (m_mapTable.ContainsKey(typeof(T)))
        {
            return (T)m_mapTable[typeof(T)];
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

