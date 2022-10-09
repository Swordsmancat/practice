using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm.Hotfix
{
    public class GameHotfixEntry
    {
        public static HPBarComponent HPBar
        {
            get;
            private set;
        }

        public static SkillComponent Skill
        {
            get;
            private set;
        }

        public static void Start()
        {
            GameEntry.Fsm.DestroyFsm<IProcedureManager>();
            var procedureManager = GameFrameworkEntry.GetModule<IProcedureManager>();
            GameFramework.Procedure.ProcedureBase[] procedures =
            {
                new ProcedureChangeScene(),
                new ProcedurePreload(),
                new ProcedureLogin(),
                new ProcedureMain(),
            };

            Skill = UnityGameFramework.Runtime.GameEntry.GetComponent<SkillComponent>();
            procedureManager.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(), procedures);
            procedureManager.StartProcedure<ProcedurePreload>();
            GameEntry.Resource.LoadAsset("Assets/GameMain/HotfixCustoms.prefab", new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFail));
            
        }

        private static void OnLoadAssetSuccess(string assetName, object asset, float duration, object userdata)
        {
            GameObject game = Object.Instantiate((GameObject)asset);
            game.name = "HotfixCustoms";

            HPBar = game.GetComponentInChildren<HPBarComponent>();
        }

        private static void OnLoadAssetFail(string assetName, LoadResourceStatus status, string errormessage, object userdata)
        {
            Log.Error("Load HotfixCustoms failed. {0}", errormessage);
        }
    }
}

