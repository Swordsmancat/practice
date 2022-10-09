
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using UMA;

namespace Farm.Hotfix
{
    /// <summary>
    /// NPC。
    /// </summary>
    public class NPCLogic : Entity
    {
        private NPCData m_NPCData;
        private readonly string PlayerTag = "Player";
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_NPCData = userData as NPCData;
            if (m_NPCData == null)
            {
                Log.Error("Player data is invalid.");
                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
            {
                return;
            }
            if (!GameEntry.UI.HasUIForm(UIFormId.DialogOptionsForm))
            {
                GameEntry.UI.OpenUIForm(UIFormId.DialogOptionsForm, m_NPCData);
            }
            else
            {
                GameEntry.UI.GetUIForm(UIFormId.DialogOptionsForm);
            }
           
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
            {
                return;
            }
            GameEntry.UI.GetUIForm(UIFormId.DialogOptionsForm).Close();
        }



    }
}
