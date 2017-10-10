using System;
using UnityEngine;

namespace ClientDataBase
{
    public class Utilities
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
            public static float StringToFloat(string value)
            {
                string temp = string.Empty;
                if (value.Contains("%"))
                {
                    return (float)Convert.ChangeType(value.Replace("%", ""), typeof(float)) * 0.01f;
                }
                else
                {
                    return (float)Convert.ChangeType(value, typeof(float));
                }
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