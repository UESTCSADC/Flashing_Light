using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 子弹类
/// </summary>
namespace FlashingLight
{



    public abstract class MonsterBulletBase
    {
        public float power = 0;
        public int atk;
        public int Bnum = 1;   //射击方式
        public int BNo = 2;    //子弹种类(贴图)


        public void SetBnumBNo(int Bnum, int BNo)
        {
            this.Bnum = Bnum;
            this.BNo = BNo;
        }
        public void SetBnum(int Bnum)
        {
            this.Bnum = Bnum;
        }
        public void SetBNo(int BNo)
        {
            this.BNo = BNo;
        }
        public abstract void Fire(Direction dir, Vector3 pos, String tag, int roomNumber, int atk);
    }

    //移动类
    public class MonsterBulletMove : MonoBehaviour
    {
        public int atk = 0;
        public Direction dir;
        public Vector3 dirVect3;
        public float Movespeed = 15f;
        public int Num = 3;
        public int Type = 1;
        public String myself;
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

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag != myself)
            {
                switch (Type)
                {
                    case 1:

                        if(collision.gameObject.tag == "Player")
                        {
                            Debug.Log("111");
                            Destroy(this.gameObject);
                            collision.gameObject.GetComponent<Player>().attributes.ReduceHp(atk);
                        }
                        if (collision.gameObject.tag == "Invincible")
                        {
                            Debug.Log("222");
                            Destroy(this.gameObject);
                            
                        }
                        break;

                    case 2:

                        if(collision.gameObject.tag == "Player")
                        {
                            Destroy(this.gameObject);
                            collision.gameObject.GetComponent<Player>().attributes.ReduceHp(atk);
                        }
                        break;
                    case 3:
                       if
                            (collision.gameObject.tag == "Player")
                        {
                            Destroy(this.gameObject);
                            collision.gameObject.GetComponent<Player>().attributes.ReduceHp(atk);
                        }
                        break;
                    case 4:
                       if
                            (collision.gameObject.tag == "Player")
                        {
                            Destroy(this.gameObject);
                            collision.gameObject.GetComponent<Player>().attributes.ReduceHp(atk);
                        }
                        break;

                    case 5:
                        if
                            (collision.gameObject.tag == "Player")
                        {
                            Destroy(this.gameObject);
                            collision.gameObject.GetComponent<Player>().attributes.ReduceHp(atk);
                        }
                        break;



                }




            }


        }
        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.tag != myself)
            {

                if (coll.gameObject.tag == "Wall")
                {
                    Destroy(this.gameObject);
                    Instantiate(LoadSprite.Instance.bulletBoom).transform.position = coll.transform.position;
                }



            }

        }
    }
    //射击+子弹类
    public class OverMonsterFire : MonsterBulletBase
    {


        public OverMonsterFire(int Bnum, int BNo)
        {
            this.Bnum = Bnum;
            this.BNo = BNo;
        }

        public OverMonsterFire(int Bnum)
        {
            this.Bnum = Bnum;
            this.BNo = 2;
        }



        public override void Fire(Direction dir, Vector3 pos, String tag, int roomNumber, int atk)
        {

            switch (Bnum)
            {
                case 1:
                    one(dir, pos, tag, roomNumber, atk);
                    break;
                case 2:
                    two(dir, pos, tag, roomNumber, atk);
                    break;
                case 3:
                    three(dir, pos, tag, roomNumber, atk);
                    break;
                case 4:
                    charge(dir, pos, tag, roomNumber, atk);
                    break;

            }
        }



        //子弹的射击方式
        //直线一个
        public void one(Direction dir, Vector3 pos, String tag, int roomNumber, int atk)
        {
            GameObject Bullet = new GameObject("BULLET");
            Setting(Bullet, 1, pos, dir, tag, roomNumber, atk);

            switch (BNo)
            {
                case 1:
                    bullet1(Bullet);
                    break;
                case 2:
                    bullet2(Bullet);
                    break;
                case 3:
                    bullet3(Bullet);
                    break;
                case 4:
                    bullet4(Bullet);
                    break;
                case 5:
                    bullet5(Bullet);
                    break;
                case 6:
                    bullet6(Bullet);
                    break;

            }
            //改变子弹速度
            //bm.atkspeed -= 30;
        }
        //两叉
        public void two(Direction dir, Vector3 pos, String tag, int roomNumber, int atk)
        {

            GameObject Bullet = new GameObject("BULLET");
            Setting(Bullet, 2, pos, dir, tag, roomNumber, atk);

            GameObject Bullet2 = new GameObject("BULLET2");
            Setting(Bullet2, 3, pos, dir, tag, roomNumber, atk);

            //子弹种类（贴图）
            switch (BNo)
            {
                case 1:
                    bullet1(Bullet);
                    bullet1(Bullet2);
                    break;
                case 2:
                    bullet2(Bullet);
                    bullet2(Bullet2);
                    break;
                case 3:
                    bullet3(Bullet);
                    bullet3(Bullet2);
                    break;
                case 4:
                    bullet4(Bullet);
                    bullet4(Bullet2);
                    break;
                case 5:
                    bullet5(Bullet);
                    bullet5(Bullet2);
                    break;
                case 6:
                    bullet6(Bullet);
                    bullet6(Bullet2);
                    break;

            }

        }
        //三叉
        public void three(Direction dir, Vector3 pos, String tag, int roomNumber, int atk)
        {

            GameObject Bullet = new GameObject("BULLET");
            Setting(Bullet, 2, pos, dir, tag, roomNumber, atk);

            GameObject Bullet2 = new GameObject("BULLET2");
            Setting(Bullet2, 3, pos, dir, tag, roomNumber, atk);

            GameObject Bullet3 = new GameObject("BULLET3");
            Setting(Bullet3, 1, pos, dir, tag, roomNumber, atk);

            //子弹种类
            switch (BNo)
            {
                case 1:
                    bullet1(Bullet);
                    bullet1(Bullet2);
                    bullet1(Bullet3);
                    break;
                case 2:
                    bullet2(Bullet);
                    bullet2(Bullet2);
                    bullet2(Bullet3);
                    break;
                case 3:
                    bullet3(Bullet);
                    bullet3(Bullet2);
                    bullet3(Bullet3);
                    break;
                case 4:
                    bullet4(Bullet);
                    bullet4(Bullet2);
                    bullet4(Bullet3);
                    break;
                case 5:
                    bullet5(Bullet);
                    bullet5(Bullet2);
                    bullet5(Bullet3);
                    break;
                case 6:
                    bullet6(Bullet);
                    bullet6(Bullet2);
                    bullet6(Bullet3);
                    break;

            }

            //改变子弹速度
            //bm.atkspeed -= 30;
        }
        //蓄力
        public void charge(Direction dir, Vector3 pos, String tag, int roomNumber, int atk)
        {
            power = Charge.Instance.Power;
            if (power > 3)
            {
                GameObject Bullet2 = new GameObject("BULLET");
                Setting(Bullet2, 1, pos, dir, tag, roomNumber, atk);

                switch (BNo)
                {
                    case 1:
                        bullet1(Bullet2);
                        break;
                    case 2:
                        bullet2(Bullet2);
                        break;
                    case 5:
                        bullet5(Bullet2);
                        break;

                }

            }
            else
            {


                GameObject Bullet = new GameObject("BULLET");
                Setting(Bullet, 1, pos, dir, tag, roomNumber, atk);

                switch (BNo)
                {
                    case 1:
                        bullet1(Bullet);
                        break;
                    case 2:
                        bullet2(Bullet);
                        break;
                    case 5:
                        bullet6(Bullet);
                        break;

                }
            }





            //改变子弹速度
            //bm.atkspeed -= 30;
        }
        //贴图
        public void bullet1(GameObject a)
        {
            //贴图
            //SpriteRenderer SR = a.AddComponent<SpriteRenderer>();
            //SR.sprite = LoadSprite.Instance.bullet;
            //SR.sortingOrder = (int) RenderLayer.bullet;
            //SR.drawMode = SpriteDrawMode.Sliced;
            //SR.size = new Vector2(0.4f, 0.4f);
            GameObject go = MonoBehaviour.Instantiate(LoadSprite.Instance.bullet1);
            go.transform.SetParent(a.transform);
            go.transform.position = a.transform.position;
            //子弹消失
            LightManager.Instance.DeleteLight(a);
            MonoBehaviour.Destroy(a, 3);

        }

        public void bullet2(GameObject a)
        {
            GameObject go = MonoBehaviour.Instantiate(LoadSprite.Instance.bullet2);
            go.transform.SetParent(a.transform);
            go.transform.position = a.transform.position;
            //子弹消失
            LightManager.Instance.DeleteLight(a);
            MonoBehaviour.Destroy(a, 3);

        }

        public void bullet3(GameObject a)
        {
            GameObject go = MonoBehaviour.Instantiate(LoadSprite.Instance.bullet3);
            go.transform.SetParent(a.transform);
            go.transform.position = a.transform.position;
            //子弹消失
            LightManager.Instance.DeleteLight(a);
            MonoBehaviour.Destroy(a, 3);

        }
        public void bullet4(GameObject a)
        {
            GameObject go = MonoBehaviour.Instantiate(LoadSprite.Instance.bullet4);
            go.transform.SetParent(a.transform);
            go.transform.position = a.transform.position;
            //子弹消失
            LightManager.Instance.DeleteLight(a);
            MonoBehaviour.Destroy(a, 3);

        }
        public void bullet5(GameObject a)
        {
            GameObject go = MonoBehaviour.Instantiate(LoadSprite.Instance.bullet5);
            go.transform.SetParent(a.transform);
            go.transform.position = a.transform.position;
            //子弹消失
            LightManager.Instance.DeleteLight(a);
            MonoBehaviour.Destroy(a, 3);

        }
        public void bullet6(GameObject a)
        {
            GameObject go = MonoBehaviour.Instantiate(LoadSprite.Instance.bullet6);
            go.transform.SetParent(a.transform);
            go.transform.position = a.transform.position;
            //子弹消失
            LightManager.Instance.DeleteLight(a);
            MonoBehaviour.Destroy(a, 3);

        }


        //public void bullet3(GameObject a)
        //{
        ////贴图
        //SpriteRenderer SR = a.AddComponent<SpriteRenderer>();
        //SR.sprite = LoadSprite.Instance.player;
        //SR.drawMode = SpriteDrawMode.Sliced;
        //SR.size = new Vector2(0.4f, 0.4f);
        //SR.sortingOrder = (int)RenderLayer.bullet;
        ////子弹消失
        //MonoBehaviour.Destroy(a, 0.5f);

        //}





        //子弹发射方向设定（默认向上行走）       
        public void Setting(GameObject bb, int num, Vector3 pos, Direction dir, String tag, int roomNumber, int atk)
        {
           
            bb.AddComponent<BoxCollider2D>();
            bb.GetComponent<BoxCollider2D>().size = new Vector2(0.3f, 0.3f);

            bb.GetComponent<BoxCollider2D>().isTrigger = true;
            Rigidbody2D RD = bb.AddComponent<Rigidbody2D>();
            RD.mass = 0;
            RD.gravityScale = 0;
            bb.transform.position = pos;
            MonsterBulletMove bm = bb.AddComponent<MonsterBulletMove>();
            bm.Num = num;
            bm.dir = dir;
            bm.myself = tag;
            bm.Type = BNo;
            bm.atk = atk;
        }   //(赋予对象，射击方向，初始位置，初始方向)

    }

}



