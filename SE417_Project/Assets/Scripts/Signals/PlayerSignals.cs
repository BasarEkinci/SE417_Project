using Extensions;
using UnityEngine;
using UnityEngine.Events;
public class PlayerSignals : MonoSingleton<PlayerSignals>
{
    public UnityAction OnPlayerHide = delegate { };
    public UnityAction OnPlayerWakeUp = delegate { };
    public UnityAction OnPlayerDie = delegate { };
}