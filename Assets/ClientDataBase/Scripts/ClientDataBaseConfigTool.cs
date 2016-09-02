/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseConfigTool.cs
**********************************************************/
using UnityEditor;

public class ClientDataBaseConfigTool : Singleton<ClientDataBaseConfigTool>
{
    public ClientDataBaseConfig m_Config
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<ClientDataBaseConfig>("Assets/ClientDataBase/Client DataBase Config.asset");
        }
    }

}
