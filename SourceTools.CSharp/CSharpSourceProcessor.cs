
namespace EsenciaDev.SourceTools.CSharp
{
    public class CSharpSourceProcessor : SourceProcessor
    {
        public override string SourceFileGlob { get { return "*.cs"; } }

        public CSharpSourceProcessor(string baseDirectory, ISourceFileFactory sourceFileFactory) : base(baseDirectory, sourceFileFactory)
        {
            string[] defaultFilePatternsToIgnore = {
                                         "*.Designer*",
                                         "AssemblyInfo*",
                                         "*.g.cs",
                                         "*.i.cs"
                                     };

            

            AddFilesToIgnore(defaultFilePatternsToIgnore);
        }
    }
}