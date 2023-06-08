using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JsonViewer : MonoBehaviour
{
    readonly string mailFilePath = Path.Combine(Application.dataPath, "Json/mails.json");
    readonly string questionFilePath = Path.Combine(Application.dataPath, "Json/JSONText.json");

    [SerializeField]
    private GameObject jsonListObj;
    [SerializeField]
    private GameObject jsonViewContent;
    [SerializeField]
    private Toggle setFile;
    [SerializeField]
    private GameObject explanationText;

    private List<Mails> mailList = new();
    private List<Question> questionList = new();
    public List<GameObject> instUIObjList;

    private void Start()
    {
        if (File.Exists(questionFilePath) && !setFile.isOn)
        {
            questionList = JsonUtility.FromJson<QuestionData>(File.ReadAllText(questionFilePath)).questions;
            InstantiateJson();
        }
        else if (File.Exists(mailFilePath) && setFile.isOn)
        {
            mailList = JsonUtility.FromJson<MailData>(File.ReadAllText(mailFilePath)).Mails;
            InstantiateJson();
        }
    }

    public void SetJsonFile() 
    {
        Debug.Log(setFile.isOn);
        if (instUIObjList.Count > 0)
            foreach (GameObject obj in instUIObjList)
                Destroy(obj);

        if (File.Exists(questionFilePath) && !setFile.isOn)
        {
            mailList = new();
            explanationText.SetActive(true);
            questionList = JsonUtility.FromJson<QuestionData>(File.ReadAllText(questionFilePath)).questions;
            InstantiateJson();
        }
        else if (File.Exists(mailFilePath) && setFile.isOn)
        {
            questionList = new();
            explanationText.SetActive(false);
            mailList = JsonUtility.FromJson<MailData>(File.ReadAllText(mailFilePath)).Mails;
            Debug.Log("Count: "+mailList.Count);
            InstantiateJson();
        }

    }

    public void InstantiateJson() 
    {
        if (questionList.Count > 0)
            for (int i=0; i < questionList.Count; i++) 
            {
                GameObject jsonUi = Instantiate(jsonListObj, jsonViewContent.transform);
                jsonUi.transform.Find("nr text").GetComponent<TMP_Text>().text = (i+1).ToString();
                jsonUi.transform.Find("vraag text").GetComponent<TMP_InputField>().text = questionList[i].fact;
                GameObject explInputField = jsonUi.transform.Find("uitleg text").gameObject;
                explInputField.GetComponent<TMP_InputField>().text = questionList[i].explanation;
                explInputField.SetActive(true);
                jsonUi.transform.Find("antwoord toggle").GetComponent<Toggle>().isOn = questionList[i].isTrue;
                jsonUi.transform.Find("Btn_Remove").GetComponent<Button>().onClick.AddListener(delegate { instUIObjList.Remove(jsonUi.gameObject); RemoveJsonEntry(jsonUi); });
                instUIObjList.Add(jsonUi);
            }
        else
            for (int i = 0; i < mailList.Count; i++)
            {
                GameObject jsonUi = Instantiate(jsonListObj, jsonViewContent.transform);
                jsonUi.transform.Find("nr text").GetComponent<TMP_Text>().text = (i + 1).ToString();
                jsonUi.transform.Find("vraag text").GetComponent<TMP_InputField>().text = mailList[i].fact;
                jsonUi.transform.Find("antwoord toggle").GetComponent<Toggle>().isOn = mailList[i].isTrue;
                jsonUi.transform.Find("Btn_Remove").GetComponent<Button>().onClick.AddListener(delegate { instUIObjList.Remove(jsonUi.gameObject); RemoveJsonEntry(jsonUi); });
                instUIObjList.Add(jsonUi);
            }
    }

    public void RemoveJsonEntry(GameObject obj) 
    {
        string fact = obj.transform.Find("vraag text").GetComponent<TMP_InputField>().text;
        if (questionList.Count > 0)
            questionList.Remove(questionList.Where(obj => obj.fact == fact).SingleOrDefault());
        else
            mailList.Remove(mailList.Where(obj => obj.fact == fact).SingleOrDefault());
        Destroy(obj);
    }

    public void AddJsonEntry() 
    {
        GameObject jsonUi = Instantiate(jsonListObj, jsonViewContent.transform);
        if (!setFile.isOn)
            jsonUi.transform.Find("uitleg text").gameObject.SetActive(true);
        jsonUi.transform.Find("Btn_Remove").GetComponent<Button>().onClick.AddListener(delegate { instUIObjList.Remove(jsonUi.gameObject); Destroy(jsonUi.gameObject); });
        instUIObjList.Add(jsonUi);
    }

    public void SaveJsonChanges() 
    {
        if (questionList.Count > 0)
        {
            QuestionData newQuestionData = new();
            List<Question> newQuestions = new();
            foreach (GameObject obj in instUIObjList)
            {
                Question question = new Question()
                {
                    fact = obj.transform.Find("vraag text").GetComponent<TMP_InputField>().text,
                    isTrue = obj.transform.Find("antwoord toggle").GetComponent<Toggle>().isOn,
                    explanation = obj.transform.Find("uitleg text").GetComponent<TMP_InputField>().text
                };
                if (question.fact != null && question.fact != string.Empty)
                    newQuestions.Add(question);
                Destroy(obj);
            }
            instUIObjList = new();
            newQuestionData.questions = newQuestions;
            File.WriteAllText(questionFilePath, JsonConvert.SerializeObject(newQuestionData));
            questionList = JsonUtility.FromJson<QuestionData>(File.ReadAllText(questionFilePath)).questions;
            InstantiateJson();
        }
        else 
        {
            MailData newMailData = new();
            List<Mails> newMails = new();
            foreach (GameObject obj in instUIObjList)
            {
                Mails mail = new Mails()
                {
                    fact = obj.transform.Find("vraag text").GetComponent<TMP_InputField>().text,
                    isTrue = obj.transform.Find("antwoord toggle").GetComponent<Toggle>().isOn,
                };
                if (mail.fact != null || mail.fact != string.Empty)
                    newMails.Add(mail);
                Destroy(obj);
            }
            instUIObjList = new();
            newMailData.Mails = newMails;
            File.WriteAllText(mailFilePath, JsonConvert.SerializeObject(newMailData));
            mailList = JsonUtility.FromJson<MailData>(File.ReadAllText(mailFilePath)).Mails;
            InstantiateJson();
        }
    }

    public class MailData
    {
        public List<Mails> Mails;
    }

    public class QuestionData
    {
        public List<Question> questions;
    }

}
