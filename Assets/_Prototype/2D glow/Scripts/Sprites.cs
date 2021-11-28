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
    public Vector2 avgFlockHeading; //Ⱥ�巽��
    [HideInInspector]
    public Vector2 avgAvoidanceHeading;
    [HideInInspector]
    public Vector2 centreOfFlockmates; //Ⱥ������
    [HideInInspector]
    public int numPerceivedFlockmates; //Ⱥ������

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

    //ǰ���Ƿ����ϰ�
    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics2D.CircleCast(position, settings.boundsRadius, direction, settings.collisionAvoidDst, settings.obstacleMask))
        {
            return true;
        }

        return false;
    }

    //��Χ�Ŀ���ײ����
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

    //ת��,�����ٶȷ���
    Vector2 SteerTowards(Vector2 vector) { 
        Vector2 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector2.ClampMagnitude(v, settings.maxSteerForce);
    }

    //ө����Ƿ��ڿ��ӷ�Χ��
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

    //ө�������
    public void DestroySprite()
    {
        Destroy(this.gameObject);
    }

    #region ʵ������ӿ�
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

