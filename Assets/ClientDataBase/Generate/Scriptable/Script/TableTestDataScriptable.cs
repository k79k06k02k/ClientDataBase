/*****************************************************************************/
/****************** Auto Generate Script, Do Not Modify! *********************/
/*****************************************************************************/
using System;
using UnityEngine;
using ClientDataBase;
using System.Text;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

[Serializable]
public class DictionaryTableTestData : SerializableDictionary<int, TableTestData> { }


public class TableTestDataScriptable : ScriptableObjectBase
{
    public static string m_gameTableName = "TestData";
	public DictionaryTableTestData m_tableDict = new DictionaryTableTestData();

	ClientDataBaseConfig _clientDataBaseConfig;

#if UNITY_EDITOR
	string path;

	public override bool LoadGameTable()
    {        
		m_tableDict.Clear();

		_clientDataBaseConfig = ClientDataBaseManager.Instance.m_config;
        path = _clientDataBaseConfig.GetGameTablePathName(m_gameTableName);

        TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        StringReader reader = null;
        int index = 0;
        string strTemp;

        if (data == null)
        {
            Debug.LogError(string.Format("Can't found GameTable txt file in [Path:{0}]", path));
            return false;
        }

        reader = new StringReader(data.text);

        if (reader != null)
        {
            while ((strTemp = reader.ReadLine()) != null)
			{        
				if (index <= 2)
				{
					index++;
					continue;
				}
			        
                string[] splitStr = strTemp.Split("\t"[0]);

				if ((int)Convert.ChangeType(splitStr[0], typeof(int)) == 0)
                {
                    continue;
                }

				TableTestData table = new TableTestData();
				try
                {
					table.id = (int)Convert.ChangeType(splitStr[1], typeof(int));
					table.name = (string)Convert.ChangeType(splitStr[2], typeof(string));
					table.skill = new int[] { (int)Convert.ChangeType(splitStr[3], typeof(int)), (int)Convert.ChangeType(splitStr[4], typeof(int)) };
					table.ability1 = new float[] { (float)Convert.ChangeType(splitStr[5], typeof(float)) };
					table.ability2 = new int[] { (int)Convert.ChangeType(splitStr[6], typeof(int)), (int)Convert.ChangeType(splitStr[7], typeof(int)) };

                    string[] temp = ((string)Convert.ChangeType(splitStr[2], typeof(string))).Split(',');
                    string[] test = new string[temp.Length];
                    for(int i=0;i<temp.Length;i++)
                    {
                        test[i]= (string)Convert.ChangeType(splitStr[3], typeof(string));
                    }
                        
                    
				}
				catch (FormatException e)
                {
                    string obvious = string.Empty;
                    string[] log = e.StackTrace.Split('\n');
                    for (int i = 0; i < log.Length; i++)
                    {
                        if (log[i].IndexOf(this.GetType().Name) > 0)
                        {
                            obvious = log[i];
                            break;
                        }
                    }

                    Debug.LogError(string.Format("{0} \n<b>{1}</b> \n\nStackTrace:{2}", e.Message, obvious, e.StackTrace));
                    return false;
                }

                m_tableDict.Add(table.id, table);

                index++;
            }

            reader.Close();
        }

		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssets();

        Debug.Log(string.Format("[{0}] GameTable Asset is Update. Source:[{1}]", this.name, path));
		return true;
    }

#else
    public override bool LoadGameTable() { return false; }
#endif

    public TableTestData GetData(int id)
    {
        if (m_tableDict.ContainsKey(id))
        {
            return m_tableDict[id];
        }
        else
        {
            Debug.LogWarning(string.Format("[{0}] GameTable Asset Dictionary Can't found key [{1}]", this.name, id));
            return null;
        }
    }
}