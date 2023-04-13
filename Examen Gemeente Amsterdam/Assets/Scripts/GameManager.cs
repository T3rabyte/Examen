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

    public float TimeLeft;
    public bool TimerOn = false;

    private float timeBetweenQuestions = 1f;

    private int correctAnswers = 0;

    public float duration =5f;

    float pendingFreezeDuration = 0f;

    bool isFrozen = false;

    
    

    /*void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }*/

    
    

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

    
    private void Update()
    {

        if (pendingFreezeDuration > 0 && !isFrozen)
        {
            StartCoroutine(DoFreeze());
        }

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

        if (correctAnswers == 20)
        {
            Win();
        }else if (TimeLeft <= 0 && correctAnswers < 20)
        {
            Lose();
        }

        
    }

    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        float random = Random.Range(0, 1);

        factText.text = currentQuestion.fact;

        unansweredQuestions.RemoveAt(randomQuestionIndex);

        if (currentQuestion.isTrue)
        {
            trueAnswerText.text = "CORRECT!";
            falseAnswerText.text = "FALSE!";
            

        }else
        {
            trueAnswerText.text = "WRONG!";
            falseAnswerText.text = "CORRECT!";
            
            if(random <= 0.5f)
            {
                Popup.SetActive(true);
            }
            else if(random >= 0.5f)
            {
                Freeze();
            }
            
        }

    }

    IEnumerator TransitionToNextQuestion()
    {

        yield return new WaitForSeconds(timeBetweenQuestions);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetCurrentQuestion();
    }

    public void UserSelectTrue()
    {

        animator.SetTrigger("True");
        if (currentQuestion.isTrue)
        {
            correctAnswers += 1;
            Debug.Log("CORRECT!");
        }else
        {
            Debug.Log("WRONG!");
        }

        StartCoroutine(TransitionToNextQuestion());
    }

    public void UserSelectFalse()
    {
        animator.SetTrigger("False");
        if (currentQuestion.isTrue)
        {
            correctAnswers += 1;
            Debug.Log("CORRECT!");
        }else
        {
            Debug.Log("WRONG!");
        }
        StartCoroutine(TransitionToNextQuestion());
    }

    private void Win()
    {
        
    }

    private void Lose()
    {
        
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
    
    public void Freeze()
    {
        pendingFreezeDuration = duration;

    }

    IEnumerator DoFreeze()
        {
            isFrozen = true;
            var original = Time.timeScale;
            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = original;
            pendingFreezeDuration = 0;
            isFrozen = false;
        }

    
}
