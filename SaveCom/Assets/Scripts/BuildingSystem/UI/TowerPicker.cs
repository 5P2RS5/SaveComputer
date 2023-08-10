using System;
using BuildingSystem.Models;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BuildingSystem.UI
{
    [System.Serializable]
    public class TowerData
    {
        public Button button;
        public Image btnImage;
        public BuildableItem buildableItem;
        public Text cost;
    }
    
    public class TowerPicker : MonoBehaviour
    {
        public BuildingManager buildingManager;
        public TowerData[] towerdatas;
        public Text moneyText;
        public Text refreshText;
        public int refreshValue;
        public AudioSource gachaSound;
        
        private int random;
        
        private void Start()
        {
            Show();
            refreshText.text = "$ " + refreshValue.ToString();
        }

        private void Update()
        {
            moneyText.text = "$ " + GameSystem.instance.player.money.ToString();
            if (Input.GetKeyDown(KeyCode.Alpha1) && GameSystem.instance.isPick == false)
            {
                Pick(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && GameSystem.instance.isPick == false)
            {
                Pick(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && GameSystem.instance.isPick == false)
            {
                Pick(2);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Refresh();
            }
        }

        public void Show()
        {
            for (int i = 0; i < towerdatas.Length; i++)
            {
                Rand(i);
            }
        }

        void Rand(int idx)
        {
            random = Random.Range(0, buildingManager.buildableItems.Length);
            towerdatas[idx].buildableItem = buildingManager.buildableItems[random];
            towerdatas[idx].btnImage.sprite = buildingManager.buildableItems[random].uiIcon;
            towerdatas[idx].cost.text = "$ " + buildingManager.buildableItems[random].price.ToString();
        }
    
        public void Pick(int idx)
        {
            if (GameSystem.instance.isPick || GameSystem.instance.player.money - towerdatas[idx].buildableItem.price  < 0)
                return;
            gachaSound.Play();
            GameSystem.instance.isPick = true;
            GameSystem.instance.buildingPlacer.SetActiveBuildable(towerdatas[idx].buildableItem);
            GameSystem.instance.player.money -= towerdatas[idx].buildableItem.price;
            Rand(idx);
        }
    
        
        public void Refresh()
        {
            if(GameSystem.instance.player.money - refreshValue < 0)
                return;
            GameSystem.instance.player.money -= refreshValue;
            Show();
        }
    }
}
