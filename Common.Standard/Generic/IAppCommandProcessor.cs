using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Contract for command execution in an application context
    /// </summary>
    public interface IAppCommandProcessor: IDisposable
    {
        /// <summary>
        /// Initialize the processor with a collection of arguments
        /// </summary>
        /// <param name="args">collection of arguments</param>
        void Init(params object[] args);

        /// <summary>
        /// Add a command to be executed by the processor
        /// </summary>
        /// <param name="command">the command to execute</param>
        /// <returns>true if successfully added</returns>
        Task<bool> AddCommand(IAppCommand command);
    }
}
