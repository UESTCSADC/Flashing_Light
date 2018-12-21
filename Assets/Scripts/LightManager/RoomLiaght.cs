using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
public class RoomLiaght : MonoBehaviour {
        public Color col;
        public Vector2 range;
        public uint roomNum;
        Vector3 Pos;
        public bool isLight = false;
        public GameObject show;
	// Use this for initialization
	void Start () {
            Pos = this.gameObject.transform.position + new Vector3(0, 0, 3);
            if (isLight)
            {
                //LightManager.Instance.AddLight(Pos, col, range, true, roomNum);
                if(show!=null)
                Instantiate(show, this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);

                //isLight = true;
            }
        }
	public void SetRoomLight(Color col,Vector2 range,uint roomNum)
        {
            this.col = col;
            this.range = range;
            this.roomNum = roomNum;
        }
	// Update is called once per frame
	void Update () {
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isLight)
            {
                LightManager.Instance.AddLight(Pos, col, range, true, roomNum);
                if (show != null)
                    Instantiate(show, this.transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
                isLight = true;
            }
           
        }
    }
}
