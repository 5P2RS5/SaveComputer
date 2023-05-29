using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager pool;
    public Player player;
    public Texture2D cursorImg;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != null)
                Destroy(this.gameObject);
        }
    }

    public void ChangeCursor()
    {
        //Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);
    }
}
