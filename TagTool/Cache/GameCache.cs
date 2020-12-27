﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.BlamFile;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Resources;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    public abstract class GameCache
    {
        public string DisplayName = "default";
        public CacheVersion Version;
        public EndianFormat Endianness;
        public TagSerializer Serializer;
        public TagDeserializer Deserializer;
        public DirectoryInfo Directory;

        public List<LocaleTable> LocaleTables;
        public abstract StringTable StringTable { get; }
        public abstract TagCache TagCache { get; }
        public abstract ResourceCache ResourceCache { get; }

        public abstract Stream OpenCacheRead();
        public abstract Stream OpenCacheReadWrite();
        public abstract Stream OpenCacheWrite();

        public abstract void Serialize(Stream stream, CachedTag instance, object definition);
        public abstract object Deserialize(Stream stream, CachedTag instance);
        public abstract T Deserialize<T>(Stream stream, CachedTag instance);

        public static GameCache Open(FileInfo file)
        {
            MapFile map = new MapFile();
            var estimatedVersion = CacheVersion.HaloOnline106708;

            using (var stream = file.OpenRead())
            using (var reader = new EndianReader(stream))
            {
                if (file.Name.Contains(".map"))
                {
                    map.Read(reader);
                    estimatedVersion = map.Version;
                }
                else if (file.Name.Equals("tags.dat"))
                    estimatedVersion = CacheVersion.HaloOnline106708;
                else
                    throw new Exception("Invalid file passed to GameCache constructor");
            }

            switch (estimatedVersion)
            {
                case CacheVersion.HaloXbox:
                case CacheVersion.HaloPC:
                case CacheVersion.HaloCustomEdition:
                    return new GameCacheGen1(map, file);

                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                case CacheVersion.Halo2Beta:
                    return new GameCacheGen2(map, file);

                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                case CacheVersion.HaloReach:
                case CacheVersion.HaloReachMCC0824:
                case CacheVersion.HaloReachMCC0887:
                case CacheVersion.HaloReachMCC1035:
                case CacheVersion.HaloReachMCC1211:
                    return new GameCacheGen3(map, file);

                case CacheVersion.HaloOnline106708:
                case CacheVersion.HaloOnline235640:
                case CacheVersion.HaloOnline301003:
                case CacheVersion.HaloOnline327043:
                case CacheVersion.HaloOnline372731:
                case CacheVersion.HaloOnline416097:
                case CacheVersion.HaloOnline430475:
                case CacheVersion.HaloOnline454665:
                case CacheVersion.HaloOnline449175:
                case CacheVersion.HaloOnline498295:
                case CacheVersion.HaloOnline530605:
                case CacheVersion.HaloOnline532911:
                case CacheVersion.HaloOnline554482:
                case CacheVersion.HaloOnline571627:
                case CacheVersion.HaloOnline700123:
                    {
                        var directory = file.Directory.FullName;
                        var tagsPath = Path.Combine(directory, "tags.dat");
                        var tagsFile = new FileInfo(tagsPath);

                        if (!tagsFile.Exists)
                            throw new Exception("Failed to find tags.dat");

                        return new GameCacheHaloOnline(tagsFile.Directory);
                    }
            }

            return null;
        }

        public abstract void SaveStrings();

        public virtual void SaveTagNames(string path = null) => throw new NotImplementedException();
    }
}
