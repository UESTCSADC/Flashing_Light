using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{


public class MiniGameOneManager : MonoBehaviour {
    public GameObject Item;
    public GameObject floor;
    public GameObject wall;
    int Scale = 2;
    List<GameObject> floorList = new List<GameObject>();
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (i == 0 || j == 0 || i == 6 || j == 8)
                {
                    if (i == 5 && j == 8)
                    {
                        floorList.Add(Instantiate(floor, new Vector3(-i * Scale + Scale, -j * Scale, 0) + transform.position, Quaternion.identity));
                    }
                    else if (i == 1 && j == 0)
                        ;
                    else
                        Instantiate(wall, new Vector3(-i * Scale + Scale, -j * Scale, 0) + transform.position, Quaternion.identity);
                }
                else if (i == 2 &&( j ==3||j==6)||i==4&&(j==2||j==5))
                    Instantiate(wall, new Vector3(-i * Scale + Scale, -j * Scale, 0) + transform.position, Quaternion.identity);
                else
                    floorList.Add(Instantiate(floor, new Vector3(-i * Scale + Scale, -j * Scale, 0) + transform.position, Quaternion.identity));
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    bool isSecond = false;
    bool first = true;
        private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag != "Player")
                return;
        {
            if (Check()&&first)
                {
                    first = false;
                    Instantiate(LoadSprite.Instance.ItemPre[Random.Range(0, LoadSprite.Instance.ItemPre.Count)], this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                }
                else
                {
                    first = false;
                    Instantiate(LoadSprite.Instance.normalMonster[Random.Range(0, LoadSprite.Instance.normalMonster.Count)], this.transform.position + new Vector3(4, 1, 0), Quaternion.identity);
                };
            foreach (var item in floorList)
            {
                item.GetComponent<MiniGameOne>().ReSet();

            }
            isSecond = false;
        }
       // else
          //  isSecond = true;
    }
    bool Check()
    {
        bool temp = true;
        foreach (var item in floorList)
        {
            if (!item.GetComponent<MiniGameOne>().isMove)
            {

                Debug.Log("iiii");
                temp = false;
            }
        }
        return temp;
    }
}
}