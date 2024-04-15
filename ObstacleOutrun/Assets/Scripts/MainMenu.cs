using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void goToLevels()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void goToLogin()
    {
        SceneManager.LoadSceneAsync(8);
    }
}
