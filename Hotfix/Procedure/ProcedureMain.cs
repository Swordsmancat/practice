//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityEngine;
using GameFramework.Event;
using System;
using GameFramework.Procedure;

namespace Farm.Hotfix
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;

        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;


        private int? m_LockFormUIID;
        private int? m_GunAimUIID;

        private LockForm m_LockForm;

        public GunAimForm m_GunAimForm;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

          //  m_Games.Add(GameMode.Survival, new SurvivalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            m_Games.Clear();
        }

        private void LoadSkillData()
        {
            GameHotfixEntry.Skill.GetAllSkillData();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            LoadSkillData();
            GameEntry.Entity.ShowMyPlayer(new MyPlayerData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "my Player",
                Position = new UnityEngine.Vector3(33, 28, 20),
                Scale = new UnityEngine.Vector3(1f, 1f, 1f)
            });
            IDataTable<DREnemy> dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60005)
            {
                Position = new UnityEngine.Vector3(55, 8, 45),
                Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            }, typeof(OrcLogic));
            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60061)
            //{
            //    Position = new UnityEngine.Vector3(55, 8, 55),
            //    Scale = new UnityEngine.Vector3(0.8f, 0.8f, 0.8f)
            //}, typeof(DragonideLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60063)
            //{
            //    Position = new UnityEngine.Vector3(55, 8, 45),
            //    Scale = new UnityEngine.Vector3(1.3f, 1.3f, 1.3f)
            //}, typeof(WeregoatLogic));
            //int i = 0;
            //while (i < 5)
            //{
            //    int n = i + 3;
            //    GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60065)
            //    {
            //        Position = new UnityEngine.Vector3(350 + n, 28, 250 + n),
            //        Scale = new UnityEngine.Vector3(1f, 1f, 1f)
            //    }, typeof(GoatLogic));
            //    i++;
            //}
            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60015)
            //{
            //    Position = new UnityEngine.Vector3(55, 8, 50),
            //    Scale = new UnityEngine.Vector3(1f, 1f, 1f)
            //}, typeof(GoatLogic));
            //GameEntry.Entity.ShowNPC(new NPCData(GameEntry.Entity.GenerateSerialId(), 80001)
            //{
            //    Position = new Vector3(50f, 10f, 50f),
            //    Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            //});
            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60011)
            //{
            //    Position = new UnityEngine.Vector3(20, 10, 20),

            //    Rotation = new UnityEngine.Quaternion(0, -90, 0, 0),
            //    Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            //}, typeof(UndeadLogic));
            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60009)
            //{
            //    Position = new UnityEngine.Vector3(9, 10, 9),
            //    //Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            //}, typeof(HarpyLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60006)
            //{
            //    Position = new UnityEngine.Vector3(9, 10, 8),
            //    //Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            //}, typeof(KoboldLogic));

            //GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60007)
            //{
            //    Position = new UnityEngine.Vector3(9, 10, 9),
            //    //Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            //}, typeof(KoboldLogic));

            GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60008)
            {
                Position = new UnityEngine.Vector3(20, 10, 20),
                //Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            }, typeof(GoblinLogic));

            //for (int i = 0; i < 5; i++)
            //{
            //    GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(), 60009)
            //    {
            //        Position = new UnityEngine.Vector3(80 + i, 20, 80 + i),
            //        //Scale = new UnityEngine.Vector3(1.5f, 1.5f, 1.5f)
            //    }, typeof(HarpyLogic));
            //}
            //GameEntry.Sound.PlaySound(50000);



            m_LockFormUIID = GameEntry.UI.OpenUIForm(UIFormId.UILock, this);
            m_GunAimUIID = GameEntry.UI.OpenUIForm(UIFormId.UIGunAim, this);
            //GameEntry.UI.OpenUIForm(UIFormId.ArenaForm, this);
            //   GameEntry.Entity.ShowEntity<PlayerLogic>()
            //   m_GotoMenu = false;
            //  GameMode gameMode = (GameMode)procedureOwner.GetData<VarByte>("GameMode").Value;
            // m_CurrentGame = m_Games[gameMode];
            //  m_CurrentGame.Initialize();
        }

        private void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {

        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if(ne.UserData != this)
            {
                return;
            }
            
            if (ne.UIForm.SerialId == m_GunAimUIID)
            {
                m_GunAimForm = (GunAimForm)ne.UIForm.Logic;
            }
            if (ne.UIForm.SerialId == m_LockFormUIID)
            {
                m_LockForm = (LockForm)ne.UIForm.Logic;
            }
        }
       public void HideLockIcon()
        {
            if (m_LockForm != null)
            {
                m_LockForm.HideLock();
            }
        }
        public void ShowLockIcon(Transform transform)
        {
            if (m_LockForm != null)
            {
                m_LockForm.ShowLock(transform);
            }
        }
        
        public void HideGunAimIcon()
        {
            if (m_GunAimForm != null)
            {
                m_GunAimForm.HideGunAim();
            }
        }

        public void ShowGunAimIcon()
        {
            if (m_GunAimForm != null)
            {
                m_GunAimForm.ShowGunAim();
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            //{
            //    m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
            //    return;
            //}

            //if (!m_GotoMenu)
            //{
            //    m_GotoMenu = true;
            //    m_GotoMenuDelaySeconds = 0;
            //}

            //m_GotoMenuDelaySeconds += elapseSeconds;
            //if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
            //{
            //    procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
            //    ChangeState<ProcedureChangeScene>(procedureOwner);
            //}
        }
    }
}
