using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class MailManager : MonoBehaviour
{
    public Question[] questions;
    private static List<Question> unansweredQuestions;


    private Question currentQuestion;

    public GameObject Popup;

    [SerializeField]
    private TMP_Text factText;

    [SerializeField]
    private Text trueAnswerText;

    [SerializeField]
    private Text falseAnswerText;

    [SerializeField]
    private Animator animator;

    private float timeBetweenQuestions = 1f;

    public float chanceP;
    public float chanceF;

    float pendingFreezeDuration = 0f;

    bool isFrozen = false;
    
    public float duration = 5f;


    

    /*void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }*/

    
    

    private void Start()
    {

        

        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Question>();
        }
        GetComponent<Timer>();

        SetCurrentQuestion();
    }

    
    private void Update()
    {

    }

    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

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
            if (isFrozen == true)
            {
                StartCoroutine(DoFreeze());
            }
            Debug.Log("CORRECT!");
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

    public void UserSelectFalse()
    {
        animator.SetTrigger("False");
        if (currentQuestion.isTrue)
        {
            if (isFrozen == true)
            {
                StartCoroutine(DoFreeze());
            }
            Debug.Log("CORRECT!");
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

    private void Win()
    {
        
    }

    private void Lose()
    {
        
    }

    public void Freeze()
    {
        pendingFreezeDuration = duration;
        Debug.Log(pendingFreezeDuration);

    }

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

    
    
}