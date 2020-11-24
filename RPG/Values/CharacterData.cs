using UnityEngine;

namespace GS.RPG.Values
{
    [CreateAssetMenu(fileName = "New CharacterData", menuName = "GS/Character/CharacterData")]
    public class CharacterData : GS.Data.ObjectData
    {
        public GS.RPG.Values.ClassStats classData;
        public GS.RPG.Values.ItemStats[] equipmentData;
    }
}
