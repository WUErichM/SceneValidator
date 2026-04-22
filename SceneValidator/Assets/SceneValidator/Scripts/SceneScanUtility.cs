using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SceneScanUtility
{
    public static List<ScanResult> ScanScene()
    {
        List<ScanResult> results = new List<ScanResult>();

        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>(true);

        foreach (GameObject obj in allObjects)
        {
            // -------------------------
            // 1. Missing Scripts
            // -------------------------
            Component[] components = obj.GetComponents<Component>();

            foreach (Component comp in components)
            {
                if (comp == null)
                {
                    results.Add(new ScanResult(
                        obj,
                        "Missing Script",
                        IssueType.MissingScript,
                        SeverityLevel.Error
                    ));
                }
            }

            // -------------------------
            // 2. Inactive Objects
            // -------------------------
            if (!obj.activeInHierarchy)
            {
                results.Add(new ScanResult(
                    obj,
                    "Inactive Object",
                    IssueType.InactiveObject,
                    SeverityLevel.Warning
                ));
            }

            // -------------------------
            // 3. Missing References
            // -------------------------
            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                if (script == null) continue;

                var fields = script.GetType().GetFields();

                foreach (var field in fields)
                {
                    if (typeof(Object).IsAssignableFrom(field.FieldType))
                    {
                        var value = field.GetValue(script) as Object;

                        if (value == null)
                        {
                            results.Add(new ScanResult(
                                obj,
                                "Unassigned Reference: " + field.Name,
                                IssueType.MissingReference,
                                SeverityLevel.Error
                            ));
                        }
                    }
                }
            }
        }

        return results;
    }

    // -------------------------
    // FIX SINGLE ISSUE
    // -------------------------
    public static void FixIssue(ScanResult result)
    {
        if (result.obj == null) return;

        switch (result.type)
        {
            case IssueType.MissingScript:
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(result.obj);
                Debug.Log("Removed missing script from: " + result.obj.name);
                break;

            case IssueType.InactiveObject:
                result.obj.SetActive(true);
                Debug.Log("Activated object: " + result.obj.name);
                break;

            case IssueType.MissingReference:
                Debug.LogWarning("Cannot auto-fix missing references on: " + result.obj.name);
                break;
        }
    }

    // -------------------------
    // FIX ALL SAFE ISSUES
    // -------------------------
    public static void FixAll(List<ScanResult> results)
    {
        foreach (var result in results)
        {
            if (result.type == IssueType.MissingScript ||
                result.type == IssueType.InactiveObject)
            {
                FixIssue(result);
            }
        }
    }
}