using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlashingLight { 
public class LightTest : MonoBehaviour {

    public GameObject lightSource;
    public Material SharedMapMaterial;
    public LightManager LM;
	// Use this for initialization

	void Start () {
        LM.SetCharacter(this.gameObject, new Color(0.4f, 0.4f, 0.4f, 1.0f), new Vector2(0.0f, 10.0f),0); //开始必须要设置主角！！！
       
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
            LM.AddLight(this.transform.position, new Color(Random.Range(0,1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f)), new Vector2(0, 10.0f), true, 0); //添加光源
        if (Input.GetKeyDown(KeyCode.Q))
            LM.AddSpark(this.transform.position, new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f)), new Vector2(0, 10.0f), 0);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(-0.1f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(0.1f, 0.0f, 0.0f);
        if (Input.GetKey(KeyCode.W))
            transform.Translate(0.0f, 0.1f, 0.0f);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(0.0f, -0.1f, 0.0f);
    }

    }
}
