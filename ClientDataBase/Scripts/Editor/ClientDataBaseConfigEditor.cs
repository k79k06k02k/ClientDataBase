/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseConfigEditor.cs
**********************************************************/
using UnityEngine;
using UnityEditor;

namespace ClientDataBase
{
    [CustomEditor(typeof(ClientDataBaseConfig))]
    public class ClientDataBaseConfigEditor : Editor
    {
        ClientDataBaseConfig _script;

        public override void OnInspectorGUI()
        {
            _script = target as ClientDataBaseConfig;

            GUILayout.Space(15);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Check", EditorStyles.boldLabel);
            _script.m_gameTableCheck = DrawNormalField("Game Table Check", _script.m_gameTableCheck);
            EditorGUILayout.EndVertical();


            GUILayout.Space(15);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Path", EditorStyles.boldLabel);
            _script.m_root = DrawNormalField("ROOT", _script.m_root);
            _script.m_pathScriptTemplates = DrawPathField("Script Templates Path", _script.m_pathScriptTemplates);
            _script.m_pathGameTable = DrawPathField("Game Table Path", _script.m_pathGameTable);
            _script.m_pathTableClass = DrawPathField("Table Class Path", _script.m_pathTableClass);
            _script.m_pathScriptableAsset = DrawPathField("Scriptable Asset Path", _script.m_pathScriptableAsset);
            _script.m_pathScriptableScripts = DrawPathField("Scriptable Scripts Path", _script.m_pathScriptableScripts);
            _script.m_pathScriptableEditor = DrawPathField("Scriptable Editor Path", _script.m_pathScriptableEditor);
            EditorGUILayout.EndVertical();


            GUILayout.Space(15);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Name", EditorStyles.boldLabel);
            _script.m_nameClassPrefix = DrawNameField("Class Name Prefix", "Preview: " + _script.m_nameClassPrefix + "[FileName]", _script.m_nameClassPrefix);
            _script.m_nameScriptableAssetSuffix = DrawNameField("Scriptable Asset Suffix", "Preview: " + _script.m_nameClassPrefix + "[FileName]" + _script.m_nameScriptableAssetSuffix, _script.m_nameScriptableAssetSuffix);
            _script.m_nameScriptableScriptSuffix = DrawNameField("Scriptable Script Suffix", "Preview: " + _script.m_nameClassPrefix + "[FileName]" + _script.m_nameScriptableScriptSuffix, _script.m_nameScriptableScriptSuffix);
            _script.m_nameScriptableEditorSuffix = DrawNameField("Scriptable Editor Suffix", "Preview: " + _script.m_nameClassPrefix + "[FileName]" + _script.m_nameScriptableEditorSuffix, _script.m_nameScriptableEditorSuffix);
            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty(_script);
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