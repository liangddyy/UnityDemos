using UnityEngine;
using System.Collections;
using System.Linq;

public class ToolGUI : MonoBehaviour {

	public enum BUTTON {

		NONE = -1,

		NEW = 0,					// 清空
		LOAD,						// 载入
		SAVE,						// 保存

		CREATE_ROAD,				// 生成道路
		RUN,						// 赛车奔跑

		TUNNEL_CREATE,				// 生成隧道
		TUNNEL_FORWARD,				// 向前移动隧道
		TUNNEL_BACKWARD,			// 向后移动隧道

		FOREST_CREATE,				// 生成树林
		FOREST_START_FORWARD,		// 将树林的开始地点向前移动
		FOREST_START_BACKWARD,		// 将树林的开始地点向后移动
		FOREST_END_FORWARD,			// 将树林的结束地点向前移动
		FOREST_END_BACKWARD,		// 将树林的结束地点向后移动

		BUIL_CREATE,				// 创建建筑街道
		BUIL_START_FORWARD,			// 将建筑街道开始地点往前移动
		BUIL_START_BACKWARD,		// 将建筑街道结束地点往后移动
		BUIL_END_FORWARD,			// 将建筑街道开始地点往前移动
		BUIL_END_BACKWARD,			// 将建筑街道结束地点往后移动

		JUMP_CREATE,				// 创建跳台
		JUMP_FORWARD,				// 将跳台往前移动
		JUMP_BACKWARD,				// 将跳台向后移动

		NUM,
	};

	public struct Button {

		public bool	current;
		public bool	previous;

		public bool	trigger_on;
		public bool	trigger_off;

		public bool	is_repeat_button;

		public float	pushed_timer;		// 按住按钮的时长
	};

	public Button[] buttons;

	public GUISkin	gui_skin;

	public AudioClip		audio_clip_click;		// 点击音效

	public bool				is_edit_mode = true;	// 是否为编辑模式？

	// ------------------------------------------------------------------------ //

	// Use this for initialization
	void Start () {

		this.buttons = new Button[(int)BUTTON.NUM];

		foreach(var i in this.buttons.Select((v, i) => i)) {

			this.buttons[i].previous = false;
			this.buttons[i].current  = false;

			this.buttons[i].pushed_timer = 0.0f;

			//

			this.buttons[i].is_repeat_button = false;

			switch((BUTTON)i) {

				case BUTTON.TUNNEL_FORWARD:
				case BUTTON.TUNNEL_BACKWARD:
				case BUTTON.FOREST_START_FORWARD:
				case BUTTON.FOREST_START_BACKWARD:
				case BUTTON.FOREST_END_FORWARD:
				case BUTTON.FOREST_END_BACKWARD:
				case BUTTON.BUIL_START_FORWARD:
				case BUTTON.BUIL_START_BACKWARD:
				case BUTTON.BUIL_END_FORWARD:
				case BUTTON.BUIL_END_BACKWARD:
				case BUTTON.JUMP_FORWARD:
				case BUTTON.JUMP_BACKWARD:
				{
					this.buttons[i].is_repeat_button = true;
				}
				break;
			}
		}

		this.is_edit_mode = true;
	}
	
	private bool	is_mouse_button_current = false;
	private bool	is_mouse_button_down    = false;

	void Update ()
	{

		this.is_mouse_button_current = Input.GetMouseButton(0);
		this.is_mouse_button_down    = Input.GetMouseButtonDown(0);
	}


	public void	onStartTestRun()
	{
		this.is_edit_mode = false;
	}
	public void	onStopTestRun()
	{
		this.is_edit_mode = true;
	}

	void OnGUI()
	{
		GUI.skin = this.gui_skin;

		//

		if(Event.current.type == EventType.Layout) {

			foreach(var i in this.buttons.Select((v, i) => i)) {

				this.buttons[i].previous = this.buttons[i].current;
				this.buttons[i].current  = false;
			}
		}

		//

		int		x, y;

		y = 20;
		x = 10;

		if(this.is_edit_mode) {

			this.on_gui_file(x, y);
		}

		x += 110;

		this.on_gui_road(x, y);
		x += 110;

		this.on_gui_tunnel(x, y);
		x += 110;

		this.on_gui_forest(x, y);
		x += 100;

		this.on_gui_buil(x, y);
		x += 100;

		this.on_gui_jump(x, y);
		x += 100;

		// RepeatButton 当按钮被一直按住移动光标时，有一瞬间
		// 按钮会变为松开状态，
		// 因此即使current 值为 false，在“之前一直被按住”并且“鼠标被按下”时也被视作按钮被按下
		if(Event.current.type == EventType.Layout) {

			foreach(var i in this.buttons.Select((v, i) => i)) {

				if(this.buttons[i].is_repeat_button) {

					if(this.buttons[i].previous && this.is_mouse_button_current) {

						this.buttons[i].current = true;
					}
				}
			}
		}

		//

		foreach(var i in this.buttons.Select((v, i) => i)) {

			this.buttons[i].trigger_on  = !this.buttons[i].previous &&  this.buttons[i].current;
			this.buttons[i].trigger_off =  this.buttons[i].previous && !this.buttons[i].current;

			if(Event.current.type == EventType.Repaint) {

				if(this.buttons[i].current) {
	
					this.buttons[i].pushed_timer += Time.deltaTime;
	
				} else {
	
					this.buttons[i].pushed_timer = 0.0f;
				}
			}

			// 按下瞬间的SE
			//

			if(this.buttons[i].trigger_on) {

				if(i == (int)BUTTON.CREATE_ROAD) {

				} else {

					//this.audio.PlayOneShot(this.audio_clip_click);
				}
			}
		}
	}

