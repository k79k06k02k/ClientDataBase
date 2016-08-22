/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseEditorWindow.cs
**********************************************************/
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class ClientDataBaseEditorWindow : EditorWindow
{
    //按下執行按鈕
    bool isExecuteButtonClick = false;
    float waitForExecute = 0.1f;

    //一次執行的開始
    bool isStartCreate = false;

    //是否是更新全部
    bool isUpdateAll = false;

    //等待 Application Compiling Script
    bool isNeedToAttach = false;
    float waitForCompile = 1;

    string[] Types = new string[] { "Create", "Update" };
    int tabIndex = 0;

    List<Object> objList;
    int nowCount = 0;
    int totalCount = 0;

    Vector2 scrollPos;
    GUIStyle BtnStyle;

    [MenuItem("KKTools/Client DataBase Tool")]
    [MenuItem("Assets/Client DataBase Tool", false, 110)]
    public static void ShowWindow()
    {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(ClientDataBaseEditorWindow));
        editorWindow.position = new Rect(editorWindow.position.xMin + 100f, editorWindow.position.yMin + 100f, 400f, 400f);
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.titleContent = new GUIContent("Client DataBase Tool");
    }

    [MenuItem("KKTools/Client DataBase Update All")]
    [MenuItem("Assets/Client DataBase Update All", false, 110)]
    public static void UpdateAll()
    {
        ClientDataBaseEditorWindow window = EditorWindow.GetWindow<ClientDataBaseEditorWindow>();
        window.isUpdateAll = true;
        window.isStartCreate = true;
        window.isExecuteButtonClick = true;
        window.objList = UtilityEditor.LoadAllAssetsAtPath(ClientDataBaseConfig.GameTablePath).ToList();
        window.CreateAsset();
    }


    void Update()
    {
        //等待 Application Compiling Script，建立 ScriptableObject Asset
        if (isNeedToAttach)
        {
            waitForCompile -= 0.01f;

            if (waitForCompile <= 0)
            {
                if (!EditorApplication.isCompiling)
                {
                    foreach (Object go in objList)
                    {
                        string path = AssetDatabase.GetAssetPath(go);
                        string fileName = Path.GetFileNameWithoutExtension(path);
                        string scriptableScriptName = ClientDataBaseConfig.GetScriptableScriptName(fileName, true);
                        string scriptableAssetName = ClientDataBaseConfig.GetScriptableAssetName(fileName, true);

                        nowCount++;
                        UpdateProgressBar("Generate Scriptable Assets", string.Format("[File Name:{0}]", scriptableAssetName));

                        if (ClientDataBaseParse.Instance.CreateScriptableAssets(scriptableScriptName, scriptableAssetName) == false)
                            continue;
                    }

                    isStartCreate = false;
                    isNeedToAttach = false;
                    waitForCompile = 1;
                    EditorUtility.ClearProgressBar();

                    if (isUpdateAll)
                        this.Close();
                }
            }
        }

        //點下按鈕後，延遲執行，先讓 Loading 畫面出來
        if (isExecuteButtonClick)
        {
            waitForExecute -= 0.01f;
            if (waitForExecute <= 0)
            {
                Execute();

                isExecuteButtonClick = false;
                waitForExecute = 0.1f;
            }
        }
    }

    void OnSelectionChange()
    {
        Repaint();
    }

    void OnGUI()
    {
        BtnStyle = new GUIStyle(GUI.skin.button);
        BtnStyle.fontSize = 16;
        BtnStyle.alignment = TextAnchor.MiddleLeft;

        //遮罩
        if (isStartCreate)
            GUI.enabled = false;


        tabIndex = UtilityEditor.Tabs(Types, tabIndex);
        GUILayout.Space(10);

        //還沒開始時才需要抓物件
        if (isStartCreate == false && isUpdateAll == false)
            objList = Selection.objects.ToList();


        //排除
        for (int i = objList.Count - 1; i >= 0; i--)
        {
            if (GetFiltered(objList[i]))
                objList.Remove(objList[i]);
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Choose GameTable Asset", EditorStyles.boldLabel, GUILayout.Width(200));
        EditorGUILayout.LabelField("Count : " + objList.Count, EditorStyles.boldLabel, GUILayout.Width(100));
        if (GUILayout.Button("Update All"))
        {
            UpdateAll();
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        if (objList.Count == 0)
        {
            EditorGUILayout.HelpBox(GetHelpString(), MessageType.Warning);
            return;
        }

        //字母排序
        objList.Sort(delegate(Object a, Object b)
        {
            return a.name.CompareTo(b.name);
        });

        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        EditorGUILayout.Space();

        foreach (Object go in objList)
        {
            if (GUILayout.Button(go.name, BtnStyle))
                EditorGUIUtility.PingObject(go);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        if (UtilityEditor.GetCommonButton(Types[tabIndex]))
        {
            if (EditorApplication.isCompiling)
            {
                Debug.LogError("After wait application compiling then try again.");
                return;
            }

            isStartCreate = true;
            isExecuteButtonClick = true;
        }

        if (isStartCreate)
        {
            GUI.enabled = true;
            UtilityEditor.ShowLoading();
        }

    }

    void OnDestroy()
    {
        if (isNeedToAttach)
            Debug.LogError("Please wait complete, Or may cause a crash...");

        EditorUtility.ClearProgressBar();
    }

    void Execute()
    {
        switch (tabIndex)
        {
            case 0:
                CreateAsset();
                break;

            case 1:
                UpdateAsset();
                break;
        }
    }

    void CreateAsset()
    {
        //乘2是因為把 Script 與 Scriptable Asset 分開處理，必須先等 Application Compiling 完，才找的到 Scriptable Class，最後才能透過 Class 建立 Scriptable Asset 
        //加1是等 Application Compiling 的區間
        totalCount = objList.Count * 2 + 1;

        nowCount = 0;

        foreach (Object go in objList)
        {
            string path = AssetDatabase.GetAssetPath(go);

            nowCount++;

            string fileName = Path.GetFileName(path);

            UpdateProgressBar("Generate GameTable Script", string.Format("[File Name:{0}]", fileName));

            if (ClientDataBaseParse.Instance.LoadGameTable(go) == false)
                continue;
        }

        nowCount++;
        UpdateProgressBar("Please Wait", "Wait Application Compiling....");
        isNeedToAttach = true;
    }

    void UpdateAsset()
    {
        foreach (Object go in objList)
        {
            ScriptableObjectBase script = (ScriptableObjectBase)go;

            if (script.LoadGameTable() == false)
                continue;
        }
        isStartCreate = false;
    }


    bool GetFiltered(Object obj)
    {
        string path = AssetDatabase.GetAssetPath(obj);
        string extension = Path.GetExtension(path);

        switch (tabIndex)
        {
            case 0:
                return extension != ClientDataBaseConfig.FILE_EXTENSION_TXT;

            case 1:
                return extension != ClientDataBaseConfig.FILE_EXTENSION_ASSET;

            default:
                return true;
        }
    }


    string GetHelpString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("No Source." + "\n\n");
        sb.Append("Please Check Select Asset：" + "\n");

        switch (tabIndex)
        {
            case 0:
                sb.Append("1.Asset in Project" + "\n");
                sb.Append("2.Asset extension must [" + ClientDataBaseConfig.FILE_EXTENSION_TXT + "]");
                break;

            case 1:
                sb.Append("1.Asset in Project" + "\n");
                sb.Append("2.Asset must ScriptableObject Asset");
                break;

            default:
                break;
        }

        return sb.ToString();
    }


    void UpdateProgressBar(string title, string info)
    {
        float process = nowCount / (float)totalCount;
        EditorUtility.DisplayProgressBar(title, string.Format("[{0}%] {1}", (int)(process * 100), info), process);
    }
}
