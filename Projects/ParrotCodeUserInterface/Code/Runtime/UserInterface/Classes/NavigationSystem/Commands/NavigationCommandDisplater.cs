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
using ParrotCode.InputSystem;
using ParrotCode.Extensions;
#endregion

namespace ParrotCode.UI
{
    public sealed class NavigationCommandDisplater
    {
        private readonly NavigationSystem navigationSystem;

        public NavigationCommandDisplater(NavigationSystem navigationSystem) => this.navigationSystem = navigationSystem;

        public void Dispatch(InputActionEvent evt)
        {
            if (!evt.Performed)
                return;

            INavigationCommand command = CreateCommand(evt);
            command.Execute();
        }

        private INavigationCommand CreateCommand(InputActionEvent evt)
        {
            switch (evt.Action)
            {
                case InputActionType.Navigate:
                    return new NavigationCommand(navigationSystem, evt.InputAxis2D.ToScreenDirection());
                case InputActionType.Submit:
                    return new SubmitCommand(navigationSystem);
                case InputActionType.Back:
                case InputActionType.Cancel:
                case InputActionType.Return:
                    return new CancelCommand(navigationSystem);
            }

            return null;
        }
    }
}
