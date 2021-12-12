using UnityEngine;

public class HannahStatusController : Entity {
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public enum MotionStates { Standing, Resting, Running, Jumping, DoubleJumping, Falling, TakingPhoto, Dying };
    public enum CharacterFacings { Left, Right };

    private MotionStates motionState;
    public MotionStates MotionState {
        get {
            motionState = (MotionStates)animator.GetInteger("MotionState");
            return motionState;
        }
        set {
            if (motionState == value)
                return;
            animator.SetInteger("MotionState", (int)value);
            motionState = (MotionStates)animator.GetInteger("MotionState");
        }
    }
    private CharacterFacings facing;
    public CharacterFacings Facing {
        get => facing;
        set {
            if (facing == value || MotionState == MotionStates.Dying)
                return;
            facing = value;
            if (facing == CharacterFacings.Left)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
    }

    public bool IsResting;
    public bool IsTakingPhoto;
    public int RestingTimeLimit;
    private int restingTime;
    public int RestingTime {
        get => restingTime;
        set {
            restingTime = value;
            if (restingTime >= RestingTimeLimit) {
                MotionState = MotionStates.Resting;
                IsResting = true;
            }
        }
    }

    public int hp;
    public int HP {
        get => hp;
        set {
            if (value <= 0) {
                value = 0;
                MotionState = MotionStates.Dying;
            }
            if (value > HPUpper)
                hp = HPUpper;
            hp = value;
        }
    }
    public int HPUpper;
    public int MP;
    public int MPUpper;
    public TileDatas TileData;

    void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Facing = CharacterFacings.Right;
        restingTime = 0;
        IsResting = false;
        IsTakingPhoto = false;
        HPUpper = 3;
        MPUpper = 5;
        MP = MPUpper;
        TileData = new TileDatas(transform);
    }
}
