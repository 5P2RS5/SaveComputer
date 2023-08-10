using System;
using System.Collections;
using System.Collections.Generic;
using BuildingSystem.Models;
using Unity.VisualScripting;
using UnityEngine;
using VirusSystem;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int towerType;
    public TowerType type;
    public AudioClip hitSound;

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;
    private float timer = 0f;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            HitorOver();
        }
    }

    public void Init(BuildableItem item, Vector3 dir)
    {
        spriter.sprite = item.bulletSprite;
        damage = item.damage;
        type = item.type;
        rigid.velocity = item.ammoSpeed * dir;
        hitSound = item.hitSound;
    }
    
    public void InitForVirus(int dmg, float ammoSpeed, AudioClip clip, Vector3 dir)
    {
        damage = dmg;
        rigid.velocity = ammoSpeed * dir;
        hitSound = clip;
    }

    public void HitorOver()
    {
        timer = 0f;
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.layer != 7)
        {
            return;
        }
        
        if (type == TowerType.Basic || type == TowerType.Ice)
        {
            HitorOver();
        }

        if (type == TowerType.Missile)
        {
            rigid.velocity = Vector2.zero;
            anim.SetTrigger("Bomb");
        }
    }
}
