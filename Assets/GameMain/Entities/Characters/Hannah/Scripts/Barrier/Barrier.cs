using UnityEngine;

public abstract class Barrier : MonoBehaviour
{
    public int damage = 0;

    protected BarrierComponent tBarrier = null;
    protected bool continueContact = false;
    protected Rigidbody2D body;
    protected Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("BarrierEnter");
        if (collision.gameObject.GetComponent<BarrierComponent>())
        {
            tBarrier = collision.gameObject.GetComponent<BarrierComponent>();
            AddEvent(tBarrier);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("BarrierExit");
        if (collision.gameObject.GetComponent<BarrierComponent>())
        {
            ReleaseEvent();
        }
    }

    public abstract void TakeBarrier();

    public virtual void TakeOffBarrier()
    { }

    public void AddEvent(BarrierComponent tBarrier)
    {
        tBarrier.OnTakeBarrier.AddListener(TakeBarrier);
        body = tBarrier.GetComponent<Rigidbody2D>();
        tBarrier.TakeBarrier(this);
    }

    public void ReleaseEvent()
    {
        tBarrier.OnTakeBarrier.RemoveListener(TakeBarrier);
        tBarrier = null;
        TakeOffBarrier();
    }
}