using UnityEngine;
[System.Serializable]
public class MomentData{
    public string id;
    public Vector3 relativePos;
    public Vector3 scale;
    public Quaternion rotation;
    public Vector3 velocity;
    public Entity entity;
    public Transform parent;
}