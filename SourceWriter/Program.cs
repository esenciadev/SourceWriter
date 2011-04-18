using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using EsenciaDev.SourceTools.CSharp;
using EsenciaDev.SourceTools.Xps;

namespace EsenciaDev.SourceTools.SourceWriter
{
    class Program
    {
        static string[] priorityFiles = { "CaptureManager.cs" };

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Incorrect number of args");
                Console.WriteLine("Usage: SourceWriter <Path to source directory> <Output file>");
                Console.WriteLine();
                return;
            }

            string sourceFolder = args[0];
            
            if (!Directory.Exists(sourceFolder))
            {
                Console.WriteLine("Source code path doesn't exist");
                return;
            }
            
            string outputFile = args[1];
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
            
            ISourceFileFactory sourceFileFactory = new CSharpSourceFileFactory();
            ISourceProcessor processor = new CSharpSourceProcessor(sourceFolder, sourceFileFactory);

            var fileExcludeFromConfig = ConfigurationManager.AppSettings["FilesToExclude"];
            if (!String.IsNullOrEmpty(fileExcludeFromConfig))
            {
                processor.AddFilesToIgnore(fileExcludeFromConfig.Split(','));
            }

            var folderExcludeFromConfig = ConfigurationManager.AppSettings["DirectoriesToExclude"];
            if (!String.IsNullOrEmpty(folderExcludeFromConfig))
            {
                processor.AddFoldersToIgnore(folderExcludeFromConfig.Split(','));
            }

            var sourceFiles = processor.ProcessFiles();

            ISourceWriter sourceWriter = new XpsWriter();
            sourceWriter.WriteSource(sourceFiles, outputFile);
        }

 
    }
}
