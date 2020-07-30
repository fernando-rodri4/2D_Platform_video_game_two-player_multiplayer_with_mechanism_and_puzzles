using UnityEngine;

[System.Serializable]
public class Dialogue
{
    /// <summary>
    /// Name of the person that speak or write
    /// </summary>
    public string name;

    /// <summary>
    /// Sentences that person says
    /// </summary>
    [TextArea(3, 10)]
    public string[] sentences;
}
