using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SinglePlayer : MonoBehaviour
{
   
   public void PlayAlone()
   {
      SceneManager.LoadScene(1);
   }
}
