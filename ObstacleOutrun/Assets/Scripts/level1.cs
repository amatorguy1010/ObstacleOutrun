using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class level1 : MonoBehaviour
{
    public void Exit()
    {
        SceneManager.LoadSceneAsync(1);
    }

}
