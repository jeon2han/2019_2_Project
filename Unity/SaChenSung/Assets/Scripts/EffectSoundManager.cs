using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSoundManager : MonoBehaviour
{
    public AudioClip[] soundExplosion;
    // 0 : 성공
    // 1 : 실패
    // 2 : 클릭
    // 3 : 정답
    // 4 : 틀림
    AudioSource audioSource;

    public static EffectSoundManager instance;

    void Awake()
    {
        float t = Time.time * 100f;
        Random.InitState((int)t);

        if (EffectSoundManager.instance == null)
            EffectSoundManager.instance = this;
    }

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.pitch = 1;
    }

    public void PlaySuccessResultSound() // 게임 성공 사룬드
    {
        audioSource.PlayOneShot(soundExplosion[0]);
    }

    public void PlayGameOverResultSound() // 게임 오버 사운드
    {
        audioSource.PlayOneShot(soundExplosion[1]);
    }

    public void PlayPlayerClickedSound() // 클릭 사운드
    {
        audioSource.PlayOneShot(soundExplosion[2]);
    }

    public void PlayCorrectSound() // 클릭 사운드
    {
        audioSource.PlayOneShot(soundExplosion[3]);
    }

    public void PlayWrongSound() // 클릭 사운드
    {
        audioSource.PlayOneShot(soundExplosion[4]);
    }
}
