using System;
using UnityEngine;

namespace EMPE {
    public partial class EntityMovementPathController : MonoBehaviour {
        public void StartMovement() {
            SetCurNode(0);
            IsMoving = true;
            if (StartMovementEventHandler != null)
                StartMovementEventHandler(this, null);
        }

        public void PauseMovement() {
            IsMoving = false;
        }

        public void ContinueMovement() {
            IsMoving = true;
        }

        public void StopMovement() {
            IsMoving = false;
            if (MovementFinishEventHandler != null)
                MovementFinishEventHandler(this, null);
            SetCurNode(0);
        }

        public void MoveToNode(int nodeIndex) {
            if (!CheckNodeIndex(nodeIndex)) return;

            if (nodeIndex == 0)
                nodeIndex = PathNodeList.Count - 1;
            else
                CurNodeIndex = nodeIndex - 1;
            strategy.NextPathNode();
        }

        public void SetCurNode(int nodeIndex) {
            if (!CheckNodeIndex(nodeIndex)) return;
            CurNodeIndex = nodeIndex;
            PathNodeData nodeData = PathNodeList[nodeIndex];
            transform.position = OriginalPos + nodeData.Position;
            transform.rotation = Quaternion.Euler(nodeData.Rotation);
            transform.localScale = nodeData.Scale;

            strategy.InitTargetNode();

            if (ArrivePathNodeEventHandler != null)
                ArrivePathNodeEventHandler(this, PathNodeList[nodeIndex]);

            strategy.NextPathNode();
        }

        public void SetOriginalPos(Vector3 originalPos) {
            OriginalPos = originalPos;
            MoveToNode(0);
        }
        public void SetOriginalRoa(Vector3 originalRoa) {
            OriginalRoa = originalRoa;
            MoveToNode(0);
        }

        public int GetNodeIndex(PathNodeData data) {
            int nodeIndex = -1;
            try {
                nodeIndex = PathNodeList.IndexOf(data);
                return nodeIndex;
            } catch (Exception e) {
                Debug.LogError("EMP_Error> 控制器:" + this + "找不到节点数据:" + data + '\n' + e.Message);
                return -1;
            }
        }

        public bool CheckNodeIndex(int nodeIndex) {
            if (nodeIndex < 0 || nodeIndex >= PathNodeList.Count) {
                Debug.LogError("EMPC_Error> 节点索引溢出:" + nodeIndex);
                return false;
            }
            return true;
        }

        public void SetPathShape(PathShapes pathShape) {
            switch (pathShape) {
                case PathShapes.Line:
                    this.strategy = new EMPE_LineMS(this);
                    break;
                case PathShapes.BezierCurve:
                    this.strategy = new EMPE_BezierCurveMS(this);
                    break;
            }
        }
    }
}
