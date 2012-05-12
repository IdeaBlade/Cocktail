﻿// ====================================================================================================================
//   Copyright (c) 2012 IdeaBlade
// ====================================================================================================================
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
//   WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
//   OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
//   OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
// ====================================================================================================================
//   USE OF THIS SOFTWARE IS GOVERENED BY THE LICENSING TERMS WHICH CAN BE FOUND AT
//   http://cocktail.ideablade.com/licensing
// ====================================================================================================================

using System.Threading.Tasks;
using Caliburn.Micro;
using IdeaBlade.EntityModel;

namespace Cocktail
{
    /// <summary>
    ///   Marks an awaitable object.
    /// </summary>
    public interface IAwaitable
    {
        /// <summary>
        ///   Converts the current object to <see cref="Task" /> .
        /// </summary>
        Task AsTask();
    }

    /// <summary>
    ///   Marks an awaitable object with a result value.
    /// </summary>
    /// <typeparam name="T"> The type of the result value. </typeparam>
    public interface IAwaitable<T> : IAwaitable
    {
        /// <summary>
        ///   Converts the current object to <see cref="Task{T}" /> .
        /// </summary>
        /// <returns> The type of the result value. </returns>
        new Task<T> AsTask();
    }

    public partial class OperationResult : IAwaitable
    {
        private TaskCompletionSource<bool> _tcs;

        /// <summary>
        /// Returns a Task for the current OperationResult.
        /// </summary>
        public Task AsTask()
        {
            if (_tcs != null) return _tcs.Task;

            _tcs = new TaskCompletionSource<bool>();
            _asyncOp.WhenCompleted(
                args =>
                {
                    if (args.Cancelled)
                        _tcs.SetCanceled();
                    else if (args.Error != null && !args.IsErrorHandled)
                    {
                        args.IsErrorHandled = true;
                        _tcs.SetException(args.Error);
                    }
                    else
                        _tcs.SetResult(true);
                });

            return _tcs.Task;
        }

        /// <summary>
        /// Implicitly converts the current OperationResult to type <see cref="Task"/>
        /// </summary>
        /// <param name="operation">The OperationResult to be converted.</param>
        public static implicit operator Task(OperationResult operation)
        {
            return operation.AsTask();
        }
    }

    public abstract partial class OperationResult<T> : IAwaitable<T>
    {
        private TaskCompletionSource<T> _tcs;

        /// <summary>
        /// Returns a Task&lt;T&gt; for the current OperationResult.
        /// </summary>
        public new Task<T> AsTask()
        {
            if (_tcs != null) return _tcs.Task;

            _tcs = new TaskCompletionSource<T>();
            ((INotifyCompleted)this).WhenCompleted(
                args =>
                {
                    if (args.Cancelled)
                        _tcs.SetCanceled();
                    else if (args.Error != null && !args.IsErrorHandled)
                    {
                        args.IsErrorHandled = true;
                        _tcs.SetException(args.Error);
                    }
                    else
                        _tcs.SetResult(args.Error == null ? Result : default(T));
                });

            return _tcs.Task;
        }

        /// <summary>
        /// Implicitly converts the current OperationResult to type <see cref="Task"/>
        /// </summary>
        /// <param name="operation">The OperationResult to be converted.</param>
        public static implicit operator Task(OperationResult<T> operation)
        {
            return operation.AsTask();
        }

        /// <summary>
        /// Implicitly converts the current OperationResult to type <see cref="Task{T}"/>
        /// </summary>
        /// <param name="operation">The OperationResult to be converted.</param>
        public static implicit operator Task<T>(OperationResult<T> operation)
        {
            return operation.AsTask();
        }
    }

    public abstract partial class DialogOperationResult<T> : IAwaitable<T>
    {
        private TaskCompletionSource<T> _tcs;

        Task IAwaitable.AsTask()
        {
            return AsTask();
        }

        /// <summary>
        /// Returns a Task&lt;T&gt; for the current DialogOperationResult.
        /// </summary>
        public Task<T> AsTask()
        {
            if (_tcs != null) return _tcs.Task;

            _tcs = new TaskCompletionSource<T>();
            ((IResult)this).Completed +=
                (sender, args) =>
                {
                    if (args.WasCancelled)
                        _tcs.SetCanceled();
                    else if (args.Error != null)
                        _tcs.SetException(args.Error);
                    else
                        _tcs.SetResult(args.Error == null ? DialogResult : default(T));
                };

            return _tcs.Task;
        }

        /// <summary>
        /// Implicitly converts the current DialogOperationResult to type <see cref="Task"/>
        /// </summary>
        /// <param name="operation">The DialogOperationResult to be converted.</param>
        public static implicit operator Task(DialogOperationResult<T> operation)
        {
            return operation.AsTask();
        }

        /// <summary>
        /// Implicitly converts the current DialogOperationResult to type <see cref="Task{T}"/>
        /// </summary>
        /// <param name="operation">The DialogOperationResult to be converted.</param>
        public static implicit operator Task<T>(DialogOperationResult<T> operation)
        {
            return operation.AsTask();
        }
    }

    public partial class NavigateResult<T> : IAwaitable
    {
        private TaskCompletionSource<bool> _tcs;

        /// <summary>
        ///   Returns a Task&lt;T&gt; for the current NavigateResult.
        /// </summary>
        public Task AsTask()
        {
            if (_tcs != null) return _tcs.Task;

            _tcs = new TaskCompletionSource<bool>();
            ((IResult)this).Completed +=
                (sender, args) =>
                {
                    if (args.WasCancelled)
                        _tcs.SetCanceled();
                    else if (args.Error != null)
                        _tcs.SetException(args.Error);
                    else
                        _tcs.SetResult(true);
                };

            if (_status == Status.WaitingToRun)
                Go();

            return _tcs.Task;
        }

        /// <summary>
        ///   Implicitly converts the current NavigateResult to type <see cref="Task" />
        /// </summary>
        /// <param name="operation"> The NavigateResult to be converted. </param>
        public static implicit operator Task(NavigateResult<T> operation)
        {
            return operation.AsTask();
        }
    }
}