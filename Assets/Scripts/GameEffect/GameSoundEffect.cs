using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class GameSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private IObjectPool<GameSoundEffect> objectPool;

    public void SetAudio(AudioMixerGroup audioMixer,AudioClip audioClip)
    {
        audioSource.outputAudioMixerGroup = audioMixer;
        audioSource.clip = audioClip;
        audioSource.Play();
        Invoke(nameof(Hide), audioSource.clip.length);
    }

    private void Hide()
    {
        objectPool.Release(this);
    }

    public void SetObjectPool(IObjectPool<GameSoundEffect> objectPool)
    {
        this.objectPool = objectPool;
    }
}
