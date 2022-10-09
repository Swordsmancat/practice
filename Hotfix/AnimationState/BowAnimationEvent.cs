//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/9/29/周四 9:47:41
//------------------------------------------------------------
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    public class BowAnimationEvent : StateMachineBehaviour
    {
        private PlayerLogic m_Player;

        [LabelText("攻击伤害判断列表"), InfoBox("基于动画时间,填入以秒为单位"), SerializeField]
        private List<float> ShootTimelist;

        private List<int> m_ShootIDList;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            m_Player = animator.gameObject.GetComponent<PlayerLogic>();
            m_ShootIDList = new List<int>();
            for (int i = 0; i < ShootTimelist.Count; i++)
            {
                m_ShootIDList.Add(GameEntry.Timer.AddOnceTimer((long)(ShootTimelist[i]*1000), () => m_Player.ShootArrow()));
            }
            
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            for (int i = 0; i < m_ShootIDList.Count; i++)
            {
                if (GameEntry.Timer.IsExistTimer(m_ShootIDList[i]))
                {
                    GameEntry.Timer.CancelTimer(m_ShootIDList[i]);
                }
            }
            m_ShootIDList.Clear();
        }

    }
}
