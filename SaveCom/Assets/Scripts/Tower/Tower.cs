using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tower : MonoBehaviour
{
    public int damage; // 타워 데미지
    public float firePeriod; // 발사 주기
    public float ammoSpeed; // 발사 속도
    
    private float timer;
    private Scanner scanner;
    private Quaternion towerLook;
    private SpriteRenderer spriter;

    public Button[] buttons;

    
    private void Awake()
    {
        scanner = GetComponent<Scanner>();
        spriter = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > firePeriod)
        {
            timer = 0f;
            Fire();
        }
    }
    
    public void Inits(TowerData data)
    {
        spriter.sprite = data.sprite;
        damage = data.damage;
        firePeriod = data.firePeriod;
        ammoSpeed = data.ammoSpeed;
    }
    
    public void InteractBtn()
    {
        Debug.Log("타워 켜기");
        foreach (Button item in buttons)
        {
            item.gameObject.SetActive(true);
        }
    }

    public void Sell()
    {
        Debug.Log("타워 끄기");
        foreach (Button item in buttons)
        {
            item.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }


    void Fire()
    {
        if (!scanner.nearestTarget)
            return;
        Debug.Log("발사중 : " + scanner.nearestTarget);
        Vector3 targetPos = scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;
        spriter.flipX = dir.x > 0;
        Transform bullet = GameManager.instance.pool.Get(0).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(0, damage, ammoSpeed, dir);
    }
    
}
