using UnityEngine;

/// <summary>結果を格納するScriptableObject</summary>
[CreateAssetMenu(fileName = "ResultData", menuName = "ResultData")]
public class ResultDataBase : ScriptableObject
{
    /// <summary>3人の得点を格納したデータを格納する配列</summary>
    public int[] ScoreData = new int[3];
}
