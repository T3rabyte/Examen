using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class MailManager : MonoBehaviour
{
    public Mails[] Mails;
    private static List<Mails> unansweredMails;


    private Mails currentMail;

    public GameObject Popup;

    [SerializeField]
    private TMP_Text factText;

    [SerializeField] 
    private TMP_Text explanationText;

    [SerializeField]
    private Text trueAnswerText;

    [SerializeField]
    private Text falseAnswerText;

    [SerializeField]
    private Animator animator;

    private float timeBetweenMails = 1f;

    public float chanceP;
    public float chanceF;

    float pendingFreezeDuration = 0f;

    bool isFrozen = false;
    
    public float duration = 5f;

    
    
    private void Awake()
    {
         string filePath = Path.Combine(Application.streamingAssetsPath, "mails.json");
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            var MailData = JsonUtility.FromJson<MailData>(data);
            Mails = MailData.Mails;
        }
        else
        {
            Debug.LogError("Mails JSON file not found!");
        }

        if (unansweredMails == null || unansweredMails.Count == 0)
        {
            unansweredMails = Mails.ToList<Mails>();
        }
        

        SetCurrentMail();
    }

    public class MailData
    {
        public Mails[] Mails;
    }

    void SetCurrentMail()
    {
        int randomMailIndex = Random.Range(0, unansweredMails.Count);
        currentMail = unansweredMails[randomMailIndex];

        foreach (Mails Mail in Mails)
        {
            string fact = Mail.fact;
            bool istrue = Mail.isTrue;

            // Use the values as needed
            // ...
        }

        factText.text = currentMail.fact;
        explanationText.text = currentMail.explanation;

        unansweredMails.RemoveAt(randomMailIndex);

        if (currentMail.isTrue)
        {
            trueAnswerText.text = "CORRECT!";
            falseAnswerText.text = "FALSE!";

        }else
        {
            trueAnswerText.text = "WRONG!";
            falseAnswerText.text = "CORRECT!";
        }

    }

    IEnumerator TransitionToNextMail()
    {

        yield return new WaitForSeconds(timeBetweenMails);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetCurrentMail();
    }

    public void UserSelectTrue()
    {

        animator.SetTrigger("True");
        if (currentMail.isTrue)
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

        StartCoroutine(TransitionToNextMail());
    }

    public void UserSelectFalse()
    {
        animator.SetTrigger("False");
        if (!currentMail.isTrue)
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
        StartCoroutine(TransitionToNextMail());
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