using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBox : MonoBehaviour
{
    public GameObject instance; // 오브젝트
    public string word; // 단어
    public string id; // 아이디
    public int row; // 가로 위치
    public int col; // 세로 위치
    public bool active; // 현재 남아있는 단어인지 판단

    public WordBox(GameObject instance, string word, string id)
    {
        this.instance = instance;
        this.word = word;
        this.id = id;
        active = true;
    }
    public WordBox()
    {
        this.word = "";
        this.id = "";
    }
}
