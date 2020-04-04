using System;
using System.Collections.Generic;
using System.Text;
using ProxyGeneration.Dto;

namespace Common.Standard.ProxyGeneration
{
    public abstract class AbstractCSharpProxyFormatter : IFormatProxyContents
    {

        #region Publics
        public IList<string> FormatProxyExtraction(ProxyExtraction extraction)
        {
            List<string> formattedList = new List<string>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region Header");
            sb.AppendLine($"/*");
            sb.AppendLine($"+------------------------------------------------------------------------------+");
            sb.AppendLine(MakeCommentLine("|- ", $"Proxy generated from: {extraction.SourceAssembly}", "-|", 80));
            sb.AppendLine($"|- Proxy generated on: {extraction.Timestamp}");
            sb.AppendLine($"+------------------------------------------------------------------------------+");
            sb.AppendLine($"*/");
            sb.AppendLine("#endregion\n");


            foreach (var typeExtraction in extraction.TypeExtractions)
            {
                var typeContent = FormatExtractedType(sb.ToString(), typeExtraction);
                formattedList.Add(typeContent);
            }

            return formattedList;
        }

        public abstract string ProxyBeforeExecution(PropertyExtraction propField, bool isGet);

        public abstract string ProxyAfterExecution(PropertyExtraction propField, bool isGet);
        #endregion

        #region Protected
        protected virtual string FormatExtractedType(string header, TypeExtraction extractType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);

            sb.AppendLine("#region Usings");
            sb.AppendLine("#endregion\n");

            sb.AppendLine($"\nnamespace {extractType.Namespace}");
            sb.AppendLine("{");

            sb.AppendLine($"\tpublic class {extractType.Name}");
            sb.AppendLine("\t{");

            sb.AppendLine("\t\t#region Fields");
            foreach (var extractTypeField in extractType.Fields)
            {
                sb.AppendLine($"\t\tpublic {extractTypeField.TypeName} {extractTypeField.Name}");
            }
            sb.AppendLine("\t\t#endregion\n");

            sb.AppendLine("\t\t#region Properties");
            foreach (var propField in extractType.Properties)
            {
                FormatProperty(sb, propField);
            }
            sb.AppendLine("\t\t#endregion\n");

            sb.AppendLine("#region Publics");
            foreach (var extractTypeMethod in extractType.Methods)
            {
                FormatMethod(sb, extractTypeMethod);
            }
            sb.AppendLine("#endregion");

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            return sb.ToString();
        }

        protected virtual void FormatProperty(StringBuilder sb, PropertyExtraction propField)
        {
            sb.AppendLine($"\t\tpublic {propField.TypeName} {propField.Name}");
            sb.AppendLine("\t\t{");
            if (propField.HasGet)
            {
                sb.AppendLine("\t\t\tget");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine($"\t\t\t\t{ProxyBeforeExecution(propField, true)}");
                sb.AppendLine($"\t\t\t\treturn _{propField.Name};");
                sb.AppendLine($"\t\t\t\t{ProxyAfterExecution(propField, true)}");
                sb.AppendLine("\t\t\t}");
            }
            if (propField.HasSet)
            {
                sb.AppendLine("\t\t\tset");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine($"\t\t\t\t{ProxyBeforeExecution(propField, false)}");
                sb.AppendLine($"\t\t\t\treturn _{propField.Name};");
                sb.AppendLine($"\t\t\t\t{ProxyAfterExecution(propField, false)}");
                sb.AppendLine("\t\t\t}");
            }
            sb.AppendLine("\t\t}\n");
        }

        protected virtual void FormatMethod(StringBuilder sb, MethodExtraction methExtract)
        {
            sb.AppendLine($"\t\tpublic {methExtract.Signature}");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t}");
        }

        private string MakeCommentLine(string prefix, string content, string suffix, int maxLineLength)
        {
            int marginLength = prefix.Length + suffix.Length;
            if (content.Length + marginLength <= maxLineLength)
            {
                return string.Format("{0}{1}{2}\n", prefix, content, suffix);
            }
            else
            {
                int splitNdx = maxLineLength - marginLength;
                string line = content.Substring(0, splitNdx);
                string line2 = content.Substring(splitNdx + 1);
                return string.Format("{0}{1}{3}\n{0}{2}{3}\n", prefix, line, line2, suffix);
            }
        }
        #endregion
    }
}
