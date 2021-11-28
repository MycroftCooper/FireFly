using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameKit
{
    public class InputManager : BaseManager<InputManager>
    {
        private bool isActive = false;
        private InputManager inputManager;

        public float horizontal
        {
            get
            {
                if (isActive)
                    return Input.GetAxisRaw("Horizontal");
                else
                    return 0;
            }
        }

        public float vertical
        {
            get
            {
                if (isActive)
                    return Input.GetAxisRaw("Vertical");
                else
                    return 0;
            }
        }

        public void SetActive(bool boolean) => isActive = boolean;

        public bool GetKey(KeyCode key)
        {
            if (!isActive)
                return false;
            return Input.GetKey(key);
        }

        public bool GetKeyDown(KeyCode key)
        {
            if (!isActive)
                return false;
            return Input.GetKeyDown(key);
        }

        public bool GetKeyUp(KeyCode key)
        {
            if (!isActive)
                return false;
            return Input.GetKeyUp(key);
        }
    }
}
