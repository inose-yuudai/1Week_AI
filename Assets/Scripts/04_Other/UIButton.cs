using System;
using UnityEngine;
using UnityEngine.EventSystems;

// ボタンにアタッチするクラス
public class UIButton : MonoBehaviour,IPointerClickHandler
{
    private Action _onClickAction;
    
    // ボタンにイベントを登録する
    public void OnClickButton(Action action)
    {
        _onClickAction += action;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _onClickAction?.Invoke();
    }
}
