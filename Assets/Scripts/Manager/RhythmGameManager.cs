using UnityEngine;
using UnityEngine.UI;

public class RhythmGameManager : MonoBehaviour
{
    [Header("音楽再生")]
    [SerializeField] private AudioSource audioSource;

    [Header("タイミング設定")]
    [SerializeField] private float pressInterval = 1.364f; // スライダーが0→pressIntervalまで行く時間（秒）

    [Header("判定ウィンドウ(秒)")]
    [SerializeField] private float greatThreshold = 0.1f;
    [SerializeField] private float goodThreshold = 0.3f;

    [Header("スライダーUI")]
    [SerializeField] private Slider rhythmSlider;

    [Header("スコア管理")]
    [SerializeField] private ScoreManager scoreManager;

     public GameManager gameManager;

    private float musicStartTime; // 音楽再生開始時刻

    private void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceが設定されていません。");
            return;
        }
        if (rhythmSlider == null)
        {
            Debug.LogError("Sliderが設定されていません。");
            return;
        }
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManagerが設定されていません。");
            return;
        }

        // ★ 最大値を pressInterval で上書き！
        rhythmSlider.minValue = 0f;
        rhythmSlider.maxValue = pressInterval;

        // 音楽を再生
        audioSource.Play();
        musicStartTime = Time.time;
    }

    private void Update()
    {
        if (!audioSource.isPlaying) return;

        // 開始からの経過秒
        float elapsed = Time.time - musicStartTime;

        // 1サイクル(pressInterval)の中で、今どこにいるか (0〜pressInterval)
        float current = elapsed % pressInterval;

        // スライダーに値を反映（最大値は pressInterval なのでちょうど 0〜1.364の範囲を示す）
        rhythmSlider.value = current;
    }

    /// <summary>
    /// ボタンを押したとき
    /// </summary>
    public void OnButtonPress()
    {
        // 経過時間
        float elapsed = Time.time - musicStartTime;
        float cyclePos = elapsed % pressInterval; // 0〜(pressInterval未満)

        // 0付近との距離
        float distanceFromZero = cyclePos; // 0に近いほど小さい

        // 最大値(pressInterval)付近との距離
        float distanceFromMax = pressInterval - cyclePos; // pressIntervalに近いほど小さい

        // 円形なので、0かmax、どちらに近いかを判断
        float minDistance = Mathf.Min(distanceFromZero, distanceFromMax);

        // 判定
        if (minDistance <= greatThreshold)
        {
            Debug.Log("Great! (0付近 or 最大値付近)");
            gameManager.timing = 2;
        }
        else if (minDistance <= goodThreshold)
        {
            Debug.Log("Good! (0付近 or 最大値付近)");
            gameManager.timing = 1;
        }
        else
        {
            Debug.Log("Miss!");
            gameManager.timing = 0;
            // Missは加算なし
        }
    }

}
