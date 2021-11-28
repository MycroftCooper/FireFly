using UnityEngine;

public interface ICapturable
{
    MomentData GetMoment(Transform camTrans);
    bool IsCatchable();
    bool IsRotatable();
    void CatchCallBack();
    void UncatchCallBack();
}