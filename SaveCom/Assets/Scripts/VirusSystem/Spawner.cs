using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using VirusSystem.Model;
using Random = UnityEngine.Random;
using UnityEngine.UI;
namespace VirusSystem
{
    public enum VirusType
    {
        Melee, Ranged
    }
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private List<VirusInfo> virusInfos;

        [SerializeField] private Transform[] spawnPoints; 
        
        [SerializeField]
        public GameObject[] virusObjs;

        [SerializeField] public Slider virusSlider;
        [SerializeField] private Image handle;
        [SerializeField] private Image fillImage;
        [SerializeField] private Sprite[] handleImages;
        [SerializeField] public Text stageText;
        [SerializeField] private float maxtime;

        private float timer;
        public int stage;
        private bool isStart;

        private void Awake()
        {
            stage = -1;
            virusSlider.maxValue = maxtime;
            timer = maxtime;
            fillImage.color = Color.yellow;
            handle.sprite = handleImages[1];
        }

        void Update()
        {
            if (stage >= GameSystem.instance.virusCounts.Length)
            {
                stageText.text = "수고하셨습니다.";
                return;
            }
            if (virusSlider.value <= 0 && isStart)
            {
                timer = maxtime;
                isStart = false;
                virusSlider.maxValue = maxtime;
                fillImage.color = Color.yellow;
                handle.sprite = handleImages[1];
            }
            
            if (timer > 0f && !isStart)
            {
                timer -= Time.deltaTime;
                virusSlider.value = timer;
                stageText.text = virusSlider.value.ToString("F1");
            }

            if (timer <= 0f && !isStart)
            {
                timer = maxtime;
                fillImage.color = new Color(1f, 0.3f, 0);
                //Debug.Log(fillImage.color);
                handle.sprite = handleImages[0];
                isStart = true;
                Debug.Log("asdasd");
                stage += 1;
                stageText.text = "Stage " + (stage + 1);
                virusSlider.maxValue = GameSystem.instance.virusCounts[stage];
                virusSlider.value = virusSlider.maxValue;
                StartCoroutine(Spawn());
            }
        }

        IEnumerator Spawn()
        {
            Virus newVirus = null;
            for (int i = 0; i < GameSystem.instance.virusCounts[stage]; i++)
            {
                int typeNum = Random.Range(0, virusObjs.Length);
                Vector3 stdPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                Vector3 modPos = new Vector3(stdPos.x + Random.Range(-2, 3), stdPos.y + Random.Range(-2, 3), stdPos.z);
                newVirus = Instantiate(virusObjs[typeNum], modPos, Quaternion.identity, transform).GetComponent<Virus>();
                newVirus.Init(virusInfos[typeNum]);
                yield return new WaitForSeconds(0.2f);
            }
            // GameObject enemy = GameManager.instance.pool.Get(1);
            // enemy.transform.position = spawnPoint.position;
            // enemy.GetComponent<Virus>().Init(virusInfos[level]);
            
            //Vector3 asd = new Vector3(transform.position.x, transform.position.y + (Random.Range(-1, 2) * 2), transform.position.z);
        }
    }
}