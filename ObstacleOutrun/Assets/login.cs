using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class login : MonoBehaviour
{
    public void Exit()
    {
        SceneManager.LoadSceneAsync(0);
    }
   
}
