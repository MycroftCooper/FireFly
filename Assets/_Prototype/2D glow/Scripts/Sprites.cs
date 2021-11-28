using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : Entity, ICapturable
{
    SpriteSetting settings;

    public Vector2 position;
    public Vector2 direction;
    Vector2 velocity;

    Vector2 acceleration;
    [HideInInspector]
    public Vector2 avgFlockHeading; //群体方向
    [HideInInspector]
    public Vector2 avgAvoidanceHeading;
    [HideInInspector]
    public Vector2 centreOfFlockmates; //群体中心
    [HideInInspector]
    public int numPerceivedFlockmates; //群体数量

    Camera main;

//    Transform cachedTransform;
    public Vector2 target;
    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(SpriteSetting settings, Vector2 target)
    {
        this.target = target;
        this.settings = settings;

        position = transform.position;
        direction = transform.up;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    // Update is called once per frame
    public void UpdateSpriteState()
    {
        acceleration = Vector3.zero;

        if (target != null)
        {
            Vector2 offsetToTarget = (target - new Vector2(transform.position.x, transform.position.y));
            acceleration = SteerTowards(offsetToTarget) * settings.targetWeight;
        }

        if(numPerceivedFlockmates != 0)
        {
            centreOfFlockmates /= numPerceivedFlockmates;

            Vector2 offsetToFlockmatesCentre = (centreOfFlockmates - new Vector2(position.x,position.y));
            if (IsSpriteInCamera(main))
            {
                var alignmentForce = SteerTowards(avgFlockHeading) * settings.alignWeight;
                var cohesionForce = SteerTowards(offsetToFlockmatesCentre) * settings.cohesionWeight;
                var seperationForce = SteerTowards(avgAvoidanceHeading) * settings.seperateWeight;

                acceleration += alignmentForce;
                acceleration += cohesionForce;
                acceleration += seperationForce;
            }
        }

        if (IsHeadingForCollision())
        {
            Vector2 collisionAvoidDir = ObstacleAround();
            Vector2 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
            acceleration += collisionAvoidForce;
        }

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector2 dir = velocity / speed; 
        speed = Mathf.Clamp(speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;

        position += velocity * Time.deltaTime;
        direction = dir;

        transform.position = new Vector3(position.x,position.y,transform.position.z);
    }

    //前方是否有障碍
    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics2D.CircleCast(position, settings.boundsRadius, direction, settings.collisionAvoidDst, settings.obstacleMask))
        {
            return true;
        }

        return false;
    }

    //周围的可碰撞物体
    Vector2 ObstacleAround()
    {
        Vector2[] rayDirections = SpritesDetector.directions;

        for(int i =0; i< rayDirections.Length; i++)
        {

            Vector2 dir = transform.TransformDirection(rayDirections[i]);
            if(!Physics2D.CircleCast(position, settings.boundsRadius, dir, settings.collisionAvoidDst, settings.obstacleMask))
            {
                return dir;
            }
        }
        return direction;
    }

    //转向,返回速度方向
    Vector2 SteerTowards(Vector2 vector) { 
        Vector2 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector2.ClampMagnitude(v, settings.maxSteerForce);
    }

    //萤火虫是否在可视范围内
    public bool IsSpriteInCamera(Camera camera)
    {
        if(camera != null)
        {
            Vector3 ObjPosInCamera = camera.WorldToViewportPoint(transform.position);
            if(ObjPosInCamera.x > 0 && ObjPosInCamera.x < 1 && ObjPosInCamera.y > 0 && ObjPosInCamera.y < 1)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    //萤火虫死亡
    public void DestroySprite()
    {
        Destroy(this.gameObject);
    }

    #region 实现相机接口
    public override bool IsCatchable() => true;
    public override bool IsRotatable() => false;
    public override void CatchCallBack() 
    {
        SpritesManager.spritesManager.DeleteSprite(this);
    }
    public override void UncatchCallBack() 
    {
        SpritesManager.spritesManager.AddSprite(this);
    }
    #endregion

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < 20; i++)
        {
            Vector2 dir = SpritesDetector.directions[i];
            if (!Physics2D.CircleCast(position, settings.boundsRadius, dir, settings.collisionAvoidDst, settings.obstacleMask))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, SpritesDetector.directions[i] + new Vector2(transform.position.x,transform.position.y));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, SpritesDetector.directions[i] + new Vector2(transform.position.x, transform.position.y));
            }
        }
    }
    */
}

