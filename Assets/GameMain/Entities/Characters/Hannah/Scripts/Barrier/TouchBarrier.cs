using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchBarrier : MonoBehaviour
{
    [SerializeField]
    public class BarrierEvent : UnityEvent { }
    [SerializeField]
    public class HealthEvent : UnityEvent<TouchBarrier> { }

    public BarrierEvent OnTakeBarrier;//碰到障碍
    public HealthEvent OnHealthSet;//UI
    public int currentHealth;
    public float invulnerabilityDuration = 3f;//无敌时间
    float inulnerabilityTimer;
    bool invulnerable;

    public bool Invulnerable { get => invulnerable; }

    private void Start()
    {
        OnTakeBarrier = new BarrierEvent();
        OnHealthSet = new HealthEvent();
    }

    void Update()
    {
        if (invulnerable)
        {
            inulnerabilityTimer -= Time.deltaTime;

            if (inulnerabilityTimer <= 0f)
            {
                Debug.Log("Uninvulnerable");
                invulnerable = false;
            }
        }
    }

    public void TakeBarrier(Barrier barrier)
    {
        //Debug.Log("TouchBarrier");
        if (!invulnerable && barrier.damage > 0)
        {  
            currentHealth -= barrier.damage;
            Debug.Log("角色受到" + barrier.damage + "点伤害,剩余血量：" + currentHealth);
            OnHealthSet.Invoke(this);
        }

        OnTakeBarrier.Invoke();

        if (currentHealth <= 0)
        {
            //Die
        }

    }

    public void EnableInvulnerable()
    {
        invulnerable = true;
        inulnerabilityTimer = invulnerabilityDuration;
    }

    public void DisableInvulnerability()
    {
        invulnerable = false;
    }

}
