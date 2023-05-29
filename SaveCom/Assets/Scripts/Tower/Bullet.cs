using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int towerType;

    private Rigidbody2D rigid;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(int towerType, float damage, float speed, Vector3 dir)
    {
        this.damage = damage;

        if (towerType == 0) // normal Tower
        {
            rigid.velocity = speed * dir;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy"))
        {
            return;
        }
        Debug.Log("맞혔다!");
        rigid.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Border"))
        {
            Debug.Log("맞혔다!");
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        
    }
}
