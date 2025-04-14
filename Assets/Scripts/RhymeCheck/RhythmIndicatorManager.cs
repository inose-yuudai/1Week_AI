using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RhythmIndicatorManager : MonoBehaviour
{
    [Header("インジケータ設定")]
    [SerializeField] private Slider rhythmIndicatorSlider; // 円形スライダー
    [SerializeField] private AudioSource audioSource; // 音楽再生用
    [SerializeField] private float interval = 1.364f; // 押すべきタイミング間隔（秒）

    [Header("スライダー表示設定")]
    [SerializeField] private Image fillImage; // スライダーの塗りつぶし部分

    private float currentIntervalTime; // 現在のインターバル内の経過時間
    private float musicStartTime; // 音楽再生開始時刻
    private bool isTimingActive = false; // タイミング中かどうか

    private void Start()
    {
        if (rhythmIndicatorSlider == null || audioSource == null || fillImage == null)
        {
            Debug.LogError("RhythmIndicatorManager: 必要なコンポーネントが設定されていません。");
            return;
        }

        // スライダーの初期設定
        rhythmIndicatorSlider.maxValue = interval;
        rhythmIndicatorSlider.value = 0;

        // 音楽を再生
        musicStartTime = Time.time;
        audioSource.Play();

        // タイミングシステムを開始
        StartCoroutine(UpdateRhythmIndicator());
    }

    private void Update()
    {
        if (isTimingActive)
        {
            // 現在の経過時間を更新
            currentIntervalTime += Time.deltaTime;
            rhythmIndicatorSlider.value = currentIntervalTime;

            // タイミングが終わった場合リセット
            if (currentIntervalTime >= interval)
            {
                ResetIndicator();
            }
        }
    }

    private IEnumerator UpdateRhythmIndicator()
    {
        while (audioSource.isPlaying)
        {
            isTimingActive = true;

          

            // 1フレーム待機してから通常色にフェード
            yield return new WaitForSeconds(0.1f);
            

            // 次のタイミングまで待機
            yield return new WaitForSeconds(interval - 0.1f);
        }
    }

    /// <summary>
    /// タイミング終了時にスライダーをリセット
    /// </summary>
    private void ResetIndicator()
    {
        currentIntervalTime = 0f;
        rhythmIndicatorSlider.value = 0f;
        isTimingActive = false;
    }
}
