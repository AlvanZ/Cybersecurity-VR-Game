using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameplayDataManager : MonoBehaviour
{
  [System.Serializable]
public class GameDataListWrapper
{
    public List<GameData> gameDataList;
}


    [System.Serializable]
public class GameData
{
    public int maxRoundReached;
    public int totalQuestionsRight;
    public List<float> timePerQuestion = new List<float>();
    public float totalPlaytime;
}
public GameData currentGameData = new GameData();


    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }



public void QuestionAnswered(bool isCorrect, float timeTaken)
{
    if (isCorrect)
    {
        currentGameData.totalQuestionsRight++;
    }
    currentGameData.timePerQuestion.Add(timeTaken);
}

    public void RoundCompleted(int roundNumber)
{
    if (roundNumber > currentGameData.maxRoundReached)
    {
        currentGameData.maxRoundReached = roundNumber;
    }
}
    public void SaveGameData()
    {
        // Calculate total playtime only when needed
    currentGameData.totalPlaytime = Time.time - startTime;

        // Convert the game data to JSON
    string gameDataJson = JsonUtility.ToJson(currentGameData, true);

        // Append to file (or create new if it doesn't exist)
        string filePath = Application.dataPath + "/Resources/SurveyResults.json";
        if (File.Exists(filePath))
        {
            string existingDataJson = File.ReadAllText(filePath);
            var dataList = JsonUtility.FromJson<GameDataListWrapper>(existingDataJson);
            dataList.gameDataList.Add(currentGameData);
            File.WriteAllText(filePath, JsonUtility.ToJson(dataList, true));
        }
        else
        {
GameDataListWrapper newDataList = new GameDataListWrapper { gameDataList = new List<GameData> { currentGameData } };
            File.WriteAllText(filePath, JsonUtility.ToJson(newDataList, true));
        }
    }

}
