using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEngine;

public class TextReader : MonoBehaviour

{
    [SerializeField] private Text showTxt;
    [SerializeField] private Image panel;
    private string path;

    private string[] contents;
    // Start is called before the first frame update

    void Start()
    {
        path = Application.dataPath;

        path += "/Resources/GameStory.txt";

        contents = System.IO.File.ReadAllLines(path);
        StartCoroutine(Typing());
    }
    
    IEnumerator Typing()
    {
        while (panel.color.a < 1f)
        {
            panel.color += new Color(0, 0, 0, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        
        string temp = "";
        for (int i = 0; i < contents.Length; i++)
        {
            for (int j = 0; j <= contents[i].Length; j++)
            {
                showTxt.text = temp + contents[i].Substring(0, j);
                yield return new WaitForSeconds(0.1f);
            }

            for (int j = 0; j < 6; j++)
            {
                if (j % 2 == 0)
                {
                    showTxt.text += '_';
                }
                else
                {
                    showTxt.text = temp + contents[i].Substring(0, contents[i].Length);
                }
                yield return new WaitForSeconds(0.1f);
            }
            temp += contents[i].Substring(0, contents[i].Length) + '\n';
        }
    }
}