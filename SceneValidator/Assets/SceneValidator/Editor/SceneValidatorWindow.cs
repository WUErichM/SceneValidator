using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SceneValidatorWindow : EditorWindow
{
    private List<ScanResult> results = new List<ScanResult>();
    private Vector2 scrollPos;

    [MenuItem("Tools/Scene Validator")]
    public static void ShowWindow()
    {
        GetWindow<SceneValidatorWindow>("Scene Validator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Validator", EditorStyles.boldLabel);

        if (GUILayout.Button("Scan Scene"))
        {
            results = SceneScanUtility.ScanScene();
        }

        GUILayout.Space(10);

        GUILayout.Label("Results:", EditorStyles.boldLabel);

        scrollPos = GUILayout.BeginScrollView(scrollPos);

        foreach (var result in results)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(result.obj.name))
            {
                Selection.activeGameObject = result.obj;
            }

            GUILayout.Label(result.issue);

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}