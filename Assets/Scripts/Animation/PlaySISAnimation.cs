using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening; // DOTween を使うために必要

public class PlaySISAnimation : MonoBehaviour
{
    public PlayableDirector playableDirector; // Timeline用
    public GameObject animationObject; // アニメーションのGameObject
    public Canvas targetCanvas; // ソート順を変更するCanvas
    public AudioClip animationSound; // アニメーションの効果音
    public AudioSource audioSource; // 効果音再生用のAudioSource

    private int originalSortOrder; // 元のソート順

    void Start()
    {
        // 再生を止めて初期状態に
        if (playableDirector != null)
        {
            playableDirector.Stop();
            playableDirector.time = 0;
        }

        // アニメーションオブジェクトを非アクティブにする
        if (animationObject != null)
        {
            animationObject.SetActive(false);
        }

        // Canvas の元のソート順を記憶
        if (targetCanvas != null)
        {
            originalSortOrder = targetCanvas.sortingOrder;
        }
    }

    /// <summary>
    /// アニメーションを再生するメソッド
    /// </summary>
    public void PlayAnimation()
    {
        if (playableDirector != null && animationObject != null)
        {
            // アニメーションオブジェクトをアクティブにする
            animationObject.SetActive(true);

            // Canvas のソート順を変更
            if (targetCanvas != null)
            {
                targetCanvas.sortingOrder = -10;
            }

            // 効果音を再生
            if (audioSource != null && animationSound != null)
            {
                audioSource.PlayOneShot(animationSound);
            }

            // アニメーションを最初から再生
            playableDirector.time = 0;
            playableDirector.Play();

            // 再生完了時に自動でオブジェクトを非アクティブにする
            StartCoroutine(WaitForAnimationEnd());
        }
    }

    /// <summary>
    /// アニメーション終了後の処理
    /// </summary>
    private System.Collections.IEnumerator WaitForAnimationEnd()
    {
        // アニメーションが終わるまで待機
        yield return new WaitForSeconds((float)playableDirector.duration);

        // Canvas のソート順を元に戻す
        if (targetCanvas != null)
        {
            targetCanvas.sortingOrder = originalSortOrder;
        }

        // アニメーションオブジェクトを非アクティブにする
        if (animationObject != null)
        {
            animationObject.SetActive(false);
        }
    }
}
