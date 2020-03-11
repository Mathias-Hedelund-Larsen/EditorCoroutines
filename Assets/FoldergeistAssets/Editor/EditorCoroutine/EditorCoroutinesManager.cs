using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using FoldergeistAssets.EditorCoroutines.Internal;

namespace FoldergeistAssets
{
    namespace EditorCoroutines
    {
        public static class EditorCoroutinesManager
        {
            private static readonly List<EditorCoroutine> coroutines = new List<EditorCoroutine>();

            public static void Execute(IEnumerator enumerator, System.Action<bool> OnUpdate = null)
            {
                if (coroutines.Count == 0)
                {
                    EditorApplication.update += Update;
                }

                var coroutine = new EditorCoroutine { _Enumerator = enumerator, _OnUpdate = OnUpdate };
                coroutines.Add(coroutine);
            }

            private static void Update()
            {
                for (int i = 0; i < coroutines.Count; i++)
                {
                    var coroutine = coroutines[i];
                    bool done = !coroutine._Enumerator.MoveNext();
                    if (done)
                    {
                        if (coroutine._History.Count == 0)
                        {
                            coroutines.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            done = false;
                            coroutine._Enumerator = coroutine._History[coroutine._History.Count - 1];
                            coroutine._History.RemoveAt(coroutine._History.Count - 1);
                        }
                    }
                    else
                    {
                        if (coroutine._Enumerator.Current is IEnumerator)
                        {
                            coroutine._History.Add(coroutine._Enumerator);
                            coroutine._Enumerator = (IEnumerator)coroutine._Enumerator.Current;
                        }
                    }

                    coroutine._OnUpdate?.Invoke(done);
                }

                if (coroutines.Count == 0) EditorApplication.update -= Update;
            }

            public static void StopAll()
            {
                coroutines.Clear();
                EditorApplication.update -= Update;
            }
        }
    }
}