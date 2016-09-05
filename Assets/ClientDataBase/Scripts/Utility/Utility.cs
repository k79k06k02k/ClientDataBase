﻿/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : Utility.cs
**********************************************************/
using UnityEngine;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Reflection;
using System.Security.Cryptography;

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
}