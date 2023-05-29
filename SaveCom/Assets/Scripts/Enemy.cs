using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health; // 현재 체력
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    private bool isLive;

    private Rigidbody2D rigid;
    private Collider2D coll;
    private Animator anim;
    private SpriteRenderer spriter;
    private WaitForFixedUpdate wait;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        coll = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        // GetCurrentAnimatorStateInfo : 현재 애니메이터의 상태 정보를 가져오는 함수
        if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        
        // 1. 플레이어가 위치한 방향 구하기
        Vector2 dirVec = target.position - rigid.position;
        // 2. 플레이어가 위치한 곳으로 어떻게 움직일지 계산
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        // 3. MovePosition으로 현재 위치에서 다음 위치로 이동 수행
        rigid.MovePosition(rigid.position + nextVec);
        // 4. 리지드바디의 물리 속도가 이동에 영향을 주지 않도록 보완하기
        rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if(!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true; // 리지드바디 컴포넌트 비활성화는 simulated에서 관리
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait; // 하나의 물리프레임을 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Bullet") || !isLive)
            return;
        
        health -= col.GetComponent<Bullet>().damage;
        StartCoroutine("KnockBack");
        
        if (health > 0)
        {
            // .. Live, Hit Action
            anim.SetTrigger("Hit");
        }
        else
        {
            // .. Die
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false; // 리지드바디 컴포넌트 비활성화는 simulated에서 관리
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
        }
    }
}
