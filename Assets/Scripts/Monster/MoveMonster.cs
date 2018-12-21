using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    [RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(CircleCollider2D))]
    public class MoveMonster : MonoBehaviour
    {
        public MonsterSettings InitializeSettings;
        public MonsterAttributes monsterAttributes = new MonsterAttributes();
        private bool IsFinding = false;
        private Room rm;
        private List<Node> nodeList = new List<Node>();
        private GameObject player;
        private Node currentNode;
        private Node playerNode;
		private Rigidbody2D rig;
		private int nodeNumber =0;
        //    private FindPath findPath;
        // Use this for initialization
        Material Mat;
        bool IsLive = false;
        void Start()
        {
            Initialize();
			rig = GetComponent<Rigidbody2D> ();
            rm = Map.Instance.Pos2Room(new Vector2(transform.position.x, transform.position.y));
            Mat = this.gameObject.GetComponent<SpriteRenderer>().material;
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

        // Update is called once per frame
        void Update()
        {
            IsLive = monsterAttributes.CheckDeath();
            if(IsLive)
            {

                Instantiate(LoadSprite.Instance.BulletPre[Random.Range(0, LoadSprite.Instance.BulletPre.Count)], this.transform.position, Quaternion.identity);
                Instantiate(LoadSprite.Instance.MonsterDeadEffect, this.transform.position, Quaternion.identity);
                GameObject.Destroy(this.gameObject);
            }
			CheckRoom ();
            if (IsFinding)
            {
                Node startNode = CheckNode(new Vector2(transform.position.x, transform.position.y));
                Node endNode = CheckNode(new Vector2(player.transform.position.x, player.transform.position.y));
				List<Node> temp = FindPath.FindingPath(startNode,endNode);
				//nodeList = FindPath.FindingPath(startNode,endNode);
				if (temp != null)
                {
					nodeList = temp;
					nodeNumber = 0;
					GoTarget ();
                }
				//GoTarget ();
            }
            Mat.SetFloat("_Dam", 1 - monsterAttributes.HPValue*1.0f / monsterAttributes.maxMonsterHP);
           // Debug.Log("MAx" + monsterAttributes.maxMonsterHP);
           // Debug.Log("min" + monsterAttributes.HPValue);
        }

		void FixedUpdate()
		{
		//	GoTarget ();
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //           if (IsFinding)
            if (collision.gameObject.tag == "Player")
            {
                IsFinding = true;
                player = collision.gameObject;
            }
            if (collision.gameObject.tag == "Door")
            {
                rig.velocity = rig.velocity * (-1.0f);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (IsFinding) return;
                IsFinding = true;
                player = collision.gameObject;
            }
            if (collision.gameObject.tag == "Door")
            {
                rig.velocity = rig.velocity * (-1.0f);
            }
        }

        private Node CheckNode(Vector2 pos)
        {
            return rm.nodeDictionary[new Vector2((int)pos.x, (int)pos.y)];
        }

		private void GoTarget()
		{
			Vector3 dir = new Vector3 (nodeList [nodeNumber].WorldPos.x, nodeList [nodeNumber].WorldPos.y, 0) - transform.position;
		/*	if (dir.magnitude < 0.5f)
			{
				nodeNumber++;
				dir = new Vector3 (nodeList [nodeNumber].WorldPos.x, nodeList [nodeNumber].WorldPos.y, 0) - transform.position;
			}*/
			dir = Vector3.Normalize (dir);
			rig.velocity = new Vector2 (dir.x, dir.y) * monsterAttributes.moveSpeed;
		}

		private void CheckRoom()
		{
            if (player == null)
            {
                IsFinding = false;
                rig.velocity = Vector2.zero;
                return;
            }
            Room playerRm = Map.Instance.Pos2Room (new Vector2(player.transform.position.x,player.transform.position.y));
//			Debug.Log (playerRm.RoomID);
			if (playerRm == null) {
				IsFinding = false;
				rig.velocity = Vector2.zero;
				return;
			}
			if (playerRm.RoomID != rm.RoomID)
			{
				IsFinding = false;
				rig.velocity = Vector2.zero;
			}
		}
    }
}
