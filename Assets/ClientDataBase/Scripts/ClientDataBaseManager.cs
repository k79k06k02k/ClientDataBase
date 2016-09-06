/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseManager.cs
**********************************************************/
using System.Collections.Generic;
using System;

public class ClientDataBaseManager : Singleton<ClientDataBaseManager>
{
    ClientDataBaseConfig _Config;
    public ClientDataBaseConfig m_Config
    {
        get
        {
            if (_Config == null)
                _Config = Utility.AssetRelate.ResourcesLoadCheckNull<ClientDataBaseConfig>("Client DataBase Config");

            return _Config;
        }
    }


    public Dictionary<Type, ScriptableObjectBase> m_TableList = new Dictionary<Type, ScriptableObjectBase>();

    public ClientDataBaseManager()
    {
        //Register(typeof(TableSampleScriptable), LoadTable(TableSampleScriptable.GameTableName));
    }

    ScriptableObjectBase LoadTable(string fileName)
    {
        return Utility.AssetRelate.ResourcesLoadCheckNull<ScriptableObjectBase>("ClientDataBase/" + m_Config.GetScriptableAssetName(fileName));
    }

    void Register(Type type, ScriptableObjectBase dataBase)
    {
        m_TableList.Add(type, dataBase);
    }

    public T GetTable<T>() where T : ScriptableObjectBase, new()
    {
        if (m_TableList.ContainsKey(typeof(T)))
        {
            return (T)m_TableList[typeof(T)];
        }

        return default(T);
    }
}
