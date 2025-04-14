using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class EnemyWordPresenter : MonoBehaviour
{
    [SerializeField] private EnemyWordsData[] enemyWordsDataList;
    [SerializeField] private Text enemyWordText1;
    [SerializeField] private Text enemyWordText2;
    [SerializeField] private RectTransform speechBubble1;
    [SerializeField] private RectTransform speechBubble2;
    [SerializeField] private AudioSource audioSource;

    public RhymeButton rhymeButton;

    private EnemyWordsData currentEnemyWordsData;
    private EnemyWordPair[] currentEnemyWordPairs = new EnemyWordPair[2];

    private bool forceCriticalWords = false;
    private bool forceSpecialWords = false;

    public string[] CurrentEnemyWords => new string[]
    {
        currentEnemyWordPairs[0].internalWord,
        currentEnemyWordPairs[1].internalWord
    };

    public AudioClip[] CurrentEnemyVoiceClips => new AudioClip[]
    {
        currentEnemyWordPairs[0].voiceClip,
        currentEnemyWordPairs[1].voiceClip
    };

    public AudioSource AudioSource => audioSource;

    private void Start()
    {
        if (enemyWordsDataList.Length > 0)
        {
            SetEnemyWordsData(enemyWordsDataList[0]);
        }
        else
        {
            Debug.LogWarning("EnemyWordsData が設定されていません");
        }
    }

    public void SetEnemyWordsData(EnemyWordsData newEnemyWordsData)
    {
        if (newEnemyWordsData == null)
        {
            Debug.LogWarning("無効な EnemyWordsData が指定されました");
            return;
        }
        currentEnemyWordsData = newEnemyWordsData;
    }

    public void ShowRandomEnemyWords()
    {
        if (currentEnemyWordsData == null)
        {
            Debug.LogWarning("EnemyWordsDataが設定されていません");
            return;
        }

        rhymeButton.HideCurrentBubble();

        if (forceCriticalWords && currentEnemyWordsData.criticalEnemyWordPairs.Length >= 1)
        {
            SetFixedEnemyWords(currentEnemyWordsData.criticalEnemyWordPairs);
        }
        else if (forceSpecialWords && currentEnemyWordsData.specialEnemyWordPairs.Length >= 1)
        {
            SetFixedEnemyWords(currentEnemyWordsData.specialEnemyWordPairs);
        }
        else
        {
            if (currentEnemyWordsData.enemyWordPairs.Length < 2)
            {
                Debug.LogWarning("EnemyWordsData に十分な単語がありません");
                return;
            }

            var shuffledPairs = currentEnemyWordsData.enemyWordPairs.OrderBy(x => Random.value).ToArray();
            currentEnemyWordPairs[0] = shuffledPairs[0];
            currentEnemyWordPairs[1] = shuffledPairs[1];

            enemyWordText1.text = currentEnemyWordPairs[0].displayWord;
            enemyWordText2.text = currentEnemyWordPairs[1].displayWord;
        }

        AnimateEnemyWords();
    }

    private void SetFixedEnemyWords(EnemyWordPair[] fixedPairs)
    {
        currentEnemyWordPairs[0] = fixedPairs[0];
        currentEnemyWordPairs[1] = fixedPairs[1];

        enemyWordText1.text = currentEnemyWordPairs[0].displayWord;
        enemyWordText2.text = currentEnemyWordPairs[1].displayWord;
    }

    private void AnimateEnemyWords()
    {
        if (enemyWordText1 != null)
        {
            Vector3 originalScale = enemyWordText1.transform.localScale;
            enemyWordText1.transform.localScale = Vector3.zero;
            enemyWordText1.transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);
        }
        if (enemyWordText2 != null)
        {
            Vector3 originalScale = enemyWordText2.transform.localScale;
            enemyWordText2.transform.localScale = Vector3.zero;
            enemyWordText2.transform.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);
        }
        if (speechBubble1 != null)
        {
            Vector3 originalScale = speechBubble1.localScale;
            speechBubble1.localScale = Vector3.zero;
            speechBubble1.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);
        }
        if (speechBubble2 != null)
        {
            Vector3 originalScale = speechBubble2.localScale;
            speechBubble2.localScale = Vector3.zero;
            speechBubble2.DOScale(originalScale, 0.5f).SetEase(Ease.OutBack);
        }
    }

    public Transform GetEnemyWordText1Transform()
    {
        return enemyWordText1?.transform;
    }

    public Transform GetEnemyWordText2Transform()
    {
        return enemyWordText2?.transform;
    }

    // --- クリティカルの有効化・無効化 ---
    public void ActivateCriticalEnemyWords()
    {
        forceCriticalWords = true;
    }

    public void DeactivateCriticalEnemyWords()
    {
        forceCriticalWords = false;
    }

    // --- スペシャルの有効化・無効化 ---
    public void ActivateSpecialEnemyWords()
    {
        forceSpecialWords = true;
    }

    public void DeactivateSpecialEnemyWords()
    {
        forceSpecialWords = false;
    }
}
