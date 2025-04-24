using UnityEngine;
using TMPro;

public class HiraganaGenerator : MonoBehaviour
{
    public TMP_Text displayText;

    private string[] hiraganaList = new string[]
    {
        // 清音
        "あ",
        "い",
        "う",
        "え",
        "お",
        "か",
        "き",
        "く",
        "け",
        "こ",
        "さ",
        "し",
        "す",
        "せ",
        "そ",
        "た",
        "ち",
        "つ",
        "て",
        "と",
        "な",
        "に",
        "ぬ",
        "ね",
        "の",
        "は",
        "ひ",
        "ふ",
        "へ",
        "ほ",
        "ま",
        "み",
        "む",
        "め",
        "も",
        "や",
        "ゆ",
        "よ",
        "ら",
        "り",
        "る",
        "れ",
        "ろ",
        "わ",
        "を",
        "ん",
        // 濁音
        "が",
        "ぎ",
        "ぐ",
        "げ",
        "ご",
        "ざ",
        "じ",
        "ず",
        "ぜ",
        "ぞ",
        "だ",
        "ぢ",
        "づ",
        "で",
        "ど",
        "ば",
        "び",
        "ぶ",
        "べ",
        "ぼ",
        // 半濁音
        "ぱ",
        "ぴ",
        "ぷ",
        "ぺ",
        "ぽ",
        // 小さい文字
        "ゃ",
        "ゅ",
        "ょ",
        "ぁ",
        "ぃ",
        "ぅ",
        "ぇ",
        "ぉ",
        "っ"
    };

    void Start()
    {
        GenerateRandomHiragana();
    }

    public void GenerateRandomHiragana()
    {
        int length = Random.Range(20, 40); // 3～10文字

        string result = "";
        for (int i = 0; i < length; i++)
        {
            int index = Random.Range(0, hiraganaList.Length);
            result += hiraganaList[index];
        }

        displayText.text = result;
    }
}
