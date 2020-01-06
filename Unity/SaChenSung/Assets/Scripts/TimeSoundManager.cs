using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSoundManager : MonoBehaviour
{
    public AudioClip soundExplosion;
    AudioSource audioSource;

    public static TimeSoundManager instance;

    void Start()
    {
        if (TimeSoundManager.instance == null)
            TimeSoundManager.instance = this;

        audioSource = this.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.pitch = 3;
    }


    void Update()
    {
        if(SaveTimeState.time < 10) // 일정 시간부터 틱톡 소리남
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = soundExplosion;
                audioSource.Play();
            }
                
        }
        if (SaveTimeState.timeOut == true)
            audioSource.Stop();
    }
}
