using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Standard.Generic
{
    /// <summary>
    /// Static Helper class containing generic helper methods that are suitable for static execution
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Executes a list of functions in parallel and returns the first one that does not return a null
        /// </summary>
        /// <typeparam name="T">the type for the return</typeparam>
        /// <param name="listOfFunctions">the func array to execute in parallel</param>
        /// <returns>the first returned item that is not null, null or default if they all return null</returns>
        public static async Task<T> GetFirstValueReturned<T>(params Func<T>[] listOfFunctions)
        {
            T returnValue = default(T);

            if (listOfFunctions != null)
            {
                List<Task<T>> tasksToRun = new List<Task<T>>();

                using (CancellationTokenSource cancelTokenSource = new CancellationTokenSource())
                {
                    var cancelToken = cancelTokenSource.Token;

                    foreach (var function in listOfFunctions)
                    {
                        tasksToRun.Add(Task.Run(() => { return function(); }, cancelToken));
                    }

                    while (tasksToRun.Count > 0)
                    {
                        var taskComplete = await Task.WhenAny(tasksToRun);
                        if (taskComplete.Result != null && !taskComplete.Result.Equals(default(T)))
                        {
                            returnValue = taskComplete.Result;
                            tasksToRun.RemoveAll(l => true);
                            break;
                        }
                        tasksToRun.Remove(taskComplete);
                    }
                    cancelTokenSource.Cancel();
                }
            }

            return returnValue;
        }
    }
}
