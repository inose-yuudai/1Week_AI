using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using DG.Tweening; // DOTween の名前空間

public class RhymeButton : MonoBehaviour
{
    [SerializeField] private Text buttonText;
    [SerializeField] private Image buttonBackground;  // ボタン背景部分の Image
    [SerializeField] private Color defaultColor = Color.white;   // 初期状態の色
    [SerializeField] private Color selectedColor = Color.green;    // 選択状態の色
    [SerializeField] private Flowchart flowchart;

    // 吹き出し用Prefab（吹き出し内にTextなどを含む）
    [SerializeField] private GameObject speechBubblePrefab;
    // 吹き出し生成時の基準となる位置（ワールド座標で指定）
    [SerializeField] private Transform speechBubbleFirstPosition;
    [SerializeField] private Transform speechBubbleSecondPosition;

    // アニメーションでの拡大倍率（1ならPrefabのサイズそのまま、1より大きいと拡大表示）
    [SerializeField] private float animationScaleMultiplier = 1f;

    // 各吹き出しに適用する回転オフセット
    [SerializeField] private float firstRotationZ = 0f;  // 奇数回の場合、X軸回転
    [SerializeField] private float secondRotationZ = 0f; // 偶数回の場合、Z軸回転

    // 外部から Hide 呼び出し時に、拡大アニメーション終了後に待つ遅延時間（秒）
    [SerializeField] private float bubbleFadeDelay = 0.2f;
    // フェードアウトにかける時間（秒）
    [SerializeField] private float bubbleFadeDuration = 0.5f;

    private string internalPlayerWord;  // 内部判定用の文字列
    private bool isSelected = false;    // ボタンが選択済みかどうか
    private Button buttonComponent;     // ボタンコンポーネント

    // ボタンが押された回数（全インスタンス共通の場合）
    private static int pressCount = 0;

    // 生成された吹き出しを保持するリスト（外部から Hide 用に呼び出せるようにする）
    private List<GameObject> currentBubbles = new List<GameObject>();
    [SerializeField] private Transform bubbleFolder;

    private void Awake()
    {
        // ボタンコンポーネントを取得
        buttonComponent = GetComponent<Button>();
        if (buttonComponent == null)
        {
            Debug.LogError("RhymeButton: Button コンポーネントがアタッチされていません");
        }
    }
    public bool interactable
    {
        get => (buttonComponent != null) && buttonComponent.interactable;
        set
        {
            if (buttonComponent != null)
            {
                buttonComponent.interactable = value;
            }
        }
    }

    /// <summary>
    /// ボタンに単語をセットする
    /// </summary>
    public void SetWord(PlayerWordPair pair)
    {
        internalPlayerWord = pair.internalWord;
        if (buttonText != null)
        {
            buttonText.text = pair.displayWord;
        }
        ResetButtonState();
    }

    /// <summary>
    /// ボタンがクリックされたときの処理
    /// </summary>
    public void OnButtonClick()
    {
        if (isSelected) return; // すでに選択されている場合は無視

        // 吹き出しを生成して、ボタンのテキストを反映（アニメーション付き）
        ShowSpeechBubble();


      switch (internalPlayerWord)
{
    case "せきずい":
        flowchart.ExecuteBlock("脊髄");
        break;

    case "ぶべつ":
        flowchart.ExecuteBlock("侮蔑");
        break;
    case "ビッグマウス":
        flowchart.ExecuteBlock("ビッグマウス");
        break;
    case "こうどう":
        flowchart.ExecuteBlock("行動");
        break;
    case "デジハリ" :
        flowchart.ExecuteBlock("デジハリ");
        break;
    case "ぼけつ":
        flowchart.ExecuteBlock("墓穴");
        break;
    case "てんぱ":
        flowchart.ExecuteBlock("天パ");
        break;
    case "かき":
        flowchart.ExecuteBlock("牡蠣");
        break;

    default:
        ExecuteOtherProcesses();
        break;
}

    }

    /// <summary>
    /// アニメーションを再生し、その後に処理を続行するコルーチン
    /// </summary>
    private System.Collections.IEnumerator PlayAnimationAndContinue(PlaySISAnimation sisAnimator)
    {
        sisAnimator.PlayAnimation();
        yield return new WaitForSeconds((float)sisAnimator.playableDirector.duration);
        ExecuteOtherProcesses();
    }

