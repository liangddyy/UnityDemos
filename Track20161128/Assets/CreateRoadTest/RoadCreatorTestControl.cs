using UnityEngine;

public class RoadCreatorTestControl : MonoBehaviour
{
    private static readonly int POSITION_NUM_MAX = 100;

    public GameObject BallPrefab = null;
    // 游戏摄像机
    private GameObject game_camera;

    public Material material;
    private STEP next_step = STEP.NONE;
    public PhysicMaterial physic_material = null;
    private int position_num;

    private Vector3[] positions;

    private RoadCreatorTest road_creator;

    private STEP step = STEP.NONE;

    // Use this for initialization
    private void Start()
    {
        // 预先探测出摄像机的实例
        game_camera = GameObject.FindGameObjectWithTag("MainCamera");

        GetComponent<LineRenderer>().SetVertexCount(0);

        positions = new Vector3[POSITION_NUM_MAX];

        road_creator = new RoadCreatorTest();
    }

//    void OnMouseUp()
//    {
//        if (this.step == STEP.DRAWING)
//        {
//            
//        }
//    }

    private void OnGUI()
    {
        float x = 100;
        float y = 100;

        GUI.Label(new Rect(x, y, 100, 100), position_num.ToString());
        y += 20;

//        if (GUI.Button(new Rect(200, 100, 100, 20), "create"))
//            if (step == STEP.DRAWED)
//                next_step = STEP.CREATED; //create

        if (GUI.Button(new Rect(310, 100, 100, 20), "clear"))
            next_step = STEP.IDLE;

        if (GUI.Button(new Rect(200, 130, 100, 20), "ball"))
            if (step == STEP.CREATED)
            {
                var ball = Instantiate(BallPrefab);

                Vector3 ball_position;

                ball_position = (road_creator.sections[0].center + road_creator.sections[1].center)/2.0f +
                                Vector3.up*1.0f;

                ball.transform.position = ball_position;
            }
    }

    // Update is called once per frame
    private void Update()
    {
        // 检测状态迁移

        switch (step)
        {
            case STEP.NONE:
            {
                next_step = STEP.IDLE;
            }
                break;

            case STEP.IDLE:
            {
                if (Input.GetMouseButton(0))
                    next_step = STEP.DRAWING;
            }
                break;

            case STEP.DRAWING:
            {
                //鼠标抬起
                if (!Input.GetMouseButton(0))
                    if (position_num >= 2)
                        next_step = STEP.CREATED;
                    else
                        next_step = STEP.IDLE;
            }
                break;
        }

        // 状态迁移时的初始化

        if (next_step != STEP.NONE)
        {
            switch (next_step)
            {
                case STEP.IDLE:
                {
                    // 删除上次生成的物体

                    road_creator.clearOutput();

                    position_num = 0;

                    GetComponent<LineRenderer>().SetVertexCount(0);
                }
                    break;

                case STEP.CREATED:
                {
                    //创建道路
                    road_creator.positions = positions;
                    road_creator.position_num = position_num;
                    road_creator.material = material;
                    road_creator.physic_material = physic_material;

                    road_creator.createRoad();
                }
                    break;
            }

            step = next_step;

            next_step = STEP.NONE;
        }

        // 各个状态对应的处理

        switch (step)
        {
            case STEP.DRAWING:
            {
                var position = unproject_mouse_position();

                // 检测顶点是否被添加到线上

                var is_append_position = false;

                if (position_num == 0)
                {
                    // 最开始的一个被无条件添加

                    is_append_position = true;
                }
                else if (position_num >= POSITION_NUM_MAX)
                {
                    // 超过最大个数时将无法添加

                    is_append_position = false;
                }
                else
                {
                    // 添加和上次被添加的顶点距离一定间隔的点

                    if (Vector3.Distance(positions[position_num - 1], position) > 0.5f)
                        is_append_position = true;
                }

                //

                if (is_append_position)
                {
                    if (position_num > 0)
                    {
                        var distance = position - positions[position_num - 1];

                        distance *= 0.5f/distance.magnitude;

                        position = positions[position_num - 1] + distance;
                    }

                    positions[position_num] = position;

                    position_num++;

                    // 重新生成LineRender 

                    GetComponent<LineRenderer>().SetVertexCount(position_num);

                    for (var i = 0; i < position_num; i++)
                        GetComponent<LineRenderer>().SetPosition(i, positions[i]);
                }
            }
                break;
        }

        /*if(is_created) {

            foreach(Section section in this.sections) {

                Debug.DrawLine(section.positions[0], section.positions[1], Color.red, 0.0f, false);
            }
        }*/
    }

    //  重要
    //  将鼠标的位置变换为3D空间内的世界坐标
    //
    //  ・穿过鼠标光标和摄像机所在位置的直线
    //  ・穿过道路中心的水平面
    //　↑求出以上两个物体的交点
    //
    private Vector3 unproject_mouse_position()
    {
        var mouse_position = Input.mousePosition;

        // 穿过道路中心的水平面（以Y轴为法线的XZ平面）
        var plane = new Plane(Vector3.up, new Vector3(0.0f, 0.0f, 0.0f));

        // 穿过摄像机位置和鼠标光标位置的直线
        var ray = game_camera.GetComponent<Camera>().ScreenPointToRay(mouse_position);

        // 求出上面两个物体的交点

        float depth;

        plane.Raycast(ray, out depth);

        Vector3 world_position;

        world_position = ray.origin + ray.direction*depth;

        return world_position;
    }

    private enum STEP
    {
        NONE = -1,

        IDLE = 0, // 空闲中
        DRAWING, // 画线（拖动过程中）
        DRAWED, // 画线过程结束
        CREATED, // 生成了道路模型

        NUM
    }
}