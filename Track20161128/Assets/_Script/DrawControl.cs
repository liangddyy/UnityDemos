using UnityEngine;

public class DrawControl : MonoBehaviour
{
    private readonly int POSITION_NUM_MAX = 1000;
    // 游戏摄像机
    private GameObject gameCamera;
    private bool isAppendPosition;
    private bool isDrawing;
    public Material material;
    private int positionNum;
    private Vector3[] positions;
    private RoadManage roadManage;

    public float width = 0.5f;
    // Use this for initialization
    private void Start()
    {
        roadManage = new RoadManage();
        positions = new Vector3[POSITION_NUM_MAX];
        GetComponent<LineRenderer>().SetVertexCount(0);
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0))
            isDrawing = true;
        if (!Input.GetMouseButton(0))
            if (isDrawing)
            {
                Debug.Log("开始绘制");

                // 绘制mesh

                roadManage.create(material, positions, positionNum, width);
                //positionNum = 0;
                isDrawing = false;
            }
        if (isDrawing)
        {
            var position = unproject_mouse_position();

            // 检测顶点是否被添加到线上

            isAppendPosition = false;

            if (positionNum == 0)
            {
                isAppendPosition = true;
            }
            else if (positionNum >= POSITION_NUM_MAX)
            {
                isAppendPosition = false;
            }
            else
            {
                if (Vector3.Distance(positions[positionNum - 1], position) > 0.5f)
                    isAppendPosition = true;
            }

            if (isAppendPosition)
            {
                if (positionNum > 0)
                {
                    var distance = position - positions[positionNum - 1];

                    distance *= 0.5f/distance.magnitude;

                    position = positions[positionNum - 1] + distance;
                }

                positions[positionNum] = position;

                positionNum++;

                // 生成LineRender 

                GetComponent<LineRenderer>().SetVertexCount(positionNum);

                for (var i = 0; i < positionNum; i++)
                    GetComponent<LineRenderer>().SetPosition(i, positions[i]);
            }
        }
    }

    private void OnMounseUp()
    {
    }

    private Vector3 unproject_mouse_position()
    {
        var mouse_position = Input.mousePosition;

        var plane = new Plane(Vector3.up, new Vector3(0.0f, 0.0f, 0.0f));

        var ray = gameCamera.GetComponent<Camera>().ScreenPointToRay(mouse_position);


        float depth;

        // 这个函数设置out 沿着射线，相交平面的距离，如果该射线是平行于平面，函数返回false，并且设置out enter到0。
        plane.Raycast(ray, out depth); //更改depth

        return ray.origin + ray.direction*depth; //交点
    }
}