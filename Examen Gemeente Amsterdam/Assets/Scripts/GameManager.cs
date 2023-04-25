using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class GameManager : MonoBehaviour
{
    public Question[] questions;
    private static List<Question> unansweredQuestions;

    public GameObject Popup;

    public GameObject minigamePanel;
    public GameObject startPanel;

    private Question currentQuestion;

    [SerializeField]
    private TMP_Text factText;

    [SerializeField]
    private Text trueAnswerText;

    [SerializeField]
    private Text falseAnswerText;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Text TimerText;

    public GameObject antiVirus;

    public GameObject progressbar;

    public GameObject installertextA;
    
    public GameObject installertextB;

    public Slider Slider;

    public float TimeLeft;
    public bool TimerOn = false;

    private float timeBetweenQuestions = 1f;

    private int correctAnswers = 0;

    public float duration = 5f;

    private bool isinstalling = false;

    


    //in editor make sure these two floats are less than one, for  example: 0.5 = 50% chance
    public float chanceP;
    public float chanceF;

    float pendingFreezeDuration = 0f;

    bool isFrozen = false;



    
    

    /*void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }*/

    
    
    //this starts the whole minigame program once you have clicked on "start"
    public void StartMinigame()
    {
        minigamePanel.SetActive(true);
        startPanel.SetActive(false);

        TimerOn = true;

        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Question>();
        }
        GetComponent<Timer>();

        SetCurrentQuestion();
    }



    void Awake()
    {
        //Slider = gameObject.GetComponent<Slider>();
    }
    
    private void Update()
    {

        //handles the tiemr
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }else
            {
                TimeLeft = 0;
                TimerOn = false;
            }
            
        }

        //handles the conditions that have to be met to activate a win or a lose
        if (correctAnswers == 20)
        {
            Win();
        }else if (TimeLeft <= 0 && correctAnswers < 20)
        {
            Lose();
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

        
    }

    //handles which questions get showed, which ones have been answered, 
    //and what happens if they have been answered correctly or not.
    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        factText.text = currentQuestion.fact;

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
            if (isFrozen == true)
            {
                StartCoroutine(DoFreeze());
            }
        }else
        {
            Debug.Log("WRONG!");

            float random = Random.Range(0, 1);

            //handles the chance of any effect happening, when entering chances make sure to 
            //keep in mind that popup will only activate if the number is equal to or lower than the given float value
            //freeze effect works the same way except for that the value generated has to be equal to or higher than the given float value
            if(random <= chanceP)
            {
                Popup.SetActive(true);
            }
            else if(random >= chanceF)
            {
                StartCoroutine(DoFreeze());
                Debug.Log("DoFreeze");
            }
        }

        StartCoroutine(TransitionToNextQuestion());
    }


    //does the same as above but for "false"
    public void UserSelectFalse()
    {
        animator.SetTrigger("False");
        if (currentQuestion.isTrue)
        {
            correctAnswers += 1;
            Debug.Log("CORRECT!");

            if (isFrozen == true)
            {
                StartCoroutine(DoFreeze());
            }
        }else
        {
            float random = Random.Range(0, 1);

            //handles the chance of any effect happening, when entering chances make sure to 
            //keep in mind that popup will only activate if the number is equal to or lower than the given float value
            //freeze effect works the same way except for that the value generated has to be equal to or higher than the given float value
            if(random <= chanceP)
            {
                Popup.SetActive(true);
            }
            else if(random >= chanceF)
            {
                Debug.Log("DoFreeze");
                StartCoroutine(DoFreeze());
            }

            Debug.Log("WRONG!");
        }
        StartCoroutine(TransitionToNextQuestion());
    }


    //pretty self explainatory
    private void Win()
    {
        
    }

    private void Lose()
    {
        
    }


    //handles the UI aspect of the timer
    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
    

    //sets the freezeduration to the set duration making it so the "DoFreeze" if statement in update() is valid
    public void Freeze()
    {
        pendingFreezeDuration = duration;
        Debug.Log(pendingFreezeDuration);

    }


    //freezes the cursor to simulate a freeze effect
    IEnumerator DoFreeze()
        {
            isFrozen = true;
            Debug.Log("freeze");
            Cursor.lockState = CursorLockMode.Locked;

            yield return new WaitForSeconds(duration);

            Cursor.lockState = CursorLockMode.None;
            pendingFreezeDuration = 0;
            Debug.Log("unfreeze");
            isFrozen = false;
        }

    public void startinstall()
    {
        isinstalling = true;
        installertextA.SetActive(true);
    }
    

    
}
