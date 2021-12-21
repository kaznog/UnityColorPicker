using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : SingletonMonoBehaviour<CoroutineHandler>
{
    protected override void Init()
    {
        base.Init();
    }

    public static Coroutine StartStaticCoroutine(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }
}
