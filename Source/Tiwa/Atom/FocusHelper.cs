// <copyright file="FocusHelper.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Wpf.FocusHelper class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Atom.Wpf
{
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    public static class FocusHelper
    {
        public static void Focus( UIElement element )
        {
            if( !element.Focus() )
            {
                element.Dispatcher.BeginInvoke( DispatcherPriority.Input, new ThreadStart( delegate() {
                    element.Focus();
                } ));
            }
        }
    }
}
