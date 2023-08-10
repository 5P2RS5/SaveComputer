using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonControl : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image[] windowImage;
    [SerializeField] private Sprite[] onButtonImg;
    [SerializeField] private Sprite[] offButtonImg;
    
    [SerializeField] private Sprite[] windowButtonSprites;
    [SerializeField] private Button windowButton;


    public float moveDistance;
    public float moveDuration = 0.5f;
    public bool isWindowOpen;
    
    private int presentTab;
    private bool isMoving;
    private Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            WindowConvert(++presentTab % 3);
        }
        
        if (Input.GetKeyDown(KeyCode.P)&& !isMoving)
        {
            WindowControl();
        }
    }

    public void WindowConvert(int on)
    {
        presentTab = on;
        buttons[on].image.sprite = onButtonImg[on];
        buttons[on].image.color = Color.white;
        windowImage[on].transform.gameObject.SetActive(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i != on)
            {
                buttons[i].image.sprite = offButtonImg[i];
                buttons[i].image.color = Color.gray;
                if(windowImage[i].IsActive())
                    windowImage[i].transform.gameObject.SetActive(false);
            }
        }
    }

    public void WindowControl()
    {
        moveDistance = isWindowOpen ? 278.5f : -278.5f;
        windowButton.image.sprite = isWindowOpen ? windowButtonSprites[0] : windowButtonSprites[1];
        isWindowOpen = !isWindowOpen;
        StartCoroutine(MoveWindow(moveDistance));
    }


    private IEnumerator MoveWindow(float moveDistance)
    {
        isMoving = true;

        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = startPosition - new Vector3(0f, moveDistance, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        transform.localPosition = targetPosition;
        isMoving = false;
    }
}
