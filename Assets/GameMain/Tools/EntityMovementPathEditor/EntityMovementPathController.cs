using System;
using System.Collections.Generic;
using UnityEngine;

namespace EMPE {
    [Serializable]
    public class PathNodeData : EventArgs {
        [SerializeField]
        public float ArrivalTime;
        [SerializeField]
        public Vector3 Position;
        [SerializeField]
        public Vector3 Rotation;
        [SerializeField]
        public Vector3 Scale;
        [SerializeField]
        public bool IsFlip;

        public void Copy(ref PathNodeData b) {
            b.ArrivalTime = this.ArrivalTime;
            b.Position = new Vector3(this.Position.x, this.Position.y, this.Position.z);
            b.Rotation = new Vector3(this.Rotation.x, this.Rotation.y, this.Rotation.z);
            b.Scale = new Vector3(this.Scale.x, this.Scale.y, this.Scale.z);
            b.IsFlip = this.IsFlip;
        }
    }
    public partial class EntityMovementPathController : MonoBehaviour {
        public enum PathShapes { Line, BezierCurve }
        public bool IsEditMode;
        public bool Is2D;
        public PathShapes PathShape;
        public bool IsLoop;
        public bool IsPhysical;
        public bool IsForced;
        public bool IsMoving;
        public int CurNodeIndex;

        [SerializeField]
        public List<PathNodeData> PathNodeList;

        public Vector3 OriginalPos { get; private set; }
        public Vector3 OriginalRoa { get; private set; }


        public EventHandler StartMovementEventHandler;
        public EventHandler MovementFinishEventHandler;
        public EventHandler ArrivePathNodeEventHandler;

        public Rigidbody rb3D;
        public Rigidbody2D rb2D;

        private MovementStrategy strategy;

        void Start() {
            OriginalPos = transform.position;
            OriginalRoa = transform.rotation.eulerAngles;
            if (IsPhysical) loadRigbody();
            SetPathShape(PathShape);
            StartMovement();
        }
        private void Update() {
            if (IsMoving) moving();
        }
        private void FixedUpdate() {
            if (IsMoving) moving();
        }

        private void loadRigbody() {
            rb2D = gameObject.GetComponent<Rigidbody2D>();
            rb3D = gameObject.GetComponent<Rigidbody>();
            if (rb2D != null)
                Is2D = true;
            else if (rb3D != null)
                Is2D = false;
            else {
                if (Is2D) {
                    rb2D = gameObject.AddComponent<Rigidbody2D>();
                    rb2D.gravityScale = 0;
                    rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                } else {
                    rb3D = gameObject.AddComponent<Rigidbody>();
                    rb3D.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    rb3D.useGravity = false;
                }
            }
        }

        // 运动控制------------------------------------------------------

        private float curSeconds;//当前秒数

        private void moving() {
            curSeconds += Time.deltaTime;

            PathNodeData originalNodeData = strategy.GetOriginalNodeData();
            PathNodeData targetNodeData = strategy.GetTargetNodeData();

            float f = curSeconds / targetNodeData.ArrivalTime;
            if (f >= 1) {
                if (IsForced)
                    forcedReset(targetNodeData);
                strategy.NextPathNode();
                curSeconds = 0;
                if (ArrivePathNodeEventHandler != null)
                    ArrivePathNodeEventHandler(this, PathNodeList[CurNodeIndex]);
                return;
            }

            Vector3 nextPosition = strategy.GetNextLerpPosition(f);
            Vector3 nextRotation = OriginalRoa + Vector3.Lerp(originalNodeData.Rotation, targetNodeData.Rotation, f);
            Vector3 nextScale = Vector3.Lerp(originalNodeData.Scale, targetNodeData.Scale, f);

            if (IsPhysical) {
                if (Is2D) {
                    rb2D.MovePosition(new Vector2(nextPosition.x, nextPosition.y));
                    rb2D.MoveRotation(nextRotation.z);
                    transform.localScale = nextScale;
                } else {
                    rb3D.MovePosition(nextPosition);
                    rb3D.MoveRotation(Quaternion.Euler(nextRotation));
                    transform.localScale = nextScale;
                }
            } else {
                transform.position = nextPosition;
                transform.rotation = Quaternion.Euler(nextRotation);
                transform.localScale = nextScale;
            }
        }

        private void forcedReset(PathNodeData targetNodeData) {
            Vector3 targetWorldPos = OriginalPos + targetNodeData.Position;
            if (IsPhysical) {
                if (Is2D) {
                    rb2D.MovePosition(new Vector2(targetWorldPos.x, targetWorldPos.y));
                    rb2D.MoveRotation(targetNodeData.Rotation.z);
                    transform.localScale = targetNodeData.Scale;
                } else {
                    rb3D.MovePosition(targetWorldPos);
                    rb3D.MoveRotation(Quaternion.Euler(targetNodeData.Rotation));
                    transform.localScale = targetNodeData.Scale;
                }
            } else {
                transform.position = targetWorldPos;
                transform.rotation = Quaternion.Euler(targetNodeData.Rotation);
                transform.localScale = targetNodeData.Scale;
                return;
            }
        }

    }

}
