using System;
using System.Collections;
using System.Collections.Generic;
using BuildingSystem;
using BuildingSystem.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerTemp : MonoBehaviour
{
    [SerializeField] private BuildableItem buildableItem;

    private float hp;
    public Vector3Int coord;
    public Slider hpSlider;
    public GameObject sliderObj;
    public Image sliderImage;
    private float timer = 0f;
    private bool isAttacked;
    private SpriteRenderer spriter;
    private Color initColor;
    private BoxCollider2D collider2D;
    
    private void Start()
    {
        hp = buildableItem.hp;
        hpSlider.maxValue = hp;
        coord = buildableItem.coords;
        spriter = GetComponent<SpriteRenderer>();
        initColor = spriter.color;
    }

    private void Update()
    {
        if (timer >= 5.0f)
        {
            sliderObj.SetActive(false);
            hpSlider.value = hpSlider.maxValue;
        }

        if (hpSlider.value <= hp / 2 && hpSlider.value > hp / 4)
        {
            sliderImage.color = Color.yellow;
        }
        else if (hpSlider.value <= hp / 4)
        {
            sliderImage.color = Color.red;
        }
        else
        {
            sliderImage.color = Color.green;
        }
        
        if (hpSlider.value <= 0)
        {
            GameSystem.instance.constructionLayer.Destroy(coord);
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttacked)
        {
            isAttacked = false;
            sliderObj.SetActive(true);
            //spriter.enabled = true;
            Debug.Log("아야!");
            hpSlider.value -= 50;
            timer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ㅇ엉?");
            spriter.color = new Color(0, 0, 0, 100);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("꾸어");
            spriter.color = initColor;
        }    
    }
}
