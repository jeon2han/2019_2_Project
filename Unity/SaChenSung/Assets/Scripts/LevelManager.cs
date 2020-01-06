using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public void LowLevelStart()
    {
        SaveLevel.level = 1;
        LoadingManager.LoadScene("PlayWindow");
    }

    public void HighLevelStart()
    {
        SaveLevel.level = 2;
        LoadingManager.LoadScene("PlayWindow");
    }

    public void Back()
    {
        SceneManager.LoadScene("TitleWindow");
    }

    
}
