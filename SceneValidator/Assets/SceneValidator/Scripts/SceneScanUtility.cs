using System.Collections.Generic;
using UnityEngine;

public static class SceneScanUtility
{
    public static List<ScanResult> ScanScene()
    {
        List<ScanResult> results = new List<ScanResult>();

        // INCLUDE inactive objects
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>(true);

        foreach (GameObject obj in allObjects)
        {
            // 1. Check for missing scripts
            Component[] components = obj.GetComponents<Component>();
            foreach (Component comp in components)
            {
                if (comp == null)
                {
                    results.Add(new ScanResult(obj, "Missing Script"));
                }
            }

            // 2. Check for inactive objects (includes parent inactive)
            if (!obj.activeInHierarchy)
            {
                results.Add(new ScanResult(obj, "Inactive Object"));
            }
        }

        return results;
    }
}