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

using UnityEngine;

namespace ParrotCode.Native
{
    [DisallowMultipleComponent]
    public class SingletonInstance<T>: BaseMonoBehaviour where T : MonoBehaviour
    {
        [SerializeField, Space(5)]
        protected bool doNotDestroyOnLoad = true;

        private static T instance;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindFirstObjectByType<T>();
                    instance ??= new GameObject($"{nameof(T)} [Singleton Instance]").AddComponent<T>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (!doNotDestroyOnLoad)
                return;

            if(instance != null && instance != this)
                Destroy(this);

            DontDestroyOnLoad(this);
        }
    }
}
