using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private void Update()
    {
    }

    public void ViewLoadingScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }
    
    public void Quit()
    {
        Application.Quit();
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void Help()
    {
        
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
