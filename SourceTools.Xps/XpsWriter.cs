using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace EsenciaDev.SourceTools.Xps
{
    public class XpsWriter : ISourceWriter
    {
        public void WriteSource(IEnumerable<ISourceFile> source, string outputFile, int maxPages = 50, int fontSize = 12)
        {
            const double columnWidth = 400;
            var document = new XpsDocument(outputFile, FileAccess.ReadWrite);

            var flowDoc = new FlowDocument { ColumnWidth = columnWidth, PagePadding = new Thickness(35) };

            var paginator = ((IDocumentPaginatorSource)flowDoc).DocumentPaginator;

            foreach (var sourceFile in source)
            {
                int i = 0;
                var src = sourceFile.Contents;

                var para = new Paragraph { FontSize = fontSize };

                flowDoc.Blocks.Add(para);
                do
                {
                    var txt = src[i];
                    var run = new Run(txt);
                    para.Inlines.Add(run);
                    var lb = new LineBreak();
                    para.Inlines.Add(lb);

                    paginator.ComputePageCount();
                    i++;
                } while (i < src.Length && paginator.PageCount <= maxPages);
            }
            var docWriter = XpsDocument.CreateXpsDocumentWriter(document);
            docWriter.Write(paginator);
            document.Close();
        }
    }
}
