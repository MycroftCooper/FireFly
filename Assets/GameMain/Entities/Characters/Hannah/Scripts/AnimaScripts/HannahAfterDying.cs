using UnityEngine;

public class HannahAfterDying : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetInteger("MotionState", 0);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetInteger("MotionState", 0);
        TotemController tc = GameObject.Find("Totem").GetComponent<TotemController>();
        tc.LoadScane();
    }
}
