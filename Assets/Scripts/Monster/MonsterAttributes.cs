using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlashingLight
{
    /// <summary>
    /// Monster attributes.
    /// </summary>
    public class MonsterAttributes
    {
        public int RoomID=0;
        #region HP Operation
        private int monsterHP;
        public int maxMonsterHP;

        /// <summary>
        /// Gets the blood value.
        /// </summary>
        /// <value>The blood value.</value>
        public int HPValue
        {
            get
            {
                return monsterHP;
            }
        }

        /// <summary>
        /// Reduces the monster hp.
        /// </summary>
        /// <param name="reduceValue">Reduce value.</param>
        public void ReduceMonsterHP(int reduceValue)
        {
            if (reduceValue < 0) return;
            monsterHP -= reduceValue;
            if (monsterHP < 0)
            {
                monsterHP = 0;
            }
        }

        /// <summary>
        /// Restores the monster hp.
        /// </summary>
        /// <param name="restoreValue">Restore value.</param>
        public void RestoreMonsterHP(int restoreValue)
        {
            if (restoreValue < 0) return;
            monsterHP += restoreValue;
        }

        /// <summary>
        /// Sets the monster max hp.
        /// </summary>
        /// <param name="setValue">Set value.</param>
        public void SetMonsterMaxHP(int setValue)
        {
            if (setValue < 0)
            {
                maxMonsterHP = 0;
                return;
            }
            maxMonsterHP = setValue;
        }

        public bool CheckDeath()
        {
            if (HPValue <=0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Monster Attack
        private int attackValue;
        public float attackSpeed;

        /// <summary>
        /// Attacks up.
        /// </summary>
        /// <param name="upValue">Up value.</param>
        public void AttackUp(int upValue)
        {
            if (upValue <= 0) return;
            attackValue += upValue;
        }

        /// <summary>
        /// Attacks down.
        /// </summary>
        /// <param name="downValue">Down value.</param>
        public void AttackDown(int downValue)
        {
            if (downValue <= 0) return;
            attackValue -= downValue;
            if (attackValue < 0)
            {
                attackValue = 0;
            }
        }

        /// <summary>
        /// Attack speed up.
        /// </summary>
        /// <param name="upValue">Up value.</param>
        public void AttackSpeedUp(float upValue)
        {
            if (upValue < 0) return;
            attackSpeed += upValue;
        }

        /// <summary>
        /// Attack speed down.
        /// </summary>
        /// <param name="downValue">Down value.</param>
        public void AttackSpeedDown(float downValue)
        {
            if (downValue < 0) return;
            attackSpeed -= downValue;
            if (attackSpeed < 0)
            {
                attackSpeed = 0;
            }
        }

        /// <summary>
        /// Sets the monster attack value.
        /// </summary>
        /// <param name="setAttack">Setk attack.</param>
        public void SetAttackValue(int setAttack)
        {
            if (setAttack < 0)
            {
                attackValue = 0;
                return;
            }
            attackValue = setAttack;
        }

        public int GetAttackValue()
        {
            return attackValue;
        }

        /// <summary>
        /// Sets the monster attack speed.
        /// </summary>
        /// <param name="setSpeed">Set speed.</param>
        public void SetAttackSpeed(float setSpeed)
        {
            if (setSpeed < 0)
            {
                attackSpeed = 0;
                return;
            }
            attackSpeed = setSpeed;
        }


        #endregion

        #region Monster Move
        private bool canMove;
		public float moveSpeed;

        /// <summary>
        /// Monster, the move speed up.
        /// </summary>
        /// <param name="upValue">Up value.</param>
        public void MoveSpeedUp(float upValue)
        {
            if (upValue < 0) return;
            moveSpeed += upValue;
        }

        /// <summary>
        /// Monster, the move speed down.
        /// </summary>
        /// <param name="downValue">Down value.</param>
        public void MoveSpeedDown(float downValue)
        {
            if (downValue < 0) return;
            moveSpeed -= downValue;
            if (moveSpeed < 0)
            {
                moveSpeed = 0;
            }
        }

        /// <summary>
        /// Sets the monster can or not move.
        /// </summary>
        /// <param name="setMove">If set to <c>true</c> set move.</param>
        public void SetCanMove(bool setMove)
        {
            canMove = setMove;
        }

        /// <summary>
        /// Sets the monster move speed.
        /// </summary>
        /// <param name="setMove">Set move.</param>
        public void SetMoveSpeed(float setMove)
        {
            if (setMove < 0)
            {
                moveSpeed = 0;
                return;
            }
            moveSpeed = setMove;
        }

        #endregion

        /// <summary>
        /// Initializes the monster.
        /// </summary>
        /// <param name="currentHP">Current hp.</param>
        /// <param name="maxHP">Max hp.</param>
        /// <param name="attack">Attack.</param>
        /// <param name="attackspeed">Attackspeed.</param>
        /// <param name="canOrNot">If set to <c>true</c> can or not.</param>
        /// <param name="monsterMoveSpeed">Monster move speed.</param>
        public void InitializeMonster(int currentHP, int maxHP, int attack, float attackspeed, bool canOrNot, float monsterMoveSpeed)
        {
            monsterHP = currentHP;
            SetMonsterMaxHP(maxHP);
            SetAttackValue(attack);
            SetAttackSpeed(attackspeed);
            SetCanMove(canOrNot);
            SetMoveSpeed(monsterMoveSpeed);
        }
    }

    /// <summary>
    /// Monster settings.
    /// </summary>
    [System.Serializable]
    public class MonsterSettings
    {
        public int CurrentHP;
        public int MaxHP;
        public int Attack;
        public float AttackSpeed;
        public bool CanMove;
        public float MoveSpeed;
    }
}
