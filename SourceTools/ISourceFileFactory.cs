namespace EsenciaDev.SourceTools
{
    public interface ISourceFileFactory
    {
        ISourceFile BuildSourceFile(string filePath);
    }
}