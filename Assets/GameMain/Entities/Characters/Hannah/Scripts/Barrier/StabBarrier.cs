using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabBarrier : Barrier
{
    public Transform initPos;

    public override void TakeBarrier()
    {
        if (tBarrier != null)
        {
            tBarrier.EnableInvulnerable();
            // ‹…À∂Øª≠
            StartCoroutine(OnTrap());
        }
    }

    private IEnumerator OnTrap()
    {
        body.velocity = Vector2.zero;
        body.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(0.5f);
        tBarrier.GetComponent<Transform>().position = initPos.position;
        body.bodyType = RigidbodyType2D.Dynamic;
    }
}