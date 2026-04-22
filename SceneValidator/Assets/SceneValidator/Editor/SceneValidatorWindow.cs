using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public class SceneValidatorWindow : EditorWindow
{
    private List<ScanResult> results = new List<ScanResult>();
    private Vector2 scroll;

    private bool showErrors = true;
    private bool showWarnings = true;
    private bool showInfo = true;

    [MenuItem("Tools/Scene Validator")]
    public static void Open()
    {
        GetWindow<SceneValidatorWindow>("Scene Validator v1.0");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Validator v1.0", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Scan Scene"))
        {
            results = SceneScanUtility.ScanScene();
        }

        if (GUILayout.Button("Fix All (Safe)"))
        {
            SceneScanUtility.FixAll(results);
            results = SceneScanUtility.ScanScene();
        }

        if (GUILayout.Button("Export Report"))
        {
            ExportReport();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.Label("Filters", EditorStyles.boldLabel);

        showErrors = GUILayout.Toggle(showErrors, "Errors");
        showWarnings = GUILayout.Toggle(showWarnings, "Warnings");
        showInfo = GUILayout.Toggle(showInfo, "Info");

        GUILayout.Space(10);

        int errorCount = results.Count(r => r.severity == SeverityLevel.Error);
        int warningCount = results.Count(r => r.severity == SeverityLevel.Warning);
        int infoCount = results.Count(r => r.severity == SeverityLevel.Info);

        GUILayout.Label($"Errors: {errorCount} | Warnings: {warningCount} | Info: {infoCount}");

        GUILayout.Space(10);

        scroll = GUILayout.BeginScrollView(scroll);

        foreach (var r in results)
        {
            if (!ShouldShow(r)) continue;

            GUILayout.BeginVertical("box");

            GUI.color = GetColor(r.severity);

            if (GUILayout.Button(r.obj.name))
            {
                Selection.activeGameObject = r.obj;
            }

            GUI.color = Color.white;

            GUILayout.Label(r.issue);
            GUILayout.Label("Type: " + r.type.ToString());

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Fix"))
            {
                SceneScanUtility.FixIssue(r);
                results = SceneScanUtility.ScanScene();
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }

    private bool ShouldShow(ScanResult r)
    {
        if (r.severity == SeverityLevel.Error && !showErrors) return false;
        if (r.severity == SeverityLevel.Warning && !showWarnings) return false;
        if (r.severity == SeverityLevel.Info && !showInfo) return false;
        return true;
    }

    private Color GetColor(SeverityLevel severity)
    {
        switch (severity)
        {
            case SeverityLevel.Error:
                return Color.red;
            case SeverityLevel.Warning:
                return Color.yellow;
            case SeverityLevel.Info:
                return Color.cyan;
            default:
                return Color.white;
        }
    }

    private void ExportReport()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Scene Validator Report v1.0");
        sb.AppendLine("===========================");

        foreach (var r in results)
        {
            sb.AppendLine($"[{r.severity}] {r.obj.name} - {r.issue}");
        }

        string path = Application.dataPath + "/SceneValidatorReport.txt";
        File.WriteAllText(path, sb.ToString());

        Debug.Log("Report exported to: " + path);
        AssetDatabase.Refresh();
    }
}