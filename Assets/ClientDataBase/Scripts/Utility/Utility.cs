/**********************************************************
// Author   : Arkai (k79k06k02k)
// FileName : Utility.cs
**********************************************************/
using UnityEngine;

namespace ClientDataBase
{
    public class Utility
    {
        public struct TypeRelate
        {
            public static bool StringToBool(string value)
            {
                if (value == "T")
                    return true;
                else if (value == "F")
                    return false;
                else
                {
                    Debug.LogError(string.Format("Unable to convert value:[{0}]", value));
                    return false;
                }
            }
            public static Vector2 StringToVector2(string value)
            {
                string[] str = value.Split(',');
                return new Vector2(float.Parse(str[0]), float.Parse(str[1]));
            }
            public static Vector3 StringToVector3(string value)
            {
                string[] str = value.Split(',');
                return new Vector3(float.Parse(str[0]), float.Parse(str[1]), float.Parse(str[2]));
            }
        }


        public struct AssetRelate
        {
            /// <summary>
            /// Resources.Load 並檢查是否null
            /// </summary>
            public static T ResourcesLoadCheckNull<T>(string name) where T : UnityEngine.Object
            {
                T loadGo = Resources.Load<T>(name);

                if (loadGo == null)
                {
                    Debug.LogError("Resources.Load [ " + name + " ] is Null !!");
                    return default(T);
                }

                return loadGo;
            }
        }
    }
}