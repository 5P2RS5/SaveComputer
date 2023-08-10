using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;
    public TextMeshProUGUI textComponent;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetAndShowToolToip(string message)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
    }

    public void HideToolTip()
    {
        Debug.Log("마우스 나가는데?");
        gameObject.SetActive(false);
        textComponent.text = string.Empty;

    }
}
