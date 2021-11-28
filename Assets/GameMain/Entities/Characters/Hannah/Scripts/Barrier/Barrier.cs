using UnityEngine;

public abstract class Barrier : Entity {
    public int damage = 0;

    protected BarrierComponent tBarrier = null;
    protected bool continueContact = false;
    protected Rigidbody2D body;
    protected Animator animator;
    private void Start() {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log("BarrierEnter");
        if (collision.gameObject.GetComponent<BarrierComponent>()) {
            tBarrier = collision.gameObject.GetComponent<BarrierComponent>();
            tBarrier.OnTakeBarrier.AddListener(TakeBarrier);
            body = tBarrier.GetComponent<Rigidbody2D>();
            tBarrier.TakeBarrier(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //Debug.Log("BarrierExit");
        if (collision.gameObject.GetComponent<BarrierComponent>()) {
            tBarrier.OnTakeBarrier.RemoveListener(TakeBarrier);
            tBarrier = null;
            TakeOffBarrier();
        }
    }

    public abstract void TakeBarrier();
    public virtual void TakeOffBarrier() { }
}
