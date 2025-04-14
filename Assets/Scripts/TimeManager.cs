using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーン切り替え等を行う場合に使用

public class TimeManager : MonoBehaviour
{
    [SerializeField] 
    private float timeLimit = 60f;  // 制限時間（秒）

    [SerializeField]
    private Text timeText;          // 残り時間を表示するUI Text

    private float currentTime;      // 残り時間
    private bool isTimerRunning;    // タイマーが動いているかどうか

    // 外部から残り時間を取得したい場合に公開しておく
    public float CurrentTime
    {
        get { return currentTime; }
    }

    // 外部から「時間切れかどうか」を確認
    public bool IsTimeUp
    {
        get { return currentTime <= 0f; }
    }

    void Awake()
    {
        // 計測開始前なので初期値を設定
        currentTime = 0f;
        isTimerRunning = false;

        // UIの初期表示（タイマー開始前は何も表示しない）
        UpdateTimeText();
    }

    void Update()
    {
        // タイマーが動いている間だけ時間を減算
        if (isTimerRunning && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                OnTimeUp();
            }
        }

        // 毎フレームUIを更新（現在の残り時間を表示）
        UpdateTimeText();
    }

    /// <summary>
    /// タイマーを開始する
    /// </summary>
    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            currentTime = timeLimit;
            isTimerRunning = true;
        }
    }

    /// <summary>
    /// タイマーを止める
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    /// <summary>
    /// 残り時間をテキストに反映する
    /// </summary>
    private void UpdateTimeText()
    {
        if (timeText != null)
        {
            int remaining = Mathf.CeilToInt(currentTime);
            timeText.text = "Time Left: " + remaining + "s";
   
        }
    }

    /// <summary>
    /// 時間切れ時の処理
    /// </summary>
    private void OnTimeUp()
    {
        Debug.Log("Time is up!");
        // 例: シーンをリロードする場合
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}