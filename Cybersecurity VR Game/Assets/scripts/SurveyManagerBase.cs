using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using TMPro;
public class SurveyManagerBase : MonoBehaviour

{
        public static SurveyManagerBase Instance { get; private set; }

    [System.Serializable]
    public class Question
    {
        public string questionText;
        public List<string> options = new List<string>();
    }

    [System.Serializable]
    public class AnswerData
    {
        public string question;
        public string selectedAnswer;
    }

    [System.Serializable]
    public class SurveyData
    {
        public List<AnswerData> answers = new List<AnswerData>();
    }

    public List<Question> questions = new List<Question>();
    public TMP_Text questionText;
    public Button[] answerButtons;

    private int currentQuestionIndex = 0;
    private SurveyData surveyResults = new SurveyData();
public AudioSource introAudioSource;
public AudioClip outroAudioClip;

public AudioClip introAudioClip;



public float fadeDuration = 0.5f; // Duration of the fade effect
public GameObject clipObject;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(PlayIntroAudio());
        
            

    }
IEnumerator PlayIntroAudio()
{
    yield return new WaitForSeconds(1f); // Small delay before the audio starts

    if (introAudioSource && introAudioClip)
    {
        introAudioSource.PlayOneShot(introAudioClip);
        yield return new WaitForSeconds(introAudioClip.length); // Wait for the duration of the audio clip
    }

    // Now that the audio is done playing, display the first question
    DisplayQuestion();
}

    public void AnswerChosen(int index)
{
    // Fade effect on the chosen button
    StartCoroutine(FadeButtonEffect(answerButtons[index]));

    // Store the answer
    AnswerData answer = new AnswerData
    {
        question = questions[currentQuestionIndex].questionText,
        selectedAnswer = questions[currentQuestionIndex].options[index]
    };
    surveyResults.answers.Add(answer);

    // Move to the next question
    currentQuestionIndex++;
    if (currentQuestionIndex < questions.Count)
    {
        DisplayQuestion();
    }
    else
    {
        EndSurvey();
    }

    // Play the audio after a slight delay
    
}



IEnumerator FadeButtonEffect(Button btn)
{
    CanvasGroup canvasGroup = btn.GetComponent<CanvasGroup>();
    if (!canvasGroup)
    {
        canvasGroup = btn.gameObject.AddComponent<CanvasGroup>();
    }

    // Fade out
    for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    {
        canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
        yield return null;
    }
    canvasGroup.alpha = 0;

    // Wait for a short moment (adjust the wait time as necessary)
    yield return new WaitForSeconds(0.2f);

    // Fade in
    for (float t = 0; t < fadeDuration; t += Time.deltaTime)
    {
        canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
        yield return null;
    }
    canvasGroup.alpha = 1;
}

    void DisplayQuestion()
    {
        Question currentQuestion = questions[currentQuestionIndex];
        questionText.text = currentQuestion.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < currentQuestion.options.Count)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.options[i];
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void EndSurvey()
    {
        string filePath = Application.dataPath + "/Resources/SurveyResults.json";

    // If the file already exists, read its content and deserialize it into the surveyResults object
    if (File.Exists(filePath))
    {
        string existingJson = File.ReadAllText(filePath);
        SurveyData existingSurveyResults = JsonUtility.FromJson<SurveyData>(existingJson);
        
        // Append new results to the existing ones
        existingSurveyResults.answers.AddRange(surveyResults.answers);
        
        // Use the updated object for the rest of the method
        surveyResults = existingSurveyResults;
    }
        // Write results to JSON
        string jsonData = JsonUtility.ToJson(surveyResults, true);
    File.WriteAllText(filePath, jsonData);
    clipObject.SetActive(true);
        introAudioSource.PlayOneShot(outroAudioClip);

    gameObject.SetActive(false);
        Debug.Log("Survey completed! Results saved to SurveyResults.json");
    }
}
