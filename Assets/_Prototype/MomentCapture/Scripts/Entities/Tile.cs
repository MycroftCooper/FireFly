using UnityEngine;

public class Tile : Entity
{
    public LayerMask detectLayer;
    [SerializeField] private bool isDetect;
    public bool OnRight;
    public bool OnLeft;
    public bool OnUp;
    public bool Ondown;
    [SerializeField] private Vector2 LRCenter = new Vector2(0.5f, 0);
    [SerializeField] private Vector2 UDCenter = new Vector2(0, 0.5f);
    [SerializeField] private Vector2 LRSize = Vector2.one;
    [SerializeField] private Vector2 UDSize = Vector2.one;
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log(other.contacts);
        }
    }

    public void CheckAllContact()
    {
        Vector3 lCenter = new Vector2(transform.position.x - LRCenter.x, transform.position.y + LRCenter.y);
        Vector3 rCenter = new Vector2(transform.position.x + LRCenter.x, transform.position.y + LRCenter.y);
        Vector3 uCenter = new Vector2(transform.position.x + UDCenter.x, transform.position.y + UDCenter.y);
        Vector3 dCenter = new Vector2(transform.position.x + UDCenter.x, transform.position.y - UDCenter.y);
        OnRight = CheckContact(rCenter, LRSize);
        OnLeft = CheckContact(lCenter, LRSize);
        OnUp = CheckContact(uCenter, UDSize);
        Ondown = CheckContact(dCenter, UDSize);
    }

    public bool CheckContact(Vector2 center, Vector2 size)
    {
        if (MomentManager.instance.moments == null)
            return false;
        Collider2D coll = Physics2D.OverlapBox(center, size, 0, detectLayer);
        if (coll == null)
            return false;
        Entity contactEntity = coll.GetComponent<Entity>();
        foreach (var item in MomentManager.instance.moments)
        {
            if (item.entity == contactEntity)
                return false;
        }
        return true;
    }
    public override void CatchCallBack() => isDetect = true;
    public override void UncatchCallBack() => isDetect = false;
    public override bool GetConstraitData(string name)
    {
        if (name == "OnRight")
            return OnRight;
        else if (name == "OnLeft")
            return OnLeft;
        else if (name == "OnUp")
            return OnUp;
        else if (name == "OnDown")
            return Ondown;
        return true;
    }

    private void Update()
    {
        if (!isDetect)
            return;
        CheckAllContact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 lCenter = new Vector2(transform.position.x - LRCenter.x, transform.position.y + LRCenter.y);
        Vector3 rCenter = new Vector2(transform.position.x + LRCenter.x, transform.position.y + LRCenter.y);
        Vector3 uCenter = new Vector2(transform.position.x + UDCenter.x, transform.position.y + UDCenter.y);
        Vector3 dCenter = new Vector2(transform.position.x + UDCenter.x, transform.position.y - UDCenter.y);
        Gizmos.DrawWireCube(lCenter, LRSize);
        Gizmos.DrawWireCube(rCenter, LRSize);
        Gizmos.DrawWireCube(uCenter, UDSize);
        Gizmos.DrawWireCube(dCenter, UDSize);
    }
}