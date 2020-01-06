using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;


public class CsvReader : MonoBehaviour
{
    // DeckManager에서 사용
    public static List<List<string>> Read(string fileName)
    {
        List<List<string>> result = new List<List<string>>();
        TextAsset _txtFile = (TextAsset)Resources.Load(fileName) as TextAsset;
        string fileFullPath = _txtFile.text;

        // \n별로 스플릿
        string[] csv_row = fileFullPath.Split('\n');

        string[] csv_row_split;
        for (int i=0; i<csv_row.Length; i++)
        {
            // ,별로 스플릿
            csv_row_split = csv_row[i].Split(',');

            List<string> temp = new List<string>();
            for(int j=0; j<csv_row_split.Length; j++)
            {
                // 빈배열 제거 // 원래 csv에 a,b,c,,,,, 식으로 되어있음
                if (csv_row_split[j] != "")
                    if (csv_row_split[j] != "\r")
                        temp.Add(csv_row_split[j]);
            }
            result.Add(temp);
        }
        return result;
    }
}
