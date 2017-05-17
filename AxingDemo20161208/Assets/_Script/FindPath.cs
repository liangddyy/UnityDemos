#region

using System;
using System.Collections.Generic;
using Astar;
using UnityEngine;

#endregion

public class FindPath : MonoBehaviour
{
    public Transform EndTransform;
    public float moveSpeed = 2.0f;
    private ShowData showData;
    public Transform StartTransform;
    private Vector3 v_end;
    private Vector3 v_start;
    // Use this for initialization
    private void Start()
    {
        v_start = Vector3.zero;
        v_end = Vector3.zero;
        showData = transform.GetComponent<ShowData>();
    }

    // Update is called once per frame
    private void Update()
    {
        v_start = Vector3.zero;
        v_end = Vector3.zero;
        //起点移动
        if (Input.GetKey(KeyCode.W))
            v_start.z = 1;
        else if (Input.GetKey(KeyCode.S))
            v_start.z = -1;
        if (Input.GetKey(KeyCode.D))
            v_start.x = 1;
        else if (Input.GetKey(KeyCode.A))
            v_start.x = -1;

        //终点移动
        if (Input.GetKey(KeyCode.UpArrow))
            v_end.z = 1;
        else if (Input.GetKey(KeyCode.DownArrow))
            v_end.z = -1;
        if (Input.GetKey(KeyCode.RightArrow))
            v_end.x = 1;
        else if (Input.GetKey(KeyCode.LeftArrow))
            v_end.x = -1;
        StartTransform.Translate(v_start*Time.deltaTime*moveSpeed);
        EndTransform.Translate(v_end*Time.deltaTime*moveSpeed);
        FindAStartPath(StartTransform.position, EndTransform.position);
    }

    private void FindAStartPath(Vector3 startPos, Vector3 endPos)
    {
        showData.openPath = new List<Node>();
        showData.ErrorPath = new List<Node>();
        Node startNode = showData.GetFromPosition(startPos);
        Node endNode = showData.GetFromPosition(endPos);
        List<Node> openNode = new List<Node>();
        List<Node> closeNode = new List<Node>();
        openNode.Add(startNode);

        startNode.parent = null;
        while (openNode.Count > 0)
        {
            Node currentNode = openNode[0];

            //找到打开节点中最小的权重的节点
            for (int i = 0; i < openNode.Count; i++)
                if ((openNode[i].fCode <= currentNode.fCode) && (openNode[i].hCode < currentNode.hCode))
                    currentNode = openNode[i];
            //将这个权重最小的节点移除
            openNode.Remove(currentNode);

            closeNode.Add(currentNode);

            // 结束
            if (currentNode == endNode)
            {
                List<Node> path = new List<Node>();
                while (currentNode.parent != null)
                {
                    try
                    {
                        path.Add(currentNode);
                    }
                    catch (Exception)
                    {
                        enabled = false;
                        throw;
                    }
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                showData.path = path;
                return;
            }
            showData.openPath.Add(currentNode);

            //搜索当前节点走位可以走得节点，然后更新权重值
            foreach (var node in showData.GetOpenAroundNode(currentNode, closeNode))
            {
                int newCode = currentNode.gCode + GetDistancePos(currentNode, node);
                if (!openNode.Contains(node) || (node.gCode > newCode))
                {
                    if (currentNode == showData.date[8, 4])
                        showData.ErrorPath.Add(node);
                    node.gCode = newCode;
                    node.hCode = GetDistancePos(node, endNode);
                    node.parent = currentNode;
                    if (!openNode.Contains(node))
                        openNode.Add(node);
                }
            }
        }
    }

    private int GetDistancePos(Node StartNode, Node EndNode)
    {
        int dx = Mathf.Abs(StartNode.x - EndNode.x);
        int dy = Mathf.Abs(StartNode.y - EndNode.y);
        return 14*(dx > dy ? dy : dx) + Mathf.Abs(10*(dx - dy));
    }
}