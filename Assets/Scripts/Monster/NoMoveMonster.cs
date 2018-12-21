using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(BoxCollider2D))]
	public class NoMoveMonster : MonoBehaviour
    {
        public MonsterSettings InitializeSettings;
        public MonsterAttributes monsterAttributes = new MonsterAttributes();
        public bool CanAttack;
		bool IsLive = false;
        MonsterBulletBase bullet=new OverMonsterFire(1,1);
		private float timer;
        Material Mat;


        void Start()
		{
			Initialize ();
            //	InvokeRepeating ("AttackMaker", 1.0f, 1.0f);
            Mat = GetComponent<SpriteRenderer>().material;
            Mat.SetFloat("_Dam", 0.0f);
		}
        /// <summary>
        /// Initialize the Monster.
        /// </summary>
        void Initialize()
        {
            monsterAttributes.InitializeMonster(InitializeSettings.CurrentHP,
                                                InitializeSettings.MaxHP,
                                                InitializeSettings.Attack,
                                                InitializeSettings.AttackSpeed,
                                                InitializeSettings.CanMove,
                                                InitializeSettings.MoveSpeed);
        }
        private void OnDestroy()
        {
        }
        void Update()
        {
            IsLive = monsterAttributes.CheckDeath(); 
	//		Debug.Log (IsLive);
            if(IsLive)
            {
                Instantiate(LoadSprite.Instance.ItemPre[Random.Range(0, LoadSprite.Instance.ItemPre.Count)], this.transform.position, Quaternion.identity);
                Instantiate(LoadSprite.Instance.MonsterDeadEffect, this.transform.position, Quaternion.identity);
                //if dead ,play animation and destroy.
                GameObject.Destroy(this.gameObject);
            }
			TimerForFire ();
            Mat.SetFloat("_Dam", 1 - monsterAttributes.HPValue*1.0f/ monsterAttributes.maxMonsterHP);
            //Debug.Log("MAx" + monsterAttributes.maxMonsterHP);
            //Debug.Log("min" + monsterAttributes.HPValue);
        }

        void FixedUpdate()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }

        void  OnParticleCollision(GameObject obj)
        {

        }

        private void AttackMaker()
        {
			bullet.Fire(Direction.Up,transform.position,this.gameObject.tag,monsterAttributes.RoomID,monsterAttributes.GetAttackValue());
			bullet.Fire(Direction.Left,transform.position,this.gameObject.tag, monsterAttributes.RoomID, monsterAttributes.GetAttackValue());
			bullet.Fire(Direction.Right,transform.position,this.gameObject.tag, monsterAttributes.RoomID, monsterAttributes.GetAttackValue());
			bullet.Fire(Direction.Down,transform.position,this.gameObject.tag, monsterAttributes.RoomID, monsterAttributes.GetAttackValue());
        }

		private void TimerForFire()
		{
			timer += Time.deltaTime;
			if (timer > 1 / monsterAttributes.attackSpeed) {
				timer = 0;
				AttackMaker ();
			}
		}
    }
}
