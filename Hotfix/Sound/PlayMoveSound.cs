//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/8/22/周一 10:37:56
//------------------------------------------------------------
using UnityEngine;

namespace Farm.Hotfix
{
    public class PlayMoveSound : MonoBehaviour
    {
        [SerializeField]
        private float m_GroundCheckHeight = 0.6f;

        [SerializeField]
        private float m_GroundCheckRadius = 0.5f;

        [SerializeField]
        private float m_GroundCheckDistance = 0.3f;

        [SerializeField]
        private float m_DistanceBetweenSteps = 2f;

        [SerializeField] 
        float minVolume = 0.3f;
        [SerializeField] 
        float maxVolume = 0.5f;

        [SerializeField]
        private LayerMask m_GroundLayers;

        private CharacterController m_Rigidbody;

        private RaycastHit m_CurrentGroundInfo;

        private bool m_IsGround;

        private float  m_StepCycleProgress;

        private int SoundID;

        private PlayerLogic m_PlayerLogic;

        private EnemyLogic m_EnemyLogic;


        private void Start()
        {
            m_Rigidbody = transform.GetComponent<CharacterController>();
            if(m_GroundLayers == 0)
            {
                m_GroundLayers = 1<<6;
            }
            m_PlayerLogic = transform.GetComponent<PlayerLogic>();
            m_EnemyLogic = transform.GetComponent<EnemyLogic>();
            if (m_PlayerLogic != null)
            {
                SoundID = m_PlayerLogic.PlayerData.WalkSoundId;
                return;
            }
            if(m_EnemyLogic != null)
            {
                SoundID = m_EnemyLogic.enemyData.WalkSoundId;
                return;
            }
        }

        private void Update()
        {
            CheckGround();
            float speed = m_Rigidbody.velocity.magnitude;
            if (m_IsGround)
            {
                AdvanceStepCycle(speed * Time.deltaTime);
            }
        }

        private void AdvanceStepCycle(float increment)
        {
            m_StepCycleProgress += increment;
            if(m_StepCycleProgress > m_DistanceBetweenSteps)
            {
                m_StepCycleProgress = 0f;
                PlayFootstep();
            }
        }

        private void PlayFootstep()
        {
            float randomVolume = Random.Range(minVolume, maxVolume);
            GameEntry.Sound.PlayMoveSound(SoundID, randomVolume);
        }

        void CheckGround()
        {
            Ray ray = new Ray(transform.position + Vector3.up * m_GroundCheckHeight, Vector3.down);

            if(Physics.SphereCast(ray,m_GroundCheckRadius,out m_CurrentGroundInfo, m_GroundCheckDistance, m_GroundLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGround = true;
            }
            else
            {
                m_IsGround = false;
            }

            if (m_IsGround)
            {

            }
        }

        void OnDrawGizmos()
        {
                Gizmos.DrawWireSphere(transform.position + Vector3.up * m_GroundCheckHeight, m_GroundCheckRadius);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position + Vector3.up * m_GroundCheckHeight, Vector3.down * (m_GroundCheckDistance + m_GroundCheckRadius));
        }


    }
}
