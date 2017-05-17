using UnityEngine;
using System.Collections;
using System.IO;

public class LineDrawerControl : MonoBehaviour
{

    enum STEP
    {

        NONE = -1,

        IDLE = 0,       // 寰呮満
        DRAWING,        // 鎻忕粯鐩寸嚎锛堟嫋鍔ㄨ繃绋嬩腑锛執
        DRAWED,         // 缁撴潫鎻忕粯

        NUM,
    };

    private STEP step = STEP.NONE;
    private STEP next_step = STEP.NONE;

    public Vector3[] positions;
    public int position_num = 0;

    private static int POSITION_NUM_MAX = 1000;

    public ToolControl root = null;

    private MousePositionSmoother smoother;

    private Vector3 previous_mouse_position;        // 榧犳爣鐨勪笂涓涓?綅缃?
    private bool is_play_drawing_sound;         // 鐢荤嚎鏃舵槸鍚︽挱鏀鹃煶鏁堬紵
    private float sound_to_stop_timer = 0.0f;       // 鐢ㄤ簬鍦ㄧ敾绾挎椂闊虫晥鍋滄?鍒ゆ柇鐨勮?鏃跺櫒


    // ------------------------------------------------------------------------ //

    void Start()
    {
        this.positions = new Vector3[POSITION_NUM_MAX];

        this.smoother = new MousePositionSmoother();
        this.smoother.create();

        this.previous_mouse_position = Vector3.zero;
        this.is_play_drawing_sound = false;
    }

    void Update()
    {
        // 鐘舵佽縼绉绘?娴娞
        if (this.next_step == STEP.NONE)
        {

            switch (this.step)
            {

                case STEP.NONE:
                    {
                        this.next_step = STEP.IDLE;
                    }
                    break;

                case STEP.IDLE:
                    {
                        if (Input.GetMouseButton(0))
                        {

                            this.next_step = STEP.DRAWING;
                        }
                    }
                    break;

                case STEP.DRAWING:
                    {
                        if (!Input.GetMouseButton(0))
                        {

                            if (this.position_num >= 2)
                            {

                                this.next_step = STEP.DRAWED;

                            }
                            else
                            {

                                this.next_step = STEP.IDLE;
                            }

                            this.GetComponent<AudioSource>().Stop();
                            this.is_play_drawing_sound = false;
                        }
                    }
                    break;
            }
        }

        // 鐘舵佽縼绉绘椂鐨勫垵濮嬪寲

        if (this.next_step != STEP.NONE)
        {

            switch (this.next_step)
            {

                case STEP.IDLE:
                    {
                        // 鍒犻櫎涓婃?鐢熸垚鐨勫?璞犔

                        this.position_num = 0;

                        this.update_line_renderer();

                        this.smoother.reset();
                    }
                    break;

                case STEP.DRAWING:
                    {
                        this.smoother.reset();

                        this.previous_mouse_position = Input.mousePosition;
                        this.is_play_drawing_sound = false;
                    }
                    break;
            }

            this.step = this.next_step;

            this.next_step = STEP.NONE;
        }

        // 鍚勪釜鐘舵佺殑澶勭悊

        switch (this.step)
        {

            case STEP.DRAWING:
                {
                    this.execute_step_drawing();
                }
                break;

            case STEP.DRAWED:
                {
                    for (int i = 0; i < this.position_num - 1; i++)
                    {

                        Debug.DrawLine(this.positions[i], this.positions[i + 1], Color.red, 0.0f, false);
                    }
                }
                break;
        }
    }

    private void execute_step_drawing()
    {
        Vector3 mouse_position = Input.mousePosition;

        // 瀵瑰厜鏍囦綅缃?仛骞虫粦澶勭悊
        mouse_position = this.smoother.append(mouse_position);

        Vector3 position;

        // 瀵归紶鏍囧厜鏍囦綅缃?繘琛岄嗛忚?鍙樻崲
        if (this.root.unproject_mouse_position(out position, mouse_position))
        {

            this.execute_step_drawing_sub(mouse_position, position);
        }
    }

