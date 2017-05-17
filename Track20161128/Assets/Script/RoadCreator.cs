using UnityEngine;
using System.Collections;

public class RoadCreator
{

    // 鍏ュ姏.

    public Vector3[] positions = null;
    public int position_num = 0;

    public Material material = null;
    public Material road_material = null;
    public Material wall_material = null;
    public PhysicMaterial physic_material = null;

    public int peak_position;

    public int[] split_points;


    public struct HeightPeg
    {

        public int position;
        public float height;
    };

    public HeightPeg[] height_pegs = null;

    // 鐢熸垚鐗?

    public GameObject[] road_mesh = null;
    public GameObject[,] wall_mesh = null;

    // 鍓?敚鐗?

    public struct Section
    {

        public Vector3 center;
        public Vector3 direction;
        public Vector3[] positions;

        public Vector3 right;
        public Vector3 up;
    };

    public Section[] sections;

    //

    public bool is_created = false;

    private enum WALL_SIDE
    {

        NONE = -1,

        LEFT = 0,
        RIGHT,

        NUM,
    };

    public static Vector3 PolygonSize = new Vector3(30.0f, 0.0f, 20.0f);

    public static float WallHeight = 100.0f;

    //public int[]	blocks = null;

    // ------------------------------------------------------------------------ //

    public int getRoadBlockIndexByName(string name)
    {
        int index = -1;

        for (int i = 0; i < this.road_mesh.Length; i++)
        {

            if (this.road_mesh[i].name == name)
            {

                index = i;
                break;
            }
        }

        return (index);
    }
    public GameObject getRoadObjectByName(string name)
    {
        GameObject road = null;

        foreach (var obj in this.road_mesh)
        {

            if (obj.name == name)
            {

                road = obj;
                break;
            }
        }

        return (road);
    }

    public void setEnableToBlock(int block_index, bool sw)
    {
        //this.road_mesh[block_index].renderer.enabled = sw;
        this.road_mesh[block_index].GetComponent<Collider>().enabled = sw;

        //this.wall_mesh[block_index, 0].renderer.enabled = sw;
        this.wall_mesh[block_index, 0].GetComponent<Collider>().enabled = sw;

        //this.wall_mesh[block_index, 1].renderer.enabled = sw;
        this.wall_mesh[block_index, 1].GetComponent<Collider>().enabled = sw;
    }

    // ------------------------------------------------------------------------ //

    // 鍙栧緱楂樺害
    private float get_height(int position_index)
    {
        float height = 0.0f;

        for (int i = 1; i < this.height_pegs.Length; i++)
        {

            if (this.height_pegs[i].position > position_index)
            {

                HeightPeg peg0 = this.height_pegs[i - 1];
                HeightPeg peg1 = this.height_pegs[i];

                float rate = Mathf.InverseLerp((float)peg0.position, (float)peg1.position, (float)position_index);

                rate = Mathf.Lerp(-Mathf.PI / 2.0f, Mathf.PI / 2.0f, rate);

                rate = (Mathf.Sin(rate) + 1.0f) / 2.0f;

                height = Mathf.Lerp(peg0.height, peg1.height, rate);

                break;
            }
        }

        return (height);
    }

