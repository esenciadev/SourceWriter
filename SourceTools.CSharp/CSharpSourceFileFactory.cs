namespace EsenciaDev.SourceTools.CSharp
{
    public class CSharpSourceFileFactory : ISourceFileFactory
    {
        //TODO: A constructor which takes the parameters to pass on creating a file

        public ISourceFile BuildSourceFile(string filePath)
        {
            return new CSharpSourceCode(filePath, removeEmptyLines: true);
        }
    }
}