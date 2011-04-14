using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EsenciaDev.SourceTools
{
    public abstract class SourceProcessor : ISourceProcessor
    {
        public DirectoryInfo BaseDirectory { get; private set; }
        public List<FileSystemInfo> ItemsToIgnore { get; set; }
        public ISourceFileFactory SourceFactory { get; private set; }
        public abstract string SourceFileGlob { get; }

        protected SourceProcessor(string baseDirectory, ISourceFileFactory sourceFileFactory)
        {
            SourceFactory = sourceFileFactory;
            BaseDirectory = new DirectoryInfo(baseDirectory);
            ItemsToIgnore = new List<FileSystemInfo>();

            string[] defaultDirectoriesToIgnore = {
                                               "bin",
                                               "obj",
                                               "*.hg",
                                               "*.svn",
                                               "*resharper*"
                                           };

            AddFoldersToIgnore(defaultDirectoriesToIgnore);
        }

        public IEnumerable<ISourceFile> ProcessFiles(SearchOption searchOption = SearchOption.AllDirectories)
        {
            return ProcessDirectory(BaseDirectory, searchOption);
        }

        protected IEnumerable<ISourceFile> ProcessDirectory(DirectoryInfo currentDirectory, SearchOption searchOption)
        {
            var sourceFiles = new List<ISourceFile>();

            foreach (var dir in currentDirectory.EnumerateDirectories())
            {
                var curDir = dir;
                if (ShouldIgnore(curDir))
                {
                    continue;
                }

                sourceFiles.AddRange(ProcessDirectory(curDir, searchOption));
            }

            foreach (var file in currentDirectory.EnumerateFiles(SourceFileGlob))
            {
                var curFile = file;
                if (ShouldIgnore(curFile))
                {
                    continue;
                }

                var source = SourceFactory.BuildSourceFile(curFile.FullName);
                if (source.HasContent)
                {
                    /*
                    if (priorityFiles.Any(p => curFile.FullName.EndsWith(p)))
                    {
                        source.Priority = 5;
                    }
                    */
                    sourceFiles.Add(source);
                }
            }

            sourceFiles.Sort();
            return sourceFiles;
        }

        public void AddFilesToIgnore(string[] filesToAdd, SearchOption searchOption = SearchOption.AllDirectories)
        {
            Array.ForEach(filesToAdd, a => ItemsToIgnore.AddRange(BaseDirectory.EnumerateFiles(a, searchOption)));
        }

        public void AddFoldersToIgnore(string[] foldersToAdd, SearchOption searchOption = SearchOption.AllDirectories)
        {
            Array.ForEach(foldersToAdd, a => ItemsToIgnore.AddRange(BaseDirectory.EnumerateDirectories(a, searchOption)));
        }

        protected bool ShouldIgnore(FileSystemInfo fsi)
        {
            return ItemsToIgnore.Any(d => String.Equals(d.FullName, fsi.FullName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}