    // 鐢熸垚閬撹矾妯″瀷锛堟樉绀虹敤鐨勫舰鐘讹紝纰版挒鍣?級
    public void createRoad()
    {
        // ------------------------------------------------------------ //
        // 鐢熸垚鎴?潰褰㈢姸

        this.sections = new Section[this.position_num];

        for (int i = 0; i < this.position_num; i++)
        {

            this.sections[i].positions = new Vector3[2];
        }

        for (int i = 0; i < this.position_num; i++)
        {

            this.sections[i].center = this.positions[i];

            this.sections[i].center.y = this.get_height(i);
        }

        //

        for (int i = 0; i < this.position_num; i++)
        {

            // 绠楀嚭鏂瑰悜鍚戦噺

            if (i == 0)
            {

                // 璧风偣

                this.sections[i].direction = this.sections[i + 1].center - this.sections[i].center;

            }
            else if (i == this.position_num - 1)
            {

                // 缁堢偣

                this.sections[i].direction = this.sections[i].center - this.sections[i - 1].center;

            }
            else
            {

                // 閫斾腑鐨勭偣
                // 鍜岃繛鎺ュ墠鍚庣偣鐨勭洿绾挎柟鍚戠浉鍚嬏

                this.sections[i].direction = this.sections[i + 1].center - this.sections[i - 1].center;
            }

            this.sections[i].direction.Normalize();

            // 姹傚嚭鍜屾柟鍚戝悜閲忓瀭鐩翠氦鍙夌殑鍚戦噺

            Vector3 right = Quaternion.AngleAxis(90.0f, Vector3.up) * this.sections[i].direction;

            right.y = 0.0f;
            right.Normalize();

            float width = RoadCreator.PolygonSize.x;

            this.sections[i].positions[0] = this.sections[i].center - right * width / 2.0f;
            this.sections[i].positions[1] = this.sections[i].center + right * width / 2.0f;

            //

            this.sections[i].right = right;
            this.sections[i].up = Vector3.Cross(this.sections[i].direction, this.sections[i].right).normalized;
        }

        // ------------------------------------------------------------ //

        this.road_mesh = new GameObject[this.split_points.Length - 1];
        this.wall_mesh = new GameObject[this.split_points.Length - 1, 2];

        for (int i = 0; i < this.split_points.Length - 1; i++)
        {

            int s = this.split_points[i];
            int e = this.split_points[i + 1];

            //

            this.road_mesh[i] = new GameObject();
            this.wall_mesh[i, 0] = new GameObject();
            this.wall_mesh[i, 1] = new GameObject();

            this.road_mesh[i].name = "Road " + i.ToString();
            this.wall_mesh[i, 0].name = "Wall(Left) " + i;
            this.wall_mesh[i, 1].name = "Wall(Right) " + i;

            this.road_mesh[i].layer = LayerMask.NameToLayer("Road Coli");

            this.create_ground_mesh(this.road_mesh[i], s, e);
            this.create_wall_mesh(this.wall_mesh[i, 0], s, e, WALL_SIDE.LEFT);
            this.create_wall_mesh(this.wall_mesh[i, 1], s, e, WALL_SIDE.RIGHT);

            this.wall_mesh[i, 0].GetComponent<MeshFilter>().mesh.name += "(Left) " + i;
            this.wall_mesh[i, 1].GetComponent<MeshFilter>().mesh.name += "(Right) " + i;

            // 璁剧疆PhysicMaterial 

            this.road_mesh[i].GetComponent<Collider>().material = this.physic_material;
            this.wall_mesh[i, 0].GetComponent<Collider>().material = this.physic_material;
            this.wall_mesh[i, 1].GetComponent<Collider>().material = this.physic_material;

            // 鐢熸垚鍐呴潰鐨勫?杈瑰舰锛堝洜涓鸿?缁樺埗涓ら潰锛執

            this.add_backface_trianbles_to_mesh(this.road_mesh[i].GetComponent<MeshFilter>().mesh);
            this.add_backface_trianbles_to_mesh(this.wall_mesh[i, 0].GetComponent<MeshFilter>().mesh);
            this.add_backface_trianbles_to_mesh(this.wall_mesh[i, 1].GetComponent<MeshFilter>().mesh);
        }

        //

        this.is_created = true;
    }

