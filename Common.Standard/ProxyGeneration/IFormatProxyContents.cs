using ProxyGeneration.Dto;
using System.Collections.Generic;

namespace Common.Standard.ProxyGeneration
{
    public interface IFormatProxyContents
    {
        IList<string> FormatProxyExtraction(ProxyExtraction extraction);

    }
}