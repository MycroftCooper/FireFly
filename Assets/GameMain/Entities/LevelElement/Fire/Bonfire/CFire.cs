using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CFire : MonoBehaviour {
    public bool burning;

    private bool fireStateChange;
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        fireStateChange = false;
    }

    private void Update() {
        if (fireStateChange) {
            fireStateChange = false;
            FireState();
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K)) {
            ChangeFireState();
        }
#endif
    }

    public void ChangeFireState() {
        fireStateChange = true;
    }

    private void FireState() {
        burning = !burning;
        animator.SetBool("Burning", burning);
    }
}
