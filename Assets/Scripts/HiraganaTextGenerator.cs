using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HiraganaTextGenerator : MonoBehaviour
{
    public HiraganaBoardController boardController;
    public GameObject textPrefab;
    public RectTransform textParent;
    public TextAsset initialPositionJson;
    public event System.Action<string> OnTextGenerated;
    public GameManager gameManager; // ← Inspectorから設定する用


    private Dictionary<char, Vector2> initialPositions;

    void Awake()
    {
        LoadInitialPositions();
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

    // 初期位置に基づいて現在の文字を反映して一つのテキストとして生成
    public string GenerateText(string inputText)
    {
        foreach (Transform child in textParent)
        {
            Destroy(child.gameObject);
        }

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

        if (!string.IsNullOrEmpty(resultText))
        {
            GameObject newTextObj = Instantiate(textPrefab, textParent);
            TextMeshProUGUI tmpText = newTextObj.GetComponent<TextMeshProUGUI>();

            if (tmpText != null)
            {
                tmpText.text = resultText;
            }
        }

        OnTextGenerated?.Invoke(resultText);

        // ★ GameManagerにチェックを依頼
        if (gameManager != null)
        {
            gameManager.CheckResult(resultText); // ← 引数で渡す
        }

        return resultText;
    }
}
