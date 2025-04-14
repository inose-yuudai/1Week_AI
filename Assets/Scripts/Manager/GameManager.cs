using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyWordPresenter enemyWordPresenter;
    [SerializeField] private RhymeButtonManager rhymeButtonManager;
    [SerializeField] private RhymeChecker rhymeChecker;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TimeManager timeManager;

    public int timing = 0;

    [Header("EnemyWordsData の管理")]
    [SerializeField] private EnemyWordsData[] enemyWordsDataList; // 複数の EnemyWordsData
    [SerializeField] private PlayerWordData[] PlayerWordsDataList; // 複数の PlayerWordsData

    // エフェクト関連
    [SerializeField] private RapidPunchEffect punchEffectController;       // 正解エフェクト
    [SerializeField] private RapidPunchEffect incorrectEffectController;   // 不正解エフェクト
    [SerializeField] private AudioClip correctSound;                       // 正解音

    [Header("エフェクト表示位置")]
    [SerializeField] private Vector3 EffectPosition = new Vector3(5, 0, 0);

    [Header("エフェクト間隔(秒)")]
    [SerializeField] private float effectDelay = 0.5f;

     [Header("Lastエフェクト設定")]
    [SerializeField] private GameObject effectPrefab;  // 通常のエフェクト
    [SerializeField] private GameObject explosionEffectPrefab; // 最後の大爆発
    [SerializeField] private int effectCount = 150;    // 生成するエフェクトの数
    [SerializeField] private float effectDuration = 0.7f; // エフェクトが消えるまでの時間
    [SerializeField] private float effectRadius = 5f; // 画面中央からのばらつき範囲
    [SerializeField] private float spawnInterval = 0.02f; // 連撃エフェクトの発生間隔

     [Header("SE設定")]
    [SerializeField] private AudioClip[] seClips;      // 連撃用SEリスト
    [SerializeField] private AudioClip finalExplosionSE; // 最後の爆発音
    [SerializeField] private AudioSource audioSource; // SEを鳴らすAudioSource
    [SerializeField] private int maxSimultaneousSE = 8; // 同時に鳴らせるSEの最大数
    [SerializeField] private float seInterval = 0.05f; // SEの発生間隔

    private string[] currentEnemyWords = new string[2]; // 敵が出す2つの単語
    private string[] playerSelections = new string[2];  // プレイヤーが選んだ2つの単語
    private int currentSelectionIndex = 0;              // プレイヤーの選択順序を追跡
    private bool isPlayingAudio = false;                // 再生中フラグ
    private bool isGameStart = false;                   // ゲーム再生フラグ

    /// <summary>
    /// EnemyWordsData を指定してセット
    /// </summary>
    public void SetEnemyWordsData(EnemyWordsData newData)
    {
        if (newData == null)
        {
            Debug.LogWarning("無効な EnemyWordsData が指定されました");
            return;
        }
        enemyWordPresenter.SetEnemyWordsData(newData);
        Debug.Log($"EnemyWordsData を {newData.name} にセットしました");
    }
    public void SetPlayerWordsData(PlayerWordData newData)
    {
        if (newData == null)
        {
            Debug.LogWarning("無効な PlayerWordsData が指定されました");
            return;
        }
        rhymeButtonManager.SetPlayerWordsData(newData);
        Debug.Log($"PlayerWordsData を {newData.name} にセットしました");
    }

    /// <summary>
    /// 任意のタイミングで呼び出される新しいターンの開始
    /// </summary>
    public void StartNewTurn()
    {
        Debug.Log("新しいターンを開始します");
        if (!isGameStart)
        {
            timeManager.StartTimer();
            isGameStart = true;
        }
        currentSelectionIndex = 0;

        // 敵の単語を2つ選んで表示
        enemyWordPresenter.ShowRandomEnemyWords();
        Debug.Log("ゲームを開始します");
        currentEnemyWords = enemyWordPresenter.CurrentEnemyWords;

        Debug.Log($"敵の単語: {currentEnemyWords[0]}, {currentEnemyWords[1]}");

        // 韻が合う選択肢を含むボタンをセットアップ
        rhymeButtonManager.SetupButtonsForTurn(currentEnemyWords);

        // ボタンを押せない状態にしておく
        rhymeButtonManager.SetButtonsInteractable(false);

        // 敵ボイスを順番に再生（再生終了後にボタンを押せるようにする）
        StartCoroutine(PlayEnemyVoices());
    }

    /// <summary>
    /// ボイスを順番に再生するコルーチン
    /// </summary>
    private IEnumerator PlayEnemyVoices()
    {
        isPlayingAudio = true;

        var enemyClips = enemyWordPresenter.CurrentEnemyVoiceClips;
        var audioSource = enemyWordPresenter.AudioSource;

        foreach (var clip in enemyClips)
        {
            if (clip != null && audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
                yield return new WaitForSeconds(audioSource.clip.length);
            }
        }

        isPlayingAudio = false;

        // ボタンを押せるようにする
        rhymeButtonManager.SetButtonsInteractable(true);
    }

    /// <summary>
    /// プレイヤーが選択した単語を処理
    /// </summary>
    public void OnPlayerSelect(string selectedWord)
    {
        if (isPlayingAudio)
        {
            Debug.LogWarning("再生中は選択できません");
            return;
        }

        if (currentSelectionIndex >= 2)
        {
            Debug.LogWarning("すでに2つの選択が完了しています");
            return;
        }

        playerSelections[currentSelectionIndex] = selectedWord;
        currentSelectionIndex++;

        if (currentSelectionIndex == 2)
        {
            rhymeButtonManager.SetButtonsInteractable(false);
            StartCoroutine(EvaluateSelectionsAndShowEffects());
        }
    }

    /// <summary>
    /// 選択結果を評価し、正解/不正解ごとにエフェクトを表示
    /// </summary>
    private IEnumerator EvaluateSelectionsAndShowEffects()
    {
        bool[] isCorrect = new bool[2];

        for (int i = 0; i < 2; i++)
        {
            isCorrect[i] = rhymeChecker.IsRhyme(currentEnemyWords[i], playerSelections[i], 3);
            Debug.Log(
                isCorrect[i]
                    ? $"選択 {i + 1}: 正解！ (Enemy: {currentEnemyWords[i]}, Player: {playerSelections[i]})"
                    : $"選択 {i + 1}: 不正解 (Enemy: {currentEnemyWords[i]}, Player: {playerSelections[i]})"
            );

            // ★正解の場合はスコア加算（ここに追加）
            if (isCorrect[i])
            {
                scoreManager.AddScore(1 + timing);
            }
        }

        ShowEffect(isCorrect[0], EffectPosition);
        yield return new WaitForSeconds(effectDelay);

        ShowEffect(isCorrect[1], EffectPosition);

        if (isCorrect[0] && isCorrect[1] && correctSound != null)
        {
            var audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
        }

        yield return new WaitForSeconds(effectDelay);

        rhymeButtonManager.ResetAllButtons();
        StartNewTurn();
    }


    /// <summary>
    /// 正解/不正解に応じたエフェクトを表示
    /// </summary>
    private void ShowEffect(bool isCorrect, Vector3 position)
    {
        RapidPunchEffect effectController = isCorrect ? punchEffectController : incorrectEffectController;

        if (effectController != null)
        {
            effectController.transform.position = position;
            if (isCorrect)
            {
                effectController.PlayCorrectEffect();

            }
            else
            {
                effectController.PlayIncorrectEffect();
            }
        }
    }
