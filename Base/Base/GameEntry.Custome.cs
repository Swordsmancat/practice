using UGFExtensions.Timer;
using UnityEngine;


namespace Farm
{
   public partial class GameEntry:MonoBehaviour
    {

        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }


        public static HuatuoComponent Huatuo
        {
            get;
            private set;
        }

        public static TimerComponent Timer
        {
            get;
            private set;
        }


        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            Huatuo = UnityGameFramework.Runtime.GameEntry.GetComponent<HuatuoComponent>();
            Timer = UnityGameFramework.Runtime.GameEntry.GetComponent<TimerComponent>();

        }

        private static void InitCustomDebuggers()
        {
            
        }
    }
}