    private void execute_step_drawing_sub(Vector3 mouse_position, Vector3 position_3d)
    {
        // 椤剁偣鐨勯棿闅旓紙锛濋亾璺??杈瑰舰绾靛悜闀垮害锛執
        float append_distance = RoadCreator.PolygonSize.z;

        int append_num = 0;

        while (true)
        {

            bool is_append_position;

            // 寰绾挎潯涓婅拷鍔犻《鐐癸紝鎵ц?妫娴娞

            is_append_position = false;

            if (this.position_num == 0)
            {

                // 绗?竴涓?棤鏉′欢杩藉姞

                is_append_position = true;

            }
            else if (this.position_num >= POSITION_NUM_MAX)
            {

                // 濡傛灉瓒呰繃鏈澶т釜鏁板垯鏃犳硶杩藉姞

                is_append_position = false;

            }
            else
            {

                // 娣诲姞鍜屼笂娆¤?娣诲姞鐨勯《鐐硅窛绂讳竴瀹氶棿闅旂殑鐐柑

                if (Vector3.Distance(this.positions[this.position_num - 1], position_3d) > append_distance)
                {

                    is_append_position = true;
                }
            }

            if (!is_append_position)
            {

                break;
            }

            //

            if (this.position_num == 0)
            {

                this.positions[this.position_num] = position_3d;

            }
            else
            {

                // 銉诲湪杩炴帴鈥滀笂涓娆¤拷鍔犵殑椤剁偣鈥濆拰鈥滈紶鏍囧厜鏍囦綅缃?濈殑鐩寸嚎涓壧
                // 銉昏拷鍔犲拰鈥滀笂涓娆¤拷鍔犵殑椤剁偣鈥濊窛绂讳负鈥渁ppend_distance鈥濈殑椤剁偣

                Vector3 distance = position_3d - this.positions[this.position_num - 1];

                distance *= append_distance / distance.magnitude;

                this.positions[this.position_num] = this.positions[this.position_num - 1] + distance;
            }

            this.position_num++;

            //

            append_num++;
        }

        if (append_num > 0)
        {

            // 閲嶆柊鐢熸垚LineRender
            this.update_line_renderer();
        }

        // 鎺у埗鐢荤嚎鏃剁殑SE

        this.drawing_sound_control(mouse_position);
    }

    // 鍒犻櫎鐩寸嚎
    public void clear()
    {
        this.next_step = STEP.IDLE;

        this.Update();
    }

    // 鏄?惁鐢荤嚎锛炋
    public bool isLineDrawed()
    {
        bool is_drawed = (this.step == STEP.DRAWED);

        return (is_drawed);
    }

    // 鏄剧ず锛忎笉鏄剧ず
    public void setVisible(bool visible)
    {
        this.set_line_render_visible(visible);
    }

    // 璇诲彇鏂囦欢
    public void loadFromFile(BinaryReader Reader)
    {
        this.position_num = Reader.ReadInt32();

        for (int i = 0; i < this.position_num; i++)
        {

            this.positions[i].x = (float)Reader.ReadDouble();
            this.positions[i].y = (float)Reader.ReadDouble();
            this.positions[i].z = (float)Reader.ReadDouble();
        }

        // 閲嶆柊鐢熸垚LineRender
        this.update_line_renderer();

        this.next_step = STEP.DRAWED;

        this.Update();
    }

    public void saveToFile(BinaryWriter Writer)
    {
        Writer.Write((int)this.position_num);

        for (int i = 0; i < this.position_num; i++)
        {

            Writer.Write((double)this.positions[i].x);
            Writer.Write((double)this.positions[i].y);
            Writer.Write((double)this.positions[i].z);
        }
    }

    // ---------------------------------------------------------------- //

