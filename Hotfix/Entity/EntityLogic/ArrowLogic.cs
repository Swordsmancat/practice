using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using UnityEngine;
using GameFramework;

namespace Farm.Hotfix
{
    public class ArrowLogic : Entity
    {
        private ArrowData m_ArrowData;
        private Rigidbody m_Rigidbody;
        private  BoxCollider m_BoxCollider;
        private TrailRenderer m_Trail;
        private bool m_DisableRotation;

        private float m_DestroyTime = 10f;
        private float m_PastTime;

        public ImpactData GetImpactData()
        {
            return new ImpactData(m_ArrowData.OwnerCamp, 0, m_ArrowData.Attack, 0);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_ArrowData = userData as ArrowData;
            if (m_ArrowData == null)
            {
                Log.Error("Arrow data is invalid.");
                return;
            }
            m_Rigidbody = GetComponent<Rigidbody>();
            m_BoxCollider = GetComponent<BoxCollider>();
            m_Trail = GetComponent<TrailRenderer>();
            m_Trail.Clear();
            m_Rigidbody.WakeUp();
            // m_Rigidbody.AddForce(m_ArrowData.ArrowImpulse*3,ForceMode.Force);
            if (m_ArrowData.IsLock)
            {
                m_Rigidbody.AddForce(m_ArrowData.ArrowImpulse,ForceMode.Impulse);
            }
            else
            {
                 m_Rigidbody.AddForce(m_ArrowData.Parent.forward*m_ArrowData.ArrowImpulse.z+ m_ArrowData.Parent.up*m_ArrowData.ArrowImpulse.y +m_ArrowData.Parent.right*m_ArrowData.ArrowImpulse.x,ForceMode.Impulse);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            //if (!m_DisableRotation)
            //{
            //    transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity);
            //}
            if (m_PastTime < m_DestroyTime)
            {
                m_PastTime += elapseSeconds;
            }
            else
            {
                m_PastTime = 0;
                m_Rigidbody.Sleep();
                GameEntry.Entity.HideEntity(this);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.tag != "Targetable Object")
            //{
            //    m_DisableRotation = true;
            //    m_Rigidbody.isKinematic = true;
            //    m_BoxCollider.isTrigger = true;
            //}
            TargetableObject entity = other.gameObject.GetComponent<TargetableObject>();
            if (entity == null)
            {
                return;
            }
            if(m_ArrowData.OwnerId == entity.Id)
            {
                return;
            }
            Vector3 point = other.bounds.ClosestPoint(transform.position);

            AIUtility.PerformCollisionBow(entity, this, point);
        }



    }
}

