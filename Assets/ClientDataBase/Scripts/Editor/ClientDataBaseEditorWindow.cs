using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ClientDataBase.Editor
{
    public class ClientDataBaseEditorWindow : EditorWindow
    {
        //按下執行按鈕
        private bool m_isExecuteButtonClick = false;
        private float m_waitForExecute = 0.1f;

        //一次執行的開始
        private bool m_startCreate = false;

        //是否是更新全部
        private bool m_updateAll = false;

        //等待 Application Compiling Script
        private bool m_needToAttach = false;
        private float m_waitForCompile = 1;

        private string[] m_types = new string[] { "Create", "Update" };
        private int m_tabIndex = 0;

        private List<Object> m_objList;
        private int m_nowCount = 0;
        private int m_totalCount = 0;

        private Vector2 m_scrollPos;
        private GUIStyle m_btnStyle;

        [MenuItem("Assets/Client DataBase/Window", false, 110)]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = EditorWindow.GetWindow(typeof(ClientDataBaseEditorWindow));
            editorWindow.position = new Rect(editorWindow.position.xMin + 100f, editorWindow.position.yMin + 100f, 400f, 400f);
            editorWindow.autoRepaintOnSceneChange = true;
            editorWindow.Show();
            editorWindow.titleContent = new GUIContent("Client DataBase Tool");
        }

        [MenuItem("Assets/Client DataBase/Update All", false, 120)]
        public static void UpdateAll()
        {
            ClientDataBaseEditorWindow window = EditorWindow.GetWindow<ClientDataBaseEditorWindow>();
            window.m_objList = UtilityEditor.LoadAllAssetsAtPath(ClientDataBaseManager.Instance.Config.GetGameTablePath()).ToList();

            if (window.m_objList.Count == 0)
            {
                Debug.Log("No GameTable file (.txt)");
                window.Close();
                return;
            }

            window.m_updateAll = true;
            window.m_startCreate = true;
            window.m_isExecuteButtonClick = true;
        }

        void Update()
        {
            //等待 Application Compiling Script，建立 ScriptableObject Asset
            if (m_needToAttach)
            {
                m_waitForCompile -= 0.01f;

                if (m_waitForCompile <= 0)
                {
                    if (!EditorApplication.isCompiling)
                    {
                        foreach (Object go in m_objList)
                        {
                            string path = AssetDatabase.GetAssetPath(go);
                            string fileName = Path.GetFileNameWithoutExtension(path);
                            string scriptableScriptName = ClientDataBaseManager.Instance.Config.GetScriptableScriptName(fileName, true);
                            string scriptableAssetName = ClientDataBaseManager.Instance.Config.GetScriptableAssetName(fileName, true);

                            m_nowCount++;
                            UpdateProgressBar("Generate Scriptable Assets", string.Format("[File Name:{0}]", scriptableAssetName));

                            if (ClientDataBaseParse.Instance.CreateScriptableAssets(scriptableScriptName, scriptableAssetName) == false)
                                continue;
                        }

                        m_startCreate = false;
                        m_needToAttach = false;
                        m_waitForCompile = 1;
                        EditorUtility.ClearProgressBar();

                        if (m_updateAll)
                            this.Close();
                    }
                }
            }

            //點下按鈕後，延遲執行，先讓 Loading 畫面出來
            if (m_isExecuteButtonClick)
            {
                m_waitForExecute -= 0.01f;
                if (m_waitForExecute <= 0)
                {
                    Execute();

                    m_isExecuteButtonClick = false;
                    m_waitForExecute = 0.1f;
                }
            }
        }

        void OnSelectionChange()
        {
            Repaint();
        }

        void OnGUI()
        {
            m_btnStyle = new GUIStyle(GUI.skin.button);
            m_btnStyle.fontSize = 16;
            m_btnStyle.alignment = TextAnchor.MiddleLeft;

            //遮罩
            if (m_startCreate)
                GUI.enabled = false;


            m_tabIndex = UtilityEditor.Tabs(m_types, m_tabIndex);
            GUILayout.Space(10);

            //還沒開始時才需要抓物件
            if (m_startCreate == false && m_updateAll == false)
                m_objList = Selection.objects.ToList();


            //排除
            for (int i = m_objList.Count - 1; i >= 0; i--)
            {
                if (GetFiltered(m_objList[i]))
                    m_objList.Remove(m_objList[i]);
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Choose GameTable Asset", EditorStyles.boldLabel, GUILayout.Width(200));
            EditorGUILayout.LabelField("Count : " + m_objList.Count, EditorStyles.boldLabel, GUILayout.Width(100));
            if (GUILayout.Button("Update All"))
            {
                UpdateAll();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

            if (m_objList.Count == 0)
            {
                EditorGUILayout.HelpBox(GetHelpString(), MessageType.Warning);
                return;
            }

            //字母排序
            m_objList.Sort(delegate (Object a, Object b)
            {
                return a.name.CompareTo(b.name);
            });

            EditorGUILayout.BeginVertical();
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, false, false);
            EditorGUILayout.Space();

            foreach (Object go in m_objList)
            {
                if (GUILayout.Button(go.name, m_btnStyle))
                    EditorGUIUtility.PingObject(go);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            if (UtilityEditor.GetCommonButton(m_types[m_tabIndex]))
            {
                if (EditorApplication.isCompiling)
                {
                    Debug.LogError("After wait application compiling then try again.");
                    return;
                }

                m_startCreate = true;
                m_isExecuteButtonClick = true;
            }

            if (m_startCreate)
            {
                GUI.enabled = true;
                UtilityEditor.ShowLoading();
            }

        }

        void OnDestroy()
        {
            if (m_needToAttach)
                Debug.LogError("Please wait complete, Or may cause a crash...");

            EditorUtility.ClearProgressBar();
        }

        void Execute()
        {
            switch (m_tabIndex)
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
            m_totalCount = m_objList.Count * 2 + 1;

            m_nowCount = 0;

            foreach (Object go in m_objList)
            {
                string path = AssetDatabase.GetAssetPath(go);

                m_nowCount++;

                string fileName = Path.GetFileName(path);

                UpdateProgressBar("Generate GameTable Script", string.Format("[File Name:{0}]", fileName));

                if (ClientDataBaseParse.Instance.LoadGameTable(go) == false)
                    continue;
            }

            m_nowCount++;
            UpdateProgressBar("Please Wait", "Wait Application Compiling....");
            m_needToAttach = true;
        }

        void UpdateAsset()
        {
            foreach (Object go in m_objList)
            {
                ScriptableObjectBase script = (ScriptableObjectBase)go;

                if (script.LoadGameTable() == false)
                    continue;
            }
            m_startCreate = false;
        }


        bool GetFiltered(Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            string extension = Path.GetExtension(path);
            string content = File.ReadAllText(path);

            switch (m_tabIndex)
            {
                case 0:
                    return extension != ClientDataBaseManager.Instance.Config.extensionTsv || content.StartsWith(ClientDataBaseManager.Instance.Config.gameTableCheck) == false;

                case 1:
                    return extension != ClientDataBaseManager.Instance.Config.extensionAsset || obj.name == ClientDataBaseManager.Instance.Config.name;

                default:
                    return true;
            }
        }


        string GetHelpString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("No Source." + "\n\n");
            sb.Append("Please Check Select Asset：" + "\n");

            switch (m_tabIndex)
            {
                case 0:
                    sb.Append("1.Asset in Project" + "\n");
                    sb.Append("2.Asset extension must [" + ClientDataBaseManager.Instance.Config.extensionTsv + "]" + "\n");
                    sb.Append("3.Asset content must table.");
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
            float process = m_nowCount / (float)m_totalCount;
            EditorUtility.DisplayProgressBar(title, string.Format("[{0}%] {1}", (int)(process * 100), info), process);
        }
    }
}