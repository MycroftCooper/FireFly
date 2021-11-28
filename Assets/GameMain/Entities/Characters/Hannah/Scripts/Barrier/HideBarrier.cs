using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBarrier : Barrier
{
    public Animator animator;
    public override void TakeBarrier()
    {
        animator.SetBool("TrapTouched", true);
    }
}
