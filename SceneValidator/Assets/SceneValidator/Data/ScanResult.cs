using UnityEngine;

[System.Serializable]
public class ScanResult
{
    public GameObject obj;
    public string issue;

    public ScanResult(GameObject obj, string issue)
    {
        this.obj = obj;
        this.issue = issue;
    }
}