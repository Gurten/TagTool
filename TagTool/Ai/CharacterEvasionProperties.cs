using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterEvasionProperties
    {
        public float EvasionDangerThreshold;
        public float EvasionDelayTimer;
        public float EvasionChance;
        public float EvasionProximityThreshold;
        public float DiveRetreatChance;
    }
}