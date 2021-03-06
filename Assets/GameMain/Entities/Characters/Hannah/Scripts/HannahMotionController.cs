using UnityEngine;
using static HannahStatusController;

/// <summary>
/// 女主角汉娜的运动控制器
/// </summary>
public class HannahMotionController : Entity {
    private bool canMove;
    public bool CanMove {
        get => canMove;
        set {
            if (value == false)
                if (hsc.HP != 0)
                    hsc.MotionState = MotionStates.Standing;
            canMove = value;
        }
    }
    HannahStatusController hsc;
    public float speed = 10;

    [Space]
    [Header("Collision")]
    public bool isDebug;
    public Color DebugColor = Color.red;
    public LayerMask groundLayer;
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;

    [Space]
    [Header("BetterJumping")]
    public ParticleSystem jumpParticle;
    public int MaxJumpTimes;
    public int JumpTimes;
    public float jumpForce = 50;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Vector2 dir;
    private Vector2 dirRow;
    private Vector2 Sensitivity;
    private bool jumpFlag;

    void Start() {
        hsc = GetComponent<HannahStatusController>();
        rb = GetComponent<Rigidbody2D>();
        jumpParticle = GetComponent<ParticleSystem>();
        JumpTimes = 0;
        jumpFlag = false;
        Sensitivity = new Vector2(0.1f, 0.1f);
        CanMove = true;
    }

    void Update() {
        if (!CanMove) return;
        axisInput();
        collisionDetection();
        doMotion();
        userInput();
    }

    // 用户虚拟轴输入
    void axisInput() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        dir = new Vector2(x, y);
        dirRow = new Vector2(xRaw, yRaw);
    }
    void userInput() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (!hsc.IsTakingPhoto) {
                hsc.MotionState = MotionStates.TakingPhoto;
                hsc.IsTakingPhoto = true;
            } else {
                hsc.IsTakingPhoto = false;
                hsc.MotionState = MotionStates.Standing;
            }

        }

    }

    // 碰撞检测
    void collisionDetection() {
        if (transform.position.y < -10)
            hsc.MotionState = MotionStates.Dying;
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onWall = onRightWall || onLeftWall;
    }

    void doMotion() {
        if (hsc.MotionState == MotionStates.Dying)
            return;
        if (dirRow.y != 1)
            jumpFlag = true;

        // 无输入操作且在地面
        if (onGround && !hsc.IsTakingPhoto) {
            if (dir == Vector2.zero && dirRow == Vector2.zero) {
                hsc.MotionState = MotionStates.Standing;
                JumpTimes = 0;
                hsc.RestingTime++;
                return;
            }
            if (hsc.MotionState == MotionStates.Falling || hsc.MotionState == MotionStates.Jumping) {
                hsc.MotionState = MotionStates.Standing;
                JumpTimes = 0;
            }
        }

        hsc.RestingTime = 0;
        if (dir.x != 0)
            run();
        if (dirRow.y == 1 && jumpFlag) {
            jump();
            jumpFlag = false;
        }
        // 不在地面
        if (!onGround) {
            // 上升期
            if (rb.velocity.y > 0) {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                return;
            }
            // 下降期
            if (rb.velocity.y < 0) {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                if (hsc.HP != 0)
                    hsc.MotionState = MotionStates.Falling;
                return;
            }
        }

        if (rb.velocity.x < Sensitivity.x && rb.velocity.y < rb.velocity.y)
            hsc.MotionState = MotionStates.Standing;
    }
    private void jump() {
        if (hsc.IsResting || !jumpFlag) return;
        if (JumpTimes >= MaxJumpTimes)
            return;
        if (onGround) {
            hsc.MotionState = MotionStates.Jumping;
            JumpTimes = 1;
            jumpFlag = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            return;
        }
        if (jumpFlag) {
            JumpTimes++;
            hsc.MotionState = MotionStates.DoubleJumping;
            jumpParticle.Play();
            jumpFlag = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    private void run() {
        if (hsc.IsResting || hsc.IsTakingPhoto) return;
        if (dir.x < 0 && onLeftWall || (dir.x > 0 && onRightWall)) return;
        if (dir.x > 0)
            hsc.Facing = CharacterFacings.Right;
        if (dir.x < 0)
            hsc.Facing = CharacterFacings.Left;
        if (hsc.MotionState == MotionStates.Resting ||
            hsc.MotionState == MotionStates.Dying)
            return;
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        if (onGround)
            hsc.MotionState = MotionStates.Running;
    }

    void OnDrawGizmos() {
        if (!isDebug)
            return;
        Gizmos.color = DebugColor;
        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
