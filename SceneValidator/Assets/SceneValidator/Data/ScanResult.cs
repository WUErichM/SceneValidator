using UnityEngine;

public enum IssueType
{
    MissingScript,
    MissingReference,
    InactiveObject
}

[System.Serializable]
public class ScanResult
{
    public GameObject obj;
    public string issue;
    public IssueType type;

    public ScanResult(GameObject obj, string issue, IssueType type)
    {
        this.obj = obj;
        this.issue = issue;
        this.type = type;
    }
}