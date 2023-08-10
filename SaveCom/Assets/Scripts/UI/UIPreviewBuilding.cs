using System;
using System.Collections;
using System.Collections.Generic;
using BuildingSystem;
using BuildingSystem.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPreviewBuilding : MonoBehaviour
{
    
    [SerializeField] private Image previewImg;
    [SerializeField] private BuildingPlacer BuildingPlacer;

    public BuildableItem[] buildableItems;
    public Text[] dosInfo; // 0 : name, 1 : hp, 2: sell, 3: reSell, 4: damage

    private string fullText;
    private string currentText;
    private int currentIndex;
    
    public float delay = 0.1f;
    private WaitForSeconds wfs;

    private void Awake()
    {
        wfs = new WaitForSeconds(delay);
    }

    private void Init(int index)
    {
        StopAllCoroutines();
        for (int i = 0; i < dosInfo.Length; i++)
        {
            dosInfo[i].text = "";
        }
        fullText = buildableItems[index].name;
        currentIndex = 0;
        currentText = "";
    }
    
    public void Preview(int index)
    {
        Init(index);
        BuildingPlacer.SetActiveBuildable(buildableItems[index]);
        previewImg.sprite = buildableItems[index].previewSprite;
        // previewImg.color = bi.btn.image.color;
        StartCoroutine(AnimateText(index));
    }

    IEnumerator AnimateText(int index)
    {
        Debug.Log("index: " + index);
        Debug.Log("currentText: " + currentText);
        Debug.Log("currentIndex: " + currentIndex);
        while (currentIndex < fullText.Length)
        {
            currentText += fullText[currentIndex];
            dosInfo[0].text = currentText;
            currentIndex++;
            yield return wfs;
        }

        dosInfo[1].text = "HP: " + (buildableItems[index].hp > 0 ? Convert.ToString(buildableItems[index].hp) : "infinity");
        dosInfo[2].text = "Price: " + Convert.ToString(buildableItems[index].price);
        dosInfo[3].text = "Sell: " + Convert.ToString(buildableItems[index].sell);
        dosInfo[4].text = "Damage: " + (buildableItems[index].damage > 0 ? Convert.ToString(buildableItems[index].damage) : "Can't Attack");
    }
}
