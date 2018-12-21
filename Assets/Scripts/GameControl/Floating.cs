using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Floating : MonoBehaviour {
    public float radian = 0; 
    public float perRadian = 0.03f;  
    public float radius = 0.8f; 
    RectTransform oldPos;

	void Start()
	{
        oldPos = GetComponent<RectTransform>();
	}
      
	void Update()
	{
        radian += perRadian*Time.deltaTime;  
		float dy = Mathf.Cos(radian) * radius;  
        GetComponent<RectTransform>().position = oldPos.position + new Vector3(0, dy, 0);
	}
}