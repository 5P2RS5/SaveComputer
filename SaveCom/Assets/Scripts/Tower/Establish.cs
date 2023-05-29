using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TowerData
{
    public Sprite sprite;
    public int damage; // 타워 데미지
    public float firePeriod; // 발사 주기
    public float ammoSpeed; // 발사 속도
}

public class Establish : MonoBehaviour
{
    public TowerData[] towerDatas;
    
    private int type;
    public Camera cam;
    private Vector3 estPos;
    private GameObject tower;
    private Vector2 worldPoint;
    private RaycastHit2D hit;
    private Button sel;
    private bool enableInstall;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        MouseClick();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            type = 0;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            type = 1;
        }
    }
    
    private void MouseClick()
    {
        if (Input.GetMouseButtonDown(0) && enableInstall)
        {
            Debug.Log("asd");
            worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.transform == null)
            {
                Debug.Log(estPos);
                Debug.Log("타워를 생성합니다.");
                install();
                tower.transform.position = worldPoint;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("타워 창 띄우기");
            worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.transform == null)
            {
                return;
            }
            else
            {
                if (hit.transform.CompareTag("Tower"))
                {
                    hit.transform.gameObject.GetComponent<Tower>().InteractBtn();
                }
            }
        }
    }

    
    void install()
    {
        tower = GameManager.instance.pool.Get(2);
        tower.GetComponent<Tower>().Inits(towerDatas[type]);
        // Cursor.SetCursor(default, Vector2.zero, CursorMode.Auto);
        enableInstall = false;
    }

    public void CanInstall()
    {
        enableInstall = true;
    }
}
