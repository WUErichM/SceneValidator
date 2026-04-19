using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SceneValidatorWindow : EditorWindow
{
    private List<ScanResult> results = new List<ScanResult>();
    private Vector2 scroll;

    [MenuItem("Tools/Scene Validator")]
    public static void Open()
    {
        GetWindow<SceneValidatorWindow>("Scene Validator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Validator (Beta)", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Scan Scene"))
        {
            results = SceneScanUtility.ScanScene();
        }

        if (GUILayout.Button("Export Report"))
        {
            ExportReport();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.Label("Issues Found: " + results.Count, EditorStyles.boldLabel);

        scroll = GUILayout.BeginScrollView(scroll);

        foreach (var r in results)
        {
            GUILayout.BeginVertical("box");

            GUI.color = GetColor(r.type);

            if (GUILayout.Button(r.obj.name))
            {
                Selection.activeGameObject = r.obj;
            }

            GUI.color = Color.white;

            GUILayout.Label(r.issue);

            GUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }

    private Color GetColor(IssueType type)
    {
        switch (type)
        {
            case IssueType.MissingScript:
                return Color.red;
            case IssueType.MissingReference:
                return Color.yellow;
            case IssueType.InactiveObject:
                return Color.cyan;
            default:
                return Color.white;
        }
    }

    private void ExportReport()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Scene Validator Report");
        sb.AppendLine("=====================");

        foreach (var r in results)
        {
            sb.AppendLine(r.obj.name + " - " + r.issue);
        }

        string path = Application.dataPath + "/SceneValidatorReport.txt";
        File.WriteAllText(path, sb.ToString());

        Debug.Log("Report exported to: " + path);
        AssetDatabase.Refresh();
    }
}