    // 鐢熸垚鍦伴潰锛堥亾璺?級澶氳竟褰√
    private void create_ground_mesh(GameObject game_object, int start, int end)
    {
        game_object.AddComponent<MeshFilter>();
        game_object.AddComponent<MeshRenderer>();
        game_object.AddComponent<MeshCollider>();

        MeshFilter mesh_filter = game_object.GetComponent<MeshFilter>();
        MeshCollider mesh_collider = game_object.GetComponent<MeshCollider>();
        Mesh mesh = mesh_filter.mesh;
        MeshRenderer render = game_object.GetComponent<MeshRenderer>();

        int point_num = end - start + 1;

        //

        mesh.Clear();
        mesh.name = "GroundMesh";

        Vector3[] vertices = new Vector3[point_num * 4];
        Vector2[] uvs = new Vector2[point_num * 4];
        int[] triangles = new int[(point_num - 1) * 2 * 3];

        for (int i = 0; i < point_num; i++)
        {

            // 灏嗛亾璺?乏鍙崇?椤剁偣鎸変袱涓?袱涓?緷娆¤?褰斕
            vertices[i * 4 + 0] = this.sections[start + i].positions[0];
            vertices[i * 4 + 1] = this.sections[start + i].positions[1];
            vertices[i * 4 + 2] = this.sections[start + i].positions[0];
            vertices[i * 4 + 3] = this.sections[start + i].positions[1];

            uvs[i * 4 + 0] = new Vector2(0.0f, 0.0f);
            uvs[i * 4 + 1] = new Vector2(1.0f, 0.0f);
            uvs[i * 4 + 2] = new Vector2(0.0f, 1.0f);
            uvs[i * 4 + 3] = new Vector2(1.0f, 1.0f);
        }

        int position_index = 0;

        for (int i = 0; i < point_num - 1; i++)
        {

            // 绗?竴涓?笁瑙掑舰
            triangles[position_index++] = i * 4 + 3;
            triangles[position_index++] = i * 4 + 2;
            triangles[position_index++] = (i + 1) * 4 + 0;

            // 绗?簩涓?笁瑙掑舰
            triangles[position_index++] = (i + 1) * 4 + 0;
            triangles[position_index++] = (i + 1) * 4 + 1;
            triangles[position_index++] = i * 4 + 3;
        }

        //
        // 鐢变簬浼氬嚭鐜拌?鍛婏紝杩欓噷鐢熸垚uv锛堥傚綋鏁板硷級

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.uv2 = uvs;
        mesh.triangles = triangles;

        mesh.Optimize();
        mesh.RecalculateNormals();

        render.material = this.road_material;
        //render.material.color = Color.red;

        mesh_collider.sharedMesh = mesh;
        mesh_collider.enabled = true;

        //
    }

    // 鐢熸垚澧欑殑澶氳竟褰√
    private void create_wall_mesh(GameObject game_object, int start, int end, WALL_SIDE side)
    {
        game_object.AddComponent<MeshFilter>();
        game_object.AddComponent<MeshRenderer>();
        game_object.AddComponent<MeshCollider>();

        MeshFilter mesh_filter = game_object.GetComponent<MeshFilter>();
        MeshCollider mesh_collider = game_object.GetComponent<MeshCollider>();
        Mesh mesh = mesh_filter.mesh;
        MeshRenderer render = game_object.GetComponent<MeshRenderer>();

        //

        mesh.Clear();
        mesh.name = "wall mesh";

        this.create_wall_mesh_sub(mesh, start, end, side, 1.0f);

        //render.material = this.material;

        /*if(side == WALL_SIDE.LEFT) {

			render.material.color = Color.green;

		} else {

			render.material.color = Color.blue;
		}*/
        render.material = this.wall_material;

        // 纰版挒缃戞牸

        Mesh coli_mesh = new Mesh();

        coli_mesh.name = "wall mesh(coli)";

        this.create_wall_mesh_sub(coli_mesh, start, end, side, RoadCreator.WallHeight);

        mesh_collider.sharedMesh = coli_mesh;
        mesh_collider.enabled = true;

        //
    }

