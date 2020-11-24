using UnityEngine;

namespace GS.Basics.Simple
{
    /// <summary>
    /// Basic npc character.
    /// 
    /// </summary>
    public class NpcCharacter : MonoBehaviour
    {
        [SerializeField] protected CharacterActions actions;
        [SerializeField] protected CharacterMovement movement;
        [SerializeField] protected PathFinder pathfinder;
    }
}
