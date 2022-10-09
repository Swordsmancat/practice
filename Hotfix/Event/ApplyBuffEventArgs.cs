//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/7/15/周五 10:53:27
//------------------------------------------------------------
using GameFramework;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class ApplyBuffEventArgs :GameEventArgs
    {
        public static readonly int EventId = typeof(ApplyBuffEventArgs).GetHashCode();

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

        public static ApplyBuffEventArgs Create(object userData = null)
        {
            ApplyBuffEventArgs applyBuffEventArgs = ReferencePool.Acquire<ApplyBuffEventArgs>();
            applyBuffEventArgs.UserData = userData;
            return applyBuffEventArgs;
        }

        public override void Clear()
        {
            UserData = default(object);
        }
    }
}
