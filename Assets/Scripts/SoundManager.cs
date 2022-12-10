using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : FastSingleton<SoundManager>
{
    [SerializeField] AudioSource soundSource;
    public List<AudioClip> listSound;
    public void PlaySound(SoundType type)
    {
        soundSource.clip = listSound[(int)type];
        soundSource.Play();
    }
}
