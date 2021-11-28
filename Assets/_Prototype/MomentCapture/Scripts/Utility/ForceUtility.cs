using UnityEngine;
public static class ForceUtility
{
    // 根据旋转角度计算作用方向，之后可以改成运算形式
    public static Vector2 calDirection(float zRotate, float force)
    {

        Debug.Log(zRotate);
        Vector2 forceMotion = Vector2.zero;
        if (Mathf.Abs(zRotate - (-90)) % 360 < 0.1f)
            forceMotion = new Vector2(force, 0);
        else if (Mathf.Abs(zRotate) % 360 < 0.1f)
            forceMotion = new Vector2(0, force);
        else if (Mathf.Abs(zRotate - 90) % 360 < 0.1f)
            forceMotion = new Vector2(-force, 0);
        else if (Mathf.Abs(zRotate - 180) % 360 < 0.1f)
            forceMotion = new Vector2(0, -force);
        Debug.Log("Agent Force Motion: " + Mathf.Abs(zRotate) % 360);
        return forceMotion;
    }
    // 根据旋转角度计算作用方向，之后可以改成运算形式（弹簧）
    public static Vector2 calDirectionString(float zRotate, float force)
    {
        Vector2 forceMotion = Vector2.zero;
        if (Mathf.Abs(zRotate - (-90)) % 360 < 0.1f)
            forceMotion = new Vector2(force, force / 1.5f);
        else if (Mathf.Abs(zRotate) % 360 < 0.1f)
            forceMotion = new Vector2(0, force);
        else if (Mathf.Abs(zRotate - 90) % 360 < 0.1f)
            forceMotion = new Vector2(-force, force / 1.5f);
        else if (Mathf.Abs(zRotate - 180) % 360 < 0.1f)
            forceMotion = new Vector2(0, -force);
        return forceMotion;
    }
}