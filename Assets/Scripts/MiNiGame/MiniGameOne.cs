using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameOne : MonoBehaviour {

    public bool isMove = false;
    public Color col = new Color(1, 0, 0, 1);
    Color ori;
	// Use this for initialization
	void Start () {
        ori = GetComponent<SpriteRenderer>().color;
        //GetComponent<SpriteRenderer>().color = col;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ReSet()
    {
        isMove = false;
        GetComponent<SpriteRenderer>().color=ori;
    }
    bool isSecond = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        Debug.Log("1111");
        //if(isSecond)
        { 
        if (isMove)
        {
            isMove = false;
            GetComponent<SpriteRenderer>().color = ori;
        }
        else
        {
            isMove = true;
            GetComponent<SpriteRenderer>().color = col;
        }
        isSecond = false;
    }
       // else
         //   isSecond = true; 

    }
}
