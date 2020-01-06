//using UnityEngine;
//using Firebase;
//using Firebase.Database;
//using Firebase.Unity.Editor;
//using System.Collections;

//public class FirebaseManager : MonoBehaviour
//{
//    public const string firebaseAddress = "https://test-8aebf.firebaseio.com/"; // firebase 경로

//    private WordDeck wordDeck = null;
//    private static DatabaseReference reference = null;
//    ArrayList arrayList;

//    private void Start()
//    {
//        arrayList = new ArrayList();
//        /*
//         여러 덱을 읽을경우 리스트 사용
//         한덱만 랜덤으로 읽을 경우 카드배치때 읽어야함 
//         */

//        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(firebaseAddress);
//        reference = FirebaseDatabase.DefaultInstance.RootReference;

//    }

//    public void readData()
//    {

//        bool nullCheck = false;

//        for (int i = 0; i < 24; i++)
//        {

//            FirebaseDatabase.DefaultInstance.GetReference("Test/" + i.ToString()).GetValueAsync().ContinueWith(task =>
//            {
//                if (task.IsFaulted)
//                {
//                    nullCheck = true;
//                }
//                else if (task.IsCompleted)
//                {
//                    DataSnapshot snapshot = task.Result;

//                    Debug.Log(snapshot.Child("word").Value.ToString());
//                    Debug.Log(snapshot.Child("id").Value.ToString());
//                    Debug.Log("-----------------------------------------------------------");
//                }
//                WordDeckList.list.Add("");
//            });
//            if (nullCheck == true) break;
//        }

//    }
//}

//public static class WordDeckList
//{
//    public static ArrayList list = null;
//}

//public class WordDeck
//{
//    public string word;
//    public string id;

//    public WordDeck(string word, string id)
//    {

//        this.word = word;
//        this.id = id;
//    }

//}
