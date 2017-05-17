using UnityEngine;
using System.Collections;

public class CarSoundControl_ : MonoBehaviour
{

    public AudioClip audio_clip_engine; // 引擎声音（循�?��
    public AudioClip audio_clip_hit_wall; // 碰撞墙�?声音
    public AudioClip audio_clip_landing; // 落地声音（跃起后（�
    public AudioSource audio_engine;

    // ------------------------------------------------------------------------ //

    // Use this for initialization
    void Start()
    {

        this.audio_engine = this.gameObject.AddComponent<AudioSource>();


        this.audio_engine.clip = this.audio_clip_engine;
        this.audio_engine.loop = true;
        this.audio_engine.Play();
    }

    private bool is_contact_wall = false; // �?��和�?壁发生�?撞？
    private float wall_hit_timer = 0.0f; // 撞�?后的计时�?	private float	hit_speed_wall = 0.0f;			// 撞�?后的速度

    private bool is_landing = false; //
}