using UnityEngine;
using System.Collections;

namespace Astar
{
    public class Node
    {
        public int gCode;
        public int hCode;
        public int x;
        public int y;
        public Vector3 _worldPos;
        public bool isCanRun = true;

        public float fCode
        {
            get { return gCode + hCode; }
        }

        public Node parent = null;
    }
}