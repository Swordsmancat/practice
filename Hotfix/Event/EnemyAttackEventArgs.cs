using GameFramework;
using GameFramework.Event;

namespace Farm
{
   public class EnemyAttackEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnemyAttackEventArgs).GetHashCode();

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

        public static EnemyAttackEventArgs Create(object userdata =null)
        {
            EnemyAttackEventArgs enemyAttackEvent = ReferencePool.Acquire<EnemyAttackEventArgs>();
            enemyAttackEvent.UserData = userdata;
            return enemyAttackEvent;
        }

        public override void Clear()
        {
            UserData = default(object);
        }
    }
}
