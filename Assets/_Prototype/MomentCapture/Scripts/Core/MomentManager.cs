using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MomentManager : MonoBehaviour
{
    public static MomentManager instance;
    private void Awake()
    {
        if(instance ==null)
            instance = this;
    }
    [SerializeField] public List<MomentData> moments = new List<MomentData>();
    public int momentMax = 9; // 最大实体数
    // public void CaptureMoment(Collider2D[] colliders)
    // {
    //     for (int i = 0; i < colliders.Length; i++)
    //     {
    //         Transform capturable = colliders[i].gameObject.GetComponent<Transform>();
    //     }
    // }

    public void AddMoment(MomentData moment)
    {
        if (moments.Count < momentMax)
            moments.Add(moment);
    }

    public void RemoveMoment(int index)
    {
        if (moments.Count > 0 && moments.Count < momentMax)
            moments.RemoveAt(index);
    }

    public void Clear()
    {
        if (moments != null)
            moments.Clear();
    }



}