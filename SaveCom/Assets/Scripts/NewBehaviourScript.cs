using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color initColor;
    private BoxCollider2D collider2D;
    private SpriteRenderer playerSpriter;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initColor = spriteRenderer.color;
        playerSpriter = GameSystem.instance.player.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("잉?");
            playerSpriter.color = new Color(1, 1, 1, 0.5f);
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("꾸어");
            playerSpriter.color = new Color(1, 1, 1, 1);
            spriteRenderer.color = initColor;
        }    
    }
}
