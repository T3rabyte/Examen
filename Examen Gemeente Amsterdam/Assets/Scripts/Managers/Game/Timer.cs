using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class Timer : MonoBehaviour
{

    public float TimeLeft;
    public bool TimerOn = false;

    [SerializeField]
    private Text TimerText;


    private void Update()
    {
         //handles the tiemr
        if (TimerOn)
        {
            if (TimeLeft > 0f)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }else if (TimeLeft <= 0f)
            {
                TimeLeft = 0f;
                TimerOn = false;
            }
            
        }
        
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }


}
