using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class HiraganaLayoutEntry
{
    public string character;
    public float x;
    public float y;
}

public class HiraganaBoardController : MonoBehaviour
{
    public GameObject hiraganaCellPrefab;
    public RectTransform parentPanel;
    public TextAsset jsonFile;

    public List<HiraganaCell> cells = new List<HiraganaCell>();

    void Update()
    {
        // スペースキーで固定順
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateFromJson(false);
        }

        // Aキーでシャッフル
        if (Input.GetKeyDown(KeyCode.A))
        {
            GenerateFromJson(true);
        }
    }

    void GenerateFromJson(bool shuffle)
    {
        // 既存セル削除
        foreach (var cell in cells)
        {
            Destroy(cell.gameObject);
        }
        cells.Clear();

        // JSON読み込み
        string json = jsonFile.text;
        HiraganaLayoutEntry[] layout = JsonHelper.FromJson<HiraganaLayoutEntry>(json);

        // シャッフル処理
        if (shuffle)
        {
            layout = GetShuffledCharacters(layout);
        }

        // セル生成
        foreach (var entry in layout)
        {
            if (string.IsNullOrWhiteSpace(entry.character))
                continue;

            GameObject go = Instantiate(hiraganaCellPrefab, parentPanel);
            HiraganaCell cell = go.GetComponent<HiraganaCell>();
            if (cell == null)
            {
                cell = go.AddComponent<HiraganaCell>();
            }

            cell.Initialize(entry.character, new Vector2(entry.x, entry.y), this);
            cells.Add(cell);
        }
    }

    HiraganaLayoutEntry[] GetShuffledCharacters(HiraganaLayoutEntry[] layout)
    {
        List<string> hiraganaList = new List<string>();
        foreach (var entry in layout)
        {
            if (!string.IsNullOrWhiteSpace(entry.character))
            {
                hiraganaList.Add(entry.character);
            }
        }

        for (int i = hiraganaList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            string temp = hiraganaList[i];
            hiraganaList[i] = hiraganaList[randomIndex];
            hiraganaList[randomIndex] = temp;
        }

        HiraganaLayoutEntry[] shuffledLayout = new HiraganaLayoutEntry[layout.Length];
        int hiraganaIndex = 0;
        for (int i = 0; i < layout.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(layout[i].character))
            {
                shuffledLayout[i] = layout[i];
            }
            else
            {
                shuffledLayout[i] = new HiraganaLayoutEntry
                {
                    character = hiraganaList[hiraganaIndex],
                    x = layout[i].x,
                    y = layout[i].y
                };
                hiraganaIndex++;
            }
        }

        return shuffledLayout;
    }

    // ドラッグ完了時に呼ばれる入れ替え処理
    public void SwapCells(HiraganaCell a, HiraganaCell b)
{
    // fixedPositionを入れ替える（論理座標のみ）
    Vector2 tempFixed = a.fixedPosition;
    a.fixedPosition = b.fixedPosition;
    b.fixedPosition = tempFixed;

    // 見た目（UI）をアニメーションで移動させる
    a.AnimateMoveTo(a.fixedPosition);
    b.AnimateMoveTo(b.fixedPosition);

    // リストの順番を入れ替える必要はない
}

}
