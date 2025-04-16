using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // お題とスコア関連
    public string currentTask;
    public int currentScore;

    // 制限時間関連
    public float timeLimit = 60.0f;
    private float currentTime;

    // 参照
    public HiraganaTextGenerator hiraganaGenerator;
    public TMP_Text taskText;
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text resultText;

    void Start()
    {
        // 初期設定
        SetNewTask("こんにちは");
        currentTime = timeLimit;
        currentScore = 0;
        // UpdateScoreText();
        UpdateTimeText();

        // 既存のhiraganaGeneratorフィールドをそのまま利用
        if (hiraganaGenerator != null)
        {
            hiraganaGenerator.OnTextGenerated += HandleGeneratedText;
        }
    }

    void HandleGeneratedText(string generatedText)
    {
        // 必要に応じてここでログを表示したり、チェック処理を自動で呼ぶ
        Debug.Log("生成されたテキスト: " + generatedText);
        //   CheckResult(); // 自動で正誤チェックしたい場合はこの行を残す
    }

    // スコアを更新
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "スコア: " + currentScore;
        }
    }

    void Update()
    {
        // 制限時間のカウントダウン
        currentTime -= Time.deltaTime;
        UpdateTimeText();

        if (currentTime <= 0)
        {
            // ゲームオーバー処理
            GameOver();
        }
    }

    // お題を設定し、テキストに反映
    public void SetNewTask(string task)
    {
        currentTask = task;
        if (taskText != null)
        {
            taskText.text = "お題: " + task;
        }
    }

    // 判定処理
    public void CheckResult(string generatedText)
    {
      
        if (generatedText == currentTask)
        {
            Debug.Log("正解です！");
            currentScore += 10;
            UpdateScoreText();
            DisplayResultMessage("正解です！");
        }
        else
        {
            DisplayResultMessage("不正解です！");
        }
    }

    // 残り時間を更新
    void UpdateTimeText()
    {
        if (timeText != null)
        {
            timeText.text = "残り時間: " + Mathf.Max(0, Mathf.FloorToInt(currentTime)) + "秒";
        }
    }

    // 結果を表示
    void DisplayResultMessage(string message)
    {
        if (resultText != null)
        {
            resultText.text = message;
        }
    }

    // ゲームオーバー処理
    void GameOver()
    {
        if (resultText != null)
        {
            resultText.text = "ゲームオーバー！";
        }

        // 必要に応じて、再スタートやメニューへの遷移を追加
    }
}
