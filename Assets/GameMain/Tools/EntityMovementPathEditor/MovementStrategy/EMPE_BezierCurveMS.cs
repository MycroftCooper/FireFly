using EMPE;
using UnityEngine;

public class EMPE_BezierCurveMS : MovementStrategy {
    EntityMovementPathController empc;

    private Vector3 curOriginalPos;
    private Vector3 curTargetPos;

    private Vector3 referenceP0;
    private Vector3 referenceP1;

    public EMPE_BezierCurveMS(EntityMovementPathController empc) {
        this.empc = empc;
    }

    public void InitTargetNode() {
        PathNodeData targetNode = GetTargetNodeData();
        if (targetNode == null) return;

        curOriginalPos = empc.transform.position;
        curTargetPos = empc.OriginalPos + targetNode.Position;

        referenceP0 = empc.OriginalPos + empc.PathNodeList[empc.CurNodeIndex + 1].Position;
        referenceP1 = empc.OriginalPos + empc.PathNodeList[empc.CurNodeIndex + 2].Position;
    }

    public void NextPathNode() {
        empc.CurNodeIndex += 3;
        if (empc.CurNodeIndex >= empc.PathNodeList.Count) {
            if (empc.IsLoop) {
                empc.CurNodeIndex = 0;
            } else {
                empc.IsMoving = false;
                if (empc.MovementFinishEventHandler != null)
                    empc.MovementFinishEventHandler(this, null);
                return;
            }
        }
        InitTargetNode();
    }

    public Vector3 GetNextLerpPosition(float t) {
        if (t >= 1 || t <= 0) {
            Debug.LogError("EMP_Error: ±´Èû¶ûÇúÏß²ÎÊý´íÎó:" + t.ToString());
            return Vector3.zero;
        }

        Vector3 p0 = curOriginalPos;
        Vector3 p1 = referenceP0;
        Vector3 p2 = referenceP1;
        Vector3 p3 = curTargetPos;
        Vector3 output = new Vector3();

        Vector3 p0_p1 = Vector3.Lerp(p0, p1, t);
        Vector3 p1_p2 = Vector3.Lerp(p1, p2, t);
        Vector3 p2_p3 = Vector3.Lerp(p2, p3, t);
        Vector3 p01_p12 = Vector3.Lerp(p0_p1, p1_p2, t);
        Vector3 p12_p23 = Vector3.Lerp(p1_p2, p2_p3, t);
        output = Vector3.Lerp(p01_p12, p12_p23, t);
        return output;
    }

    public PathNodeData GetTargetNodeData() {
        if (empc.IsLoop && empc.CurNodeIndex == empc.PathNodeList.Count - 3)
            return empc.PathNodeList[0];
        if (!empc.CheckNodeIndex(empc.CurNodeIndex + 3)) return null;
        return empc.PathNodeList[empc.CurNodeIndex + 3];
    }

    public PathNodeData GetOriginalNodeData() {
        PathNodeData originalNodeData = new PathNodeData();
        empc.PathNodeList[empc.CurNodeIndex].Copy(ref originalNodeData);
        originalNodeData.Position = curOriginalPos - empc.OriginalPos;
        return originalNodeData;
    }
}
