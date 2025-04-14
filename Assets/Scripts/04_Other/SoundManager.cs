using UnityEngine;

/// <summary>サウンドを鳴らす機構 </summary>
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [Header("Audioを鳴らすオブジェクト")] 
    [SerializeField] private AudioSource[] _seAudioSources;

    // まだ鳴らしていないAudioにクリップをいれる
    public void PlaySE(AudioClip clip)
    {
        for (int i = 0; i < _seAudioSources.Length; i++)
        {
            // 現在鳴っている状態かどうかを確認
            if (_seAudioSources[i].isPlaying)
            {
                continue;
            }

            _seAudioSources[i].clip = clip;
            _seAudioSources[i].Play();
            break;
        }
    }
}