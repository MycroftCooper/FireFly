using UnityEngine;

namespace EMPE {
    public class EMPE_LineMS : MovementStrategy {
        EntityMovementPathController empc;

        private Vector3 curOriginalPos;
        private Vector3 curTargetPos;

        public EMPE_LineMS(EntityMovementPathController empc) {
            this.empc = empc;
        }

        public void InitTargetNode() {
            PathNodeData targetNode = GetTargetNodeData();
            if (targetNode == null) return;

            curOriginalPos = empc.transform.position;
            curTargetPos = empc.OriginalPos + targetNode.Position;
        }

        public void NextPathNode() {
            empc.CurNodeIndex++;
            if (empc.CurNodeIndex == empc.PathNodeList.Count) {
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
            Vector3 output = Vector3.Lerp(curOriginalPos, curTargetPos, t);
            return output;
        }

        public PathNodeData GetTargetNodeData() {
            if (empc.IsLoop && empc.CurNodeIndex == empc.PathNodeList.Count - 1)
                return empc.PathNodeList[0];
            if (!empc.CheckNodeIndex(empc.CurNodeIndex + 1)) return null;
            return empc.PathNodeList[empc.CurNodeIndex + 1];
        }

        public PathNodeData GetOriginalNodeData() {
            PathNodeData originalNodeData = new PathNodeData();
            empc.PathNodeList[empc.CurNodeIndex].Copy(ref originalNodeData);
            originalNodeData.Position = curOriginalPos - empc.OriginalPos;
            return originalNodeData;
        }
    }
}
