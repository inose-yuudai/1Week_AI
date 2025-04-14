using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWordsData", menuName = "ScriptableObjects/EnemyWordsData", order = 1)]
public class EnemyWordsData : ScriptableObject
{
    public EnemyWordPair[] enemyWordPairs;

    // ▼ 追加: Special・Critical用の単語（各1セットのみでシンプル化）
    public EnemyWordPair[] specialEnemyWordPairs;
    public EnemyWordPair[] criticalEnemyWordPairs;
}

[System.Serializable]
public struct EnemyWordPair
{
    public string displayWord;
    public string internalWord;
    public AudioClip voiceClip;
}
