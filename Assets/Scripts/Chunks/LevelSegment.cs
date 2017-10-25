using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KyleStankovich.BattleDash
{
    public class LevelSegment : MonoBehaviour
    {
        [HideInInspector]
        public float[] ObstaclePositions = new float[0];

        [SerializeField]
        private Transform m_PathParent = null;

        [SerializeField]
        private Transform m_ObjectRoot = null;
        [SerializeField]
        private Transform m_CollectableTransform = null;

        [SerializeField]
        private Transform[] m_PossibleObstacles = null;

        private LevelManager m_LevelManager = null;
        private float m_WorldLength = 0.0f;

        #region Properties

        public float WorldLength
        {
            get { return m_WorldLength; }
        }

        #endregion

        #region Methods

        #region Unity methods
        private void OnEnable()
        {
            UpdateWorldLength();

            GameObject obj = new GameObject("ObjectRoot");
            obj.transform.SetParent(transform);
            m_ObjectRoot = obj.transform;

            obj = new GameObject("Collectables");
            obj.transform.SetParent(m_ObjectRoot);
            m_CollectableTransform = obj.transform;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(m_PathParent == null)
            {
                return;
            }

            Color c = Gizmos.color;

            Gizmos.color = Color.red;
            for(int i = 1; i < m_PathParent.childCount; ++i)
            {
                Transform origin = m_PathParent.GetChild(i - 1);
                Transform end = m_PathParent.GetChild(i);

                Gizmos.DrawLine(origin.position, end.position);
            }

            Gizmos.color = Color.blue;
            for(int i = 0; i < ObstaclePositions.Length; ++i)
            {
                Vector3 pos;
                Quaternion rot;
                GetPointAt(ObstaclePositions[i], out pos, out rot);
                Gizmos.DrawSphere(pos, 0.5f);
            }

            Gizmos.color = c;
        }
#endif
        #endregion

        #region Public methods
        public void GetPointAtInWorldUnit(float wt, out Vector3 pos, out Quaternion rot)
        {
            float t = wt / m_WorldLength;
            GetPointAtInWorldUnit(t, out pos, out rot);
        }

        public void GetPointAt(float t, out Vector3 pos, out Quaternion rot)
        {
            float clampedT = Mathf.Clamp01(t);
            float scaledT = (m_PathParent.childCount - 1) * clampedT;
            int index = Mathf.FloorToInt(scaledT);
            float segmentT = scaledT - index;

            Transform origin = m_PathParent.GetChild(index);
            if(index == m_PathParent.childCount - 1)
            {
                pos = origin.position;
                rot = origin.rotation;
                return;
            }

            Transform target = m_PathParent.GetChild(index + 1);

            pos = Vector3.Lerp(origin.position, target.position, segmentT);
            rot = Quaternion.Lerp(origin.rotation, target.rotation, segmentT);
        }

        public void Cleanup()
        {
            while(m_CollectableTransform.childCount > 0)
            {
                Transform t = m_CollectableTransform.GetChild(0);
                t.SetParent(null);
                //Coin.coinPool.Free(t.gameObject);
            }

            Destroy(gameObject);
        }
        #endregion

        #region Private methods
        private void UpdateWorldLength()
        {
            m_WorldLength = 0.0f;

            for(int i = 0; i < m_PathParent.childCount; ++i)
            {
                Transform origin = m_PathParent.GetChild(i - 1);
                Transform end = m_PathParent.GetChild(i);

                Vector3 vec = end.position - origin.position;
                m_WorldLength += vec.magnitude;
            }
        }
        #endregion

        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LevelSegment))]
    class LevelSegmentEditor : Editor
    {
        private LevelSegment m_Segment = null;

        #region Methods

        #region Unity methods
        private void OnEnable()
        {
            m_Segment = target as LevelSegment;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Add Obstacles"))
            {
                ArrayUtility.Add(ref m_Segment.ObstaclePositions, 0.0f);
            }

            if(m_Segment.ObstaclePositions != null)
            {
                int toremove = -1;
                for(int i = 0; i < m_Segment.ObstaclePositions.Length; ++i)
                {
                    GUILayout.BeginHorizontal();

                    m_Segment.ObstaclePositions[i] = EditorGUILayout.Slider(m_Segment.ObstaclePositions[i], 0.0f, 1.0f);
                    if(GUILayout.Button("-", GUILayout.MaxWidth(32.0f)))
                    {
                        toremove = i;
                    }

                    GUILayout.EndHorizontal();
                }

                if(toremove != -1)
                {
                    ArrayUtility.RemoveAt(ref m_Segment.ObstaclePositions, toremove);
                }
            }
        }
        #endregion

        #endregion
    }
#endif
}
