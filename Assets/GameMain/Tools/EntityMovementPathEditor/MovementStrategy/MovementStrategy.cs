using EMPE;
using UnityEngine;

public interface MovementStrategy {
    public void InitTargetNode();
    public void NextPathNode();
    public Vector3 GetNextLerpPosition(float t);
    public PathNodeData GetTargetNodeData();
    public PathNodeData GetOriginalNodeData();
}
