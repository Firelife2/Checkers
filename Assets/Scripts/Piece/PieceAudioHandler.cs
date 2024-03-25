using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] clips;
    [SerializeField] [Range(0f, 1f)] float volume = 0.2f;

    public void PlayMoveSound()
    {
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length - 1)],
            volume);
    }
}
