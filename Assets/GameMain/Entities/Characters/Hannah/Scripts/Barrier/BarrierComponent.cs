using UnityEngine;
using UnityEngine.Events;

public class BarrierComponent : MonoBehaviour {
    [SerializeField]
    public class BarrierEvent : UnityEvent { }
    [SerializeField]
    public class HealthEvent : UnityEvent<BarrierComponent> { }

    public bool IsHannah;
    HannahStatusController hsc;
    public BarrierEvent OnTakeBarrier;//碰到障碍
    public HealthEvent OnHealthSet;//UI
    public float invulnerabilityDuration = 3f;//无敌时间
    float inulnerabilityTimer;
    bool invulnerable;

    public bool Invulnerable { get => invulnerable; }

    private void Start() {
        OnTakeBarrier = new BarrierEvent();
        OnHealthSet = new HealthEvent();
        hsc = gameObject.GetComponent<HannahStatusController>();
        if (hsc == null) IsHannah = false;
        else IsHannah = true;
    }

    void Update() {
        if (invulnerable) {
            inulnerabilityTimer -= Time.deltaTime;

            if (inulnerabilityTimer <= 0f) {
                Debug.Log("Uninvulnerable");
                invulnerable = false;
            }
        }
    }

    public void TakeBarrier(Barrier barrier) {
        //Debug.Log("TouchBarrier");
        if (IsHannah && !invulnerable && barrier.damage > 0) {
            hsc.HP -= barrier.damage;
            Debug.Log("角色受到" + barrier.damage + "点伤害,剩余血量：" + hsc.HP);
            OnHealthSet.Invoke(this);
            if (hsc.HP <= 0) {
                //Die
            }
        }
        OnTakeBarrier.Invoke();
    }

    public void EnableInvulnerable() {
        invulnerable = true;
        inulnerabilityTimer = invulnerabilityDuration;
    }

    public void DisableInvulnerability() {
        invulnerable = false;
    }

}
