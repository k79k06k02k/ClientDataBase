/**********************************************************
// Author   : Arkai (k79k06k02k)
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
            _script.gameTableCheck = DrawNormalField("Game Table Check", _script.gameTableCheck);
            EditorGUILayout.EndVertical();


            GUILayout.Space(15);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Path", EditorStyles.boldLabel);
            _script.root = DrawNormalField("ROOT", _script.root);
            _script.pathScriptTemplates = DrawPathField("Script Templates Path", _script.pathScriptTemplates);
            _script.pathGameTable = DrawPathField("Game Table Path", _script.pathGameTable);
            _script.pathTableClass = DrawPathField("Table Class Path", _script.pathTableClass);
            _script.pathScriptableAsset = DrawPathField("Scriptable Asset Path", _script.pathScriptableAsset);
            _script.pathScriptableScripts = DrawPathField("Scriptable Scripts Path", _script.pathScriptableScripts);
            _script.pathScriptableEditor = DrawPathField("Scriptable Editor Path", _script.pathScriptableEditor);
            EditorGUILayout.EndVertical();


            GUILayout.Space(15);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField("Name", EditorStyles.boldLabel);
            _script.nameClassPrefix = DrawNameField("Class Name Prefix", "Preview: " + _script.nameClassPrefix + "[FileName]", _script.nameClassPrefix);
            _script.nameScriptableAssetSuffix = DrawNameField("Scriptable Asset Suffix", "Preview: " + _script.nameClassPrefix + "[FileName]" + _script.nameScriptableAssetSuffix, _script.nameScriptableAssetSuffix);
            _script.nameScriptableScriptSuffix = DrawNameField("Scriptable Script Suffix", "Preview: " + _script.nameClassPrefix + "[FileName]" + _script.nameScriptableScriptSuffix, _script.nameScriptableScriptSuffix);
            _script.nameScriptableEditorSuffix = DrawNameField("Scriptable Editor Suffix", "Preview: " + _script.nameClassPrefix + "[FileName]" + _script.nameScriptableEditorSuffix, _script.nameScriptableEditorSuffix);
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