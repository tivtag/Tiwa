// <copyright file="ReflectionExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.ReflectionExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom
{
    using System;

    /// <summary>
    /// Provides reflection related extension methods.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="Type"/> implements another other <see cref="Type"/>.
        /// </summary>
        /// <param name="thisType">The type that is supposed to implement the other type.</param>
        /// <param name="type">The type that is supposed to be implemented.</param>
        /// <returns>
        /// Returns true if <paramref name="thisType"/> implements the given <paramref name="type"/>;
        /// otherwise false.
        /// </returns>
        public static bool Implements( this Type thisType, Type type )
        {
            return type != null && type.IsAssignableFrom( thisType );
        }
    }
}