    // 鐢熸垚澧欑殑澶氳竟褰√
    private void create_wall_mesh_sub(Mesh mesh, int start, int end, WALL_SIDE side, float height)
    {
        int point_num = end - start + 1;

        // ---------------------------------------------------- //
        // 澧欑殑鎴?潰褰㈢姸

        Vector3[] wall_vertices;

        wall_vertices = new Vector3[4];

        wall_vertices[0] = Vector3.zero;
        wall_vertices[1] = wall_vertices[0] + Vector3.up * 0.5f;
        wall_vertices[2] = wall_vertices[1] + Vector3.right * (1.0f);
        wall_vertices[3] = wall_vertices[2] + Vector3.up * height;

        // ---------------------------------------------------- //
        // 椤剁偣锛堜綅缃?潗鏍囷紝uv锛執

        // 涓涓?煩褰㈡墍闇瑕佺殑椤剁偣绱㈠紩锛堜袱涓?笁瑙掑舰鍥犳?鏄?涓?級
        const int quad_index_num = 6;

        Vector3[] vertices = new Vector3[point_num * wall_vertices.Length];
        Vector2[] uvs = new Vector2[point_num * wall_vertices.Length];
        int[] triangles = new int[(point_num - 1) * wall_vertices.Length * quad_index_num];

        Section section;
        Vector2 uv;

        if (side == WALL_SIDE.LEFT)
        {

            for (int i = 0; i < point_num; i++)
            {

                section = this.sections[start + i];

                for (int j = 0; j < wall_vertices.Length; j++)
                {

                    vertices[i * wall_vertices.Length + j] = section.positions[0];
                    vertices[i * wall_vertices.Length + j] += -wall_vertices[j].x * section.right + wall_vertices[j].y * section.up;

                    //

                    uv.x = (float)j / (float)(wall_vertices.Length - 1);
                    uv.y = (float)i * 2.0f + 0.5f;
                    uvs[i * wall_vertices.Length + j] = uv;
                }
            }

        }
        else
        {

            for (int i = 0; i < point_num; i++)
            {

                section = this.sections[start + i];

                for (int j = 0; j < wall_vertices.Length; j++)
                {

                    vertices[i * wall_vertices.Length + j] = section.positions[1];
                    vertices[i * wall_vertices.Length + j] += wall_vertices[j].x * section.right + wall_vertices[j].y * section.up;

                    //

                    uv.x = (float)j / (float)(wall_vertices.Length - 1);
                    uv.y = (float)i * 2.0f + 0.5f;
                    uvs[i * wall_vertices.Length + j] = uv;
                }
            }
        }

        // ---------------------------------------------------- //
        // 鐢熸垚涓夎?褰?紙椤剁偣绱㈠紩鏁扮粍锛執

        int position_index = 0;
        int i00, i10, i01, i11;

        if (side == WALL_SIDE.LEFT)
        {

            for (int i = 0; i < point_num - 1; i++)
            {

                for (int j = 0; j < wall_vertices.Length - 1; j++)
                {

                    i00 = (i + 1) * wall_vertices.Length + (j + 1);     // 宸︿笂
                    i10 = (i + 1) * wall_vertices.Length + (j + 0);     // 鍙充笂
                    i01 = (i + 0) * wall_vertices.Length + (j + 1);     // 宸︿笅
                    i11 = (i + 0) * wall_vertices.Length + (j + 0);     // 鍙充笅

                    RoadCreator.add_quad_index(triangles, position_index, i00, i10, i01, i11);
                    position_index += 6;
                }
            }

        }
        else
        {

            for (int i = 0; i < point_num - 1; i++)
            {

                for (int j = 0; j < wall_vertices.Length - 1; j++)
                {

                    i00 = (i + 1) * wall_vertices.Length + (j + 0);     // 宸︿笂
                    i10 = (i + 1) * wall_vertices.Length + (j + 1);     // 鍙充笂
                    i01 = (i + 0) * wall_vertices.Length + (j + 0);     // 宸︿笅
                    i11 = (i + 0) * wall_vertices.Length + (j + 1);     // 鍙充笅

                    RoadCreator.add_quad_index(triangles, position_index, i00, i10, i01, i11);
                    position_index += 6;
                }
            }
        }

        //
        // 鐢变簬浼氬嚭鐜拌?鍛婏紝杩欓噷鐢熸垚uv锛堥傚綋鏁板硷級

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.uv2 = uvs;
        mesh.triangles = triangles;

        mesh.Optimize();
        mesh.RecalculateNormals();

    }

