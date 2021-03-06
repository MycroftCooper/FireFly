using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameKit;

namespace GameKit
{
    public class GameManager : MonoBehaviour
    {
        public Dictionary<string, Object> regisCenter;
        public static GameManager instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void Register<T>(string name, T target) where T : Object
        {
            if (regisCenter == null)
                regisCenter = new Dictionary<string, Object>();
            if (!regisCenter.ContainsKey(name))
                regisCenter.Add(name, target as Object);
        }

        public T GetRegister<T>(string name) where T : Object
        {
            if (regisCenter.ContainsKey(name))
                return regisCenter[name] as T;
            return null;
        }
    }
}

