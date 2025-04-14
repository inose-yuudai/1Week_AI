using UnityEngine;

/// <summary>Effectを生成するクラス</summary>
public class PlayEffect : MonoBehaviour
{
    // エフェクトを生成する
    public void EffectInstance(Vector3 position, string effectName)
    {
        // EffectをResourcesフォルダから探してくる
        EffectSettings effectPrefab = Resources.Load<EffectSettings>(effectName);

        // エフェクトを生成する
        EffectSettings effectInstance = Instantiate(
            effectPrefab,
            position,
            Quaternion.Euler(
                effectPrefab.transform.rotation.x,
                effectPrefab.transform.rotation.y,
                effectPrefab.transform.rotation.z
            )
        );
    }
}
