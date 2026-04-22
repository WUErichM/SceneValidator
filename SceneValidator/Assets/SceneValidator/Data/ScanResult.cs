using UnityEngine;

public enum IssueType
{
    MissingScript,
    MissingReference,
    InactiveObject
}

public enum SeverityLevel
{
    Info,
    Warning,
    Error
}

[System.Serializable]
public class ScanResult
{
    public GameObject obj;
    public string issue;
    public IssueType type;
    public SeverityLevel severity;

    public ScanResult(GameObject obj, string issue, IssueType type, SeverityLevel severity)
    {
        this.obj = obj;
        this.issue = issue;
        this.type = type;
        this.severity = severity;
    }
}