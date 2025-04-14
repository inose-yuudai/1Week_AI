using UnityEngine;
using DG.Tweening;

public class VerticalSquash : MonoBehaviour
{
    [Header("設定したい BPM (例: 88)")]
    public float bpm = 88f;

    [Header("屈伸時のY縮尺 (例: 0.9)")]
    public float scaleDownFactor = 0.9f;

    private void Start()
    {
        // 1拍あたりの長さを計算
        float beatTime = 60f / bpm;    // BPM=88 の場合 約0.68秒

        // 元のスケールを取得
        Vector3 originalScale = transform.localScale;

        // 縦方向(Y軸)だけ縮めたスケールを計算
        Vector3 shrunkenScale = new Vector3(
            originalScale.x,
            originalScale.y * scaleDownFactor,
            originalScale.z
        );

        // DOTween Sequence を作成
        Sequence seq = DOTween.Sequence()
            // 半拍(beatTime/2)かけて Y 軸を縮める
            .Append(transform.DOScale(shrunkenScale, beatTime / 2f)
                .SetEase(Ease.OutSine))
            // 半拍かけて元のスケールに戻す
            .Append(transform.DOScale(originalScale, beatTime / 2f)
                .SetEase(Ease.InSine))
            // 無限ループ
            .SetLoops(-1, LoopType.Restart);
    }
}