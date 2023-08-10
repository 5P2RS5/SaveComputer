using System.Collections;
using System.Collections.Generic;
using BuildingSystem;
using GameInput;
using UnityEngine;
using UnityEngine.UI;
using VirusSystem;

public class GameSystem : MonoBehaviour
{
    public static GameSystem instance;
    
    public PoolManager pool;
    public Player player;
    public Texture2D cursorImg;
    public MouseUser mouseUser;
    public GameObject subMenu;
    
    public ConstructionLayer constructionLayer;
    public TowerStatusControl towerStatusControl;
    public BuildingPlacer buildingPlacer;
    public BuildingManager buildingManager;
    public Spawner spawner;
    public GameObject towerStaus;

    public int[] virusCounts;
    public Vector3 serverPos; // 몬스터가 공격할 서버 위치

    public bool isOpenStatus;
    public bool isPick;
    private bool isEnd;
    
    [Header("게임 클리어 관련")]
    public int serverCnt = 4;

    [SerializeField] private Sprite[] clearSprites; // 0 = 패배,  1 = 승리
    [SerializeField] private GameObject clearObj;
    [SerializeField] private Image clearImg;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != null)
                Destroy(this.gameObject);
        }
    }
    
    private void Update()
    {
        if(isEnd)
            return;
        if (serverCnt == 0 && !isEnd)
        {
            Lose();
        }
        
        if(spawner.stage == virusCounts.Length && !isEnd)
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            subMenu.SetActive(!subMenu.activeSelf);
            Time.timeScale = subMenu.activeSelf ? 0 : 1; // 서브 메뉴 키면 게임 일시정지
        }
    }

    void Lose()
    {
        isEnd = true;
        audioSource.clip = audioClips[0];
        audioSource.Play();
        clearImg.sprite = clearSprites[0];
        clearObj.SetActive(true);
        Time.timeScale = 0;
    }

    void Win()
    {
        isEnd = true;
        audioSource.clip = audioClips[1];
        audioSource.Play();
        clearImg.sprite = clearSprites[1];
        clearObj.SetActive(true);
        Time.timeScale = 0;
    }

    public void ChangeCursor()
    {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);
    }
}