    // 灏嗙煩褰?紙涓夎?褰⒚栺锛夎拷鍔犲埌绱㈠紩鏁扮粍
    //
    // i00--i10
    //  |    |
    // i01--i11
    //
    private static void add_quad_index(int[] indices, int index, int i00, int i10, int i01, int i11)
    {
        indices[index++] = i10;
        indices[index++] = i11;
        indices[index++] = i00;

        indices[index++] = i11;
        indices[index++] = i01;
        indices[index++] = i00;
    }

    // 灏嗗唴闈㈣拷鍔犲埌mesh 
    private void add_backface_trianbles_to_mesh(Mesh mesh)
    {
        int face_num = mesh.triangles.Length / 3;

        int[] faces = new int[face_num * 3 * 2];

        for (int i = 0; i < face_num; i++)
        {

            faces[i * 3 + 0] = mesh.triangles[i * 3 + 0];
            faces[i * 3 + 1] = mesh.triangles[i * 3 + 1];
            faces[i * 3 + 2] = mesh.triangles[i * 3 + 2];
        }

        for (int i = 0; i < face_num; i++)
        {

            faces[(face_num + i) * 3 + 0] = mesh.triangles[i * 3 + 2];
            faces[(face_num + i) * 3 + 1] = mesh.triangles[i * 3 + 1];
            faces[(face_num + i) * 3 + 2] = mesh.triangles[i * 3 + 0];
        }

        mesh.triangles = faces;
    }

    // 鍒犻櫎鎵鏈夌殑鐢熸垚瀵硅薄
    public void clearOutput()
    {
        if (this.is_created)
        {

            foreach (var road_mesh in this.road_mesh)
            {

                GameObject.Destroy(road_mesh);
            }

            foreach (var wall_mesh in this.wall_mesh)
            {

                GameObject.Destroy(wall_mesh);
            }

            this.road_mesh = null;
            this.wall_mesh = null;

            this.sections = null;

            this.positions = null;
            this.position_num = 0;

            this.is_created = false;
        }
    }

    // 姹傚嚭璧涢亾涓婄殑浣嶇疆
    public Vector3 getPositionAtPlace(float place)
    {
        int place_i = (int)place;
        float place_f = place - (float)place_i;

        if (place_i >= this.sections.Length - 1)
        {

            place_i = this.sections.Length - 1 - 1;
            place_f = 1.0f;
        }

        RoadCreator.Section section_prev = this.sections[place_i];
        RoadCreator.Section section_next = this.sections[place_i + 1];

        Vector3 position = Vector3.Lerp(section_prev.center, section_next.center, place_f);

        return (position);
    }
    public Quaternion getRotationAtPlace(float place)
    {
        int place_i = (int)place;

        if (place_i >= this.sections.Length - 1)
        {

            place_i = this.sections.Length - 1 - 1;
        }

        RoadCreator.Section section_prev = this.sections[place_i];
        //RoadCreator.Section		section_next = this.sections[place_i + 1];

        Quaternion rotation = Quaternion.LookRotation(section_prev.direction, section_prev.up);

        return (rotation);
    }
    public Quaternion getSmoothRotationAtPlace(float place)
    {
        int place_i = (int)place;
        float place_f = place - (float)place_i;

        if (place_i >= this.sections.Length - 1)
        {

            place_i = this.sections.Length - 1 - 1;
            place_f = 1.0f;
        }

        RoadCreator.Section section_prev = this.sections[place_i];
        RoadCreator.Section section_next = this.sections[place_i + 1];

        Quaternion rotation_prev = Quaternion.LookRotation(section_prev.direction, section_prev.up);
        Quaternion rotation_next = Quaternion.LookRotation(section_next.direction, section_next.up);

        Quaternion rotation = Quaternion.Lerp(rotation_prev, rotation_next, place_f);

        return (rotation);
    }


}
