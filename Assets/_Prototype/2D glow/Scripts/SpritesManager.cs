using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesManager : MonoBehaviour
{
    public static SpritesManager spritesManager;

    public SpriteSetting setting;
    List<Sprites> sprites = new List<Sprites>();

    private void Awake()
    {
        if (spritesManager == null)
        {
            spritesManager = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Sprites[] FindALLSprites = FindObjectsOfType<Sprites>();
        foreach(Sprites child in FindALLSprites)
        {
            sprites.Add(child);
            child.Initialize(setting, TargetPoint());
        }
    }

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

    //ÊµÊ±ÐÞ¸ÄÓ©»ð³æµÄÒÆ¶¯×´Ì¬
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
             s.target = TargetPoint();

        }
        s.numPerceivedFlockmates = companionNum;
    }

    //¸úËæÊó±ê
    Vector2 TargetPoint()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        return mouseWorldPos;
    }

    //¼ÓÈëÓ©»ð³æ
    public void AddSprite(Sprites s)
    {
        sprites.Add(s);
        s.Initialize(setting, TargetPoint());
    }

    //É¾³ýÓ©»ð³æ
    public void DeleteSprite(Sprites s)
    {
        sprites.Remove(s);
    }
}
