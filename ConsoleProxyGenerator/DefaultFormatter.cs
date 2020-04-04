using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Standard.ProxyGeneration;
using ProxyGeneration.Dto;

namespace ConsoleProxyGenerator
{
    class DefaultFormatter: AbstractCSharpProxyFormatter
    {
        public override string ProxyAfterExecution(PropertyExtraction propField, bool isGet)
        {
            return string.Empty;
        }

        public override string ProxyBeforeExecution(PropertyExtraction propField, bool isGet)
        {
            return string.Empty;
        }
    }
}
