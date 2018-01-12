﻿using BlamCore.Cache;
using BlamCore.Commands;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Bitmaps;
using TagTool.CollisionModels;
using TagTool.Common;
using TagTool.Definitions;
using TagTool.Editing;
using TagTool.Files;
using TagTool.PhysicsModels;
using TagTool.RenderModels;
using TagTool.Strings;

namespace TagTool.Tags
{
    public static class TagCacheContextFactory
    {
        public static CommandContext Create(CommandContextStack stack, GameCacheContext cacheContext)
        {
            var context = new CommandContext(stack.Context, "tags");

            context.AddCommand(new HelpCommand(stack));
            context.AddCommand(new ClearCommand());
            context.AddCommand(new DumpLogCommand());
            context.AddCommand(new EchoCommand());
            context.AddCommand(new SetLocaleCommand());
            context.AddCommand(new CleanCsvFileCommand(cacheContext));
            context.AddCommand(new TagDependencyCommand(cacheContext));
            context.AddCommand(new ExtractTagCommand(cacheContext));
            context.AddCommand(new ImportTagCommand(cacheContext));
            context.AddCommand(new GetTagInfoCommand(cacheContext));
            context.AddCommand(new ListTagsCommand(cacheContext));
            context.AddCommand(new GetMapInfoCommand());
            context.AddCommand(new DuplicateTagCommand(cacheContext));
            context.AddCommand(new GetTagAddressCommand());
            context.AddCommand(new TagResourceCommand());
            context.AddCommand(new DeleteTagCommand(cacheContext));
            context.AddCommand(new CleanCacheFilesCommand(cacheContext));
            context.AddCommand(new RebuildCacheFilesCommand(cacheContext));
            context.AddCommand(new TestCommand(cacheContext));
            context.AddCommand(new ListUnusedTagsCommand(cacheContext));
            context.AddCommand(new ListNullTagsCommand(cacheContext));
            context.AddCommand(new CreateTagCommand(cacheContext));
            context.AddCommand(new ExtractAllTagsCommand(cacheContext));
            context.AddCommand(new EditTagCommand(stack, cacheContext));
            context.AddCommand(new CollisionModelTestCommand(cacheContext));
            context.AddCommand(new PhysicsModelTestCommand(cacheContext));
            context.AddCommand(new StringIdCommand(cacheContext));
            context.AddCommand(new ListAllStringsCommand(cacheContext));
            context.AddCommand(new GenerateTagStructuresCommand(cacheContext));
            context.AddCommand(new RenderModelTestCommand(cacheContext));
            context.AddCommand(new ConvertPluginsCommand(cacheContext));
            context.AddCommand(new GenerateTagNamesCommand(cacheContext));
            context.AddCommand(new NameTagCommand(cacheContext));
            context.AddCommand(new SaveTagNamesCommand(cacheContext));
            context.AddCommand(new MatchTagsCommand(cacheContext));
            context.AddCommand(new ConvertTagCommand(cacheContext));
            context.AddCommand(new UpdateMapFilesCommand(cacheContext));
            context.AddCommand(new ExtractBitmapsCommand(cacheContext));
            context.AddCommand(new GenerateAssemblyPluginsCommand());
            context.AddCommand(new RelocateResourcesCommand(cacheContext));
            context.AddCommand(new ListUnnamedTagsCommand(cacheContext));

            var exeFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var dllFile = new FileInfo(Path.Combine(exeFile.Directory.FullName, "Porting.dll"));

            if (dllFile.Exists)
            {
                var dll = Assembly.LoadFile(dllFile.FullName);

                foreach (var type in dll.GetExportedTypes())
                {
                    if (type.Name == "OpenCacheFileCommand")
                    {
                        var command = Activator.CreateInstance(type, new object[] { stack, cacheContext });
                        context.AddCommand(command as Command);
                        break;
                    }
                }
            }

            return context;
        }
    }
}