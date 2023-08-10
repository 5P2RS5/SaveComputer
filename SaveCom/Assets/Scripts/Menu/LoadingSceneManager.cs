using System;
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public LoadingSceneManager instance = null;
    [SerializeField] private Slider progressbar;
    [SerializeField] private Text loadText;
    public int sceneCount;

    // void Awake()
    // {
    //     if (null == instance)
    //     {
    //         instance = this;
    //     }
    //     else
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
    
    private void Start()
    {
        progressbar.value = 0;
        StartCoroutine(LoadScene());
    }
    

    // 비동기 로드는 Scene을 불러올 때 멈추지 않고 다른 작업을 할 수 있습니다.
    // LoadScene()로 Scene을 불러오면 완료될 때까지 다른 작업을 수행하지 않습니다.
    // // operation.isDone : 작업의 완료 유무를 bool 값으로 반환
    // operation.progress : 진행정도를 float형 0, 1반환한다. 0 - 진행중, 1- 진행완료
    // operation.allowSceneActivation : true면 로딩이 완료되면 바로 씬을 넘기고 false면 progress가 0.9f에서 멈춘다. 다시 true로 하면 씬을 넘어갈 수 있다.
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("FrontEnd"); // 비동기식으로 씬 불러오기
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            Debug.Log("자 한 번 해 보 자 고");
            yield return null;
            if (progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }

            else if (operation.progress >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }
            
            if(progressbar.value >= 1f)
            {
                loadText.text = "Press Any Key";
            }

            if (Input.anyKeyDown && progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                yield break;
            }
        }
    }
    
    
}