using GameFramework.DataTable;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Farm.Hotfix
{
    [Serializable]
    public class EnemyData : TargetableObjectData
    {
        [Title("基础属性")]
        [SerializeField ,HideLabel,GUIColor(1,0,0),LabelText("最大血量")]
        private int m_MaxHP = 0;
        [SerializeField, HideLabel, GUIColor(1, 0, 0), LabelText("最大精力值")]
        private int m_MaxMP = 0;

        //private int m_MaxMP = 0;

        [SerializeField,HideLabel,LabelText("攻击力")]
        private int m_Attack = 0;

        [SerializeField, HideLabel, LabelText("防御力")]
        private int m_Defense;

        [SerializeField, HideLabel, LabelText("攻击范围")]
        private float m_AttackRange;

        [SerializeField, HideLabel, LabelText("视野范围")]
        private float m_SeekRange;

        [SerializeField, HideLabel, LabelText("视野角度")]
        private float m_SeekAngle;

        [SerializeField, HideLabel, LabelText("攻速")]
        private float m_AttackSpeed;

        [SerializeField, HideLabel]
        [LabelText("武器列表")]
        [Searchable]
        [TableList]
        private List<WeaponData> m_WeaponDatas = new List<WeaponData>();

        [SerializeField, HideLabel, LabelText("走路移动速度")]
        private float m_MoveSpeedWalk = 0f;

        [SerializeField, HideLabel, LabelText("跑步移动速度")]
        private float m_MoveSpeedRun = 0f;

        [SerializeField,HideLabel,LabelText("死亡特效ID")]
        private int m_DeadEffectId = 0;

        [SerializeField,HideLabel,LabelText("死亡声音ID")]
        private int m_DeadSoundId = 0;

        [SerializeField, HideLabel, LabelText("血液特效ID")]
        private int m_BloodEffectId = 0;

        [SerializeField, HideLabel, LabelText("血液声音ID")]
        private int m_ByAttackSoundId = 0;

        [SerializeField, HideLabel, LabelText("霸体受击特效ID")]
        private int m_StoicHurtEffectId = 0;

        [SerializeField, HideLabel, LabelText("霸体受击声音ID")]
        private int m_StoicHurtSoundId = 0;

        private int m_WalkSoundId = 0;
        private int m_ShoutSoundId = 0;
        private int m_MotionSoundId0 = 0;
        private int m_MotionSoundId1 = 0;
        private int m_MotionSoundId2 = 0;
        private int m_MotionSoundId3 = 0;
        private int m_MotionSoundId4 = 0;
        private int m_MotionSoundId5 = 0;
        private int m_MotionSoundId6 = 0;
        private int m_MotionSoundId7 = 0;

        public EnemyData(int entityId ,int typeId) : base(entityId, typeId, CampType.Enemy)
        {
            IDataTable<DREnemy> dtEnemies = GameEntry.DataTable.GetDataTable<DREnemy>();
            DREnemy drEnemy = dtEnemies.GetDataRow(TypeId);
            if(drEnemy == null)
            {
                return;
            }
            int weaponCount = drEnemy.WeaponIdCount;
            for (int index = 0, weaponId = 0; index < weaponCount; index++)
            {
                weaponId = drEnemy.GetWeaponIdAt(index);
                AttachWeaponData(new WeaponData(GameEntry.Entity.GenerateSerialId(), weaponId, Id, Camp));
            }
            HP = m_MaxHP = drEnemy.MaxHP;
            MP = m_MaxMP = drEnemy.MaxMP;
            //  m_Attack = drEnemy.Attack;
            //  m_Speed = drEnemy.Speed;
            m_AttackRange = drEnemy.AttackRange;
            m_AttackSpeed = drEnemy.AttackSpeed;
            m_SeekRange = drEnemy.SeekRange;
            m_SeekAngle = drEnemy.SeekAngle;
            m_MoveSpeedWalk = drEnemy.MoveSpeedWalk;
            m_MoveSpeedRun = drEnemy.MoveSpeedRun;
            m_DeadEffectId = drEnemy.DeadEffectId;
            m_DeadSoundId = drEnemy.DeadSoundId;
            m_BloodEffectId = drEnemy.BloodEffctId;
            m_ByAttackSoundId = drEnemy.ByAttackSoundId;
            m_MotionSoundId0 = drEnemy.MotionSoundId0;
            m_MotionSoundId1 = drEnemy.MotionSoundId1;
            m_MotionSoundId2 = drEnemy.MotionSoundId2;
            m_MotionSoundId3 = drEnemy.MotionSoundId3;
            m_WalkSoundId = drEnemy.WalkSoundId;
            m_ShoutSoundId = drEnemy.ShoutSoundId;
            m_StoicHurtSoundId = drEnemy.StoicHurtSoundId;
            m_StoicHurtEffectId = drEnemy.StoicHurtEffectId;
        }

        public override int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        public override int MaxMP
        {
            get
            {
                return m_MaxMP;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        public float AttackSpeed
        {
            get
            {
                return m_AttackSpeed;
            }
        }

        public float MoveSpeedWalk
        {
            get
            {
                return m_MoveSpeedWalk;
            }
        }

        public float MoveSpeedRun
        {
            get
            {
                return m_MoveSpeedRun;
            }
        }

        public int Defense
        {
            get
            {
                return m_Defense;
            }
        }

        public float AttackRange
        {
            get
            {
                return m_AttackRange;
            }
        }

        public float AttackAngle
        {
            get
            {
                return m_SeekAngle;
            }
        }
        public float SeekRange
        {
            get
            {
                return m_SeekRange;
            }
        }

        public int DeadEffectId
        {
            get
            {
                return m_DeadEffectId;
            }
        }

        public int DeadSoundId
        {
            get
            {
                return m_DeadSoundId;
            }
        }

        public int BloodEffectId
        {
            get
            {
                return m_BloodEffectId;
            }
        }

        public int ByAttackSoundId
        {
            get
            {
                return m_ByAttackSoundId;
            }
        }

        public int StoicHurtEffectId
        {
            get
            {
                return m_StoicHurtEffectId;
            }
        }

        public int StoicHurtSoundId
        {
            get
            {
                return m_StoicHurtSoundId;
            }
        }



        public int MotionSoundId0
        {
            get
            {
                return m_MotionSoundId0;
            }
        }

        public int MotionSoundId1
        {
            get
            {
                return m_MotionSoundId1;
            }
        }

        public int MotionSoundId2
        {
            get
            {
                return m_MotionSoundId2;
            }
        }

        public int MotionSoundId3
        {
            get
            {
                return m_MotionSoundId3;
            }
        }
        public int MotionSoundId4
        {
            get
            {
                return m_MotionSoundId4;
            }
        }
        public int MotionSoundId5
        {
            get
            {
                return m_MotionSoundId5;
            }
        }
        public int MotionSoundId6
        {
            get
            {
                return m_MotionSoundId6;
            }
        }
        public int MotionSoundId7
        {
            get
            {
                return m_MotionSoundId7;
            }
        }

        public int WalkSoundId
        {
            get
            {
                return m_WalkSoundId;
            }
        }

        public int ShoutSoundId
        {
            get
            {
                return m_ShoutSoundId;
            }
        }

        public List<WeaponData> GetAllWeaponDatas()
        {
            return m_WeaponDatas;
        }

        public void AttachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            if (m_WeaponDatas.Contains(weaponData))
            {
                return;
            }

            m_WeaponDatas.Add(weaponData);
        }
    }
}
