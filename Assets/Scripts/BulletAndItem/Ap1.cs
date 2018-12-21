using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlashingLight {
    public class Ap1 : MonoBehaviour {
        public BulletType BulletNum;
        public Item ItemNum;
        public Activeprops Ap;
        public enum Item
        {
            None=0,
            AtkUp_small=1,
            AtkUp_mid= 2,
            AtkUp_large=3,

            AtkSpeedUp_small=4,
            AtkSpeedUp_mid = 5,
            AtkSpeedUp_large=6,

            RestoreHp_small = 7,
            RestoreHp_mid=8,
            RestoreHp_large=9,


        }
        public enum BulletType
        {
            None=0,
            BulletBlue= 1,
            PoisonMissileBlue= 2,
            SymbolMissileBlue = 3,
            FatMissileBlue = 4,
            StormMissile = 5,
            TwoFire = 6,
            ThreeFire =7,
            
        }

        public enum Activeprops
        {
            None=0,
            explosionisart =1,
            Rua=2,
            Flash=3,
            
        }


        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
        void OnCollisionEnter2D(Collision2D coll)
        {
       
            if (coll.gameObject.tag == "Player")
            {
                switch (BulletNum)
                {
                    case BulletType.BulletBlue:
                        coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBNo(1);
                        UIShow.Instance.SetText("Normal");
                        break;
                    case BulletType.PoisonMissileBlue:
                        coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBNo(2);
                        UIShow.Instance.SetText("SmokeBullet");
                        break;
                    case BulletType.SymbolMissileBlue:
                        coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBNo(3);
                        UIShow.Instance.SetText("LightBullet");
                        break;
                    case BulletType.FatMissileBlue:
                        coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBNo(4);
                        UIShow.Instance.SetText("Geometry");
                        break;
                    case BulletType.StormMissile:
                        coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBnumBNo(4, 5);
                        UIShow.Instance.SetText("EnergyPower");
                        break;
                    case BulletType.TwoFire:
                        coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBnum(2);
                        UIShow.Instance.SetText("DoubleFire");
                        break;
                    case BulletType.ThreeFire:
                        coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBnum(3);
                        UIShow.Instance.SetText("TribleFire");
                        break;
                }
                switch (ItemNum)
                {
                    case Item.AtkUp_small:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreAtk(1);
                        break;
                    case Item.AtkUp_mid:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreAtk(2);
                        break;
                    case Item.AtkUp_large:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreAtk(3);
                        break;

                    case Item.AtkSpeedUp_small:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreAtkSpeed(1);
                        break;
                    case Item.AtkSpeedUp_mid:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreAtkSpeed(2);
                        break;
                    case Item.AtkSpeedUp_large:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreAtkSpeed(3);
                        break;

                    case Item.RestoreHp_small:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreHp(50);
                        break;
                    case Item.RestoreHp_mid:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreHp(100);
                        break;
                    case Item.RestoreHp_large:
                        coll.gameObject.GetComponent<Player>().attributes.RestoreHp(200);
                        break;

                }
                switch (Ap)
                {
                    case Activeprops.explosionisart:
                        coll.gameObject.GetComponent<Player>().attributes.activeprop.SetItemNum(1);
                        SkillCD.Instance.SetSkill(LoadSprite.Instance.Ap1UI);
                        UIShow.Instance.SetText("Explosion is art");
                        break;
                    case Activeprops.Rua:
                        coll.gameObject.GetComponent<Player>().attributes.activeprop.SetItemNum(2);
                        SkillCD.Instance.SetSkill(LoadSprite.Instance.Ap1UI);
                        UIShow.Instance.SetText("Holy Shield");
                        break;
                }                     
                //删除这个道具
                Destroy(this.gameObject);
               


            }


        }
    }
}


////攻击力减少
//coll.gameObject.GetComponent<Player>().attributes.ReduceAtk(1);

////主动道具解锁
//coll.gameObject.GetComponent<Player>().attributes.activeprop = new OverUse(2);

////切换子弹射击方式
//coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBnum(2);
////切换子弹形态
//coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBNo(6);
////切换子弹射击方式以及形态
//coll.gameObject.GetComponent<Player>().attributes.bulletbase.SetBnumBNo(4,5);


