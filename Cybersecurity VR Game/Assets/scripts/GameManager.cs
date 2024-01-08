using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class GameManager : MonoBehaviour
{
    public List<GameObject> weaponPrefabs; // List of weapon prefabs


    public List<GameObject> enemyPrefabs; // List of different enemy prefabs
    public Transform[] spawnPoints;
    public Transform weaponlocation;
    public int baseEnemiesPerRound = 5;
    public int additionalEnemiesPerRound = 2;
    public float healthIncreasePerRound = 10f;
    public float roundStartDelay = 10f; // 5 seconds before the next round starts
    public TMP_Text countdownText;

    private int currentRound = 1;
    public List<GameObject> activeEnemies = new List<GameObject>();

    public static GameManager Instance;

    //quiz popup
    public GameObject quizPopup; // Drag your Quiz Popup Panel here
    public TMP_Text questionText;   // Drag your Question Text component here
    public TMP_Text questionAnnouncement;   // Drag your Question Text component here

    public Button[] answerButtons; // Drag your 3 answer buttons here

    private float buffMultiplier = 1f; // For instance, 50% increase in damage
    public AudioClip backgroundMusicClip; // Assign your background music clip here in the inspector
    private AudioSource audioSource;
    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] options;
        public int correctAnswer;
    }

    [System.Serializable]
    public class QuizData
    {
        public List<QuizQuestion> questions;
    }
        private float startTime;

public GameplayDataManager gameplayDataManager;

    public QuizData quizData;  // This will hold our loaded quiz data
    private int currentQuestionIndex = 0;
    private void Start()
    {
        LoadQuizData();
        StartCoroutine(StartNewRoundAfterDelay());
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // Initialize the AudioSource component and play the background music
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusicClip;
        audioSource.loop = true; // Loop the music
        audioSource.Play();
    }
    public void StartNewRound()
    {
        int enemiesToSpawn = baseEnemiesPerRound + (additionalEnemiesPerRound * (currentRound - 1));

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Randomly select an enemy prefab from the list
            GameObject selectedEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(selectedEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.MaxHealth += healthIncreasePerRound * (currentRound - 1);
                enemyHealth.CurrentHealth = enemyHealth.MaxHealth; // Reset the current health to the new max
            }
            activeEnemies.Add(enemy);
        }
    }

    public void OnEnemyDefeated(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        // Check if all enemies are defeated
        if (activeEnemies.Count == 0)
        {
            ShowQuiz();
        }
    }
    // Load quiz data from JSON
    void LoadQuizData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("quizData");
        quizData = JsonUtility.FromJson<QuizData>(jsonFile.text);
    }

    void ShowQuiz()
    {
        if (currentQuestionIndex < quizData.questions.Count)
        {
            quizPopup.SetActive(true);
            startTime = Time.time;
            QuizQuestion currentQuestion = quizData.questions[currentQuestionIndex];
            questionText.GetComponent<TypingEffect>().StartTyping(currentQuestion.question);

            for (int i = 0; i < answerButtons.Length; i++)
            {
                int localAnswerIndex = i;  // Capture the current value of i

                answerButtons[i].gameObject.SetActive(true); // Enable the button
                answerButtons[i].GetComponentInChildren<TypingEffect>().StartTyping(currentQuestion.options[i]);

                answerButtons[i].onClick.AddListener(() => CheckAnswer(localAnswerIndex == currentQuestion.correctAnswer));

                ShootableButton shootableButton = answerButtons[i].GetComponent<ShootableButton>();
                if (shootableButton != null)
                {
                    // Assign the index of the answer to the button's answerIndex
                    shootableButton.answerIndex = localAnswerIndex;

                    // Clear previous listeners to avoid stacking
                    shootableButton.OnButtonShot.RemoveAllListeners();

                    // Add a listener to the button's OnButtonShot event
                    // This will call the CheckAnswer function with the button's answer index when the button is shot
                    shootableButton.OnButtonShot.AddListener(() => CheckAnswer(shootableButton.answerIndex == currentQuestion.correctAnswer));
                }
            }

        }
    }

    public void CheckAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            // Increase damage buff if the answer is correct
            buffMultiplier += 0.5f;
            questionAnnouncement.GetComponent<TypingEffect>().StartTyping("Correct! You have received a damage buff!");
            gameplayDataManager.QuestionAnswered(isCorrect,Time.time - startTime);
        }
        else
        {
            // Decrease damage buff if the answer is wrong
            buffMultiplier -= 0.2f;
            questionAnnouncement.GetComponent<TypingEffect>().StartTyping("Wrong! You have received a damage debuff.");
        }

        // Ensure the buffMultiplier doesn't go below 1
        buffMultiplier = Mathf.Max(buffMultiplier, 1f);
        questionText.text = "";
        // Clear all button listeners
        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(false);  // Add this line to hide the button
            btn.onClick.RemoveAllListeners();
        }
            quizPopup.SetActive(false);



        // Continue with the next round
        currentRound++;
        currentQuestionIndex++; // Move to the next question for the next round

        // Check if we have exceeded the number of available questions
        if (currentQuestionIndex >= quizData.questions.Count)
        {
            currentQuestionIndex = 0; // Reset the index if you want to loop the questions
                                      // Or handle the end of the quiz in some other way
        }
        StartCoroutine(StartNewRoundAfterDelay());
    }

    public float GetDamageWithBuff(float originalDamage)
    {
        return originalDamage * buffMultiplier;
    }
    private IEnumerator StartNewRoundAfterDelay()
    {
        StartCoroutine(WeaponDrop());
        
        float countdown = roundStartDelay;

        while (countdown > 0)
        {
            countdownText.text = "Round " + currentRound + " in: " + Mathf.CeilToInt(countdown).ToString() + " seconds";
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = ""; // Clear the countdown text
        questionAnnouncement.text = "";
gameplayDataManager.RoundCompleted(currentRound);
        StartNewRound();
    }
      private IEnumerator WeaponDrop()
{
    // Ensure there are weapons in the list
    if (weaponPrefabs.Count > 0)
    {
        // Select a random weapon prefab
        GameObject selectedWeaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Count)];

        // Spawn the selected weapon at the specified location
        Instantiate(selectedWeaponPrefab, new Vector3(weaponlocation.transform.position.x, weaponlocation.transform.position.y + 2, weaponlocation.transform.position.z), Quaternion.identity);

        // Wait for 0 seconds (you can remove this line if not needed)
        yield return new WaitForSeconds(0f);
    }
}

}
