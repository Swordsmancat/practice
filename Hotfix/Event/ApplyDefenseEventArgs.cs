//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/9/1/周四 15:02:23
//------------------------------------------------------------
using GameFramework;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class ApplyDefenseEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ApplyDefenseEventArgs).GetHashCode();
        public override int Id
        {
            
            get
            {
                return EventId;
            }
        }

        public object UserData
        {
            get;
            private set;
        }

        public static ApplyDefenseEventArgs Create(object userData = null)
        {
            ApplyDefenseEventArgs applyDefenseEventArgs = ReferencePool.Acquire<ApplyDefenseEventArgs>();
            applyDefenseEventArgs.UserData = userData;
            return applyDefenseEventArgs;
        }

        public override void Clear()
        {
            UserData = default(object);
        }
    }
}
