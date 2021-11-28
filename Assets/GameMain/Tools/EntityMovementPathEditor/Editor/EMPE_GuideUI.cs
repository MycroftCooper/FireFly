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

        // ���Ʋ�����
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

        // ��������ֱ��
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

        // ���Ʊ�������������
        private void drawBezierCurve() {
            List<PathNodeData> pathNodeList = empc.PathNodeList;
            if (pathNodeList.Count < 4) return;
            Handles.color = Color.yellow;
            Vector3 p = empc.OriginalPos;
            float width = 2f;
            //����1 ��ʼ�����꣬ ����2�����������꣬ ����3 ��ʼ����λ�ã� ���� 4����������λ�ã� ���� 5 �ߵ���ɫ �������� �ߵĿ��
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
