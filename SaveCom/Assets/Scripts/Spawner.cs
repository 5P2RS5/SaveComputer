using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}

public class Spawner : MonoBehaviour
{
    //public Transform[] spawnPoint; // 스폰지점이 다수일 때
    public Transform spawnPoint;
    public SpawnData[] spawnDatas;
    
        
    private int level;
    private float timer;

    private void Awake()
    {
        //spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        // level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnDatas.Length - 1);
        // Mathf.FloorToInt : 소수점 아래는 버리고 Int형으로 바꾸는 함수 ex) 3.4 -> 3
        // Mathf.CeilToInt : 소수점 아래를 올리고 Int형으로 바꾸는 함수 ex) 3.4 -> 4
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            level += 1;
        }
        
        if (timer > spawnDatas[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
        
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(1);
        enemy.transform.position = spawnPoint.position;
        enemy.GetComponent<Enemy>().Init(spawnDatas[level]);
    }
}