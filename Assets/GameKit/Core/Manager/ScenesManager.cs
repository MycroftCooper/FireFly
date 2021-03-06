using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using GameKit;

namespace GameKit
{
    public class ScenesManager : BaseManager<ScenesManager>
    {
        public Scene GetScene()
        {
            return SceneManager.GetActiveScene();
        }
        public void LoadScene(string name, UnityAction callback)
        {
            SceneManager.LoadScene(name);
            callback.Invoke();
        }

        public void LoadSceneAsyn(string name, UnityAction callback)
        {
            MonoManager.instance.StartCoroutine(LoadSceneAsynIE(name, callback));
        }

        public void LoadSceneAsynAdd(string name, UnityAction callback)
        {
            MonoManager.instance.StartCoroutine(LoadSceneAdd(name, callback));
        }

        public void UnloadSceneAsyn(string name, UnityAction callback)
        {
            MonoManager.instance.StartCoroutine(UnloadSceneAsyncIE(name, callback));
        }

        IEnumerator LoadSceneAsynIE(string name, UnityAction callback)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);

            while (!ao.isDone)
            {
                EventCenter.instance.EventTrigger("Loading Scene", ao.progress);
                yield return ao.progress;
            }
            callback?.Invoke();
        }

        IEnumerator LoadSceneAdd(string name, UnityAction callback)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            while (!ao.isDone)
            {
                EventCenter.instance.EventTrigger("Loading Scene", ao.progress);
                yield return ao.progress;
            }
            callback?.Invoke();
        }

        IEnumerator UnloadSceneAsyncIE(string name, UnityAction callback)
        {
            AsyncOperation ao = SceneManager.UnloadSceneAsync(name);
            while (!ao.isDone)
            {
                EventCenter.instance.EventTrigger("Removing Scene", ao.progress);
                yield return ao.progress;
            }
            callback?.Invoke();
        }
    }
}
