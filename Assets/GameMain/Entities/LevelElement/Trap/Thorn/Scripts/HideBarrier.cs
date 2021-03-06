using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBarrier : Barrier
{
    public GameObject topObj;
    public GameObject buttonObj;

    public override void TakeBarrier()
    {
        buttonObj.SetActive(true);
        topObj.SetActive(false);
    }

    public void OnInit()
    {
        buttonObj.SetActive(false);
        topObj.SetActive(true);
    }
}