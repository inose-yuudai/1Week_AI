using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Fungus;
using System.Collections.Generic;

public class HiraganaCell
    : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IPointerClickHandler
{
    public Vector2 fixedPosition;
    public string character;
    public RectTransform rectTransform;

    private HiraganaBoardController board;
    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private bool isDragging = false;

    private Image image;

    [Header("サウンド設定")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip swapSound;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetHighlight(bool highlight, Color? color = null)
    {
        if (image == null)
            return;

        image.color = highlight ? (color ?? Color.yellow) : Color.white;
    }

    public void Initialize(string character, Vector2 pos, HiraganaBoardController controller)
    {
        this.character = character;
        this.fixedPosition = pos;
        this.board = controller;

        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pos;

        UpdateText();
    }

    public void UpdateText()
    {
        var text = GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = character;
        }
    }

    public void AnimateMoveTo(Vector2 targetPos, float duration = 0.2f)
    {
        rectTransform.DOAnchorPos(targetPos, duration).SetEase(Ease.OutQuad);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos = eventData.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 見た目はそのままでOK
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        dragEndPos = eventData.position;
        Vector2 dragDelta = dragEndPos - dragStartPos;

        Vector2 direction = Vector2.zero;
        if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
            direction = dragDelta.x > 0 ? Vector2.right : Vector2.left;
        else
            direction = dragDelta.y > 0 ? Vector2.up : Vector2.down;

        Vector2 targetPos = fixedPosition + direction * 75f;

        HiraganaCell neighbor = board.cells.Find(c => c.fixedPosition == targetPos);
        if (neighbor != null)
        {
            board.SwapCells(this, neighbor);
            board.RefreshDefaultHighlights();

            // ✅ 入れ替え時の効果音
            if (swapSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(swapSound);
            }
        }
        else
        {
            AnimateMoveTo(fixedPosition);
        }

        Flowchart.BroadcastFungusMessage("ChangeCell");
        Flowchart.BroadcastFungusMessage("光る");
    }

    public void SetCharacter(string newCharacter)
    {
        this.character = newCharacter;
        UpdateText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleCharacterVariant();

        // ✅ クリック時の効果音
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        Flowchart.BroadcastFungusMessage("ChangeCell");
    }

    private void ToggleCharacterVariant()
    {
        List<List<string>> variantGroups = new List<List<string>>
        {
            new List<string> { "か", "が" },
            new List<string> { "き", "ぎ" },
            new List<string> { "く", "ぐ" },
            new List<string> { "け", "げ" },
            new List<string> { "こ", "ご" },
            new List<string> { "さ", "ざ" },
            new List<string> { "し", "じ" },
            new List<string> { "す", "ず" },
            new List<string> { "せ", "ぜ" },
            new List<string> { "そ", "ぞ" },
            new List<string> { "た", "だ" },
            new List<string> { "ち", "ぢ" },
            new List<string> { "つ", "づ", "っ" },
            new List<string> { "て", "で" },
            new List<string> { "と", "ど" },
            new List<string> { "は", "ば", "ぱ" },
            new List<string> { "ひ", "び", "ぴ" },
            new List<string> { "ふ", "ぶ", "ぷ" },
            new List<string> { "へ", "べ", "ぺ" },
            new List<string> { "ほ", "ぼ", "ぽ" },
            new List<string> { "あ", "ぁ" },
            new List<string> { "い", "ぃ" },
            new List<string> { "う", "ぅ" },
            new List<string> { "え", "ぇ" },
            new List<string> { "お", "ぉ" },
            new List<string> { "や", "ゃ" },
            new List<string> { "ゆ", "ゅ" },
            new List<string> { "よ", "ょ" },
            new List<string> { "わ", "ゎ" },
        };

        foreach (var group in variantGroups)
        {
            int index = group.IndexOf(character);
            if (index != -1)
            {
                int nextIndex = (index + 1) % group.Count;
                character = group[nextIndex];
                UpdateText();
                return;
            }
        }
    }
}
