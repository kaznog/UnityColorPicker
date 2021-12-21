using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourWithInit: MonoBehaviour
{
    private bool _isInitialized = false;

    public void InitIfNeeded()
    {
        if (_isInitialized)
        {
            return;
        }
        Init();
        _isInitialized = true;
    }

    protected virtual void Init() { }

    protected virtual void Awake() { }

    protected virtual void OnDestroy() { }
}

public class SingletonMonoBehaviour<T> : MonoBehaviourWithInit where T : MonoBehaviourWithInit
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(go);
                    }
                    go.hideFlags = HideFlags.DontSave;
                }
            }
            _instance.InitIfNeeded();
            return _instance;
        }
    }
}
