namespace EsenciaDev.SourceTools
{
    public interface ISourceFile
    {
        string FullName { get; }
        int Priority { get; }
        string[] Contents { get; }
        bool HasContent { get; }
        bool RemoveEmptyLines { get; }
        bool RemoveComments { get; }
    }
}