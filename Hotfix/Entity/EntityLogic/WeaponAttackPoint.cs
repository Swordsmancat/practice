using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
   public class WeaponAttackPoint :MonoBehaviour
    {

        [SerializeField]
        private AttackPointBox attackPointBox;

        private Entity m_Owner;

        private float m_Timer;

        private float m_CheckTimer = 0.1f;

        private bool m_Check = false;

        [Serializable]
        public class AttackPointBox
        {
            public Vector3 halfExtents;
            public Vector3 offset;
            public Transform attackRoot;
        }

        List<TargetableObject> hitList;
        List<Vector3> hitPoint;
        private void Start()
        {
            m_Owner = GetComponent<WeaponLogic>();
            hitList = new List<TargetableObject>();
            hitPoint = new List<Vector3>();
        }

        private void Update()
        {
            if (m_Check)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer >= m_CheckTimer)
                {
                    m_Timer = 0;
                    m_Check = false;
                }
            }
        }

        //public void SetAttackPoint()
        //{
        //    for (int i = 0; i < attackPoints.Length; ++i)
        //    {
        //        WeaponAttackPoint.AttackPoint pts = attackPoints[i];

        //        Vector3 worldPos = pts.attackRoot.position + pts.attackRoot.TransformVector(pts.offset);
        //        Vector3 attackVector = worldPos - m_PreviousPos[i];
        //        if (attackVector.magnitude < 0.001f)
        //        {
        //            attackVector = Vector3.forward * 0.0001f;
        //        }

        //        Ray r = new Ray(worldPos, attackVector.normalized);
        //        int contacts = Physics.SphereCastNonAlloc(r, pts.radius, s_RaycastHitsCache, attackVector.magnitude, 1<<LayerMask.NameToLayer("Targetable Object"), QueryTriggerInteraction.Ignore);
        //        for (int j = 0; j < contacts; ++j)
        //        {
        //            Collider col = s_RaycastHitsCache[j].collider;
        //            if (col != null)
        //            {
        //                TargetableObject entity = col.gameObject.GetComponent<TargetableObject>();
        //                if (entity == null)
        //                {
        //                    //对于身体上的胶囊碰撞查找父级
        //                    entity = col.gameObject.GetComponentInParent<TargetableObject>();
        //                    if(entity == null)
        //                    {
        //                        continue;
        //                    }
        //                }
        //                var toEntry = GameEntry.Entity.GetParentEntity(m_Owner.Id);
        //                var angle = AIUtility.GetAngleInSeek(entity, toEntry);

        //                //Debug.Log(entity.Name +"      " + angle);
        //                if (entity.IsDefense && angle <= 40)
        //                { 
        //                    Debug.Log("格挡");
        //                    StartCoroutine(entity.transform.GetComponent<PlayerLogic>().DefenseWhileAtked("DefenseWhileAtked", 0.3f));
        //                    return;
        //                }

        //                //if (entity is TargetableObject && entity.Id >= owner.Id)
        //                //{
        //                //    // 碰撞事件由 Id 小的一方处理，避免重复处理
        //                //    return;
        //                //}
        //                AIUtility.PerformCollisionAttack(m_Owner, entity);
        //            }
        //        }

        //        m_PreviousPos[i] = worldPos;
        //        pts.previousPositions.Add(m_PreviousPos[i]);
        //    }
        //}

        public void SetAttackPoint()
        {
            if (m_Check)
            {
                return;
            }
            Collider[] colliders = Physics.OverlapBox(attackPointBox.attackRoot.position+ attackPointBox.attackRoot.TransformVector(attackPointBox.offset), attackPointBox.halfExtents, attackPointBox.attackRoot.rotation, 1 << LayerMask.NameToLayer("Targetable Object"));
            var entity = GameEntry.Entity.GetParentEntity(m_Owner.Id);
            TargetableObject owner = (TargetableObject)entity.Logic;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    TargetableObject other = colliders[i].gameObject.GetComponent<TargetableObject>();
                    if (other == null)
                    {
                        other = colliders[i].gameObject.GetComponentInParent<TargetableObject>();
                        if (other == null)
                        {
                            continue;
                        }

                        //  var toEntity = GameEntry.Entity.GetParentEntity(m_Owner.Id);
                        //  var angle = AIUtility.GetAngleInSeek(other, toEntity);

                        //if (other.IsDefense)
                        //{
                            // Debug.Log("格挡");
                            //StartCoroutine(other.transform.GetComponent<PlayerLogic>().DefenseWhileAtked("DefenseWhileAtked", 0.3f));
                            
                        //}
                    }
                    if(other == owner)
                    {
                        continue;
                    }
                    Vector3 location = transform.position;
                    Vector3 hitlocation = colliders[i].ClosestPointOnBounds(location);                   
                    if (!hitList.Contains(other))
                    {
                        hitList.Add(other);
                        hitPoint.Add(hitlocation);
                    }
                }
            }

            if (hitList.Count > 0)
            {
                m_Check = true;
            }
            for (int i = 0; i < hitList.Count; i++)
            {
                AIUtility.PerformCollisionAttack(owner, hitList[i], hitPoint[i]);
            }
            
            hitList.Clear();
            hitPoint.Clear();
        }



        public void SetAttackPointSkill(TargetableObject owner,Skill skilldata)
        {
            Collider[] colliders = Physics.OverlapBox(attackPointBox.attackRoot.position+ attackPointBox.attackRoot.TransformVector(attackPointBox.offset), attackPointBox.halfExtents, attackPointBox.attackRoot.rotation, 1 << LayerMask.NameToLayer("Targetable Object"));
           // Collider[] colliders = Physics.OverlapSphere(attackPoints[0].attackRoot.position, attackPoints[0].radius, 1 << LayerMask.NameToLayer("Targetable Object"));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    TargetableObject entity = colliders[i].gameObject.GetComponent<TargetableObject>();
                    if (entity == null)
                    {
                        entity = colliders[i].gameObject.GetComponentInParent<TargetableObject>();
                        if (entity == null)
                        {
                            continue;
                        }
                    }
                    GameHotfixEntry.Skill.SkillCollision(owner, entity, skilldata);
                   // AIUtility.PerformCollisionAttack(owner, entity, attackPoints[0]);

                }
            }
        }


#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (attackPointBox.attackRoot != null)
            {
                Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
                Gizmos.matrix = Matrix4x4.TRS(attackPointBox.attackRoot.position + attackPointBox.attackRoot.TransformVector(attackPointBox.offset), attackPointBox.attackRoot.rotation, attackPointBox.attackRoot.localScale);
                Gizmos.DrawWireCube(Vector3.zero, attackPointBox.halfExtents * 2);
            }
            
        }


#endif
    }
}
