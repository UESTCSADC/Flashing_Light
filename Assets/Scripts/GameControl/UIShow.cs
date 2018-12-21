using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShow : MonoBehaviour {

    public Text text;
    float showTime = 100F;
    float showTimer = 1.5f;

    private static UIShow instance;

    public static UIShow Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    // Use this for initialization
    void Start () {
        if (instance == null)
            instance = this;
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (showTime > showTimer)
            text.text = null;
        showTime += Time.deltaTime;
	}

    public void SetText(string content)
    {
        text.text = content;
        showTime = 0;
    }
}
