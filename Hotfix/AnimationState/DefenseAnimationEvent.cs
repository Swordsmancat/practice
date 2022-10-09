//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/8/17/周三 14:59:26
//------------------------------------------------------------
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Farm.Hotfix
{
    public class DefenseAnimationEvent : StateMachineBehaviour
    {
        private PlayerLogic m_Player;

        [LabelText("退出当前动画时取消防御"),ToggleLeft,HideLabel,SerializeField]
        private bool m_IsExitDefense;

        [LabelText("进入或取消防御"), ToggleLeft, HideLabel,SerializeField,DisableIf("m_IsExitDefense")]
        private bool m_IsDefense;
       
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            m_Player = animator.gameObject.GetComponent<PlayerLogic>();
            GameEntry.Timer.AddOnceTimer(0, () => m_Player.SetDefense(m_IsDefense));
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (m_IsExitDefense)
            {
                GameEntry.Timer.AddOnceTimer(0, () => m_Player.SetDefense(false));
            }
        }
    }
}
