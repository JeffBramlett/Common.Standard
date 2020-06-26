using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Generic Builder contract (Builder design pattern)
    /// </summary>
    /// <typeparam name="T">type to be built</typeparam>
    public interface IGenericBuilder<T>: IDisposable where T:class, new()
    {
        /// <summary>
        /// Build a instance of type T using some arguments
        /// </summary>
        /// <param name="args">arguments for building an object of type T</param>
        /// <returns>the new object of type T</returns>
        T Build(params object[] args);
    }
}
