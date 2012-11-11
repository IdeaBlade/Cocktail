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

using System;
using System.Runtime.Serialization;

#if !LIGHT
using IdeaBlade.Core;
#endif

namespace Cocktail
{
#if !LIGHT
    /// <summary>
    ///   Exception thrown if an entity cannot be found.
    /// </summary>
    public sealed class EntityNotFoundException : IdeaBladeException
    {
        /// <summary>
        ///   Initializes a new EntityNotFoundException.
        /// </summary>
        /// <param name="message"> A message added to describe the exception. </param>
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
#else
    /// <summary>
    ///   Exception thrown if an entity cannot be found.
    /// </summary>
    public sealed class EntityNotFoundException : Exception
    {
        /// <summary>
        ///   Initializes a new EntityNotFoundException.
        /// </summary>
        /// <param name="message"> A message added to describe the exception. </param>
        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }
#endif
}