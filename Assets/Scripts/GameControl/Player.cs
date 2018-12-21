using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    public class Player : MonoBehaviour
    {
        #region UNITY自带函数
        private void Awake()
        {
           
        }

        private void Start()
        {
            sprite = LoadSprite.Instance.player;
            mat = LoadSprite.Instance.playerMat;
            if (sprite != null)
                ChangeSprite();
            attributes.ReSet();
            AddPhyCompents();
            SetCenter();
            LightManager.Instance.SetCharacter(this.gameObject, new Color(0.4f, 0.4f, 0.4f, 1.0f), new Vector2(0.0f, 10.0f), (uint)RoomID());
            GameManager.Instance.gameOver += Dead;
            LightManager.Instance.CharacterFlash(0.0f);
            // LightManager.Instance.SetCharacter(this.gameObject, new Color(0.4f, 0.4f, 0.4f, 1.0f), new Vector2(0.0f, 10.0f),(uint)RoomID());
        }
        //public float time = 0;
        public float length = 1;
        bool isDead = false;
        float InDark = 1;
        float InDarkTime = 0;
        private void Update()
        {
            if(!isDead)
            {
                if (InDark < InDarkTime && !LightManager.Instance.InLightRange())
                {
                    attributes.ChangeBlood(-3);
                    InDarkTime = 0;
                }
                else
                    InDarkTime += Time.deltaTime;
            Move(speed);
            RoomManger();
            Vector3 pos = this.transform.position;
            fireTime += Time.deltaTime;
            if(Map.Instance.Pos2Room(this.transform.position)!=null)
            LightManager.Instance.SetCamera(new Vector2(Map.Instance.Pos2Room(this.transform.position).x_Point, Map.Instance.Pos2Room(this.transform.position).y_Point), (uint)RoomID());
           // LightManager.Instance.SetCharacter(this.gameObject, new Color(0.4f, 0.4f, 0.4f, 1.0f), new Vector2(0.0f, 10.0f), (uint)RoomID());
            //this.transform.position = new Vector3(pos.x, pos.y, 8 + length * Mathf.Sin(time));
            //time += Time.deltaTime*3;
            lifeTrail.time = attributes.BloodInTime();
                lifeTrailMaker.hp= attributes.BloodInTime()*10;
            }
            else
            {
                Move(0);
            }
        }
        #endregion
        #region 死亡处理
        public void Dead()
        {
            isDead = true;
            LightManager.Instance.CharacterFlash(1.02f);
        }
        #endregion
        #region 渲染部分
        /// <summary>
        /// 初始人物贴图人物贴图
        /// </summary>
        public Sprite sprite;
        public Material mat;
        private void ChangeSprite(Sprite sprite)
        {
            SpriteRenderer _render = GetComponent<SpriteRenderer>();
            if (_render == null)
                _render = this.gameObject.AddComponent<SpriteRenderer>();
            _render.sprite = sprite;
            _render.material = mat;
            _render.sortingOrder = 20;
        }
        private void ChangeSprite()
        {
            SpriteRenderer _render = GetComponent<SpriteRenderer>();
            if (_render == null)
                _render = this.gameObject.AddComponent<SpriteRenderer>();
            _render.sprite = sprite;
            _render.material = mat;
            _render.sortingOrder = (int)RenderLayer.player;
            _render.drawMode = SpriteDrawMode.Sliced;
            _render.size = new Vector2(0.8f,0.8f);
            //_render.sortingLayerID = 3;
        }
        #endregion
        //人物属性
        public PlayerAttributes attributes = new PlayerAttributes();
        #region 移动
        public float speed = 6f;
        public void Move(float moveDistance)
        {
            switch (attributes.currentDirection)
            {
                case Direction.Left:
                    lifeObject.transform.eulerAngles = new Vector3(0, 0, 270);
                    rg.velocity = -Vector2.right * moveDistance;

                    break;
                case Direction.Up:
                    lifeObject.transform.eulerAngles = new Vector3(0, 0, 180);
                    rg.velocity =Vector2.up * moveDistance;
                    break;
                case Direction.Right:
                    lifeObject.transform.eulerAngles = new Vector3(0, 0, 90);
                    rg.velocity = Vector2.right * moveDistance;
                    break;
                case Direction.Down:
                    lifeObject.transform.eulerAngles = new Vector3(0, 0, 0);
                    rg.velocity = -Vector2.up * moveDistance;
                    break;
            }
        }
        #endregion
        #region 加载组件
        /// <summary>
        /// 增加物理组件和输入组件
        /// </summary>
        Rigidbody2D rg;
        GameObject lifeObject;
        TrailRenderer lifeTrail;
        TrailMaker lifeTrailMaker;
        private void AddPhyCompents()
        {
            this.gameObject.AddComponent<CircleCollider2D>();//.isTrigger = false;

            rg = this.gameObject.AddComponent<Rigidbody2D>();
            rg.constraints = RigidbodyConstraints2D.FreezeRotation;
            rg.gravityScale = 0;
            this.gameObject.AddComponent<MyInput>();
            //this.gameObject.AddComponent<LightTest>();//.type = LightType.Point;
            //GameObject go = new GameObject("Music");
            //go.AddComponent<LineRenderer>();
            //go.AddComponent<MusicJump>();
            //go.transform.SetParent(this.transform);
            //go.transform.position = this.transform.position;
            this.gameObject.tag = "Player";
            lifeObject = Instantiate(LoadSprite.Instance.playerObj);
            lifeObject.transform.SetParent(this.transform);
            lifeObject.transform.position= this.transform.position;
            lifeTrail = lifeObject.GetComponent<TrailRenderer>();
            lifeTrailMaker = lifeObject.GetComponent<TrailMaker>();
        }
        #endregion
        #region 设置玩家为中心
        /// <summary>
        /// 设置人物为相机中心
        /// </summary>
        private void SetCenter()
        {
            if (!(Camera.main.GetComponent<CameraFollow>()))

                Camera.main.gameObject.AddComponent<CameraFollow>().m_Target = this.transform;
            else
                Camera.main.GetComponent<CameraFollow>().m_Target = this.transform;
        }
        #endregion

        #region 子弹发射，道具使用
        public float fireTime = 0;
        public void Fire()
        {
            if(fireTime>1/attributes.attackSpeed)
            {
                fireTime = 0;
                attributes.bulletbase.Fire(attributes.currentDirection, this.transform.position, this.gameObject.tag,RoomID(),attributes.Atk());
            }
            
        }

        public void UseItem()
        {
            if(attributes.activeprop!=null&&SkillCD.Instance.canUse)
            {
                attributes.activeprop.UseItem(attributes.currentDirection, this.transform.position, this.transform, this.gameObject.tag, attributes.Atk());
                SkillCD.Instance.skillBtn.onClick.Invoke();
            }
        }
        #endregion

        #region 位置相关、房间管理
        public void SetPos(Vector2 pos)
        {
            this.transform.position = new Vector3(pos.x, pos.y, 3);
            lifeTrailMaker.CleanAll();
        }
        public int RoomID()
        {
            Room Rm = Map.Instance.Pos2Room(this.transform.position);
            if (Rm != null)
                return Rm.RoomID;              
            else
                return 0;
        }
        int roomID=-1;
        void RoomManger()
        {
            if (roomID != RoomID())
            {
                Map.Instance.roomList[RoomID()].SetMonsterActive(true);
                if(roomID>=0)
                {
                    Map.Instance.roomList[roomID].SetMonsterActive(false);
                }
                   
                roomID = RoomID();
            }
              

        }

        #endregion
        #region 碰撞检测
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Wall")
                attributes.ReduceHp(20);
            if (collision.transform.tag == "Monster2")
                attributes.ReduceHp(50);
        }
        #endregion
    }

}
