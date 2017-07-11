using UnityEngine;
using System.Collections.Generic;
using UFramework;


namespace Babybus.PathCreator
{
    public class CurvePath : MonoBehaviour
    {
        public int PointCount
        {
            get { return points.Count; }
        }

        public int PathCount
        {
            get { return PointCount - 1; }
        }

        public int selectedPointIndex;

        public int selectedPathIndex;

        public List<Vector3> points = new List<Vector3>();
        public List<bool> isBezier = new List<bool>();
        public List<Vector3> startControlPoints = new List<Vector3>();
        public List<Vector3> endControlPoints = new List<Vector3>();

        public enum PointMode
        {
            points,
            paths
        }

        public PointMode pointMode;

        public Vector3 GetPosition(float percent)
        {

            float step = 1.0f / PathCount;
            for (int i = 0; i < PathCount; i++)
            {
//                Debug.Log(i + isBezier.Count);
                if (percent >= i * step && percent <= (i + 1) * step)
                {
                    if (isBezier[i])
                        return
//                            MathUtil.CalculateBezierPoint(points[GetIndex(i)],
//                                MathUtil.GetLineRake(0, points[GetIndex(i + 1)], points[GetIndex(i - 1)]),
//                                MathUtil.GetLineRake(0, points[GetIndex(i + 2)], points[GetIndex(i)]),
//                                points[GetIndex(i + 1)], (percent - step * i) * PathCount); // 贝塞尔曲线
                    MathUtil.GetPoint(points[GetIndex(i-1)],points[GetIndex(i)], points[GetIndex(i - 1)],
                                points[GetIndex(i + 2)], (percent - step * i) * PathCount); // 贝塞尔曲线
//                        return MathUtil.CalculateBezierPoint(points[i], startControlPoints[i] - points[i],endControlPoints[i] - points[i + 1], points[i + 1], (percent - step * i) * PathCount); // 贝塞尔曲线
                    else
                        return Vector3.Lerp(points[i], points[i + 1], (percent - step * i) * PathCount); // 线性移动
                }
            }

            return Vector3.zero;
        }
    

    private int GetIndex(int i)
        {
            if (i < 0)
            {
                return (points.Count - 1);
            }
            else if (i >= points.Count)
            {
                return 0;
            }
            else
            {
                return i;
            }
        }

        public void InsertPoint(int index)
        {
            if (PointCount == 0)
                points.Add(Vector3.zero);
            else
            {
                points.Insert(index, points[index]);

                isBezier.Insert(index, false);
                InsertControlPoint(index);
            }
        }

        public void DeletePoint(int index)
        {
            points.RemoveAt(index);
            if (PointCount == 0)
                return;

            if (index == PointCount)
                DeleteControlPoint(index - 1);
            else
                DeleteControlPoint(index);
        }

        private void InsertControlPoint(int index)
        {
            startControlPoints.Insert(index, points[index]);
            endControlPoints.Insert(index, points[index + 1]);
        }

        private void DeleteControlPoint(int index)
        {
            startControlPoints.RemoveAt(index);
            endControlPoints.RemoveAt(index);
        }
    }
}