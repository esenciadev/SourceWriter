using System.Collections.Generic;
using System.IO;

namespace EsenciaDev.SourceTools
{
    public interface ISourceProcessor
    {
        DirectoryInfo BaseDirectory { get; }
        string SourceFileGlob { get; }
        ISourceFileFactory SourceFactory { get; }

        IEnumerable<ISourceFile> ProcessFiles(SearchOption searchOption = SearchOption.AllDirectories);
        void AddFilesToIgnore(string[] filesToAdd, SearchOption searchOption = SearchOption.AllDirectories);
        void AddFoldersToIgnore(string[] foldersToAdd, SearchOption searchOption = SearchOption.AllDirectories);
    }
}