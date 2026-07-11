/*

Parrot Code
Copyright (c) 2026 Sludgy Parrot (Pty) Ltd. All Rights Reserved.

This source code is proprietary and confidential software owned by
Sludgy Parrot (Pty) Ltd.

Parrot Code is a commercial software product developed and distributed
by Sludgy Parrot (Pty) Ltd.

Unauthorized copying, modification, distribution, sublicensing,
reverse engineering, decompilation, disclosure, or use of this
software, in whole or in part, is strictly prohibited without
prior written permission from Sludgy Parrot (Pty) Ltd.

This software is provided under the terms of a separate license
agreement. Possession of this source code does not grant any rights
to use, modify, distribute, or create derivative works unless
explicitly authorized by a valid written license.

THE SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, EXCEPT AS REQUIRED BY APPLICABLE LAW.

For licensing inquiries:
licensing@sludgyparrot.com

*/

#region Included Parrot Code Assemblies
using ParrotCode.Native;
#endregion

namespace ParrotCode.UI
{
    public sealed class ExplicitNavigation : INavigationStrategy
    {
        private readonly NavigationSystem navigationSystem;

        public ExplicitNavigation(NavigationSystem navigationSystem) => 
            this.navigationSystem = navigationSystem;

        public void Navigate(ISelectable selectable, ScreenDirection direction)
        {
            Selectable component = selectable as Selectable;

            switch(direction)
            {
                case ScreenDirection.Up:
                    if(component.OnSelectionUp != null)
                    {
                        navigationSystem.Select(component.OnSelectionUp);
                        navigationSystem.Deselect(component);
                    }
                    break;
                case ScreenDirection.Down:
                    if (component.OnSelectionDown != null)
                    {
                        navigationSystem.Select(component.OnSelectionDown);
                        navigationSystem.Deselect(component);
                    }
                    break;
                case ScreenDirection.Left:
                    if (component.OnSelectionLeft != null)
                    {
                        navigationSystem.Select(component.OnSelectionLeft);
                        navigationSystem.Deselect(component);
                    }
                    break;
                case ScreenDirection.Right:
                    if (component.OnSelectionRight != null)
                    {
                        navigationSystem.Select(component.OnSelectionRight);
                        navigationSystem.Deselect(component);
                    }
                    break;
            }
        }
    }
}
