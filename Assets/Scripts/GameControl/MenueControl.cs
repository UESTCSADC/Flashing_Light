using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenueControl : MonoBehaviour {
    public GameObject LoadSprite;
    public Slider slider;
    AsyncOperation async;
    int progress = 0;
    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        if(async!=null)
        {
            progress = (int)(async.progress * 100);
            slider.value = progress;
        }
    }  
    IEnumerator loadScene()
    {
        //异步读取场景。
        //Globe.loadName 就是A场景中需要读取的C场景名称。
        async = SceneManager.LoadSceneAsync("test");
        yield return async;
    }
    public void LoadScence()
    {
        StartCoroutine(loadScene());
    }
    public void Quit()
    {
        Application.Quit();
    }
}
