using UnityEngine;

[CreateAssetMenu(fileName = "RhymeData", menuName = "ScriptableObjects/RhymeData", order = 1)]
public class RhymeData : ScriptableObject
{
    [System.Serializable]
    public struct KanaVowelPair
    {
        public string kana;  // 例: "あ"
        public string vowel; // 例: "a"
    }

    public KanaVowelPair[] kanaVowelMappings;

    // ★右クリックメニュー or インスペクターの"..."から呼び出せるように
    [ContextMenu("自動で初期化")]
    private void InitializeKanaVowelList()
    {
        kanaVowelMappings = new KanaVowelPair[]
        {
            // --- ひらがな ---
            new KanaVowelPair { kana = "あ", vowel = "a" },
            new KanaVowelPair { kana = "い", vowel = "i" },
            new KanaVowelPair { kana = "う", vowel = "u" },
            new KanaVowelPair { kana = "え", vowel = "e" },
            new KanaVowelPair { kana = "お", vowel = "o" },

            new KanaVowelPair { kana = "か", vowel = "a" },
            new KanaVowelPair { kana = "き", vowel = "i" },
            new KanaVowelPair { kana = "く", vowel = "u" },
            new KanaVowelPair { kana = "け", vowel = "e" },
            new KanaVowelPair { kana = "こ", vowel = "o" },

            new KanaVowelPair { kana = "が", vowel = "a" },
            new KanaVowelPair { kana = "ぎ", vowel = "i" },
            new KanaVowelPair { kana = "ぐ", vowel = "u" },
            new KanaVowelPair { kana = "げ", vowel = "e" },
            new KanaVowelPair { kana = "ご", vowel = "o" },

            new KanaVowelPair { kana = "さ", vowel = "a" },
            new KanaVowelPair { kana = "し", vowel = "i" },
            new KanaVowelPair { kana = "す", vowel = "u" },
            new KanaVowelPair { kana = "せ", vowel = "e" },
            new KanaVowelPair { kana = "そ", vowel = "o" },

            new KanaVowelPair { kana = "ざ", vowel = "a" },
            new KanaVowelPair { kana = "じ", vowel = "i" },
            new KanaVowelPair { kana = "ず", vowel = "u" },
            new KanaVowelPair { kana = "ぜ", vowel = "e" },
            new KanaVowelPair { kana = "ぞ", vowel = "o" },

            new KanaVowelPair { kana = "た", vowel = "a" },
            new KanaVowelPair { kana = "ち", vowel = "i" },
            new KanaVowelPair { kana = "つ", vowel = "u" },
            new KanaVowelPair { kana = "て", vowel = "e" },
            new KanaVowelPair { kana = "と", vowel = "o" },

            new KanaVowelPair { kana = "だ", vowel = "a" },
            new KanaVowelPair { kana = "ぢ", vowel = "i" },
            new KanaVowelPair { kana = "づ", vowel = "u" },
            new KanaVowelPair { kana = "で", vowel = "e" },
            new KanaVowelPair { kana = "ど", vowel = "o" },

            new KanaVowelPair { kana = "な", vowel = "a" },
            new KanaVowelPair { kana = "に", vowel = "i" },
            new KanaVowelPair { kana = "ぬ", vowel = "u" },
            new KanaVowelPair { kana = "ね", vowel = "e" },
            new KanaVowelPair { kana = "の", vowel = "o" },

            new KanaVowelPair { kana = "は", vowel = "a" },
            new KanaVowelPair { kana = "ひ", vowel = "i" },
            new KanaVowelPair { kana = "ふ", vowel = "u" },
            new KanaVowelPair { kana = "へ", vowel = "e" },
            new KanaVowelPair { kana = "ほ", vowel = "o" },

            new KanaVowelPair { kana = "ば", vowel = "a" },
            new KanaVowelPair { kana = "び", vowel = "i" },
            new KanaVowelPair { kana = "ぶ", vowel = "u" },
            new KanaVowelPair { kana = "べ", vowel = "e" },
            new KanaVowelPair { kana = "ぼ", vowel = "o" },

            new KanaVowelPair { kana = "ぱ", vowel = "a" },
            new KanaVowelPair { kana = "ぴ", vowel = "i" },
            new KanaVowelPair { kana = "ぷ", vowel = "u" },
            new KanaVowelPair { kana = "ぺ", vowel = "e" },
            new KanaVowelPair { kana = "ぽ", vowel = "o" },

            new KanaVowelPair { kana = "ま", vowel = "a" },
            new KanaVowelPair { kana = "み", vowel = "i" },
            new KanaVowelPair { kana = "む", vowel = "u" },
            new KanaVowelPair { kana = "め", vowel = "e" },
            new KanaVowelPair { kana = "も", vowel = "o" },

            new KanaVowelPair { kana = "や", vowel = "a" },
            new KanaVowelPair { kana = "ゆ", vowel = "u" },
            new KanaVowelPair { kana = "よ", vowel = "o" },

            new KanaVowelPair { kana = "ら", vowel = "a" },
            new KanaVowelPair { kana = "り", vowel = "i" },
            new KanaVowelPair { kana = "る", vowel = "u" },
            new KanaVowelPair { kana = "れ", vowel = "e" },
            new KanaVowelPair { kana = "ろ", vowel = "o" },

            new KanaVowelPair { kana = "わ", vowel = "a" },
            new KanaVowelPair { kana = "を", vowel = "o" },
            new KanaVowelPair { kana = "ん", vowel = "n" },

            // ▼小文字の母音付き仮名・拗音など（ひらがな）
            new KanaVowelPair { kana = "ぁ", vowel = "a" },
            new KanaVowelPair { kana = "ぃ", vowel = "i" },
            new KanaVowelPair { kana = "ぅ", vowel = "u" },
            new KanaVowelPair { kana = "ぇ", vowel = "e" },
            new KanaVowelPair { kana = "ぉ", vowel = "o" },

            new KanaVowelPair { kana = "ゃ", vowel = "a" },
            new KanaVowelPair { kana = "ゅ", vowel = "u" },
            new KanaVowelPair { kana = "ょ", vowel = "o" },

            new KanaVowelPair { kana = "ゎ", vowel = "a" },

            // ▼濁音の拡張（ゔ・ヴ）
            new KanaVowelPair { kana = "ゔ", vowel = "u" }, // ひらがな "う" に濁点
            new KanaVowelPair { kana = "ヴ", vowel = "u" }, // カタカナ

            // ▼その他、必要に応じて
            new KanaVowelPair { kana = "ヵ", vowel = "a" },
            new KanaVowelPair { kana = "ヶ", vowel = "e" },
            new KanaVowelPair { kana = "ゕ", vowel = "a" },
            new KanaVowelPair { kana = "ゖ", vowel = "e" },

            // --- カタカナ ---
            new KanaVowelPair { kana = "ア", vowel = "a" },
            new KanaVowelPair { kana = "イ", vowel = "i" },
            new KanaVowelPair { kana = "ウ", vowel = "u" },
            new KanaVowelPair { kana = "エ", vowel = "e" },
            new KanaVowelPair { kana = "オ", vowel = "o" },

            new KanaVowelPair { kana = "カ", vowel = "a" },
            new KanaVowelPair { kana = "キ", vowel = "i" },
            new KanaVowelPair { kana = "ク", vowel = "u" },
            new KanaVowelPair { kana = "ケ", vowel = "e" },
            new KanaVowelPair { kana = "コ", vowel = "o" },

            new KanaVowelPair { kana = "ガ", vowel = "a" },
            new KanaVowelPair { kana = "ギ", vowel = "i" },
            new KanaVowelPair { kana = "グ", vowel = "u" },
            new KanaVowelPair { kana = "ゲ", vowel = "e" },
            new KanaVowelPair { kana = "ゴ", vowel = "o" },

            new KanaVowelPair { kana = "サ", vowel = "a" },
            new KanaVowelPair { kana = "シ", vowel = "i" },
            new KanaVowelPair { kana = "ス", vowel = "u" },
            new KanaVowelPair { kana = "セ", vowel = "e" },
            new KanaVowelPair { kana = "ソ", vowel = "o" },

            new KanaVowelPair { kana = "ザ", vowel = "a" },
            new KanaVowelPair { kana = "ジ", vowel = "i" },
            new KanaVowelPair { kana = "ズ", vowel = "u" },
            new KanaVowelPair { kana = "ゼ", vowel = "e" },
            new KanaVowelPair { kana = "ゾ", vowel = "o" },

            new KanaVowelPair { kana = "タ", vowel = "a" },
            new KanaVowelPair { kana = "チ", vowel = "i" },
            new KanaVowelPair { kana = "ツ", vowel = "u" },
            new KanaVowelPair { kana = "テ", vowel = "e" },
            new KanaVowelPair { kana = "ト", vowel = "o" },

            new KanaVowelPair { kana = "ダ", vowel = "a" },
            new KanaVowelPair { kana = "ヂ", vowel = "i" },
            new KanaVowelPair { kana = "ヅ", vowel = "u" },
            new KanaVowelPair { kana = "デ", vowel = "e" },
            new KanaVowelPair { kana = "ド", vowel = "o" },

            new KanaVowelPair { kana = "ナ", vowel = "a" },
            new KanaVowelPair { kana = "ニ", vowel = "i" },
            new KanaVowelPair { kana = "ヌ", vowel = "u" },
            new KanaVowelPair { kana = "ネ", vowel = "e" },
            new KanaVowelPair { kana = "ノ", vowel = "o" },

            new KanaVowelPair { kana = "ハ", vowel = "a" },
            new KanaVowelPair { kana = "ヒ", vowel = "i" },
            new KanaVowelPair { kana = "フ", vowel = "u" },
            new KanaVowelPair { kana = "ヘ", vowel = "e" },
            new KanaVowelPair { kana = "ホ", vowel = "o" },

            new KanaVowelPair { kana = "バ", vowel = "a" },
            new KanaVowelPair { kana = "ビ", vowel = "i" },
            new KanaVowelPair { kana = "ブ", vowel = "u" },
            new KanaVowelPair { kana = "ベ", vowel = "e" },
            new KanaVowelPair { kana = "ボ", vowel = "o" },

            new KanaVowelPair { kana = "パ", vowel = "a" },
            new KanaVowelPair { kana = "ピ", vowel = "i" },
            new KanaVowelPair { kana = "プ", vowel = "u" },
            new KanaVowelPair { kana = "ペ", vowel = "e" },
            new KanaVowelPair { kana = "ポ", vowel = "o" },

            new KanaVowelPair { kana = "マ", vowel = "a" },
            new KanaVowelPair { kana = "ミ", vowel = "i" },
            new KanaVowelPair { kana = "ム", vowel = "u" },
            new KanaVowelPair { kana = "メ", vowel = "e" },
            new KanaVowelPair { kana = "モ", vowel = "o" },

            new KanaVowelPair { kana = "ヤ", vowel = "a" },
            new KanaVowelPair { kana = "ユ", vowel = "u" },
            new KanaVowelPair { kana = "ヨ", vowel = "o" },

            new KanaVowelPair { kana = "ラ", vowel = "a" },
            new KanaVowelPair { kana = "リ", vowel = "i" },
            new KanaVowelPair { kana = "ル", vowel = "u" },
            new KanaVowelPair { kana = "レ", vowel = "e" },
            new KanaVowelPair { kana = "ロ", vowel = "o" },

            new KanaVowelPair { kana = "ワ", vowel = "a" },
            new KanaVowelPair { kana = "ヲ", vowel = "o" },
            new KanaVowelPair { kana = "ン", vowel = "n" },

            // ▼小文字の母音付き仮名・拗音など（カタカナ）
            new KanaVowelPair { kana = "ァ", vowel = "a" },
            new KanaVowelPair { kana = "ィ", vowel = "i" },
            new KanaVowelPair { kana = "ゥ", vowel = "u" },
            new KanaVowelPair { kana = "ェ", vowel = "e" },
            new KanaVowelPair { kana = "ォ", vowel = "o" },

            new KanaVowelPair { kana = "ャ", vowel = "a" },
            new KanaVowelPair { kana = "ュ", vowel = "u" },
            new KanaVowelPair { kana = "ョ", vowel = "o" },

            // 必要なら「ヮ」「ヰ」「ヱ」等も追加可
            new KanaVowelPair { kana = "ヮ", vowel = "a" },
        };
    }
}
