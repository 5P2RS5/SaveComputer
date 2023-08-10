using System;
using System.Collections;
using BuildingSystem.Models;
using UnityEngine;
using VirusSystem.Model;
using Random = UnityEngine.Random;

namespace VirusSystem
{
    public class Virus : MonoBehaviour
    {
        [Header("Virus Information")]
        public float speed; // 속도
        private int hp; // 체력
        private int power; // 공격력
        private float initSpeed; // 이동속도를 가지고 Idle <-> Walk를 컨트롤 하므로 필요
        private RuntimeAnimatorController animCon;
        private string name; // 몬스터 이름
        private float attackPeriod;
        private float timer;
        private int cost;
        private VirusType type;
        
        private Buildable targetTower;
        
        private bool isLive;
        private bool isAttack;
        private bool isFind;
        private bool isIced;

        private Rigidbody2D rigid;
        private Collider2D coll;
        private Animator anim;
        private SpriteRenderer spriter;
        private WaitForSeconds wait;
        private Scanner scanner;
        private Transform targetPos;
        private Tower tower;

        public AudioClip[] audioClips;
        private AudioSource audioSource;

        void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            spriter = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            wait = new WaitForSeconds(0.5f);
            coll = GetComponent<Collider2D>();
            scanner = GetComponent<Scanner>();
            audioSource = GetComponent<AudioSource>();
        }
        

        private void Update()
        {
            if (hp <= 0 && isLive)
            {
                isLive = false;
                GameSystem.instance.spawner.virusSlider.value--;
                audioSource.clip = audioClips[1];
                audioSource.Play();
                anim.SetTrigger("Dead");
            }

            if (isLive)
            {
                timer += Time.deltaTime;
                targetPos = scanner.nearestTarget;
                if (targetPos == null)
                {
                    speed = isIced ? initSpeed * 0.5f : initSpeed;
                    isAttack = false;
                    return;
                }
                speed = 0f;
                isAttack = true;
                if (isAttack)
                {
                    if (type == VirusType.Melee)
                    {
                        // targetTower에 tower의 정보를 가져온다.
                        tower = GameSystem.instance.constructionLayer.TowerInfo(targetPos.position).gameObject
                            .GetComponent<Tower>();
                        // 타워가 파괴되지 않았다면
                        if (tower != null && timer >= attackPeriod)
                        {
                            timer = 0f;
                            isAttack = tower.Attacked(targetPos.position, power);
                            tower.sliderObj.SetActive(true);
                            audioSource.clip = audioClips[0];
                            audioSource.Play();
                            anim.SetTrigger("Attack");
                        }
                    }
                    else 
                    {
                        if (timer >= attackPeriod)
                        {
                            timer = 0f;
                            Fire();
                        }
                    }
                    
                }
            }
        }

        
        void Fire()
        {
            if (!scanner.nearestTarget || !isAttack || !isLive)
                return;
            Debug.Log("발사중 : " + scanner.nearestTarget);
            // Vector3 targetPos = scanner.nearestTarget.position;
            Debug.Log("보라 아님");
            Vector3 targetPos = scanner.nearestTarget.transform.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;
            spriter.flipX = dir.x > 0;
            Transform bullet = GameSystem.instance.pool.Get(7).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<VirusBullet>().Init(power, 6, audioClips[2], dir);
            audioSource.clip = audioClips[0];
            audioSource.pitch = 1;
            audioSource.Play();
            anim.SetTrigger("Attack");
        }
        
        void FixedUpdate()
        {
            // GetCurrentAnimatorStateInfo : 현재 애니메이터의 상태 정보를 가져오는 함수
            if(!isLive || isAttack)
                return;
        
            // 1. 서버가 위치한 방향 구하기
            Vector2 dirVec = GameSystem.instance.serverPos - gameObject.transform.position;
            // 2. 플레이어가 위치한 곳으로 어떻게 움직일지 계산
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            // 3. MovePosition으로 현재 위치에서 다음 위치로 이동 수행
            rigid.MovePosition(rigid.position + nextVec);
            // 4. 리지드바디의 물리 속도가 이동에 영향을 주지 않도록 보완하기
            rigid.velocity = Vector3.zero;
        }

        private void LateUpdate()
        {
            if(!isLive)
                return;
            anim.SetFloat("Speed", speed);
            spriter.flipX = GameSystem.instance.serverPos.x < rigid.position.x;
        }

        private void OnEnable()
        {
            //target = GameManager.instance.player.GetComponent<Rigidbody2D>();
            //GameManager.instance.targetServer = GameManager.instance.targetServer;
            isLive = true;
            coll.enabled = true;
            rigid.simulated = true; // 리지드바디 컴포넌트 비활성화는 simulated에서 관리
            spriter.sortingOrder = 2;
        }

        // 바이러스 초기화 함수
        public void Init(VirusInfo virusInfo) 
        {
            anim.runtimeAnimatorController = virusInfo.animCon;
            speed = virusInfo.speed;
            hp = virusInfo.hp;
            power = virusInfo.power;
            name = virusInfo.name;
            scanner.scanRange = virusInfo.scanRange;
            initSpeed = speed; // 애니메이션 컨트롤을 위함
            attackPeriod = virusInfo.attackPeriod;
            cost = virusInfo.cost;
            timer = attackPeriod;
            type = virusInfo.Type;
        }
        
        void Dead()
        {
            Debug.Log("시체좀 꺼라!");
            GameSystem.instance.player.money += cost;
            //int k = Random.Range(0, 10);
            //if(k == 7)
                DropGem();
            Destroy(gameObject);
        }

        IEnumerator Attacked(TowerType type)
        {
            if (type == TowerType.Basic)
            {
                spriter.color = Color.red;
            }
            else if (type == TowerType.Laser)
            {
                spriter.color = Color.green;
            }
            else
            {
                spriter.color = Color.magenta;
            }
            yield return wait;
            spriter.color = Color.white;
        }

        IEnumerator AttackedIce()
        {
            spriter.color = Color.blue;
            yield return wait;
            spriter.color = Color.white;
            isIced = false;
        }

        void DropGem()
        {
             GameObject gem = GameSystem.instance.pool.Get(2);
             gem.transform.position = transform.position;
        }
    
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Bullet") || !isLive)
                return;
            Bullet bullet = col.GetComponent<Bullet>();
            hp -= bullet.damage;
            
            audioSource.clip = bullet.hitSound;
            audioSource.Play();
            
            if (hp > 0  && bullet.type == TowerType.Ice && !isIced) // 얼음 공격 당했을 떄
            {
                isIced = true;
                StartCoroutine(AttackedIce());
            }
            else if(hp > 0 && (bullet.type == TowerType.Basic || bullet.type == TowerType.Laser || bullet.type == TowerType.Missile))
            {
                StartCoroutine(Attacked(bullet.type));
            }
            // else
            // {
            //     // .. Die
            //     isLive = false;
            //     coll.enabled = false;
            //     rigid.simulated = false; // 리지드바디 컴포넌트 비활성화는 simulated에서 관리
            //     spriter.sortingOrder = 1;
            //     anim.SetTrigger("Dead");
            // }
        }
    }
}
