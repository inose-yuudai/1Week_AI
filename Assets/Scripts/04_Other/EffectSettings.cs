using UnityEngine;

// エフェクト自身にアタッチするオブジェクト
public class EffectSettings : MonoBehaviour
{
    [SerializeField, Header("Effectを消す時間")]
    private float _duration;

    private void Start()
    {
        Destroy(gameObject, _duration);
    }
}