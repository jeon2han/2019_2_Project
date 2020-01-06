using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void GameStart()
    {
        LoadingManager.LoadScene("LevelWindow");
    }

    public void Exit()
	{
	    Application.Quit ();
        //UnityEditor.EditorApplication.isPlaying = false;
	}
}