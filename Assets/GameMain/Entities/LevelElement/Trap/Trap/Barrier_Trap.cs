using UnityEngine;

public class Barrier_Trap : Barrier {
    public float trapTime;
    float offsetY;

    void Update() {
        if (IsInvoking()) {
            body.velocity = Vector2.zero;
            tBarrier.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, 0);
        }
    }

    public override void TakeBarrier() {
        if (tBarrier != null) {
            offsetY = body.transform.position.y - transform.position.y;
            tBarrier.gameObject.GetComponent<HannahMotionController>().CanMove = false;
            animator.SetBool("TrapTouched", true);
            Invoke("releaseTrap", trapTime);
        }

    }

    private void releaseTrap() {
        animator.SetBool("TrapTouched", false);
        tBarrier.gameObject.GetComponent<HannahMotionController>().CanMove = true;
    }
}
