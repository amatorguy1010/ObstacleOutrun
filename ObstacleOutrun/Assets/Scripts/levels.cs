using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levels : MonoBehaviour
{
    public void LevelOne()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void LevelTwo()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void LevelThree()
    {
        SceneManager.LoadSceneAsync(4);
    }
    public void LevelFour()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void LevelFive()
    {
        SceneManager.LoadSceneAsync(6);
    }
    public void LevelSix()
    {
        SceneManager.LoadSceneAsync(7);
    }
    public void Exit()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
