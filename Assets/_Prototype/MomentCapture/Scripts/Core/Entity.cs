using UnityEngine;
/// <summary>
/// 能被捕捉的运动实体继承GameEntity（条件：需要rigidbody，受UnityPhysic影响）
/// 所有数据通过Icapture接口传递
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour, ICapturable
{
    public Rigidbody2D rb; // 由具体类设置，避免层级分离的情况
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        OnStart();
    }

    protected virtual void OnStart() { }
    public virtual MomentData GetMoment(Transform camTrans)
    {
        MomentData data = new MomentData();
        data.id = this.gameObject.name;
        data.relativePos = this.transform.position - camTrans.position;
        data.rotation = this.transform.rotation;
        data.scale = this.transform.localScale;
        data.entity = (this as Entity);
        data.velocity = this.rb.velocity;
        data.parent = this.transform.parent;
        return data;
    }

    public virtual bool IsCatchable() => true;
    public virtual bool IsRotatable() => true;
    public virtual void CatchCallBack() {  }
    public virtual void UncatchCallBack() {  }
    public virtual bool GetConstraitData(string name) { return false; }
}