using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EsenciaDev.SourceTools
{
    public interface ISourceWriter
    {
        /// <summary>
        /// Writes the collection of source files to the specified output file.
        /// </summary>
        /// <param name="sourceFiles">SourceFiles to write</param>
        /// <param name="outputFile">Path to the output file. If it exists it will be overwritten</param>
        /// <param name="maxPages">Maximum number of pages to write, defaults to 50</param>
        /// <param name="fontSize">Font size for the output, defaults to 12</param>
        void WriteSource(IEnumerable<ISourceFile> sourceFiles, string outputFile, int maxPages = 50, int fontSize = 12);
    }
}