    // 鏄剧ず锛忎笉鏄剧ず 鐩寸嚎
    private void set_line_render_visible(bool visible)
    {
        this.GetComponent<LineRenderer>().enabled = visible;
    }

    // 閲嶆柊鐢熸垚LineRender
    private void update_line_renderer()
    {
        this.GetComponent<LineRenderer>().SetVertexCount(this.position_num);

        for (int i = 0; i < this.position_num; i++)
        {

            this.GetComponent<LineRenderer>().SetPosition(i, this.positions[i]);
        }
    }


    private float DRAW_SE_VOLUME_MIN = 0.3f;
    private float DRAW_SE_VOLUME_MAX = 1.0f;

    private float DRAW_SE_PITCH_MIN = 0.75f;
    private float DRAW_SE_PITCH_MAX = 1.5f;

    // 鎺у埗鐢荤嚎鏃剁殑SE
    private void drawing_sound_control(Vector3 mouse_position)
    {
        float distance = Vector3.Distance(mouse_position, this.previous_mouse_position) / Time.deltaTime;

        // 榧犳爣闈欐?鏃堕棿瓒呰繃杩欎釜鍊硷紝鍒欏仠姝㈢敾绾跨殑SE
        // 濡傛灉涓嶈繖鏍峰?鐞嗗皢浼氬嚭鐜版潅闊蔡
        const float stop_time = 0.3f;

        if (this.is_play_drawing_sound)
        {

            if (distance < 60.0f)
            {

                // 榧犳爣鐨勭Щ鍔ㄩ噺鍙樺皬

                this.sound_to_stop_timer += Time.deltaTime;

                if (this.sound_to_stop_timer > stop_time)
                {

                    this.GetComponent<AudioSource>().Stop();
                    this.is_play_drawing_sound = false;
                    this.sound_to_stop_timer = 0.0f;
                }

            }
            else
            {

                this.sound_to_stop_timer = 0.0f;

            }

        }
        else
        {

            if (distance >= 60.0f)
            {

                this.GetComponent<AudioSource>().Play();
                this.is_play_drawing_sound = true;
                this.sound_to_stop_timer = 0.0f;
            }
        }

        // 閫氳繃鐢荤嚎鐨勯熷害鏀瑰彉闊宠皟鍜岄煶閲幪

        if (this.is_play_drawing_sound)
        {

            float speed_rate;

            speed_rate = Mathf.InverseLerp(60.0f, 500.0f, distance);

            speed_rate = Mathf.Clamp01(speed_rate);

            speed_rate = Mathf.Round(speed_rate * 3.0f) / 3.0f;

            // 闊抽噺

            float next_volume = Mathf.Lerp(DRAW_SE_VOLUME_MIN, DRAW_SE_VOLUME_MAX, speed_rate);
            float current_volume = this.GetComponent<AudioSource>().volume;

            float diff = next_volume - current_volume;

            if (diff > 0.1f)
            {

                diff = 0.1f;

            }
            else if (diff < -0.05f)
            {

                diff = -0.05f;
            }

            next_volume = current_volume + diff;

            this.GetComponent<AudioSource>().volume = next_volume;

            // 闊宠皟

            float next_pitch = Mathf.Lerp(DRAW_SE_PITCH_MIN, DRAW_SE_PITCH_MAX, speed_rate);
            float current_pitch = this.GetComponent<AudioSource>().pitch;

            float pitch_diff = next_pitch - current_pitch;

            if (pitch_diff > 0.1f)
            {

                pitch_diff = 0.1f;

            }
            else if (pitch_diff < -0.1f)
            {

                pitch_diff = -0.1f;
            }

            next_pitch = current_pitch + pitch_diff;

            this.GetComponent<AudioSource>().pitch = next_pitch;

        }

        this.previous_mouse_position = mouse_position;
    }

    // ---------------------------------------------------------------- //

}