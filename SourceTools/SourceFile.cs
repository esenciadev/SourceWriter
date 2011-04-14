using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EsenciaDev.SourceTools
{
    public abstract class SourceFile : IComparable<ISourceFile>, IComparable, ISourceFile
    {
        public string FullName { get; protected set; }
        public int Priority { get; protected set; }
        public string[] Contents { get; protected set; }
        public bool RemoveEmptyLines { get; protected set; }
        public bool RemoveComments { get; protected set; }
        public bool HasContent { get { return Contents != null && Contents.Length > 0; } }

        protected SourceFile(string sourceFilePath, int priority, bool removeEmptyLines, bool removeComments)
        {
            FullName = sourceFilePath;
            Priority = priority;
            RemoveEmptyLines = removeEmptyLines;
            RemoveComments = removeComments;
        }

        public int CompareTo(ISourceFile other)
        {
            return this.Priority.CompareTo(other.Priority);
        }

        public int CompareTo(object obj)
        {
            var other = obj as ISourceFile;
            if (other != null)
            {
                return CompareTo(other);
            }

            throw new NotSupportedException(@"Cannot compare to any object which isn't an ISourceFile");
        }
    }
}
