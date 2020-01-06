using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveDeck
{
    //GameManeger에서 flag 사용
    public static bool flag = true; // true = NewStart, false = ReStart
    public static WordDeck[] deck;
    public static WordDeck[] getDeck()
    {
        if (flag == true)
            deck = DeckManager.MakeDeck();
        return deck;
    }
}
