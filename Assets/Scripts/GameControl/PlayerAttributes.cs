using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    /// <summary>
    /// 记录修改玩家的属性
    /// </summary>
    public class PlayerAttributes
    {
        #region 方向
        public Direction lastDirection = Direction.Up;//上一个方向
        public Direction currentDirection = Direction.Up;
        public void SetDirection(Direction value)
        {
            //水平只能切换到纵轴，纵轴只能切换到水平轴
            if ((int)(value - currentDirection) % 2 != 0)
            {
                lastDirection = currentDirection;
                currentDirection = value;
            }
        }
        #endregion
        #region 生命值
        private int bloodValue = 0;
        private int maxBloodValue = 0;
        float timeMax = 2;
        public void ChangeBlood(int value)
        {
            BloodValue += value;
            if (BloodValue <= 0)
            {
                GameManager.Instance.GameOver();
                BloodValue = 0;
            }
            if (BloodValue > maxBloodValue)
            {
                BloodValue = maxBloodValue;
            }
        }
        public int BloodValue
        {
            get
            {
                return bloodValue;
            }

            set
            {
                bloodValue = value;
            }
        }
        public float BloodInTime()
        {
            return bloodValue * timeMax / maxBloodValue;
        }
        /// <summary>
        /// 减少血量
        /// </summary>
        /// <param name="value">减少的值应该是大于0的，增加血量用RestoreHp(int value)</param>
        public void ReduceHp(int value)
        {
            if (value < 0)
                return;
            LightManager.Instance.CharacterFlash(0.5f);
            ChangeBlood(-value);
           
        }
        /// <summary>
        /// 增加血量
        /// </summary>
        /// <param name="value">增加的值大于0，减少血量用ReduceHp(int value)</param>
        public void RestoreHp(int value)
        {
            if (value < 0)
                return;
            ChangeBlood(value);
            UIShow.Instance.SetText("HP+" + value);
        }
        /// <summary>
        /// 设置血量最大值
        /// </summary>
        /// <param name="value"></param>
        public void SetMaxBloodValue(int value)
        {
            if (value < 0)
                return;
            if (BloodValue > value)
                BloodValue = value;
            maxBloodValue = value;
        }
        #endregion

        #region 攻击力
        private int attack = 5;

        public int Atk()
        {   
            return attack;
        }
        /// <summary>
        /// 减少攻击力
        /// </summary>
        /// <param name="value"></param>


        public void ReduceAtk(int value)
        {
            if (value < 0)
                return;
            attack -= value;
            if (attack < 0)
                attack = 0;
        }
        /// <summary>
        /// 增加攻击力
        /// </summary>
        public void RestoreAtk(int value)
        {
            if (value < 0)
                return;
            attack += value;
            UIShow.Instance.SetText("ATK+" + value);
        }
        #endregion
        #region 攻击速度
        public float attackSpeed = 1;
        /// <summary>
        /// 减少攻击力速度，每秒攻击次数
        /// </summary>
        /// <param name="value"></param>
        public void ReduceAtkSpeed(float value)
        {
            if (value < 0)
                return;
            attackSpeed -= value;
            if (attackSpeed < 0)
                attackSpeed = 0;
        }
        /// <summary>
        /// 增加攻击力速度，每秒攻击次数
        /// </summary>
        public void RestoreAtkSpeed(float value)    
        {
            if (attackSpeed < 0)
                return;
            attackSpeed += value;
            UIShow.Instance.SetText("ATKSpeed+" + value);
        }
        #endregion
        public PlayerAttributes()
        {
            // GameManager.Instance.gameStart += ReSet;
        }

        public void ReSet()
        {
            lastDirection = Direction.Up;
            currentDirection = Direction.Up;
            bloodValue = 500;
            maxBloodValue = 500;
            attack = 50;
            attackSpeed = 2;
        }
        public void ReSet(Direction lastDir, Direction currentDir, int blood, int maxBlood, int Atk, float atkSpeed)
        {
            lastDirection = lastDir;
            currentDirection = currentDir;
            bloodValue = blood;
            maxBloodValue = maxBlood;
            attack = Atk;
            attackSpeed = atkSpeed;
        }
        /// <summary>
        /// 主动道具
        /// </summary>
        public Activeprop activeprop = new OverUse(1);
        /// <summary>
        /// 子弹
        /// </summary>
        public BulletBase bulletbase = new OverFire(1, 1);

    }
    /// <summary>
    /// 人物移动方向
    /// </summary>
    public enum Direction
    {
        None = -1,
        Left = 3,
        Up = 0,
        Right = 1,
        Down = 2
    }

    public enum RenderLayer
    {
        player=20,
        bullet=19,
        item=18
    }
}
