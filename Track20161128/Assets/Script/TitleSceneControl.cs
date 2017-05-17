using UnityEngine;
using System.Collections;

public class TitleSceneControl : MonoBehaviour
{

    // 閫茶?鐘舵厠.
    public enum STEP
    {

        NONE = -1,

        TITLE = 0,              // 鏄剧ず鏍囬?锛堢瓑寰呮寜閿?級
        WAIT_SE_END,            // 绛夊緟寮濮嬮煶鏁堢粨鏉炋

        NUM,
    };

    private STEP step = STEP.NONE;
    private STEP next_step = STEP.NONE;
    private float step_timer = 0.0f;

    // -------------------------------------------------------------------------------- //

    // Use this for initialization
    void Start()
    {

        this.next_step = STEP.TITLE;
    }

    // Update is called once per frame
    void Update()
    {

        this.step_timer += Time.deltaTime;

        // 妫娴嬫槸鍚﹁縼绉诲埌涓嬩竴涓?姸鎬?
        switch (this.step)
        {

            case STEP.TITLE:
                {
                    // 榧犳爣琚?寜涓娞
                    //
                    if (Input.GetMouseButtonDown(0))
                    {

                        this.next_step = STEP.WAIT_SE_END;
                    }
                }
                break;

            case STEP.WAIT_SE_END:
                {
                    // SE 鎾?斁缁撴潫鍚庯紝缁撴潫鍦烘櫙杞藉叆

                    bool to_finish = true;

                    do
                    {

                        if (!this.GetComponent<AudioSource>().isPlaying)
                        {

                            break;
                        }

                        if (this.GetComponent<AudioSource>().time >= this.GetComponent<AudioSource>().clip.length)
                        {

                            break;
                        }

                        to_finish = false;

                    } while (false);

                    if (to_finish)
                    {

                        Application.LoadLevel("GameScene");
                    }
                }
                break;
        }

        // 鐘舵佸彉鍖栨椂鐨勫垵濮嬪寲澶勭悊

        if (this.next_step != STEP.NONE)
        {

            switch (this.next_step)
            {

                case STEP.WAIT_SE_END:
                    {
                        // 鎾?斁寮濮嬬殑SE
                        this.GetComponent<AudioSource>().Play();
                    }
                    break;
            }

            this.step = this.next_step;
            this.next_step = STEP.NONE;

            this.step_timer = 0.0f;
        }

        // 鍚勪釜鐘舵佺殑鎵ц?澶勭悊

        /*switch(this.step) {

			case STEP.TITLE:
			{
			}
			break;
		}*/

    }
}
