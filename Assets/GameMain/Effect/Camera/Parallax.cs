using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform camTrans;
    private Vector3 lastCamTrans;

    public Transform background;

    public float maxDepth; //背景最大深度

    //偏移量
    public float moveINX;
    public float moveINY;

    // Start is called before the first frame update
    void Start()
    {
        camTrans = Camera.main.transform;
        lastCamTrans = camTrans.transform.position;
        if(maxDepth == 0f && background.childCount != 0)
        {
            foreach(Transform child in background)
            {
                if(child.position.z >= maxDepth)
                {
                    maxDepth = child.position.z;
                }
            }
        }
        else
        {
            maxDepth = 100f;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        float diff = camTrans.position.x - lastCamTrans.x;

        Vector3 Pmove = camTrans.position - lastCamTrans;

        lastCamTrans = camTrans.position;

        Pmove.x = Pmove.x * moveINX;
        Pmove.y = Pmove.y * moveINY;

        foreach(Transform child in background)
        {
            Vector3 backMove = child.position + Pmove * (child.position.z / maxDepth);
            child.position = backMove;
        }


    }
}
