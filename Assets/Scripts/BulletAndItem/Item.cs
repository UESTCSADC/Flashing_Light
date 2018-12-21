using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

namespace FlashingLight
{

    public abstract class Activeprop
    {


        public float energe = 0;
        public int Itemnum = 1;

        public abstract void UseItem(Direction dir, Vector3 pos, Transform trans, string tag,int atk);


        public void SetItemNum(int Itemnum)
        {
            this.Itemnum = Itemnum;
        }


        public void activation1()
        {
            Itemnum = 1;
        }

        public void activation2()
        {
            Itemnum = 2;
        }
    }

    public class ApMove : MonoBehaviour
    {
        public Direction dir;
        public Vector3 dirVect3;
        public float Movespeed = 20;
        public int Num = 1;
        public int Type = 1;
        public string myself;
        public int atk = 0;
        //移动
        private void Start()
        {
            switch (Num)
            {
                case 1:
                    Move();
                    break;
                case 2:
                    Move2();
                    break;
                case 3:
                    Move3();
                    break;
            }

        }

        private void Update()
        {
            this.transform.position += dirVect3 * Time.deltaTime;
        }
        //朝着移动方向射击
        //直线
        public void Move()
        {

            switch (dir)
            {
                case Direction.Left:
                    dirVect3 += -Vector3.right * Movespeed;
                    break;
                case Direction.Up:
                    dirVect3 += Vector3.up * Movespeed;
                    break;
                case Direction.Right:
                    dirVect3 += Vector3.right * Movespeed;
                    break;
                case Direction.Down:
                    dirVect3 += -Vector3.up * Movespeed;
                    break;
            }
        }
        //右上角
        public void Move2()
        {

            switch (dir)
            {
                case Direction.Left:
                    dirVect3 += -Vector3.right * Movespeed;
                    dirVect3 += Vector3.up * Movespeed;
                    break;
                case Direction.Up:
                    dirVect3 += Vector3.right * Movespeed;
                    dirVect3 += Vector3.up * Movespeed;
                    break;
                case Direction.Right:
                    dirVect3 += Vector3.right * Movespeed;
                    dirVect3 += -Vector3.up * Movespeed;
                    break;
                case Direction.Down:
                    dirVect3 += -Vector3.up * Movespeed;
                    dirVect3 += -Vector3.right * Movespeed;
                    break;
            }
        }
        //左上角
        public void Move3()
        {

            switch (dir)
            {
                case Direction.Left:
                    dirVect3 += -Vector3.right * Movespeed;
                    dirVect3 += -Vector3.up * Movespeed;
                    break;
                case Direction.Up:
                    dirVect3 += -Vector3.right * Movespeed;
                    dirVect3 += Vector3.up * Movespeed;
                    break;
                case Direction.Right:
                    dirVect3 += Vector3.right * Movespeed;
                    dirVect3 += Vector3.up * Movespeed;
                    break;
                case Direction.Down:
                    dirVect3 += -Vector3.up * Movespeed;
                    dirVect3 += Vector3.right * Movespeed;
                    break;
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
 
         
                        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Monster")
                        {

                            Destroy(this.gameObject);
                            Instantiate(LoadSprite.Instance.bulletBoom5).transform.position = collision.transform.position;

                        }
                        if (collision.gameObject.tag == "Monster")
                        {
                            collision.gameObject.GetComponent<NoMoveMonster>().monsterAttributes.ReduceMonsterHP(atk*5);
                        }


           
                        


            

        }

        //public void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Monster")
        //    {

        //        Destroy(this.gameObject);
        //        Instantiate(LoadSprite.Instance.bulletBoom5).transform.position = collision.transform.position;

        //    }
        //    if (collision.gameObject.tag == "Monster")
        //    {
        //        collision.gameObject.GetComponent<NoMoveMonster>().monsterAttributes.ReduceMonsterHP(atk * 5);
        //    }


        //    Destroy(this.gameObject);


        //}

        
    }
        public class OverUse : Activeprop
        {

            public OverUse(int Itemnum)
            {

                this.Itemnum = Itemnum;
            }



            public override void UseItem(Direction dir, Vector3 pos, Transform trans, string tag,int atk)
            {


                switch (Itemnum)
                {
                    case 1:
                        Ap1(dir, pos, tag,atk);
                        break;
                    case 2:
                        Ap2(dir, pos, trans); ;
                        break;

                }


            }

            //发射一枚特殊子弹
            public void Ap1(Direction dir, Vector3 pos, string tag,int atk)
            {

                energe = Charge.Instance.Energe;
                if (energe == 5)
                {

                    GameObject Ap1 = MonoBehaviour.Instantiate(LoadSprite.Instance.AP1);
                    //Ap1.transform.Rotate(0,90, 0);
                    Setting(Ap1, dir, pos, 1, tag,atk);
                    
                MonoBehaviour.Destroy(Ap1, 3);
                }



            }
            //形成一个护盾
            public void Ap2(Direction dir, Vector3 pos, Transform trans)
            {

                energe = Charge.Instance.Energe;
                if (energe == 5)
                {

                    GameObject Ap2 = MonoBehaviour.Instantiate(LoadSprite.Instance.AP2);
                    Setting2(Ap2, pos, trans);
                    
                    MonoBehaviour.Destroy(Ap2, 5);
                }


            }
            //
            public void Ap3(Direction dir, Vector3 pos)
            {


            }

            //射击类道具位置初始化     
            public void Setting(GameObject Ap, Direction dir, Vector3 pos, int move, string tag,int atk)
            {
                Ap.AddComponent<BoxCollider2D>();
                Ap.GetComponent<BoxCollider2D>().isTrigger = true;
                Ap.GetComponent<BoxCollider2D>().size = new Vector2(0.3f, 0.3f);
                Ap.transform.position = pos;
                Rigidbody2D RD = Ap.AddComponent<Rigidbody2D>();
                RD.mass = 0;
                RD.gravityScale = 0;

                ApMove bm = Ap.AddComponent<ApMove>();
                bm.Num = move;
                bm.dir = dir;
                bm.atk = atk;
                bm.myself = tag;
                bm.Type = Itemnum;



            }   //(赋予对象，射击方向，初始位置，初始方向)


            //跟随性道具位置初始化
            public void Setting2(GameObject Ap, Vector3 pos, Transform trans)
            {
                Ap.AddComponent<BoxCollider2D>();
                Ap.GetComponent<BoxCollider2D>().isTrigger = true;
                Ap.GetComponent<BoxCollider2D>().size = new Vector2(5, 5);
                Ap.transform.position = pos;
                Ap.transform.parent = trans;
                Ap.AddComponent<SpriteRenderer>();


            }


        }

    
}