public void ShowLastEffect()
    {
        StartCoroutine(SpawnRapidEffects());
    }

    private IEnumerator SpawnRapidEffects()
    {
        Vector3 centerPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(new Vector3(centerPosition.x, centerPosition.y, 10));

        for (int i = 0; i < effectCount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-effectRadius, effectRadius),
                Random.Range(-effectRadius, effectRadius),
                0
            );

            GameObject effect = Instantiate(effectPrefab, worldCenter + randomOffset, Quaternion.identity);
            effect.transform.localScale = Vector3.zero;
            effect.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            Destroy(effect, effectDuration);

            if (i % 10 == 0) // 10回に1回SEを鳴らす
            {
                PlayRandomSE();
            }

            yield return new WaitForSeconds(spawnInterval); // 連撃感を出す
        }

        yield return new WaitForSeconds(0.5f); // 連撃エフェクトの余韻

        SpawnFinalExplosion(worldCenter);
    }

    private void PlayRandomSE()
    {
        if (seClips.Length > 0 && audioSource != null)
        {
            AudioClip clip = seClips[Random.Range(0, seClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void SpawnFinalExplosion(Vector3 position)
    {
        GameObject explosion = Instantiate(explosionEffectPrefab, position, Quaternion.identity);
        explosion.transform.localScale = Vector3.zero;
        explosion.transform.DOScale(Vector3.one * 3f, 0.5f).SetEase(Ease.OutBack);
        Destroy(explosion, 1.5f);

        if (finalExplosionSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(finalExplosionSE);
        }
    }
}