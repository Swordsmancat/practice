using UnityEngine;
using UnityGameFramework.Runtime;

namespace Farm
{
   public partial class GameEntry:MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();

            InitCustomComponents();

            InitCustomDebuggers();
        }
    }
}
