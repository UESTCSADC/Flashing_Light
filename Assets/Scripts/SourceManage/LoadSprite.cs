using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    /// <summary>
    /// 负责所有贴图的加载
    /// </summary>
    public class LoadSprite
    {
        static LoadSprite instance;

        /// <summary>
        /// 玩家贴图
        /// </summary>
        public Sprite player;
        public Sprite bullet;
        public Material playerMat;
        public Material mapMat;
        public Material MusicMat;
        public GameObject[] floor=new GameObject[249];
        int floorLength = 249;
        public GameObject playerObj;
        public GameObject bullet1;
        public GameObject bullet2;

        public GameObject bulletBoom3;
        public GameObject bullet3;
        public GameObject bulletBoom4;
        public GameObject bullet4;

        public GameObject bullet5;
        public GameObject bulletBoom5;
        public GameObject bullet6;

        public GameObject bulletBoom;
        public GameObject bulletBoom2;

        public List<GameObject> normalMonster = new List<UnityEngine.GameObject>();

        public List<GameObject> BulletPre= new List<GameObject>();

        public List<GameObject> ItemPre = new List<GameObject>();

        public List<GameObject> HPPre = new List<GameObject>();

        public GameObject MiniGame;

        public GameObject AP1;
        public GameObject AP2;
        public Sprite Ap1UI;

        public GameObject MonsterDeadEffect;

        public LoadSprite()
        {
            if (instance == null)
                instance = this;
            MonsterDeadEffect = Resources.Load<GameObject>("MonsterPrefab/BeamUpBlue(Clone)");
            MiniGame = Resources.Load<GameObject>("MiniGamePre/MiniGamePerfab");
            bulletBoom = Resources.Load<GameObject>("test/BulletImpactBlue");
            bulletBoom2 = Resources.Load<GameObject>("test/PoisonImpactBlue");
            bulletBoom3 = Resources.Load<GameObject>("test/SymbolExplosionBlue");
            bulletBoom4 = Resources.Load<GameObject>("test/FatExplosionBlue");
            bulletBoom5 = Resources.Load<GameObject>("test/StormImpact");
            bullet1 = Resources.Load<GameObject>("test/BulletBlue");
            bullet2 = Resources.Load<GameObject>("test/PoisonMissileBlue");
            bullet3 = Resources.Load<GameObject>("test/SymbolMissileBlue");
            bullet4 = Resources.Load<GameObject>("test/FatMissileBlue");
            bullet5 = Resources.Load<GameObject>("test/StormMissile");
            bullet6 = Resources.Load<GameObject>("test/StormMissile1");
            playerObj = Resources.Load<GameObject>("EffectPrefab/OrbBlue");
            player = Resources.Load<Sprite>("Textures/hero00");
            bullet = Resources.Load<Sprite>("Textures/Triangle");
            for (int i = 0; i < floorLength; i++)
            {
                floor[i] = Resources.Load<GameObject>("SpriteMapPrefab/perfab"+i);
            }
            
            mapMat = Resources.Load<Material>("Materials/MapMat");
            playerMat= Resources.Load<Material>("Materials/Player");
            MusicMat= Resources.Load<Material>("Materials/MusicMat");

            normalMonster.Add( Resources.Load<GameObject>("MonsterPrefab/NoMoveMonster"));
            normalMonster.Add(Resources.Load<GameObject>("MonsterPrefab/MoveMonster"));

            BulletPre.AddRange(Resources.LoadAll<GameObject>("Bullet&ItemPre/bullet"));

            ItemPre.AddRange(Resources.LoadAll<GameObject>("Bullet&ItemPre/item"));
            HPPre.AddRange(Resources.LoadAll<GameObject>("Bullet&ItemPre/HP"));

            AP1 = Resources.Load<GameObject>("test/StormMissile2");
            AP2 = Resources.Load<GameObject>("test/BoostPadBlue");
            Ap1UI= Resources.Load<Sprite>("Textures/Ap1");
        }
        public static LoadSprite Instance
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
    }
}
