using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWordData", menuName = "ScriptableObjects/PlayerWordData", order = 1)]
public class PlayerWordData : ScriptableObject
{
    public PlayerWordPair[] playerWordPairs;
    public PlayerWordPair[] specialWordPairs; // スペシャルワードのリスト

    public PlayerWordPair[] criticalWordPairs; // スペシャルワードのリスト
}

[System.Serializable]
public struct PlayerWordPair
{
    public string displayWord;  // 表示用 (例: "銀座")
    public string internalWord; // 判定用 (例: "ぎんざ")
    public bool isSpecial;      // スペシャルワードかどうかを示すフラグ
    public bool isCritical;     // クリティカルワードかどうかを示すフラグ
}