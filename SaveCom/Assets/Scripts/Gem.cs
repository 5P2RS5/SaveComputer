using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gem : MonoBehaviour
{
    [SerializeField] private Sprite[] gems;

    public float moveSpeed = 10f;
    public float magnetDistance = 15f;
    private int gemType; // 0 = power, 1 = cpu, 2 = gpu
    private Transform player;
    private SpriteRenderer spriter;
    private AudioSource audioSource;
    private bool isOn;
    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        audioSource = GameSystem.instance.player.audioSource;
    }

    private void OnEnable()
    {
        isOn = true;
        gemType = Random.Range(0, gems.Length);
        spriter.sprite = gems[gemType];
        player = GameSystem.instance.player.transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= magnetDistance)
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed *
                Time.deltaTime);
        if (transform.position == player.position)
        {
            audioSource.Play();
            GameSystem.instance.towerStatusControl.statusDatas[gemType].countGem += 1;
            gameObject.SetActive(false);
        }
    }
}

