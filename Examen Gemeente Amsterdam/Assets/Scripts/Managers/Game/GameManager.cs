using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public Question[] questions;
    private static List<Question> unansweredQuestions;

    public GameObject Popup;

    public GameObject minigamePanel;
    public GameObject startPanel;

    [SerializeField] private GameObject win;

    [SerializeField] private GameObject lose;

    private Question currentQuestion;

    [SerializeField] private TMP_Text factText;
    [SerializeField] private TMP_Text explanationText;
    [SerializeField] public TMP_Text corrAnswTxt;

    [SerializeField] private Text trueAnswerText;

    [SerializeField] private Text falseAnswerText;

    [SerializeField] private Animator animator;

    private NetworkVarManager networkVarManager;

    

    public GameObject antiVirus;

    [SerializeField]
    private GameObject menu;

    public GameObject progressbar;

    public GameObject installertextA;
    
    public GameObject installertextB;

    public Slider Slider;

    public Timer timer;

    private bool menuOn = false;

    
    [SerializeField]
    private float timeBetweenQuestions;

    private int correctAnswers;

    public float duration = 5f;

    private bool isinstalling = false;

    


    //in editor make sure these two floats are less than one, for  example: 0.5 = 50% chance
    [SerializeField] private float chanceP;

    [SerializeField] private float chanceF;




    //this starts the whole minigame program once you have clicked on "start"
    public void StartMinigame()
    {
        minigamePanel.SetActive(true);
        startPanel.SetActive(false);

        timer.TimerOn = true;

        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Question>();
        }
        GetComponent<Timer>();

        SetCurrentQuestion();
    }


    //the moment the script loads it sets the filepath of the JSONText.json file, 
    //reads the text in the json file, and sets it to be the value of questions.
    //if for whatever reason the file cannot be found it will throw an error. 
    void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "JSONText.json");
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            var questionData = JsonUtility.FromJson<QuestionData>(data);
            questions = questionData.questions;
        }
        else
        {
            Debug.LogError("Questions JSON file not found!");
        }
        
        //gets the timer script so we can use it's values
        timer = GetComponent<Timer>();

    }

    private void Start()
    {
        try {
            networkVarManager = GameObject.Find("NetworkVarManager(Clone)").GetComponent<NetworkVarManager>();
        }
        catch (Exception ex) 
        {
            Debug.Log("Network manager not found");
        }
    }

    public class QuestionData
    {
        public Question[] questions;
    }
    
    private void Update()
    {
        //handles the conditions that have to be met to activate a win or a lose
        if (correctAnswers >= 15)
        {
            Win();
            networkVarManager.gameFinished = true;
            networkVarManager.ShowScreenServerRpc();
            
        }
        else if (timer.TimeLeft <= 0f && correctAnswers <= 14)
        {
            Lose();
            networkVarManager.ShowScreenServerRpc();
            
        }

        if(isinstalling == true)
        {
            if (Slider.value >= 1)
            {
                isinstalling = false;
                chanceF += 0.1f;
                chanceP -= 0.1f;
                installertextA.SetActive(false);
                installertextB.SetActive(true);
                Debug.Log("installed!");
                
            }
            
            
            Slider.value += 0.01f * Time.deltaTime;
        }
        
        if(!menuOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowMenu();
                menuOn = true;
            }
            
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideMenu();
                menuOn = false;
            }
        }

        UpdateText();
        

        
    }

    //handles which questions get showed, which ones have been answered, 
    //and what happens if they have been answered correctly or not.
    void SetCurrentQuestion()
    {
        int randomQuestionIndex = UnityEngine.Random.Range(0, unansweredQuestions.Count-1);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        

        foreach (Question question in questions)
        {
            string fact = question.fact;
            bool istrue = question.isTrue;

            // Use the values as needed
            // ...
        }
        
        factText.text = currentQuestion.fact;
        explanationText.text = currentQuestion.explanation;

        unansweredQuestions.RemoveAt(randomQuestionIndex);

        if (currentQuestion.isTrue)
        {
            trueAnswerText.text = "CORRECT!";
            falseAnswerText.text = "WRONG!";
            

        }else
        {
            trueAnswerText.text = "WRONG!";
            falseAnswerText.text = "CORRECT!";
            
        }

    }

    

    
    //gives the program some time to register the next question
    IEnumerator TransitionToNextQuestion()
    {

        yield return new WaitForSeconds(timeBetweenQuestions);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetCurrentQuestion();
    }


    //handles the pressing of the "true" button
    public void UserSelectTrue()
    {

        animator.SetTrigger("True");
        if (currentQuestion.isTrue)
        {

            
            correctAnswers += 1;
            Debug.Log("CORRECT!");

            StartCoroutine(TransitionToNextQuestion());
            
        }else
        {
            Debug.Log("WRONG!");

            float random = UnityEngine.Random.Range(0, 1);

            //handles the chance of any effect happening, when entering chances make sure to 
            //keep in mind that popup will only activate if the number is equal to or lower than the given float value
            //freeze effect works the same way except for that the value generated has to be equal to or higher than the given float value
            if(random <= chanceP)
            {
                Popup.SetActive(true);
            }
            
        }

        
    }


    //does the same as above but for "false"
    public void UserSelectFalse()
    {
        animator.SetTrigger("False");
        if (!currentQuestion.isTrue)
        {
            correctAnswers += 1;
            Debug.Log("CORRECT!");

            

            StartCoroutine(TransitionToNextQuestion());
            
        }else
        {
            float random = UnityEngine.Random.Range(0, 1);

            //handles the chance of any effect happening, when entering chances make sure to 
            //keep in mind that popup will only activate if the number is equal to or lower than the given float value
            //freeze effect works the same way except for that the value generated has to be equal to or higher than the given float value
            if(random <= chanceP)
            {
                Popup.SetActive(true);
            }
            

            Debug.Log("WRONG!");

            
        }
        
    }


    //pretty self explainatory
    public void Win()
    {
        win.SetActive(true);
        Debug.Log("gewonnen");
    }

    public void Lose()
    {
        lose.SetActive(true);
    }

    public void transition()
    {
        StartCoroutine(TransitionToNextQuestion());
    }

    public void UpdateText()
    {
        corrAnswTxt.text = "Correcte Antwoorden: " + correctAnswers.ToString() + " / 15";
    }
  
    public void MMButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    

    public void startinstall()
    {
        isinstalling = true;
        installertextA.SetActive(true);
    }
    

    public void Toggle()
    {
        chanceF += 0.1f ; 
        chanceP -= 0.1f;
    }

    

    
}
