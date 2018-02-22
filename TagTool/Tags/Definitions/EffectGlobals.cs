using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "effect_globals", Tag = "effg", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "effect_globals", Tag = "effg", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class EffectGlobals
    {
        public List<UnknownBlock> Unknown;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused1;

        [TagStructure(Size = 0x14)]
        public class UnknownBlock
        {
            public uint Unknown;
            public uint Unknown2;
            public List<UnknownBlock2> Unknown3;

            [TagStructure(Size = 0x10)]
            public class UnknownBlock2
            {
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
            }
        }
    }
}