using System.Reflection;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using System.Collections.Generic;
using GameFramework;
using System.IO;


namespace Farm
{
    public class ProcedureCodeInit : ProcedureBase
    {

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_HuatuoMode = GameEntry.Huatuo.HuatuoMode;
            m_LoadAssemblyComplete = false;
            LoadAssembly();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_LoadAssemblyComplete)
            {
                AllAsmLoadComplete();
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
        private bool m_HuatuoMode;

        private LoadAssetCallbacks m_Callbacks;

        private Assembly m_MainLogicAsm;


        private int loadingCount;
        private int failureCount;
        private bool m_LoadAssemblyComplete;

        private enum EAsmLoadState
        {
            Invlaid,
            Ready,
            Loading,
            WaitingResult,
            Finish,
        }

        private EAsmLoadState m_State = EAsmLoadState.Invlaid;

        private void LogAssemblyInfo()
        {
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            int unityAsmCount = 0;
            int systemAsmCount = 0;
            int otherAsmCount = 0;
            foreach (var asm in asms)
            {
                if (asm.ToString().StartsWith("Unity"))
                {
                    unityAsmCount++;
                }
                else if (asm.ToString().StartsWith("System"))
                {
                    systemAsmCount++;
                }
                else
                {
                    var types = asm.GetTypes();
                    otherAsmCount++;
                    if (HotfixDefine.AllHotUpdateDllNames.Contains($"{asm.GetName().Name}.dll"))
                    {
                        Log.Debug($"asm: [{asm.GetName().Name} ]");
                        Log.Debug($"typesCount: [{types.Length}]");
                        foreach (var type in types)
                        {
                            Log.Debug($"  type :[{ type }]");
                        }
                        if (types.Length == 0)
                        {
                            Log.Warning("no type in assembly");
                        }
                    }
                }
            }

            Log.Debug($"Unity asm count: [ {unityAsmCount} ]");
            Log.Debug($"System asm count: [ {systemAsmCount} ]");
            Log.Debug($"Other asm count: [ {otherAsmCount} ]");
        }

        private void LoadAssembly()
        {
            loadingCount = 0;
            failureCount = 0;
            if (!m_HuatuoMode)
            {
                Log.Info("Skip load Assenbly");
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (string.Compare(HotfixDefine.LogicEntranceDllName, $"{asm.GetName().Name}.dll", StringComparison.Ordinal) == 0)
                    {
                        m_MainLogicAsm = asm;
                        break;
                    }
                }
                AllAsmLoadComplete();
                return;
            }
            m_State = EAsmLoadState.Loading;
            m_Callbacks ??= new LoadAssetCallbacks(LoadAssetSuccess, LoadAssetFailure);
            foreach (var hotUpdateDllName in HotfixDefine.AllHotUpdateDllNames)
            {
                var assetPath = Utility.Path.GetRegularPath(Path.Combine(HotfixDefine.AssemblyTextAssetResPath, $"{hotUpdateDllName}.{HotfixDefine.AssemblyTextAssetExtension}"));
                Log.Debug($"LoadAsset[{assetPath}]");
                loadingCount++;
                GameEntry.Resource.LoadAsset(assetPath, m_Callbacks, hotUpdateDllName);
            }
            m_State = EAsmLoadState.WaitingResult;
            if (loadingCount == 0)
            {
               // AllAsmLoadComplete();
            }
        }

        private void LoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            loadingCount--;
            Log.Debug($"LoadAssetSuccess, assetName: [ {assetName} ], duration: [ {duration} ], userData: [ {userData} ]");
            var textAsset = asset as TextAsset;
            if (textAsset == null)
            {
                Log.Debug($"Load text asset [ {assetName} ] failed.");
                return;
            }

            try
            {
                var asm = System.Reflection.Assembly.Load(textAsset.bytes);
                if (string.Compare(HotfixDefine.LogicEntranceDllName, userData as string, StringComparison.Ordinal) ==
                    0)
                {
                    m_MainLogicAsm = asm;
                }
                Log.Debug($"Assembly [ {asm.GetName().Name} ] loaded");
                LogAssemblyInfo();
            }
            catch (Exception e)
            {
                failureCount++;
                Log.Fatal(e);
                throw;
            }
            finally
            {
                Log.Debug($"_state: [ {m_State} ], _loadingCnt: [ {loadingCount} ]");
                if (EAsmLoadState.WaitingResult == m_State && 0 == loadingCount)
                {
                    m_LoadAssemblyComplete = true;
                   // AllAsmLoadComplete();
                }
            }
        }

        private void LoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Log.Warning($"LoadAssetFailure, assetName: [ {assetName} ], status: [ {status} ], errorMessage: [ {errorMessage} ], userData: [ {userData} ]");
            loadingCount--;
            failureCount++;
        }

        private void AllAsmLoadComplete()
        {
            m_State = EAsmLoadState.Finish;
            Log.Debug($"All assemblies load complete. failure cnt: [ {failureCount} ], loading duration: [ {GameEntry.Procedure.CurrentProcedureTime} ]");
            LogAssemblyInfo();
            RunMain();

        }

        private void RunMain()
        {
            if (m_MainLogicAsm == null)
            {
                Log.Fatal("Main business logic assembly missing.");
                return;
            }
            var asmType = m_MainLogicAsm.GetType("Farm.Hotfix.GameHotfixEntry");//进入类
            if (asmType == null)
            {
                Log.Fatal("GameHotfixEntry type missing.");
                return;
            }
            var entryMethod = asmType.GetMethod("Start");
            if (entryMethod == null)
            {
                Log.Fatal("GameHotfixEntry method 'Start' missing.");
                return;
            }
            //  object[] objects = new object[] { new object[] { this, m_procedureOwner } };
            entryMethod.Invoke(null, null);
        }
    }
}
