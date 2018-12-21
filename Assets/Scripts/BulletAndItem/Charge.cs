using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用于蓄力设计和道具充能，在游戏开始挂在GameManager;
/// </summary>
public class Charge : MonoBehaviour {
    public float power;
    public float energe;
    public float energeMax = 5;
    static Charge instance;

    public static Charge Instance
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

    public float Energe
    {
        get
        {
            float a = energe;
            if(a>=5)
            energe = 0f;
            return a;

        }

        set
        {
            energe = value;
        }
    }

    public float Power
    {
        get
        {
            float a= power;
            if (a > 0)
            power = 0f;
            return a;
        }

        set
        {
            power = value;
        }
    }


    // Use this for initialization
    void Start () {
       
        if(instance==null)
        instance = this;
	}

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Z))
        {
            //记录按下的帧数  
            Power += Time.deltaTime;
        }




        if (Energe < energeMax)
        {
            Energe += Time.deltaTime;
        }
        else
        {
            Energe = energeMax;
        }
 







    }

}