    /// <summary>
    /// 既存の処理をまとめたメソッド
    /// </summary>
    public void ExecuteOtherProcesses()
    {
        isSelected = true;
        ChangeButtonColor(selectedColor);

        if (buttonComponent != null)
        {
            buttonComponent.interactable = false;
        }

        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.OnPlayerSelect(internalPlayerWord);
        }
    }

    /// <summary>
    /// ボタンの色を変更する
    /// </summary>
    private void ChangeButtonColor(Color color)
    {
        if (buttonBackground != null)
        {
            buttonBackground.color = color;
        }
        else
        {
            Debug.LogWarning("RhymeButton: buttonBackground が設定されていません");
        }
    }

    /// <summary>
    /// ボタンの状態をリセットする
    /// </summary>
    public void ResetButtonState()
    {
        isSelected = false;
        ChangeButtonColor(defaultColor);
        if (buttonComponent != null)
        {
            buttonComponent.interactable = true;
        }
    }

    /// <summary>
    /// 吹き出し内にボタンのテキストを表示し、設定した倍率分まで拡大するアニメーションを実行する。
    /// 奇数回は speechBubbleFirstPosition、偶数回は speechBubbleSecondPosition の位置に生成し、
    /// 各場合に応じた回転オフセットを付与します。
    /// 自動で HideBubble() は呼ばず、外部からの呼び出しで非表示にできるように currentBubbles に追加します。
    /// （Canvas内に生成）
    /// </summary>
    private void ShowSpeechBubble()
    {
        if (speechBubblePrefab == null)
        {
            Debug.LogWarning("RhymeButton: speechBubblePrefab が設定されていません");
            return;
        }

        int currentPress = pressCount;
        pressCount++;

        Vector3 spawnPosition = Vector3.zero;
        Transform parentTransform = null;
        Quaternion rotationOffset = Quaternion.identity;

        if (currentPress % 2 == 0 && speechBubbleFirstPosition != null)
        {
            spawnPosition = speechBubbleFirstPosition.position;
            parentTransform = speechBubbleFirstPosition.parent; // Canvas内にある前提
            rotationOffset = Quaternion.Euler( 0f, 0f,firstRotationZ);
        }
        else if (currentPress % 2 == 1 && speechBubbleSecondPosition != null)
        {
            spawnPosition = speechBubbleSecondPosition.position;
            parentTransform = speechBubbleSecondPosition.parent;
            rotationOffset = Quaternion.Euler(0f, 0f, secondRotationZ);
        }
        else
        {
            spawnPosition = transform.position;
            parentTransform = transform.parent;
        }

        // 親を指定して吹き出しPrefabを生成（Canvas内に生成）
        GameObject bubble = Instantiate(speechBubblePrefab, spawnPosition, Quaternion.identity, parentTransform);
        // 回転オフセットを付与
        bubble.transform.rotation *= rotationOffset;

        // 吹き出し内のTextコンポーネントを取得して、ボタンのテキストを反映
        Text bubbleText = bubble.GetComponentInChildren<Text>();
        if (bubbleText != null)
        {
            bubbleText.text = buttonText.text;
        }

        // 初期スケールを0にしてターゲットスケールへアニメーション
        Vector3 originalScale = bubble.transform.localScale;
        bubble.transform.localScale = Vector3.zero;
        Vector3 targetScale = originalScale * animationScaleMultiplier;
        bubble.transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBack);

        // リストに追加しておく
        currentBubbles.Add(bubble);
    }

    /// <summary>
    /// 外部から呼び出せる、現在生成されているすべての吹き出しを非表示にする処理
    /// </summary>
   public void HideCurrentBubble()
    {
        if(bubbleFolder == null)
        {
            return;
        }
        foreach(Transform child in bubbleFolder)
        {
            // speechBubbleFirstPosition と speechBubbleSecondPosition に対応するオブジェクトは除外
            if(child == speechBubbleFirstPosition || child == speechBubbleSecondPosition)
            {
                continue;
            }
            if(child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }

    }

    /// <summary>
    /// 吹き出しオブジェクトをフェードアウトさせて非表示にし、Destroy する処理
    /// </summary>
    /// <param name="bubble">対象の吹き出しオブジェクト</param>
    private void HideBubble(GameObject bubble)
    {
        if (bubble == null) return;
        // Tween を kill してからフェードアウト開始
        bubble.transform.DOKill();
        CanvasGroup cg = bubble.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = bubble.AddComponent<CanvasGroup>();
        }
        cg.DOKill();
        cg.DOFade(0, bubbleFadeDuration).OnComplete(() => Destroy(bubble));
    }
}
