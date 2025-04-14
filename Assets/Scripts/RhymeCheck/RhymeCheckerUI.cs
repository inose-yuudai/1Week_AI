using UnityEngine;
using UnityEngine.UI; // Text, InputField を使う場合
// using TMPro;       // TextMeshPro を使うならこっちも

public class RhymeCheckerUI : MonoBehaviour
{
    [Header("これオフにしちゃだめ")]
    [SerializeField] private RhymeChecker rhymeChecker;
    [SerializeField] private InputField enemyInput;   // or TMP_InputField
    [SerializeField] private InputField playerInput;  // or TMP_InputField

    [SerializeField] private Text resultText; // or TMP_Text

    public void OnCheckRhyme()
    {
        if (rhymeChecker == null)
        {
            Debug.LogWarning("RhymeChecker がアサインされていません");
            return;
        }

        // 入力された文字列を取得
        string enemyWord = enemyInput.text;
        string playerWord = playerInput.text;

        // とりあえず母音をコンソールで確認
        var enemyVowel = rhymeChecker.ConvertToVowelString(enemyWord);
        var playerVowel = rhymeChecker.ConvertToVowelString(playerWord);
        Debug.Log($"[Enemy] {enemyWord} -> {enemyVowel}");
        Debug.Log($"[Player] {playerWord} -> {playerVowel}");

        // 簡易韻判定 (末尾2文字の母音が一致するか)
        bool isRhyme = rhymeChecker.IsRhyme(enemyWord, playerWord, 2);

        // 結果をコンソール＆UIに表示
        Debug.Log($"韻判定: {isRhyme}");
        if (resultText != null)
        {
            resultText.text = isRhyme ? "韻が合ってるよ！" : "韻が違うかも…";
        }
    }
}