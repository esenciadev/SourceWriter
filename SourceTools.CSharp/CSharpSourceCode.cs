using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EsenciaDev.SourceTools.CSharp
{
    public sealed class CSharpSourceCode : SourceFile
    {
        private const string ExcludeBlockBegin = @"/**-- TS EXCLUSION BEGIN --**/";
        private const string ExcludeBlockEnd = @"/**-- TS EXCLUSION END --**/";
        private const string ExcludeFile = @"/**-- TS EXCLUDE FILE --**/";

        public bool RemoveUsingImports { get; private set; }

        /// <summary>
        /// Initalizes a sourcefile with the contents of the file at the given path, processing it for exclusions. This file will have the default priority of 10
        /// </summary>
        /// <param name="fullPath">path to the file</param>
        /// <param name="priority">priority of the file, 1 thru 10, default is 10 (lowest)</param>
        /// <param name="removeComments">Remove whole line comments and flowerboxes from output. This will NOT remove comments at the end of a statement</param>
        /// <param name="removeEmptyLines">Remove empty lines (lines which are only whitespace or newlines) from the output</param>
        /// <param name="removeRegionDirectives">Remove the region 'import' directives from the output</param>
        public CSharpSourceCode(string fullPath, int priority = 10, bool removeComments = true, bool removeEmptyLines = false, bool removeRegionDirectives = true) 
            : base(fullPath, priority, removeEmptyLines, removeComments)
        {
            RemoveUsingImports = removeRegionDirectives;
            var excluding = false;
            var inFlowerBox = false;

            var myLines = new List<string>();

            foreach (var line in File.ReadLines(FullName))
            {
                var cleanLine = line.Trim();

                if (String.Equals(cleanLine, ExcludeFile))
                {
                    return;
                }

                if (excluding)
                {
                    if (String.Equals(cleanLine, ExcludeBlockEnd))
                    {
                        excluding = false;
                    }

                    continue;
                }

                if (String.Equals(cleanLine, ExcludeBlockBegin))
                {
                    excluding = true;
                    continue;
                }

                if (removeComments)
                {
                    if (cleanLine.StartsWith("//"))
                    {
                        continue;
                    }

                    if (cleanLine.StartsWith("/**"))
                    {
                        inFlowerBox = true;
                        continue;
                    }

                    if (inFlowerBox)
                    {
                        if (cleanLine.EndsWith("**/"))
                        {
                            inFlowerBox = false;
                        }

                        continue;
                    }
                }

                if (removeEmptyLines)
                {
                    if (String.IsNullOrEmpty(cleanLine))
                    {
                        continue;
                    }
                }

                if (removeRegionDirectives)
                {
                    if (cleanLine.StartsWith("using") && cleanLine.EndsWith(";"))
                    {
                        continue;
                    }
                }

                if (removeRegionDirectives && (cleanLine.StartsWith("#region") || cleanLine.StartsWith("#endregion")))
                {
                    continue;
                }

                myLines.Add(line);
            }

            Contents = myLines.ToArray();
        }
    }
}
