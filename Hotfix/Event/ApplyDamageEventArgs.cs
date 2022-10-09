using GameFramework;
using GameFramework.Event;

namespace Farm
{
   public class ApplyDamageEventArgs:GameEventArgs
    {
        public static readonly int EventId = typeof(ApplyDamageEventArgs).GetHashCode();

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

        public static ApplyDamageEventArgs Create(object userData =null)
        {
            ApplyDamageEventArgs applyDamageEventArgs = ReferencePool.Acquire<ApplyDamageEventArgs>();
            applyDamageEventArgs.UserData = userData;
            return applyDamageEventArgs;
        }

        public override void Clear()
        {
            UserData = default(object);
        }
    }
}
