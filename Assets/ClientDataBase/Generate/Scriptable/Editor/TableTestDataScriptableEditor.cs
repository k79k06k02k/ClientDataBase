/*****************************************************************************/
/****************** Auto Generate Script, Do Not Modify! *********************/
/*****************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TableTestDataScriptable))]
public class TableTestDataScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TableTestDataScriptable script = (TableTestDataScriptable)target;

        if (GUILayout.Button("Update"))
			script.LoadGameTable();

        GUILayout.Space(20);

        DrawDefaultInspector();
    }
}