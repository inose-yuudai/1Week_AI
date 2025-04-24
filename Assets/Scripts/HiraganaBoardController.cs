using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Fungus;

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

    private HiraganaCell heldCell = null;

    public List<HiraganaCell> cells = new List<HiraganaCell>();

    public HiraganaCell selectedCell = null;
    public GameObject controllerCursor; // ハイライト用の枠（Imageなど）
    private bool usingController = false;
    private Vector2 heldOriginPosition; // 移動範囲制限の基準
    public GameManager gameManager;
    public HiraganaTextGenerator hiraganaGenerator;

    public string lastHighlightedText = "すみ";
    private Color defaultHighlightColor = new Color(1.0f, 0.713f, 0.757f);

    void Update()
    {
        // スペースキーで固定順
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
        if (Input.GetMouseButtonDown(0))
        {
            usingController = false;
            controllerCursor.SetActive(false);
        }
        // Aキーでシャッフル

        // コントローラーの十字キー入力で操作開始
        if (
            !usingController
            && (
                Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.RightArrow)
            )
        )
        {
            usingController = true;

            if (selectedCell == null && cells.Count > 0)
            {
                selectedCell = cells[0]; // 初期選択セル
            }

            controllerCursor.SetActive(true);
            controllerCursor.transform.position = selectedCell.rectTransform.position;
        }
        // マウス操作が行われたらコントローラー操作解除
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            usingController = false;
            controllerCursor.SetActive(false);
        }

        // 十字キーでカーソル移動
        if (usingController)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                MoveCursor(Vector2.up);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                MoveCursor(Vector2.down);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                MoveCursor(Vector2.left);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                MoveCursor(Vector2.right);

            if (Input.GetKeyDown(KeyCode.JoystickButton0)) // Aボタン
            {
                if (heldCell == null)
                {
                    heldCell = selectedCell;
                }
                else
                {
                    SwapCells(heldCell, selectedCell);

                    heldCell = null;
                }
            }

            // ハイライト位置更新
            if (selectedCell != null && controllerCursor != null)
            {
                controllerCursor.transform.position = selectedCell.rectTransform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton0)) // Aボタン
        {
            if (heldCell == null)
            {
                heldCell = selectedCell;
                heldOriginPosition = selectedCell.fixedPosition;

                // 演出：持ち上げる（例：スケール変更）
                heldCell.rectTransform.DOScale(1.2f, 0.1f);
            }
            else
            {
                // 入れ替え実行
                SwapCells(heldCell, selectedCell);

                // 演出戻す
                heldCell.rectTransform.DOScale(1f, 0.1f);

                heldCell = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1)) // Bボタンでキャンセル
        {
            if (heldCell != null)
            {
                heldCell.rectTransform.DOScale(1f, 0.1f);
                heldCell = null;
            }
        }
    }

    void MoveCursor(Vector2 dir)
    {
        if (selectedCell == null)
            return;

        Vector2 newPos = selectedCell.fixedPosition + dir * 75f;
        HiraganaCell target = cells.Find(c => c.fixedPosition == newPos);

        if (target != null)
        {
            if (heldCell != null)
            {
                // 移動可能範囲は heldOriginPosition から 1マス以内
                float dx = Mathf.Abs(newPos.x - heldOriginPosition.x);
                float dy = Mathf.Abs(newPos.y - heldOriginPosition.y);

                if (dx + dy <= 75f) // 1マス以内
                {
                    selectedCell = target;
                }
            }
            else
            {
                selectedCell = target;
            }
        }
    }

    public void SetFirstTask()
    {
        GenerateFromJson(false);
        gameManager.SetNewTask("こんにちはとうってみよう");
       HighlightDefaultPositionsOnly("こんにちは", new Color(1.0f, 0.713f, 0.757f)); // 薄い赤色 (RGB: 255, 182, 193)
    }

    public void GenerateFromJson(bool shuffle)
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

    public void HighlightDefaultPositionsOnly(string inputText, Color defaultPositionColor)
    {
        Dictionary<char, Vector2> initialPositions = hiraganaGenerator.GetInitialPositionMap();

        // 全セルのハイライトをリセット
        foreach (var cell in cells)
        {
            cell.SetHighlight(false);
        }

        // 入力文字の「本来の位置」にあるセルをハイライト（今何があるかは気にしない）
        foreach (char c in inputText)
        {
            if (initialPositions.TryGetValue(c, out Vector2 defaultPos))
            {
                HiraganaCell cellAtDefault = cells.Find(cell => cell.fixedPosition == defaultPos);
                if (cellAtDefault != null)
                {
                    cellAtDefault.SetHighlight(true, defaultPositionColor);
                }
            }
        }
    }

    public void RefreshDefaultHighlights()
    {
        if (!string.IsNullOrEmpty(lastHighlightedText))
        {
            HighlightDefaultPositionsOnly(lastHighlightedText, defaultHighlightColor);
        }
          Flowchart.BroadcastFungusMessage("ChangeCell");
    }
}
