using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidParticle : Entity
{
    public override MomentData GetMoment(Transform camTrans)
    {
        MomentData data = new MomentData();
        data.id = this.gameObject.name;
        data.relativePos = this.transform.position - camTrans.position;
        data.rotation = Quaternion.identity;
        data.scale = this.transform.localScale;
        data.entity = (this as Entity);
        data.velocity = rb.velocity;
        return data;
    }
    public override bool IsCatchable() => false;
    public override bool IsRotatable() => false;
    public override void CatchCallBack() => rb.isKinematic = true;
    public override void UncatchCallBack() => rb.isKinematic = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CapCam"))
        {
            this.rb.gravityScale = 0.1f;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CapCam"))
        {
            this.rb.gravityScale = 3f;
        }
    }
}
