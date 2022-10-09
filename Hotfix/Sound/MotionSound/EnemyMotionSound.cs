using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    public class EnemyMotionSound : MonoBehaviour
    {

        private EnemyData m_EnemyData;

        void Start()
        {
            EnemyLogic m_EnemyLogic = gameObject.GetComponent<EnemyLogic>();
            m_EnemyData = m_EnemyLogic.enemyData;
        }

        /// <summary>
        /// 敌人动作声音
        /// </summary>
        public void EnemyMotionSoundOne()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.MotionSoundId0);
        }

        public void EnemyMotionSoundTwo()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.MotionSoundId1);
        }

        public void EnemyMotionSoundThree()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.MotionSoundId2);
        }

        public void EnemyMotionSoundFour()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.MotionSoundId3);
        }
        public void EnemyMotionSoundFive()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.MotionSoundId4);
        }
        //public void EnemyMotionSoundSix()
        //{
        //    GameEntry.Sound.PlaySound(m_EnemyData.MotionSoundId5);
        //}
        //public void EnemyMotionSoundSeven()
        //{
        //    GameEntry.Sound.PlaySound(m_EnemyData.MotionSoundId6);
        //}
        //public void EnemyMotionSoundEight()
        //{
        //    GameEntry.Sound.PlaySound(m_EnemyData.MotionSoundId7);
        //}


        /// <summary>
        /// 具体声音
        /// </summary>
        public void EnemyWalkSound()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.WalkSoundId);
        }

        public void EnemyShoutSound()
        {
            GameEntry.Sound.PlaySound(m_EnemyData.ShoutSoundId);
        }
    }
}

