using UnityEngine;
using System.Collections;

public class RapidPunchEffect : MonoBehaviour
{
    [Header("パンチエフェクトのPrefab")]
    public GameObject punchEffectPrefab;

    [Header("不正解エフェクトのPrefab")]
    public GameObject incorrectEffectPrefab;

    [Header("パンチサウンド")]
    public AudioClip punchSound;

    [Header("不正解サウンド")]
    public AudioClip incorrectSound;

    [Header("1回のエフェクトで出す回数")]
    [Range(1, 10)]
    public int effectCount = 5;

    [Header("エフェクトを連続生成する間隔(秒)")]
    [Range(0.01f, 1f)]
    public float effectInterval = 0.1f;

    // エフェクト生成位置のオフセット
    public Vector3 effectOffset = new Vector3(0, 1, 1);

    private AudioSource audioSource;

    public void Awake()
    {
        // AudioSourceの用意
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2Dサウンドの場合
    }

    /// <summary>
    /// 正解エフェクトを発動
    /// </summary>
    public void PlayCorrectEffect()
    {
        StartCoroutine(PlayEffect(punchEffectPrefab, punchSound));
    }

    /// <summary>
    /// 不正解エフェクトを発動
    /// </summary>
    public void PlayIncorrectEffect()
    {
        StartCoroutine(PlayEffect(incorrectEffectPrefab, incorrectSound));
    }

    /// <summary>
    /// エフェクトを連続生成
    /// </summary>
    private IEnumerator PlayEffect(GameObject effectPrefab, AudioClip sound)
    {
        for (int i = 0; i < effectCount; i++)
        {
            // エフェクト生成
            if (effectPrefab != null)
            {
                Vector3 spawnPos = transform.position + transform.forward * effectOffset.z
                                                      + transform.up * effectOffset.y
                                                      + transform.right * effectOffset.x*2;
                Instantiate(effectPrefab, spawnPos, Quaternion.identity);
            }

            // サウンド再生
            if (sound != null && audioSource != null)
            {
                audioSource.PlayOneShot(sound);
            }

            // 次のエフェクトまでの間隔
            yield return new WaitForSeconds(effectInterval);
        }
    }
}
