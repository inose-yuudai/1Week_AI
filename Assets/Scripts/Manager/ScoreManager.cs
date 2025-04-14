using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    // 数字を表示する親コンテナ
    [SerializeField] private Transform scoreContainer;
    // 数字用のImageプレハブ（Imageコンポーネントが必要）
    [SerializeField] private GameObject digitPrefab;
    // 0～9の数字画像を格納（0番目が「0」、1番目が「1」…）
    [SerializeField] private Sprite[] digitSprites;

    [SerializeField] private Text resultText; // 判定結果を表示するテキスト

    private int currentScore = 0;
    
    // 数字間の余白（ピクセル単位で調整）
    [SerializeField] private float spacing = 2f;

    /// <summary>
    /// スコアを追加
    /// </summary>
    /// <param name="value">加算するスコア値</param>
    public void AddScore(int value, string result = "")
    {
        currentScore += value;
        UpdateScoreUI();

        // 判定結果があればUIに表示
        if (!string.IsNullOrEmpty(result))
        {
            ShowResult(result);
        }
    }

    /// <summary>
    /// スコアUIを画像で更新（各桁の数字を並べる）
    /// </summary>
    private void UpdateScoreUI()
    {
        // 既存の子要素（数字画像）を削除
        foreach (Transform child in scoreContainer)
        {
            Destroy(child.gameObject);
        }

        string scoreStr = currentScore.ToString();
        
        // 1桁分の幅を取得
        RectTransform prefabRect = digitPrefab.GetComponent<RectTransform>();
        float digitWidth = prefabRect.rect.width;
        
        // 全体の幅を計算（各桁の幅＋間隔）
        float totalWidth = scoreStr.Length * digitWidth + (scoreStr.Length - 1) * spacing;
        
        // コンテナの中心を基準に左端のX座標を計算（コンテナのpivotが0.5,0.5の場合）
        float startX = -totalWidth * 0.5f + digitWidth * 0.5f;
        
        // 各桁のImageを生成し、位置を調整
        for (int i = 0; i < scoreStr.Length; i++)
        {
            int digit = scoreStr[i] - '0';
            GameObject digitObj = Instantiate(digitPrefab, scoreContainer);
            RectTransform rt = digitObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                // 各桁の位置を計算してセット（Y軸は0に固定）
                rt.localPosition = new Vector3(startX + i * (digitWidth + spacing), 0, 0);
                Debug.Log(digitWidth);
            }
            Image digitImage = digitObj.GetComponent<Image>();
            if (digitImage != null && digitSprites != null && digitSprites.Length > digit)
            {
                digitImage.sprite = digitSprites[digit];
            }
        }
    }

    /// <summary>
    /// 判定結果を表示
    /// </summary>
    /// <param name="result">判定結果の文字列</param>
    private void ShowResult(string result)
    {
        if (resultText != null)
        {
            resultText.text = result;

            // 判定結果を一時的に表示し、消すためのコルーチンを呼び出す
            StopAllCoroutines(); // 前回のコルーチンを停止
            StartCoroutine(HideResultAfterDelay());
        }
    }

    /// <summary>
    /// 判定結果を一定時間後に消す
    /// </summary>
    private IEnumerator HideResultAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // 1秒後に非表示
        if (resultText != null)
        {
            resultText.text = "";
        }
    }
}
