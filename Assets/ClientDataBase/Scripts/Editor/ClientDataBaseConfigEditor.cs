using UnityEngine;

namespace ClientDataBase.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(ClientDataBaseConfig))]
    public class ClientDataBaseConfigEditor : Editor
    {
        private ClientDataBaseConfig m_script;

        public override void OnInspectorGUI()
        {
            m_script = target as ClientDataBaseConfig;

            GUILayout.Space(15);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Check", EditorStyles.boldLabel);
            m_script.gameTableCheck = DrawNormalField("Game Table Check", m_script.gameTableCheck);
            EditorGUILayout.EndVertical();


            GUILayout.Space(15);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Path", EditorStyles.boldLabel);
            m_script.root = DrawNormalField("ROOT", m_script.root);
            m_script.pathScriptTemplates = DrawPathField("Script Templates Path", m_script.pathScriptTemplates);
            m_script.pathGameTable = DrawPathField("Game Table Path", m_script.pathGameTable);
            m_script.pathTableClass = DrawPathField("Table Class Path", m_script.pathTableClass);
            m_script.pathScriptableAsset = DrawPathField("Scriptable Asset Path", m_script.pathScriptableAsset);
            m_script.pathScriptableScripts = DrawPathField("Scriptable Scripts Path", m_script.pathScriptableScripts);
            m_script.pathScriptableEditor = DrawPathField("Scriptable Editor Path", m_script.pathScriptableEditor);
            EditorGUILayout.EndVertical();


            GUILayout.Space(15);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Name", EditorStyles.boldLabel);
            m_script.nameClassPrefix = DrawNameField("Class Name Prefix", "Preview: " + m_script.nameClassPrefix + "[FileName]", m_script.nameClassPrefix);
            m_script.nameScriptableAssetSuffix = DrawNameField("Scriptable Asset Suffix", "Preview: " + m_script.nameClassPrefix + "[FileName]" + m_script.nameScriptableAssetSuffix, m_script.nameScriptableAssetSuffix);
            m_script.nameScriptableScriptSuffix = DrawNameField("Scriptable Script Suffix", "Preview: " + m_script.nameClassPrefix + "[FileName]" + m_script.nameScriptableScriptSuffix, m_script.nameScriptableScriptSuffix);
            m_script.nameScriptableEditorSuffix = DrawNameField("Scriptable Editor Suffix", "Preview: " + m_script.nameClassPrefix + "[FileName]" + m_script.nameScriptableEditorSuffix, m_script.nameScriptableEditorSuffix);
            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty(m_script);
        }

        string DrawNormalField(string name, string field)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.Width(160));
            field = EditorGUILayout.TextField(field);
            EditorGUILayout.EndHorizontal();

            return field;
        }

        string DrawPathField(string name, string field)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.Width(160));
            EditorGUILayout.LabelField("ROOT +", GUILayout.Width(50));
            field = EditorGUILayout.TextField(field);
            EditorGUILayout.EndHorizontal();

            return field;
        }

        string DrawNameField(string name, string preview, string field)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(name, GUILayout.Width(160));
            field = EditorGUILayout.TextField(field);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(160));
            EditorGUILayout.LabelField(preview);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);


            return field;
        }

    }
}