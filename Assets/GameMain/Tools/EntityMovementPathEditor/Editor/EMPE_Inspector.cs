using UnityEditor;
using UnityEditorInternal;

namespace EMPE
{
    [CustomEditor(typeof(EntityMovementPathController))]
    public class EMPE_Inspector : Editor
    {
        private EntityMovementPathController empc;
        private ReorderableList pathNodeList;

        private void OnEnable()
        {
            pathNodeList = new ReorderableList(serializedObject, serializedObject.FindProperty("PathNodeList"), true, true, true, true);
            empc = (EntityMovementPathController)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //�Զ����ֻ����б�
            pathNodeList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
