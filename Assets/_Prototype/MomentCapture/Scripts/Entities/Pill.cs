using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pill : Entity
{
    protected override void OnStart()
    {
        rb.isKinematic = true;
    }
    public override bool IsCatchable() => false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Agent")
        {
            this.transform.DOScale(Vector3.zero, 0.2f).OnComplete(()=>{
                Destroy(this.gameObject);
            });
        }
    }
}
