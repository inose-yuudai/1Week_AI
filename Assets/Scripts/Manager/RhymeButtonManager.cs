using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RhymeButtonManager : MonoBehaviour
{
    [SerializeField] private RhymeChecker rhymeChecker;
    [SerializeField] private PlayerWordData playerWordData;
    [SerializeField] private RhymeButton[] rhymeButtons;
    private bool forceCallSpecialWords = false;
    private bool forceCallCriticalWords = false; // 追加: CriticalWord用フラグ

    public void SetPlayerWordsData(PlayerWordData newPlayerWordsData)
    {
        if (newPlayerWordsData == null)
        {
            Debug.LogWarning("無効な PlayerWordsData が指定されました");
            return;
        }
        playerWordData = newPlayerWordsData;
    }

    public void SetupButtonsForTurn(string[] enemyWords)
    {
        // CriticalWordsがアクティブなら最優先
        if (ShouldCallCriticalWords())
        {
            if (playerWordData.criticalWordPairs.Length == 0)
            {
                Debug.LogWarning("CriticalWordPairsが存在しません");
                return;
            }

            for (int i = 0; i < rhymeButtons.Length; i++)
            {
                var randomIndex = Random.Range(0, playerWordData.criticalWordPairs.Length);
                var criticalWord = playerWordData.criticalWordPairs[randomIndex];
                rhymeButtons[i].SetWord(criticalWord);
            }
            return; // CriticalWordsを設定したらここで終了
        }

        // 次にSpecialWordsの処理（既存のロジック）
        if (ShouldCallSpecialWords())
        {
            if (playerWordData.specialWordPairs.Length == 0)
            {
                Debug.LogWarning("SpecialWordPairsが存在しません");
                return;
            }

            for (int i = 0; i < rhymeButtons.Length; i++)
            {
                var randomIndex = Random.Range(0, playerWordData.specialWordPairs.Length);
                var specialWord = playerWordData.specialWordPairs[randomIndex];
                rhymeButtons[i].SetWord(specialWord);
            }
            return;
        }

        // 以下は通常処理（変更なし）
        if (playerWordData.playerWordPairs.Length < 4 || rhymeButtons.Length < 4)
        {
            Debug.LogWarning("十分なデータがありません");
            return;
        }

        var specialWords = GetSpecialWords(enemyWords);
        int correctWordCount = specialWords.Length > 0 ? 1 : 2;

        var correctWords = playerWordData.playerWordPairs
            .Where(pair => enemyWords.Any(enemy => rhymeChecker.IsRhyme(enemy, pair.internalWord, 5)))
            .OrderBy(_ => Random.value)
            .Take(correctWordCount)
            .ToArray();

        var wrongWords = playerWordData.playerWordPairs
            .Where(pair => !enemyWords.Any(enemy => rhymeChecker.IsRhyme(enemy, pair.internalWord, 5)))
            .OrderBy(_ => Random.value)
            .Take(2)
            .ToArray();

        var allWords = specialWords.Concat(correctWords).Concat(wrongWords).OrderBy(_ => Random.value).ToArray();

        for (int i = 0; i < rhymeButtons.Length; i++)
        {
            rhymeButtons[i].SetWord(allWords[i]);
        }
    }

    private PlayerWordPair[] GetSpecialWords(string[] enemyWords)
    {
        if (ShouldCallSpecialWords())
        {
            return playerWordData.specialWordPairs
                .OrderBy(_ => Random.value)
                .Take(1)
                .ToArray();
        }

        return new PlayerWordPair[0];
    }

    // --- CriticalWords 制御メソッド追加 ---
    public void ActivateCriticalWords()
    {
        forceCallCriticalWords = true;
    }

    public void DeactivateCriticalWords()
    {
        forceCallCriticalWords = false;
    }

    private bool ShouldCallCriticalWords()
    {
        return forceCallCriticalWords;
    }

    // --- SpecialWords 制御メソッド（既存）---
    public void ActivateSpecialWords()
    {
        forceCallSpecialWords = true;
    }

    public void DeactivateSpecialWords()
    {
        forceCallSpecialWords = false;
    }

    private bool ShouldCallSpecialWords()
    {
        return forceCallSpecialWords;
    }

    public void SetButtonsVisible(bool isVisible)
    {
        foreach (var button in rhymeButtons)
        {
            button.gameObject.SetActive(isVisible);
        }
    }

    public void ResetAllButtons()
    {
        foreach (var button in rhymeButtons)
        {
            button.ResetButtonState();
        }
    }

    public void SetButtonsInteractable(bool canInteract)
    {
        foreach (var btn in rhymeButtons)
        {
            btn.interactable = canInteract;
        }
    }
}
