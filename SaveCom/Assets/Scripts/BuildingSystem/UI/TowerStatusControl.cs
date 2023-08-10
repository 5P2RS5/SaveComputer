using System;
using System.Collections;
using System.Collections.Generic;
using BuildingSystem.Models;
using GameInput;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class StatusData
{
    public int upgradeVal;
    public int level;
    public int countGem;
    public Text value;
    public Text levelText;
    public Button upgradeButton;
    public Image upgradeImage;
    public Slider slider;
    public Image sliderColor;
}

public class TowerStatusControl : MonoBehaviour
{
    [SerializeField] private Text towerName;
    [SerializeField] private float[] maxValues;
    
    public StatusData[] statusDatas;
    public Vector3 coords;
    public GameObject scanObj;
    public float resize;
    
    public AudioSource upgradeSound;
    
    private Buildable buildable;
    private Tower tower;
    private bool isTowerSelect;
    private bool canPower;
    private bool canCpu;
    private bool canGpu;

    public GameObject scanImg;
    public GameObject deImg;
    public Image range;
    public Image property;
    public Tooltip rangeText;
    public Tooltip propertyText;

    void Start()
    {
        statusDatas[0].levelText.text = "Lv." + statusDatas[0].level.ToString();
        statusDatas[1].levelText.text = "Lv." + statusDatas[1].level.ToString();
        statusDatas[2].levelText.text = "Lv." + statusDatas[2].level.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }

        if (Input.GetKeyDown(KeyCode.E) && isTowerSelect)
        {
            Sell();
        }
        
        for(int i = 0; i < 3; i++)
        {
            statusDatas[i].slider.value = statusDatas[i].countGem;
            if (statusDatas[i].countGem < statusDatas[i].slider.maxValue)
            {
                statusDatas[i].upgradeButton.image.color = Color.gray;
                statusDatas[i].sliderColor.color = Color.gray;
                statusDatas[i].upgradeButton.enabled = false;
            }
            else
            {
                statusDatas[i].upgradeButton.image.color = Color.white;
                if (i == 0)
                {
                    statusDatas[i].sliderColor.color = new Color(0, 0, 255);
                    canPower = true;
                }
                else if (i == 1)
                {
                    statusDatas[i].sliderColor.color = new Color(0, 255, 0);
                    canCpu = true;
                }
                else
                {
                    statusDatas[i].sliderColor.color = new Color(180, 0, 255);
                    canGpu = true;
                }
                statusDatas[i].upgradeButton.enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.Z) && canPower)
            {
                canPower = false;
                UpgradePower();
            }
            if (Input.GetKeyDown(KeyCode.X) && canCpu)
            {
                canCpu = false;
                UpgradeCpu();
            }
            if (Input.GetKeyDown(KeyCode.C) && canGpu)
            {
                canGpu = false;
                UpgradeGpu();
            }
        }
    }

    public void ViewTower()
    {
        isTowerSelect = true;
        buildable = GameSystem.instance.constructionLayer.TowerInfo(coords);
        tower = buildable.gameObject.GetComponent<Tower>();
        towerName.text = buildable.buildableType.name;
        scanObj.SetActive(true);
        scanObj.transform.position = new Vector3(MathF.Floor(coords.x) + 0.5f,MathF.Floor(coords.y) + 0.5f, 0);
        //scanObj.transform.localScale = new Vector3(tower.originTowerInfo.scanRange * resize, tower.originTowerInfo.scanRange * resize);
        statusDatas[0].value.text = buildable.buildableType.hp.ToString();
        statusDatas[1].value.text = buildable.buildableType.damage.ToString();
        statusDatas[2].value.text = buildable.buildableType.period.ToString("F1");
        statusDatas[0].levelText.text = "Lv." + statusDatas[0].level.ToString();
        statusDatas[1].levelText.text = "Lv." + statusDatas[1].level.ToString();
        statusDatas[2].levelText.text = "Lv." + statusDatas[2].level.ToString();
        scanImg.SetActive(true);
        deImg.SetActive(true);
        range.sprite = buildable.buildableType.rangeSprite;
        rangeText.message = buildable.buildableType.rangetext;
        property.sprite = buildable.buildableType.descriptionSprite;
        propertyText.message = buildable.buildableType.description;
    }
    
    public void UnViewTower()
    {
        scanObj.SetActive(false);
        towerName.text = "";
        statusDatas[0].value.text = "";
        statusDatas[1].value.text = "";
        statusDatas[2].value.text = "";
        statusDatas[0].levelText.text = "Lv." + statusDatas[0].level.ToString();
        statusDatas[1].levelText.text = "Lv." + statusDatas[1].level.ToString();
        statusDatas[2].levelText.text = "Lv." + statusDatas[2].level.ToString();
        scanImg.SetActive(false);
        deImg.SetActive(false);
    }
    
    public void Sell()
    {
        if (tower.originTowerInfo.name == "Server")
            return;
        tower.isDestroy = true;
        tower.towerCoord = coords;
        tower.audioSource.clip = tower.audioClips[1];
        tower.audioSource.pitch = 2.7f;
        tower.audioSource.Play();
        tower.anim.SetTrigger("Destroy");
        Exit();
    }
    

    public void Exit()
    {
        isTowerSelect = false;
        UnViewTower();
        for (int i = 0; i < 3; i++)
        {
            statusDatas[i].value.text = "";
        }
        //GameManager.instance.mouseUser.inputActions.Enable();
        GameSystem.instance.isOpenStatus = false;
    }

    public void UpgradePower() // 체력 업글
    {
        upgradeSound.Play();
        Debug.Log("파워업!");
        statusDatas[0].levelText.text = "Lv." + (++statusDatas[0].level).ToString();
        statusDatas[0].countGem -= (int)maxValues[0];
        statusDatas[0].slider.maxValue += 2;
        //statusDatas[0].value.text = tower.hp.ToString();
        for (int i = 0; i < GameSystem.instance.buildingManager.buildableItems.Length; i++)
        {
            GameSystem.instance.buildingManager.buildableItems[i].hp +=
                statusDatas[0].upgradeVal * statusDatas[0].level;
        }
        if(isTowerSelect)
            statusDatas[0].value.text = buildable.buildableType.hp.ToString();
    }
    
    public void UpgradeCpu()
    {
        upgradeSound.Play();
        statusDatas[1].levelText.text = "Lv." + (++statusDatas[1].level).ToString();
        statusDatas[1].countGem -= (int)maxValues[1];
        statusDatas[1].slider.maxValue += 2;
        //statusDatas[1].value.text = tower.damage.ToString();
        for (int i = 0; i < GameSystem.instance.buildingManager.buildableItems.Length; i++)
        {
            GameSystem.instance.buildingManager.buildableItems[i].damage += statusDatas[1].upgradeVal * statusDatas[1].level;
        }
        if(isTowerSelect)
            statusDatas[1].value.text = buildable.buildableType.damage.ToString();
    }
    
    public void UpgradeGpu() // 체력 업글
    {
        upgradeSound.Play();
        statusDatas[2].levelText.text = "Lv." + (++statusDatas[2].level).ToString();
        statusDatas[2].countGem -= (int)maxValues[2];
        statusDatas[2].slider.maxValue += 2;
        //statusDatas[2].value.text = tower.period.ToString("F1");
        for (int i = 0; i < GameSystem.instance.buildingManager.buildableItems.Length; i++)
        {
            GameSystem.instance.buildingManager.buildableItems[i].period += statusDatas[2].upgradeVal * statusDatas[2].level;
        }
        if(isTowerSelect)
            statusDatas[2].value.text = buildable.buildableType.period.ToString("F1");
    }
}
