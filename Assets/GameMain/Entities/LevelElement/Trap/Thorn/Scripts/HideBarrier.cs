using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBarrier : Barrier
{
    public GameObject buttonObj;

    public override void TakeBarrier()
    {
        buttonObj.SetActive(true);
        gameObject.SetActive(false);
    }
}