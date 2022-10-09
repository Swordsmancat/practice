
using UnityEngine;
using UnityEngine.Animations;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
   public class AttackStateMachine:MonoBehaviour
    {

        public bool isAttack = false;
       public void OnAttackStart()
        {
            isAttack = true;
            Log.Info("播放攻击");
        }

        public void OnAttackEnd()
        {
            isAttack = false;
            Log.Info("结束攻击播放");
        }
    }
}
