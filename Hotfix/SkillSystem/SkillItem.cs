//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/7/5/周二 10:07:36
//------------------------------------------------------------
using GameFramework.Event;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class SkillItem:MonoBehaviour
    {
        private TargetableObject m_Owner = null;
        private int m_OwnerId = 0;
        private int m_SkillID = 0;
        private readonly string SkillLayer = "Skill";
        private Animator m_Animator;
        private int m_skillTimerID;

        public int SkillID
        {
            get
            {
                return m_SkillID;
            }
        }
        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        private Skill m_SkillData;
        private float pastTime = 0;


        public void Init(TargetableObject owner,int skillID,Skill skillData)
        {
            if(owner == null)
            {
                Log.Error("Owner is invalid");
                return;
            }

            if(skillData == null)
            {
                Log.Error("SkillData is invalid");
                return;
            }

            if(m_Owner !=owner||m_OwnerId != owner.Id)
            {
                m_Owner = owner;
                m_OwnerId = owner.Id;
                m_SkillID = skillID;
                m_SkillData = skillData;
            }
            AddEntitySkillCD();
            PlayAnimation();
            PlayAudioClicp();
            PlayEffect();
            CalculateConsumption();
            SetCanBreak();
            ApplyBuff();
            m_skillTimerID = GameHotfixEntry.Skill.AddTimer(m_Owner, m_SkillData);
            GameEntry.Event.Subscribe(ApplyDamageEventArgs.EventId, OnApplyDamageEvent);
            Refresh();
        }

        private void ApplyBuff()
        {
            m_Owner.TargetableObjectData.HP += m_SkillData.Buffs.Treatment;
            m_Owner.TargetableObjectData.MP += m_SkillData.Buffs.Energy;
            if (m_SkillData.Buffs.SuperArmor)
            {
                m_Owner.IsCanBreak = false;
            }
        }

        private void OnApplyDamageEvent(object sender, GameEventArgs e)
        {
            ApplyDamageEventArgs ne = (ApplyDamageEventArgs)e;
            if(ne.UserData != m_Owner)
            {
                return;
            }
            if (m_SkillData.IsCanBreak)
            {
                GameHotfixEntry.Skill.RemoveTimerSkill(m_skillTimerID);
                GameHotfixEntry.Skill.HideSkill(this);
            }
        }

        private void SetCanBreak()
        {
            m_Owner.IsCanBreak = m_SkillData.IsCanBreak;
        }

        /// <summary>
        /// 添加技能cd
        /// </summary>
        private void AddEntitySkillCD()
        {
            if (m_Owner.SkillCD.ContainsKey(m_SkillID))
            {
                m_Owner.SkillCD[m_SkillID] = m_SkillData.CD;
            }
            else
            {
                m_Owner.SkillCD.Add(m_SkillID, m_SkillData.CD);
            }
        }

        /// <summary>
        /// 计算消耗
        /// </summary>
        private void CalculateConsumption()
        {
            m_Owner.TargetableObjectData.HP -= m_SkillData.NeedHPValue;
            m_Owner.TargetableObjectData.MP -= m_SkillData.NeedMPValue;
            //if(m_Owner.TargetableObjectData.Camp == CampType.Player)
            //{
            //    PlayerLogic player = (PlayerLogic)m_Owner;

            //}

        }

        /// <summary>
        /// 播放动画
        /// </summary>
        private void PlayAnimation()
        {
            if (string.IsNullOrEmpty(m_SkillData.AnimationClipName))
            {
                Log.Warning("技能:'{0}' 动画为空", m_SkillData.SkillName);
                return;
            }
            m_Animator = m_Owner.gameObject.GetComponent<Animator>();
            m_Animator.Play(m_SkillData.AnimationClipName, m_Animator.GetLayerIndex(SkillLayer));
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        private void PlayAudioClicp()
        {
            if (string.IsNullOrEmpty(m_SkillData.SoundName))
            {
                Log.Warning("技能:'{0}' 音效为空", m_SkillData.SkillName);
                return;
            }
            GameEntry.Sound.PlaySound(AssetUtility.GetSkillSoundAsset(m_SkillData.SoundName),"Skill");
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        private void PlayEffect()
        {
            if (string.IsNullOrEmpty(m_SkillData.EffectName))
            {
                Log.Warning("技能:'{0}' 特效为空", m_SkillData.SkillName);
                return;
            }
            int entityId = GameEntry.Entity.GenerateSerialId();
            GameEntry.Entity.ShowEntity(entityId, typeof(EffectLogic), AssetUtility.GetSkillEffectAsset(m_SkillData.EffectName), "Effect", Constant.AssetPriority.EffectAsset, new EffectData(entityId) { KeepTime = m_SkillData.Duration });
        }

        private void Update()
        {
         

            pastTime += Time.deltaTime;
            if (pastTime > m_SkillData.Duration)
            {
                GameHotfixEntry.Skill.HideSkill(this);
            }
        }

        public void Refresh()
        {
            gameObject.SetActive(true);
            pastTime = 0;  
        }


        public void Reset()
        {
            m_Owner.IsCanBreak = true;
            GameEntry.Event.Unsubscribe(ApplyDamageEventArgs.EventId, OnApplyDamageEvent);
            m_Owner = null;
            gameObject.SetActive(false);
        }
    }
}
