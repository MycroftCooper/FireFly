using UnityEngine;

public class Barrier_Spring : Barrier {
    public float force = 10;


    public override void TakeBarrier() {
        Vector2 forceMotion = ForceUtility.calDirectionString(this.transform.rotation.eulerAngles.z, force);
        animator.SetBool("OnSpring", true);
        tBarrier.gameObject.GetComponent<Rigidbody2D>().AddForce(forceMotion);
    }
    public override void TakeOffBarrier() {
        base.TakeOffBarrier();
        animator.SetBool("OnSpring", false);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (tBarrier != null) {
            tBarrier.TakeBarrier(this);
        }
    }

}