	// 文件相关
	private void	on_gui_file(int x, int y)
	{
		if(GUI.Button(new Rect(x, y, 100, 20), "消除")) {

			this.buttons[(int)BUTTON.NEW].current = true;

		}
		y += 30;
#if UNITY_EDITOR
		if(GUI.Button(new Rect(x, y, 100, 20), "载入读取")) {

			this.buttons[(int)BUTTON.LOAD].current = true;
		}
		y += 30;

		if(GUI.Button(new Rect(x, y, 100, 20), "写入")) {

			this.buttons[(int)BUTTON.SAVE].current = true;
		}
		y += 30;
#endif
	}

	// 道路生成相关
	private void	on_gui_road(int x, int y)
	{
		if(GUI.Button(new Rect(x, y, 100, 20), "生成道路")) {

			this.buttons[(int)BUTTON.CREATE_ROAD].current = true;
		}
		y += 30;

		string	text;

		if(this.is_edit_mode) {

			text = "赛车奔跑";

		} else {

			text = "返回";
		}

		if(GUI.Button(new Rect(x, y, 100, 20), text)) {

			this.buttons[(int)BUTTON.RUN].current = true;
		}
		y += 30;
	}

	// 隧道相关
	private void	on_gui_tunnel(int x, int y)
	{
		if(GUI.Button(new Rect(x, y, 100, 20), "漫长隧道")) {

			this.buttons[(int)BUTTON.TUNNEL_CREATE].current = true;
		}
		y += 30;

		if(GUI.RepeatButton(new Rect(x, y, 40, 20), "<<")) {

			this.buttons[(int)BUTTON.TUNNEL_FORWARD].current = true;
		}
		if(GUI.RepeatButton(new Rect(x + 50, y, 40, 20), ">>")) {

			this.buttons[(int)BUTTON.TUNNEL_BACKWARD].current = true;
		}
		y += 30;
	}

	// 树林相关
	private void	on_gui_forest(int x, int y)
	{
		if(GUI.Button(new Rect(x, y, 90, 20), "创建树林")) {

			this.buttons[(int)BUTTON.FOREST_CREATE].current = true;
		}
		y += 30;

		if(GUI.RepeatButton(new Rect(x, y, 40, 20), "<<")) {

			this.buttons[(int)BUTTON.FOREST_START_BACKWARD].current = true;
		}
		if(GUI.RepeatButton(new Rect(x + 50, y, 40, 20), ">>")) {

			this.buttons[(int)BUTTON.FOREST_START_FORWARD].current = true;
		}
		y += 30;

		if(GUI.RepeatButton(new Rect(x, y, 40, 20), "<<")) {

			this.buttons[(int)BUTTON.FOREST_END_BACKWARD].current = true;
		}
		if(GUI.RepeatButton(new Rect(x + 50, y, 40, 20), ">>")) {

			this.buttons[(int)BUTTON.FOREST_END_FORWARD].current = true;
		}
		y += 30;
	}

	// 建筑相关
	private void	on_gui_buil(int x, int y)
	{
		if(GUI.Button(new Rect(x, y, 90, 20), "高大建筑")) {

			this.buttons[(int)BUTTON.BUIL_CREATE].current = true;
		}
		y += 30;

		if(GUI.RepeatButton(new Rect(x, y, 40, 20), "<<")) {

			this.buttons[(int)BUTTON.BUIL_START_BACKWARD].current = true;
		}
		if(GUI.RepeatButton(new Rect(x + 50, y, 40, 20), ">>")) {

			this.buttons[(int)BUTTON.BUIL_START_FORWARD].current = true;
		}
		y += 30;

		if(GUI.RepeatButton(new Rect(x, y, 40, 20), "<<")) {

			this.buttons[(int)BUTTON.BUIL_END_BACKWARD].current = true;
		}
		if(GUI.RepeatButton(new Rect(x + 50, y, 40, 20), ">>")) {

			this.buttons[(int)BUTTON.BUIL_END_FORWARD].current = true;
		}
		y += 30;
	}

	// 跳台相关
	private void	on_gui_jump(int x, int y)
	{
		if(GUI.Button(new Rect(x, y, 90, 20), "还可以飞跃")) {

			this.buttons[(int)BUTTON.JUMP_CREATE].current = true;
		}
		y += 30;

		if(GUI.RepeatButton(new Rect(x, y, 40, 20), "<<")) {

			this.buttons[(int)BUTTON.JUMP_BACKWARD].current = true;
		}
		if(GUI.RepeatButton(new Rect(x + 50, y, 40, 20), ">>")) {

			this.buttons[(int)BUTTON.JUMP_FORWARD].current = true;
		}
		y += 30;
	}
}
