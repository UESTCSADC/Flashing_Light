using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    public class RestoreProps : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Player")
            {
                collision.transform.GetComponent<Player>().attributes.RestoreHp(50);
                Destroy(this.gameObject);
            }
               
        }
    }
    public class RestorePropsManager
    {
        Sprite sprite;
        public RestorePropsManager(Sprite sprite)
        {
            this.sprite = sprite;
        }


    }
    public enum RestorePropsState
    {
        微光碎片,
        光晕,
        拉的光辉
    }
}
