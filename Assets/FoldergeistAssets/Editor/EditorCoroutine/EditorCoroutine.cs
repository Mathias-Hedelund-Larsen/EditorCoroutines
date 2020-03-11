using System;
using System.Collections;
using System.Collections.Generic;

namespace FoldergeistAssets
{
    namespace EditorCoroutines
    {
        namespace Internal
        {
            public class EditorCoroutine
            {
                public Action<bool> _OnUpdate;
                public IEnumerator _Enumerator;
                public List<IEnumerator> _History = new List<IEnumerator>();
            }
        }
    }
}