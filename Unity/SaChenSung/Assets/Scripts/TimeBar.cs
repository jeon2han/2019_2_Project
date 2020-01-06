using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    Image timerBar;
    float maxTime = 100f;
    float timeLeft;

    public void Make() // 타이머 생성
    {
        timerBar = GameObject.Find("TimeBar").GetComponent<Image>();
        timeLeft = maxTime;
    }

    public void TicTok() // 시간가는 중
    {
        SaveTimeState.time = timeLeft;
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else
        {
            SaveTimeState.timeOut = true;
            Time.timeScale = 0f;
        }
    }
}
