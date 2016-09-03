/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ClientDataBaseConfigEditor.cs
**********************************************************/
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClientDataBaseConfig))]
public class ClientDataBaseConfigEditor : Editor
{
    ClientDataBaseConfig script;

    public override void OnInspectorGUI()
    {
        script = target as ClientDataBaseConfig;

        GUILayout.Space(15);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Check", EditorStyles.boldLabel);
        script.GameTableCheck = DrawNormalField("Game Table Check", script.GameTableCheck);
        EditorGUILayout.EndVertical();


        GUILayout.Space(15);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Path", EditorStyles.boldLabel);
        script.ROOT = DrawNormalField("ROOT", script.ROOT);
        script.ScriptTemplatesPath = DrawPathField("Script Templates Path", script.ScriptTemplatesPath);
        script.GameTablePath = DrawPathField("Game Table Path", script.GameTablePath);
        script.TableClassPath = DrawPathField("Table Class Path", script.TableClassPath);
        script.ScriptableAssetPath = DrawPathField("Scriptable Asset Path", script.ScriptableAssetPath);
        script.ScriptableScriptsPath = DrawPathField("Scriptable Scripts Path", script.ScriptableScriptsPath);
        script.ScriptableEditorPath = DrawPathField("Scriptable Editor Path", script.ScriptableEditorPath);
        EditorGUILayout.EndVertical();


        GUILayout.Space(15);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Name", EditorStyles.boldLabel);
        script.ClassNamePrefix = DrawNameField("Class Name Prefix", "Preview: " + script.ClassNamePrefix + "[FileName]", script.ClassNamePrefix);
        script.ScriptableScriptSuffix = DrawNameField("Scriptable Script Suffix", "Preview: " + script.ClassNamePrefix + "[FileName]" + script.ScriptableScriptSuffix, script.ScriptableScriptSuffix);
        script.ScriptableAssetSuffix = DrawNameField("Scriptable Asset Suffix", "Preview: " + script.ClassNamePrefix + "[FileName]" + script.ScriptableAssetSuffix, script.ScriptableAssetSuffix);
        script.ScriptableEditorSuffix = DrawNameField("Scriptable Editor Suffix", "Preview: " + script.ClassNamePrefix + "[FileName]" + script.ScriptableEditorSuffix, script.ScriptableEditorSuffix);
        EditorGUILayout.EndVertical();


        GUILayout.Space(15);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Extension", EditorStyles.boldLabel);
        script.FileExtensionTXT = DrawNormalField("File Extension TXT", script.FileExtensionTXT);
        script.FileExtensionCS = DrawNormalField("File Extension CS", script.FileExtensionCS);
        script.FileExtensionASSET = DrawNormalField("File Extension ASSET", script.FileExtensionASSET);
        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(script);
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
