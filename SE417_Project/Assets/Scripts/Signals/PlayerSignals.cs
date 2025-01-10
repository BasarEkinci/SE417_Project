using Extensions;
using UnityEngine.Events;
public class PlayerSignals : MonoSingleton<PlayerSignals>
{
    public UnityAction OnPlayerHide = delegate { };
    public UnityAction OnPlayerWakeUp = delegate { };
    public UnityAction OnPlayerDie = delegate { };
    public UnityAction<int> OnCompleteLevel = delegate { };
}