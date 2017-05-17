using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Astar;

public class ShowData : MonoBehaviour
{
    public Transform StartTransform;
    public LayerMask WallLayer;
    public float radius = 0.1f;
    private float height = 0;
    private float widht = 0;
    private float StartX = 0;
    private float StartY = 0;
    [HideInInspector] public int MaxX = 0;
    [HideInInspector] public int MaxY = 0;
    private Vector3 v_startData;
    [HideInInspector] public List<Node> path;
    [HideInInspector] public List<Node> openPath;
    [HideInInspector] public Node[,] date;

    public List<Node> ErrorPath;
    // Use this for initialization
    void Start()
    {
        height = transform.GetComponent<MeshFilter>().mesh.bounds.size.z;
        widht = transform.GetComponent<MeshFilter>().mesh.bounds.size.x;
        v_startData = new Vector3(transform.position.x - widht, 0, transform.position.z - height);
        StartX = Mathf.RoundToInt(transform.position.x - widht) + radius/2;
        StartY = Mathf.RoundToInt(transform.position.z - height) + radius/2;
        MaxX = Mathf.RoundToInt((height*2)/radius);
       // MaxX = Mathf.RoundToInt((height*2)/radius);
        MaxY = Mathf.RoundToInt((widht*2)/radius);
       // MaxY = Mathf.RoundToInt((widht*2)/radius);
        date = new Node[MaxX, MaxY];
        for (int i = 0; i < MaxX; i++)
        {
            // 11111111111111111111
            for (int j = 0; j < MaxY; j++)
            {
                date[i, j] = new Node();
                date[i, j]._worldPos = new Vector3(StartX + radius*i, 0, StartY + radius*j);
                date[i, j].isCanRun = Physics.OverlapSphere(date[i, j]._worldPos, radius/2, WallLayer).Length == 0;
                date[i, j].x = i;
                date[i, j].y = j;
            }
        }
        
        transform.GetComponent<FindPath>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(height*2, 1, widht*2));
        Gizmos.color = Color.white;
        if (date != null)
        {
            foreach (var e in date)
            {
                if (e.isCanRun)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(e._worldPos, Vector3.one*(radius - radius/10));
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(GetFromPosition(StartTransform.position)._worldPos, Vector3.one*(radius - radius/10));

            
            if (openPath != null)
            {
                foreach (var e in openPath)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(e._worldPos, Vector3.one*(radius - radius/10));
                }
            }
            // 绘制path
            if (path != null)
            {
                foreach (var e in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(e._worldPos, Vector3.one*(radius - radius/10));
                }
            }
        }
    }

    public Node GetFromPosition(Vector3 pos)
    {
        Vector3 pPos = new Vector3((pos.x - StartX)/radius, 0, (pos.z - StartY)/radius);
        return date[Mathf.RoundToInt(pPos.x)%MaxX, Mathf.RoundToInt(pPos.z)%MaxY];
    }

    /// <summary>
    /// 
    /// </summary>
    public List<Node> GetOpenAroundNode(Node currentNode, List<Node> closeNode)
    {
        List<Node> OpenNodeList = new List<Node>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (currentNode.x + i > -1 && currentNode.x + i < MaxX && currentNode.y + j > -1 &&
                    currentNode.y + j < MaxY)
                {
                    if (date[currentNode.x + i, currentNode.y + j].isCanRun &&
                        !closeNode.Contains(date[currentNode.x + i, currentNode.y + j]))
                    {
                        OpenNodeList.Add(date[currentNode.x + i, currentNode.y + j]);
                    }
                }
            }
        }
        return OpenNodeList;
    }
}