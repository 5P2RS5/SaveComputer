using System;
using System.Collections;
using System.Collections.Generic;
using BuildingSystem.Models;
using UnityEngine;
using UnityEngine.UI;
using VirusSystem;

public class Tower : MonoBehaviour
{
    public bool canAttack;
    public int hp;
    public int damage; // 타워 데미지
    public float period; // 발사 주기
    public float ammoSpeed; // 총알 속도
    public float scanRange;
    public int maxHp;
    public Animator anim;
    public SpriteRenderer spriter;
    public AudioClip[] audioClips;
    public bool isWall;
    public bool isDestroy;

    private float timer;
    private Scanner scanner;
    private Quaternion towerLook;
    
    public AudioSource audioSource;
    public BuildableItem originTowerInfo;

    public Slider hpSlider;
    public GameObject sliderObj;
    public Image sliderImage;
    public Vector3 towerCoord;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        timer = period;
        audioSource.clip = audioClips[0];
        audioSource.pitch = 2.7f;
        audioSource.Play();
        anim.SetTrigger("Build");
    }


    
    void Update()
    {
        if (isDestroy)
        {
            return;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Build"))
        {
            if (isWall)
                spriter.enabled = true;
            return;
        }
        else
        {
            if (isWall)
                spriter.enabled = false;
        }
        if (canAttack)
        {
            timer += Time.deltaTime;
            if (timer > period)
            {
                timer = 0f;
                Fire();
            }
        }
        // if (timer >= 5.0f)
        // {
        //     sliderObj.SetActive(false);
        //     hpSlider.value = hpSlider.maxValue;
        // }

        if (hpSlider.value <= maxHp / 2 && hpSlider.value > maxHp / 4)
        {
            sliderImage.color = Color.yellow;
        }
        else if (hpSlider.value <= maxHp / 4)
        {
            sliderImage.color = Color.red;
        }
        else
        {
            sliderImage.color = Color.green;
        }
    }

    void Fire()
    {
        if (!scanner.nearestTarget || !canAttack || isDestroy)
            return;
        Debug.Log("발사중 : " + scanner.nearestTarget);
        // Vector3 targetPos = scanner.nearestTarget.position;
        
        if (originTowerInfo.type == TowerType.Missile)
        {
            Vector3 targetPos = scanner.nearestTarget.transform.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;
            spriter.flipX = dir.x > 0;
            Transform bullet = GameSystem.instance.pool.Get((int)originTowerInfo.type + 2).transform;
            bullet.position = targetPos;
            bullet.GetComponent<Bullet>().Init(originTowerInfo, dir);
            audioSource.clip = audioClips[2];
            audioSource.pitch = 1;
            audioSource.Play();
            anim.SetTrigger("Fire");
        }
        else
        {
            Debug.Log("보라 아님");
            Vector3 targetPos = scanner.nearestTarget.transform.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;
            spriter.flipX = dir.x > 0;
            Transform bullet = GameSystem.instance.pool.Get((int)originTowerInfo.type + 2).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(originTowerInfo, dir);
            audioSource.clip = audioClips[2];
            audioSource.pitch = 1;
            audioSource.Play();
            anim.SetTrigger("Fire");
        }
    }

    public bool Attacked(Vector3 coord, int e_damage)
    {
        if (isDestroy)
            return false;
        towerCoord = coord;
        hp -= e_damage;
        hpSlider.value = hp;
        if (hp <= 0)
        {
            spriter.enabled = true;
            audioSource.clip = audioClips[1];
            audioSource.pitch = 2.7f;
            audioSource.Play();
            anim.SetTrigger("Destroy");
            isDestroy = true;
            return false;
        }
        return true;
    }

    public void TowerInit(BuildableItem item, Vector3 coord)
    {
        towerCoord = coord;
        originTowerInfo = item;
        hp = originTowerInfo.hp;
        maxHp = originTowerInfo.hp;
        if (originTowerInfo.type != TowerType.Wall)
        {
            scanner = GetComponent<Scanner>();
            damage = originTowerInfo.damage;
            period = originTowerInfo.period;
            ammoSpeed = originTowerInfo.ammoSpeed;
            gameObject.GetComponent<Scanner>().scanRange = originTowerInfo.scanRange;
        }
    }

    void DestroyTower()
    {
        GameSystem.instance.constructionLayer.Destroy(towerCoord);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 15 && !isDestroy)
        {
            Debug.Log("앙 맞았디");
            VirusBullet bullet = col.GetComponent<VirusBullet>();
            audioSource.clip = bullet.hitSound;
            audioSource.Play();
            hp -= bullet.damage;
            
            if (hp <= 0)
            {
                spriter.enabled = true;
                audioSource.clip = audioClips[1];
                audioSource.pitch = 2.7f;
                audioSource.Play();
                anim.SetTrigger("Destroy");
                isDestroy = true;
            }
            
        }
    }
}
