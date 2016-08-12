/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : UtilityEditor.cs
**********************************************************/
using UnityEngine;
using UnityEditor;

public class UtilityEditor
{
    /// <summary>
    /// 建立資料夾，自動建子資料夾 
    /// EX: GameResources/Prefabs/Sprites/Enemy
    /// </summary>
    /// <param name="name">路徑層級，從 Assets 下一層資料夾開始</param>
    /// <returns>資料夾是否存在</returns>
    public static bool CreateFolder(string name)
    {
        //路徑字串開頭如果是 Assets，進行剃除
        if (name.StartsWith("Assets/"))
            name = name.Replace("Assets/", "");

        //路徑字串最後如果有 / ，進行剃除
        if (name[name.Length - 1] == '/')
            name = name.Substring(0, name.Length - 1);


        if (System.IO.Directory.Exists(Application.dataPath + "/" + name))
        {
            Debug.Log("Folder [ Assets/" + name + " ] is Exist!!");
            return false;
        }
        else
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/" + name);
            AssetDatabase.Refresh();

            Debug.Log("Folder [ Assets/" + name + " ] is Create!!");
            return true;
        }
    }


    /// <summary>
    /// 通用按鈕
    /// </summary>
    /// <param name="btnName">按鈕名稱</param>
    /// <returns>是否按下</returns>
    public static bool GetCommonButton(string btnName)
    {
        GUIStyle BtnStyle = new GUIStyle(GUI.skin.button);
        BtnStyle.fontSize = 25;
        BtnStyle.fixedHeight = 50;

        return GUILayout.Button(btnName, BtnStyle);
    }


    /// <summary>
    /// 橫向頁籤
    /// </summary>
    /// <param name="options">名稱</param>
    /// <param name="selected">選擇的index</param>
    /// <returns>選擇的index</returns>
    public static int Tabs(string[] options, int selected)
    {
        const float DarkGray = 0.6f;
        const float LightGray = 0.9f;
        const float StartSpace = 10;

        GUILayout.Space(StartSpace);
        Color storeColor = GUI.backgroundColor;
        Color highlightCol = new Color(LightGray, LightGray, LightGray);
        Color bgCol = new Color(DarkGray, DarkGray, DarkGray);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.padding.bottom = 8;

        GUILayout.BeginHorizontal();
        {
            for (int i = 0; i < options.Length; ++i)
            {
                GUI.backgroundColor = i == selected ? highlightCol : bgCol;
                if (GUILayout.Button(options[i], buttonStyle))
                {
                    selected = i;
                }
            }
        }
        GUILayout.EndHorizontal();

        GUI.backgroundColor = storeColor;

        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, highlightCol);
        texture.Apply();
        GUI.DrawTexture(new Rect(0, buttonStyle.lineHeight + buttonStyle.border.top + buttonStyle.margin.top + StartSpace, Screen.width, 4), texture);

        return selected;
    }

    /// <summary>
    /// 顯示Loading畫面
    /// </summary>
    public static void ShowLoading()
    {
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
        texture.Apply();
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);

        GUIStyle LableStyle = new GUIStyle(GUI.skin.label);
        LableStyle.fontSize = 40;
        LableStyle.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Loading...", LableStyle);
    }

}

