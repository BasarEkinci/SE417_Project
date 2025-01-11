using Extensions;
using UnityEngine.Events;

namespace Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction OnPlayerHide = delegate { };
        public UnityAction OnPlayerWakeUp = delegate { };
        public UnityAction OnPlayerDie = delegate { };
        public UnityAction<int> OnCompleteObjective = delegate { };
        public UnityAction OnCollectObject = delegate { };
        public UnityAction<int> OnCompleteLevel = delegate { };
        public UnityAction OnGameStart = delegate { };
    }
}