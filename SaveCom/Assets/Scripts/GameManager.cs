using System;
using System.Collections;
using System.Collections.Generic;
using BuildingSystem;
using GameInput;
//using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirusSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioClip[] bgms;
    public AudioSource audioSource;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != null)
                Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            audioSource.clip = bgms[0];
        
        if (SceneManager.GetActiveScene().buildIndex == 2)
            audioSource.clip = bgms[1];
        
        if (!audioSource.isPlaying)
        {
            Debug.Log("재생중이 아니라 재생한다.");
            audioSource.Play();
        }
    }
}
