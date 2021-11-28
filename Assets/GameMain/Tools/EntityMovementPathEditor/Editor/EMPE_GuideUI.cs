using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static EMPE.EntityMovementPathController;

namespace EMPE {
    [CustomEditor(typeof(EntityMovementPathController))]
    public class EMPE_GuideUI : Editor {
        private EntityMovementPathController empc;
        void OnEnable() {
            empc = (EntityMovementPathController)target;
        }
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }
        private void OnSceneGUI() {
            //if (GUI.changed)
            //{
            //    EditorUtility.SetDirty(empc);
            //}
            if (!empc.IsEditMode) return;

            drowHandle();
            switch (empc.PathShape) {
                case PathShapes.Line:
                    drawLine();
                    break;
                case PathShapes.BezierCurve:
                    drawBezierCurve();
                    break;
            }
        }

        // 绘制操作柄
        private void drowHandle() {
            List<PathNodeData> pathNodeList = empc.PathNodeList;
            for (int i = 0; i < empc.PathNodeList.Count; i++) {
                PathNodeData node = pathNodeList[i];
                Vector3 worldPos = empc.OriginalPos + node.Position;
                node.Position = Handles.PositionHandle(worldPos, Quaternion.Euler(node.Rotation)) - empc.OriginalPos;
                node.Rotation = Handles.RotationHandle(Quaternion.Euler(node.Rotation), worldPos).eulerAngles;
                node.Scale = Handles.ScaleHandle(node.Scale, worldPos, Quaternion.Euler(node.Rotation), 2f);
            }
        }

        // 绘制连接直线
        private void drawLine() {
            Handles.color = Color.red;
            List<PathNodeData> pathNodeList = empc.PathNodeList;
            Vector3 p = empc.OriginalPos;
            for (int i = 0; i < empc.PathNodeList.Count; i++) {
                if (i + 1 == empc.PathNodeList.Count)
                    break;
                Handles.DrawLine(p + pathNodeList[i].Position, p + pathNodeList[i + 1].Position);
            }
            if (empc.IsLoop)
                Handles.DrawLine(p + pathNodeList[empc.PathNodeList.Count - 1].Position, p + pathNodeList[0].Position);
        }

        // 绘制贝塞尔链接曲线
        private void drawBezierCurve() {
            List<PathNodeData> pathNodeList = empc.PathNodeList;
            if (pathNodeList.Count < 4) return;
            Handles.color = Color.yellow;
            Vector3 p = empc.OriginalPos;
            float width = 2f;
            //参数1 开始点坐标， 参数2，结束点坐标， 参数3 开始切线位置， 参数 4，结束切线位置， 参数 5 线的颜色 ，参数六 线的宽度
            for (int i = 0; i <= pathNodeList.Count - 4; i += 3) {
                Handles.DrawBezier(pathNodeList[i].Position + p, pathNodeList[i + 3].Position + p, pathNodeList[i + 1].Position + p, pathNodeList[i + 2].Position + p, Color.yellow, null, width);
            }
            if (empc.IsLoop) {
                if (empc.PathNodeList.Count < 6) return;
                int i = empc.PathNodeList.Count - 3;
                Handles.DrawBezier(pathNodeList[i].Position + p, pathNodeList[0].Position + p, pathNodeList[i + 1].Position + p, pathNodeList[i + 2].Position + p, Color.yellow, null, width);
            }
        }
    }
}
