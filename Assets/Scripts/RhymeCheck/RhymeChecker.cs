using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RhymeChecker : MonoBehaviour
{
    // ScriptableObject をインスペクターからアサインする

    [SerializeField] private RhymeData rhymeData;

    // kana -> vowel の辞書（高速検索向け）
    private Dictionary<string, string> kanaToVowelDict;

    private void Awake()
    {
        // RhymeData に設定されている kanaVowelMappings を辞書化
        kanaToVowelDict = new Dictionary<string, string>();
        foreach (var pair in rhymeData.kanaVowelMappings)
        {
            // まだ登録されていなければ登録
            if (!kanaToVowelDict.ContainsKey(pair.kana))
            {
                kanaToVowelDict.Add(pair.kana, pair.vowel);
            }
        }
    }

    /// <summary>
    /// 文字列(例: "アキバ")を母音列(例: "a i a")に変換する簡易メソッド
    /// </summary>
    public string ConvertToVowelString(string input)
    {
        // ひらがな/カタカナの種類をまとめて処理したい場合は
        // 事前にすべてひらがなに変換するなどの前処理をしておく
        // 例: "アキバ" -> "あきば" に変換

        // 結果をためる用
        List<string> vowels = new List<string>();

        // 1文字ずつ取り出して母音に変換
        foreach (char c in input)
        {
            string character = c.ToString();
            // 辞書にあれば母音を取得、なければ空白または適当な文字
            if (kanaToVowelDict.ContainsKey(character))
            {
                vowels.Add(kanaToVowelDict[character]);
            }
            else
            {
                // 変換先が見つからない場合、ゲーム仕様によってはスキップや "?" を返すなど
                // ここでは一旦スキップしてみる
                continue;
            }
        }

        // スペースで連結するか、単に string.Join なしで繋ぐかは好み
        return string.Join("", vowels);
    }

    /// <summary>
    /// 簡易韻判定: 最後の n 母音が一致していれば "韻を踏んでいる" とみなす
    /// </summary>
    public bool IsRhyme(string enemyWord, string playerWord, int compareLength = 5)
    {
        string enemyVowels = ConvertToVowelString(enemyWord);
        string playerVowels = ConvertToVowelString(playerWord);

        // 指定数の母音が足りない場合への考慮など、細かい処理は適宜追加
        string enemyEnd = GetLastNChars(enemyVowels, compareLength);
        string playerEnd = GetLastNChars(playerVowels, compareLength);

        // 完全一致であれば true
        return enemyEnd == playerEnd;
    }

    private string GetLastNChars(string input, int n)
    {
        if (string.IsNullOrEmpty(input)) return "";
        if (input.Length <= n) return input;
        return input.Substring(input.Length - n);
    }
}
