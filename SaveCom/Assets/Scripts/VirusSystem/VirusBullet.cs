using BuildingSystem.Models;
using UnityEngine;

namespace VirusSystem
{
    public class VirusBullet : MonoBehaviour
    {
        public int damage;
        public int towerType;
        public AudioClip hitSound;

        private Rigidbody2D rigid;
        private float timer = 0f;
        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 3f)
            {
                HitorOver();
            }
        }

        public void Init(int dmg, float ammoSpeed, AudioClip clip, Vector3 dir)
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
            if (col.transform.gameObject.layer == 9 || col.transform.gameObject.layer == 10)
            {
                Debug.Log("때렸당");
                HitorOver();
            }
        }
    }
}
