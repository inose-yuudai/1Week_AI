using UnityEngine;
using DG.Tweening;

public class FlappingObject : MonoBehaviour
{
    [Tooltip("最大回転角度（度）")]
    public float maxAngle = 30f;
    [Tooltip("1回の開閉にかかる時間（秒）")]
    public float flapCycleDuration = 1f;

    void Start()
    {
        // UIのRectTransformの場合、pivotを右端（1, 0.5）に設定
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.pivot = new Vector2(1f, 0.5f);
        }
        else
        {
            // 非UIの場合は、親オブジェクトを使ってピボットを調整する方法を検討してください。
            Debug.LogWarning("RectTransformが見つからなかったので、右端をピボットにする設定はできませんでした");
        }

        // 右端を中心に0°～maxAngleまでパタパタ動かすTweenシーケンス
        Sequence flapSequence = DOTween.Sequence();
        // 右端をピボットとして、0°からmaxAngleへ回転
        flapSequence.Append(transform.DOLocalRotate(new Vector3(0, 0, maxAngle), flapCycleDuration / 2)
            .SetEase(Ease.InOutSine));
        // maxAngleから0°に戻す
        flapSequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 0), flapCycleDuration / 2)
            .SetEase(Ease.InOutSine));
        flapSequence.SetLoops(-1);
    }
}
