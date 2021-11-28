using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 基础2D移动，水平方向因为不想有惯性所以先直接Translate
/// </summary>

public class PlayerPhysic : Entity
{
    private Collision coll;
    
    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public ParticleSystem jumpParticle;



    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, 0);


        if (Input.GetButtonDown("Jump") && coll.onGround)
        {
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpForce);
        }

        if ((dir.x < 0 && coll.onLeftWall) || (dir.x > 0 && coll.onRightWall))
            return;
        this.transform.Translate(dir * speed * Time.deltaTime);
    }
}
