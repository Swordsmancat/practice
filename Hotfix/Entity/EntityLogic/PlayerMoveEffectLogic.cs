using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class PlayerMoveEffectLogic : EffectLogic
    {
        private readonly string ChildName = "Player_Run_Smoke";

        private GameObject m_PlayerObject = null;
        private ParticleSystem m_EffectParticleSystem = null;
        private ParticleSystem m_ChildEffect = null;

        private int m_Index = 0;
        private bool m_Cando = false;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Cando)
            {
                var parentEmission = m_EffectParticleSystem.emission;
                var childEmission = m_ChildEffect.emission;

                transform.SetPositionAndRotation(m_PlayerObject.transform.position + Vector3.up,
                    m_PlayerObject.transform.rotation);
                m_ChildEffect.gameObject.transform.SetPositionAndRotation(m_PlayerObject.transform.position,
                    m_PlayerObject.transform.rotation);

                if (m_PlayerObject.GetComponent<PlayerLogic>().isRun)
                {
                    parentEmission.enabled = true;
                    childEmission.enabled = true;
                }
                else
                {
                    parentEmission.enabled = false;
                    childEmission.enabled = false;
                }
            }


        }
    }
}

