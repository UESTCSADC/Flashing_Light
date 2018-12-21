using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{


public class MusicControl : MonoBehaviour {

    public AudioSource normal;
    public AudioSource battle;
    float normalVom = 1.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            Room Rm = Map.Instance.Pos2Room(this.transform.position);
            if (Rm != null)
            {
                if (Rm.roomStyle == "NormalRoom")
                    normalVom = Mathf.Lerp(0, 1, normalVom + Time.deltaTime*0.5f);
                else
                    normalVom= Mathf.Lerp(0, 1, normalVom - Time.deltaTime*0.5f);
            }
        normal.volume = normalVom;
        battle.volume = (1 - normalVom)*0.7f;
	}
}
}
