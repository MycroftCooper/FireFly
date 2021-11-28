using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Vector3 a = new Vector3(1,2,3);
    Vector2 b;
    // Start is called before the first frame update
    void Start()
    {
        b = a;
        Debug.Log("b:" + b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
