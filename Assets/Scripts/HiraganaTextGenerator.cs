using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using MoonSharp.Interpreter.Debugging;

public class HiraganaTextGenerator : MonoBehaviour
{
    public HiraganaBoardController boardController;
    public TextAsset initialPositionJson;
    public GameManager gameManager;
    public ChatCommenter chatCommenter;
    public ChatManager chatManager; // ← インスペクタでセット忘れずに！



    public event System.Action<string> OnTextGenerated;

    private Dictionary<char, Vector2> initialPositions;

    public RectTransform textParent;
    public GameObject textPrefab;

    void Awake()
    {
        LoadInitialPositions();
    }

    public Dictionary<char, Vector2> GetInitialPositionMap()
    {
        return initialPositions;
    }

    void LoadInitialPositions()
    {
        initialPositions = new Dictionary<char, Vector2>();

        string json = initialPositionJson.text;
        HiraganaLayoutEntry[] entries = JsonHelper.FromJson<HiraganaLayoutEntry>(json);

        foreach (var entry in entries)
        {
            if (!string.IsNullOrEmpty(entry.character))
            {
                initialPositions[entry.character[0]] = new Vector2(entry.x, entry.y);
            }
        }
    }

    /// <summary>
    /// 結果を生成して返す（Text更新なし、イベント通知あり）
    /// </summary>
    public string GenerateText(string inputText, bool sendComment = false)
    {
        return GenerateTextInternal(inputText, null, sendComment);
    }

    /// <summary>
    /// 特定のTextに直接表示（Text更新あり、イベント通知あり）
    /// </summary>
    public string GenerateText(string inputText, Text targetText, bool sendComment = false)
    {
        return GenerateTextInternal(inputText, targetText, sendComment);
    }

    /// <summary>
    /// 内部処理まとめ（共通処理）
    /// </summary>
    private string GenerateTextInternal(string inputText, Text targetText, bool sendComment)
    {
        string resultText = "";

        Debug.Log("ゲームから来てるか1: " + inputText);

        foreach (char c in inputText)
        {
            if (initialPositions.TryGetValue(c, out Vector2 initialPos))
            {
                HiraganaCell currentCell = boardController.cells.Find(
                    cell => cell.fixedPosition == initialPos
                );

                if (currentCell != null)
                {
                    resultText += currentCell.character;
                }
            }
        }

        // ✅ Textが指定されていれば表示更新
        if (targetText != null)
        {
            targetText.text = resultText;
        }

        // ✅ sendCommentがtrueのときだけイベント発火（吹き出し表示につながる）
        if (sendComment)
        {
            OnTextGenerated?.Invoke(resultText);

            // ✅ コメント送信もこの中に入れておく（sendCommentがtrueなら）
            if (chatCommenter != null)
            {
                chatCommenter.SendCommentRequest(resultText);
            }
        }



        return resultText;
    }
   
    public void GenerateAndSendPlayerMessage(string inputText)
    {
        string resultText = "";

        foreach (char c in inputText)
        {
            if (initialPositions.TryGetValue(c, out Vector2 initialPos))
            {
                HiraganaCell currentCell = boardController.cells.Find(
                    cell => cell.fixedPosition == initialPos
                );

                if (currentCell != null)
                {
                    resultText += currentCell.character;
                }
            }
        }

        // プレイヤーの吹き出しとして表示！
        if (chatCommenter != null)
        {
            chatCommenter.SendCommentRequest(resultText); // ← ChatGPTへ送るならこれ
        }

        if (chatManager != null)
        {
            chatManager.AddChatMessage(
                resultText,
                chatManager.playerBubblePrefab,
                chatManager.playerStartPos,
                true // プレイヤー側
            );
        }


        OnTextGenerated?.Invoke(resultText);
    }
}
