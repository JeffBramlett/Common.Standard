using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Contract for an command execution in an application context
    /// </summary>
    public interface IAppCommand
    {
        /// <summary>
        /// Use the arguments to determine if the command can be executed
        /// </summary>
        /// <param name="args">the arguments to used to determine if this command can be executed</param>
        /// <returns>true if the command can be executed</returns>
        Task<bool> CanExecute(params object[] args);

        /// <summary>
        /// Execute the command using the arguments supplied
        /// </summary>
        /// <param name="args">the collection of argument objects</param>
        /// <returns>void</returns>
        Task Execute(params object[] args);
    }
}
