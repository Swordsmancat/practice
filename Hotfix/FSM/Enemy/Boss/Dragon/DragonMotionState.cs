using GameFramework;
using GameFramework.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.Hotfix
{
    /// <summary>
    /// 黑龙运动(地面)状态
    /// </summary>
    public class DragonMotionState : EnemyMotionState
    {
        private new DragonLogic owner;

        protected override void OnEnter(IFsm<EnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner as DragonLogic;
            owner.IsFying = false;
        }

        protected override void OnUpdate(IFsm<EnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            //AIUtility.RotateToTarget(owner.find_Player, owner, -10, 10);
        }

        public static new DragonMotionState Create()
        {
            DragonMotionState state = ReferencePool.Acquire<DragonMotionState>();
            return state;
        }

        public static void SmoothRotate(Transform a, Transform b)
        {
            Vector3 targetDir = a.position - b.position;

            float angle = Vector3.SignedAngle(targetDir, b.up, Vector3.right);
            if (angle < 0)
            {
                //up
                //Debug.Log("up");
                //Debug.Log(angle + 90);
                angle += 90;
                angle = -angle;
                //Debug.Log(angle);
            }
            else
            {
                //down
                //Debug.Log("down");
                //Debug.Log(angle - 90);
                angle -= 90;
                angle = -angle;
                //Debug.Log(angle);
            }
            //Debug.Log(b.eulerAngles);
            //Debug.Log(b.rotation);
            Quaternion rotate = Quaternion.Euler(angle, b.eulerAngles.y, b.eulerAngles.z);
            b.rotation = rotate;

            //Debug.Log(rotate);
            //Debug.Log(b.rotation);
            //if (angle >= 20)
            //{
            //    //向左旋转
            //    Quaternion targetRotate = Quaternion.Euler(b.localEulerAngles.x - 3f, 0f, 0f);
            //    b.rotation = Quaternion.RotateTowards(b.rotation, targetRotate, 300 * Time.deltaTime);
            //}
            //else if (angle <= -20)
            //{
            //    //向右旋转
            //    Quaternion targetRotate = Quaternion.Euler(b.localEulerAngles.x + 3f, 0f, 0f);
            //    b.rotation = Quaternion.RotateTowards(b.rotation, targetRotate, 300 * Time.deltaTime);
            //}

        }
    }
}

