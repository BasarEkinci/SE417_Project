using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayeData : ScriptableObject
{
    public List<ParticleSystem> ParticleEffects;
    public List<AudioClip> SoundEffects;
    public float Health;
    public float MovementSpeed;
}
