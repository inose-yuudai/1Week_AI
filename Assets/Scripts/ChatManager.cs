using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class ChatManager : MonoBehaviour
{
    [Header("吹き出しプレハブ & 表示設定")]
    public RectTransform chatArea;
    public int maxMessages = 5;

    [Header("プレイヤーとAIの吹き出し設定")]
    public GameObject playerBubblePrefab;
    public GameObject aiBubblePrefab;

    public Vector2 playerStartPos = new Vector2(200, -200);
    public Vector2 aiStartPos = new Vector2(-200, -200);

    public float playerYSpacing = 80f;
    public float aiYSpacing = 60f;

    private List<GameObject> messageBubbles = new List<GameObject>();

    [Header("効果音設定")]
    public AudioSource audioSource; // 効果音を再生するAudioSource
    public AudioClip playerMessageSound; // プレイヤーのメッセージ効果音
    public AudioClip aiMessageSound; // AIのメッセージ効果音

    /// <summary>
    /// プレイヤーかAIかを指定してメッセージを追加
    /// </summary>
    public GameObject AddChatMessage(
        string message,
        GameObject prefab,
        Vector2 spawnPosition,
        bool isPlayer
    )
    {
        float spacing = isPlayer ? playerYSpacing : aiYSpacing;

        // 吹き出し生成
        GameObject bubble = Instantiate(prefab, chatArea);
        bubble.transform.localPosition = spawnPosition;

        // テキストセット
        TMP_Text text = bubble.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = message;

        // 効果音再生
        PlayMessageSound(isPlayer);

        // 既存の吹き出しを上にずらす（個別spacingで）
        foreach (GameObject obj in messageBubbles)
        {
            RectTransform oldRect = obj.GetComponent<RectTransform>();
            oldRect.DOAnchorPosY(oldRect.anchoredPosition.y + spacing, 0.3f).SetEase(Ease.OutCubic);
        }

        messageBubbles.Add(bubble);

        // 古い吹き出し削除
        if (messageBubbles.Count > maxMessages)
        {
            Destroy(messageBubbles[0]);
            messageBubbles.RemoveAt(0);
        }

        return bubble;
    }

    /// <summary>
    /// 効果音を再生
    /// </summary>
    private void PlayMessageSound(bool isPlayer)
    {
        if (audioSource != null)
        {
            AudioClip clip = isPlayer ? playerMessageSound : aiMessageSound;
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
