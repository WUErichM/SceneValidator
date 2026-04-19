using System.Collections.Generic;
using UnityEngine;

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
                        IssueType.MissingScript
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
                    IssueType.InactiveObject
                ));
            }

            // -------------------------
            // 3. Missing References (BASIC CHECK)
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
                                IssueType.MissingReference
                            ));
                        }
                    }
                }
            }
        }

        return results;
    }
}