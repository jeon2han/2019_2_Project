using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    WordBoxManager wbm;
    TimeBar timeBar;

    bool game_state = false; // update 내에서 한 번만 실행 되게 하기위한 변수

    private void Awake()
    {
        if(gm == null)
            gm = this;
    }

    void Start()
    {
        // WordBox 생성
        wbm = new WordBoxManager();
        wbm.CreateBtns(SaveLevel.level,gameObject);
        // 타이머 생성
        timeBar = new TimeBar();
        timeBar.Make();
        SaveTimeState.timeOut = false; // 시간 초과 여부 판단

        if (wbm.Status() == false) NewStart();
    }

    void Update() // 매 프레임마다 호출 됨
    {
        timeBar.TicTok(); // 시간 감소
        if (SaveTimeState.timeOut == true)// 시간 초과
        {
            if(game_state == false)
            {
                FailOn();
                game_state = true;
            }           
        }
            
           
        if (wbm.wordNum == 0)
        {
            if (game_state == false)
            {
                SuccessOn();
                game_state = true;
            }           
        }
            
        if(SaveState.state == false)
        {
            SaveState.state = true;
            FailOn();
        }
    }

    public void PauseOnOff() // 일시정지 화면
    {
        if (Time.timeScale > 0f) // 멈춤
        {
            GameObject.Find("GameBoard").transform.Find("PauseWindow").gameObject.SetActive(true); // 비활성화 오브젝트는 부모에서 타고 내려와서 찾아야 검색됨
            wbm.HideOnOff(false);
            Time.timeScale = 0f;
        }
        else // 재생
        {
            GameObject.Find("PauseWindow").SetActive(false);
            wbm.HideOnOff(true);
            Time.timeScale = 1f;
        }
    }

    public void SuccessOn() // 성공 화면 
    {
        GameObject.Find("GameBoard").transform.Find("SuccessWindow").gameObject.SetActive(true);
        EffectSoundManager.instance.PlaySuccessResultSound();
        Time.timeScale = 0f;               
    }

    public void FailOn() // 실패 화면 
    {
        GameObject.Find("GameBoard").transform.Find("FailWindow").gameObject.SetActive(true);
        EffectSoundManager.instance.PlayGameOverResultSound();
        wbm.HideOnOff(false);
    }

    public void Exit() // 나가기
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleWindow");
    }

    public void ReStart() // 다시하기
    {
        SaveDeck.flag = false;
        LoadingManager.LoadScene("PlayWindow");
        Debug.Log("다시시작");
    }

    public void NewStart() // 새로하기
    {
        SaveDeck.flag = true;
        LoadingManager.LoadScene("PlayWindow");
        Debug.Log("새로하기");
    }

    public void Hint()
    {
        wbm.Hint();
    }
}


