using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCooldown : MonoBehaviour
{
    [SerializeField]
    private float cooldownTime = 2f; // 再度押せるようになるまでの秒数
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    void OnButtonClicked()
    {
        // ここに押された時の処理を書く
        Debug.Log("ボタンが押されました");

        // ボタンを非活性化
        button.interactable = false;

        // クールダウンを開始
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownTime);
        button.interactable = true;
    }
}
