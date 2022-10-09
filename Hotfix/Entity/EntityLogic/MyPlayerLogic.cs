
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
  public  class MyPlayerLogic :PlayerLogic
    {
        [SerializeField]
        private MyPlayerData m_MyPlayerData = null;

        private AbilityDataPlayer m_abilityDataPlayer;

        private Vector3 m_TargetPosition = Vector3.zero;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_MyPlayerData = userData as MyPlayerData;
            if(m_MyPlayerData == null)
            {
                Log.Warning("My Player data is invalid.");
                return;
            }
           // m_abilityDataPlayer = gameObject.AddComponent<AbilityDataPlayer>();
            //m_abilityDataPlayer.OnInit();
            GameEntry.Scene.RefreshMainCamera();

           // AddSkill(90001);
           
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetKeyDown(KeyCode.K))
            {
                m_abilityDataPlayer.Save();
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                // GameEntry.Entity.ShowSkill(new SkillData(GameEntry.Entity.GenerateSerialId(), 90001, m_MyPlayerData.Id, m_MyPlayerData.Camp));
                GameHotfixEntry.Skill.ShowSkill(this, 90001);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                GameHotfixEntry.Skill.ShowSkill(this, 90002);
            }
        }


    }
}
