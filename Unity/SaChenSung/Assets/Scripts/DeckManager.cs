using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // 결과값을 saveDeck에 값 저장
    const int row = 4;
    const int col = 4;

    public static WordDeck[] MakeDeck()
    {
        float t = Time.time * 100f;
        Random.InitState((int)t);

        WordDeck[] deck = new WordDeck[row*col];
               
        int random_num = Random.Range(1, 65);
        List<List<string>> csv = CsvReader.Read("csv/deck_"+random_num.ToString()); // random_num.ToString() csv 행열 개수가 다 다름

        
        // csv 순서대로 읽어 id와 단어 저장
        int cnt = 0;
        for (int i = 0; i < csv.Count; i++)
        {
            for(int j=1; j<csv[i].Count; j++)
            {
                deck[cnt] = new WordDeck(csv[i][j], csv[i][0]);
                Debug.Log(csv[i][j]+" "+ csv[i][0]);
                cnt++;
            }
        }

        // 셔플
        for (int i=0; i<deck.Length; i++)
        {
            random_num = Random.Range(0, deck.Length);
            WordDeck temp = deck[i];
            deck[i] = deck[random_num];
            deck[random_num] = temp;
        }

        for (int i = 0; i < row*col; i++)
            Debug.Log(deck[i].word);
        return deck;
    }
}

public class WordDeck
{
    public string word;
    public string id;

    public WordDeck(string word, string id)
    {

        this.word = word;
        this.id = id;
    }
}
