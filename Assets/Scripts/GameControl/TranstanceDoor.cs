using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{


public class TranstanceDoor : MonoBehaviour {

    public Vector3 POS;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.gameObject.tag == "Player")
                collision.gameObject.GetComponent<Player>().SetPos(POS);
    }
}
}
