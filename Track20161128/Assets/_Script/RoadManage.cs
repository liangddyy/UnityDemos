using UnityEngine;

public class RoadManage
{
    //private bool is_created;
    // 生成物材质
    private Material material;

    private int positionNum;
    private Vector3[] positions;

    private GameObject roadGameObject;
    private Section[] sections;
    private float width;
    

    // 生成道路模型（用于显示的形状，以及碰撞体）
    public void create(Material material, Vector3[] positions, int positionNum, float width)
    {
        this.width = width;
        this.positionNum = positionNum;
        this.positions = positions;
        this.material = material;

        // 生成截面形状 

        // 截面信息
        sections = new Section[this.positionNum];

        for (var i = 0; i < this.positionNum; i++)
            sections[i].positions = new Vector3[2]; //2

        //float	height_max = this.positionNum*0.1f;
        var height_max = this.positionNum*0f;

        for (var i = 0; i < this.positionNum; i++)
            sections[i].center = this.positions[i];

        // 坐标
        for (var i = 0; i < this.positionNum; i++)
        {
            if (i < this.positionNum - 1)
                sections[i].direction = sections[i + 1].center - sections[i].center;
            else
                sections[i].direction = sections[i].center - sections[i - 1].center;

            sections[i].direction.y = 0.0f;
            sections[i].direction.Normalize();

            //绕axis轴旋转angle，创建一个旋转。
            var right = Quaternion.AngleAxis(90.0f, Vector3.up)*sections[i].direction;

            // 左右两点
            sections[i].positions[0] = sections[i].center - right*width/2.0f;
            sections[i].positions[1] = sections[i].center + right*width/2.0f;
        }

        if (roadGameObject == null)
        {

            roadGameObject = new GameObject();

            roadGameObject.name = "Road";
            roadGameObject.AddComponent<MeshFilter>();
            roadGameObject.AddComponent<MeshRenderer>();
            roadGameObject.AddComponent<MeshCollider>();
        }
        

        // 创建 mesh
        createGroundMesh(roadGameObject);
        add_backface_trianbles_to_mesh(this.roadGameObject.GetComponent<MeshFilter>().mesh);

        //is_created = true;
    }

    private void createGroundMesh(GameObject game_object)
    {
        //var mesh_filter1 = game_object.GetComponent<MeshFilter>();

        // 网格过滤器
        var mesh_filter = game_object.GetComponent<MeshFilter>();

        var mesh = mesh_filter.mesh;
        var mesh_collider = game_object.GetComponent<MeshCollider>();
        var render = game_object.GetComponent<MeshRenderer>();

        //

        mesh.Clear();
        mesh.name = "GroundMesh";

        // 坐标
        var vertices = new Vector3[positionNum*2];
        // UV信息
        var uvs = new Vector2[positionNum*2];

        // 索引（int）
        var triangles = new int[(positionNum - 1)*2*3];

        for (var i = 0; i < positionNum; i++)
        {
            vertices[i*2 + 0] = sections[i].positions[0];
            vertices[i*2 + 1] = sections[i].positions[1];

            //uvs[i*2 + 0] = new Vector2(0.0f, i/(float) (positionNum - 1));
            //uvs[i*2 + 1] = new Vector2(1.0f, i/(float) (positionNum - 1));
        }

        var position_index = 0;

        // 建立索引
        for (var i = 0; i < positionNum - 1; i++)
        {
            triangles[position_index++] = i*2 + 1;
            triangles[position_index++] = i*2 + 0;
            triangles[position_index++] = (i + 1)*2 + 0;

            triangles[position_index++] = (i + 1)*2 + 0;
            triangles[position_index++] = (i + 1)*2 + 1;
            triangles[position_index++] = i*2 + 1;
        }


        mesh.vertices = vertices;

        //mesh.uv = uvs;
        //mesh.uv2 = uvs;
        mesh.triangles = triangles;

        mesh.Optimize();
        mesh.RecalculateNormals();

        render.material = material;
        render.material.color = Color.red;

        mesh_collider.sharedMesh = mesh;
        mesh_collider.enabled = true;
        
    }


    // 一个mesh
    private void add_backface_trianbles_to_mesh(Mesh mesh)
    {
        var face_num = mesh.triangles.Length/3;

        var faces = new int[face_num*3*2];

        // 之前的mesh
        for (var i = 0; i < face_num; i++)
        {
            faces[i*3 + 0] = mesh.triangles[i*3 + 0];
            faces[i*3 + 1] = mesh.triangles[i*3 + 1];
            faces[i*3 + 2] = mesh.triangles[i*3 + 2];
        }

        //
        for (var i = 0; i < face_num; i++)
        {
            faces[(face_num + i)*3 + 0] = mesh.triangles[i*3 + 2];
            faces[(face_num + i)*3 + 1] = mesh.triangles[i*3 + 1];
            faces[(face_num + i)*3 + 2] = mesh.triangles[i*3 + 0];
        }

        mesh.triangles = faces;
    }

    // 中心 法线 左右两点
    public struct Section
    {
        public Vector3 center;
        public Vector3 direction;
        public Vector3[] positions;
    }
}