using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HiraganaCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 fixedPosition;
    public string character;
    public RectTransform rectTransform;

    private HiraganaBoardController board;
    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private bool isDragging = false;

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
        // 見た目はそのままでOK（動かさない）
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        dragEndPos = eventData.position;
        Vector2 dragDelta = dragEndPos - dragStartPos;

        // ドラッグ方向を判定（xかyの大きい方）
        Vector2 direction = Vector2.zero;
        if (Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y))
        {
            direction = dragDelta.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            direction = dragDelta.y > 0 ? Vector2.up : Vector2.down;
        }

        // マスサイズ60なので、それに合わせた移動先を検索
        Vector2 targetPos = fixedPosition + direction * 45f;

        HiraganaCell neighbor = board.cells.Find(c => c.fixedPosition == targetPos);
        if (neighbor != null)
        {
            board.SwapCells(this, neighbor);
        }
        else
        {
            AnimateMoveTo(fixedPosition); // 失敗したら戻す
        }
    }

    public void SetCharacter(string newCharacter)
    {
        this.character = newCharacter;
        UpdateText();
    }
}
