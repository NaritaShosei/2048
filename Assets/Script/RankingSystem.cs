using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    const string DATA_NAME = "ScoreData";
    public static List<int> ScoreList = new();
    [System.Serializable]
    public class ScoreData
    {
        public List<int> scoreList;
    }

    public static void RankingSave()
    {
        ScoreData saveData = new ScoreData { scoreList = ScoreList };
        string data = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(DATA_NAME, data);
        PlayerPrefs.Save();
    }

    public static void RankingLoad()
    {
        string json = PlayerPrefs.GetString(DATA_NAME, "");
        ScoreData saveData = JsonUtility.FromJson<ScoreData>(json);
        if (string.IsNullOrEmpty(json)) { ScoreList = new() { 0, 0, 0, 0, 0 }; }
        else { ScoreList = saveData.scoreList; }
    }

    public static void RankingReset()
    {
        ScoreList.Clear();
        PlayerPrefs.DeleteKey(DATA_NAME);
        PlayerPrefs.Save();
    }
}
