using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesManager : MonoBehaviour
{
    public static SpritesManager spritesManager;

    public SpriteSetting setting;
    public Transform playePos;
    public List<Sprites> capturedSprites = new List<Sprites>(); 
    public List<Sprites> sprites = new List<Sprites>();

    public int capturedNum=0; //ӵ��ө��������

    private void Awake()
    {
        if (spritesManager == null)
        {
            spritesManager = this;
        }
        else
            Destroy(gameObject);
    }

    /*
    private void Start()
    {
        Sprites[] FindALLSprites = FindObjectsOfType<Sprites>();
        foreach(Sprites child in FindALLSprites)
        {
            sprites.Add(child);
            child.Initialize(setting, TargetPoint(playePos));
        }
    }
*/

    private void Update()
    {
        if(sprites != null)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                ChangeStateOfSprite(sprites[i]);
//                sprites[i].numPerceivedFlockmates = sprites.Count;

                sprites[i].UpdateSpriteState();
            }
        }
    }

    //ʵʱ�޸�ө�����ƶ�״̬
    void ChangeStateOfSprite(Sprites s)
    {
        int companionNum = 0;
        for (int i = 0; i<sprites.Count; i++)
        {
            if(s.gameObject.GetInstanceID() != sprites[i].gameObject.GetInstanceID())
            {
                Vector2 offset = sprites[i].position - s.position;
                float sqrDistance = offset.x * offset.x + offset.y * offset.y;

                if(sqrDistance < setting.perceptionRadius * setting.perceptionRadius)
                {
                    companionNum += 1;
                    s.avgFlockHeading += sprites[i].direction;
                    s.centreOfFlockmates += new Vector2(sprites[i].position.x, sprites[i].position.y);

                    if(sqrDistance < setting.avoidanceRadius * setting.avoidanceRadius)
                    {
                        s.avgAvoidanceHeading -= offset / sqrDistance;
                    }
                }
            }
             s.target = TargetPoint(playePos);

        }
        s.numPerceivedFlockmates = companionNum;
    }

    //����Ŀ��
    Vector2 TargetPoint(Transform playerPostion)
    {
 //       Vector3 mousePosition = Input.mousePosition;
 //       Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        return playerPostion.position;
    }
    #region �������ʱ��Ч��
    //����ө���
    public void AddSprite(Sprites s)
    {
        sprites.Add(s);
        s.Initialize(setting, TargetPoint(playePos));
    }

    //ɾ��ө���
    public void DeleteSprite(Sprites s)
    {
        sprites.Remove(s);
    }
    #endregion

    //�Ƿ��Ѿ���׽����
    public bool IsMySprite(Sprites s)
    {
        if (capturedSprites.Contains(s))
            return true;
        else
            return false;
    }

    //��׽ө���ķ���
    public void Capture(Sprites s)
    {
        sprites.Add(s);
        capturedSprites.Add(s);
        s.Initialize(setting, TargetPoint(playePos));
        capturedNum++;
    }

}
