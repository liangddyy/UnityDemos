using UnityEngine;
using System.Collections;

public class JumpSlopeCreator {

	public RoadCreator		road_creator = null;
	public GameObject		main_camera;

	public bool				is_created = false;
	public bool				is_draw_icon = false;

	public GameObject		JumpSlopePrefab = null;		// 跳台

	public float			place = 0.0f;

	public float			place_min, place_max;

	// ---------------------------------------------------------------- //

	public Texture			texture_jump_icon;
	public SimpleSpriteGUI	jump_icon;

	public GameObject		jump_slope_object = null;

	// ---------------------------------------------------------------- //

	public void		create()
	{
		this.jump_icon = new SimpleSpriteGUI();
		this.jump_icon.create();
		this.jump_icon.setTexture(this.texture_jump_icon);
		this.jump_icon.setScale(new Vector3(0.5f, 0.5f, 1.0f));
	}

	public void		onGUI()
	{
		if(this.is_created) {

			if(this.is_draw_icon) {

				this.jump_icon.draw();
			}
		}
	}

	public void		createJumpSlope()
	{
		if(this.jump_slope_object == null) {

			this.jump_slope_object = GameObject.Instantiate(this.JumpSlopePrefab) as GameObject;

			this.is_created = true;

			this.setPlace(this.place);
		}
	}

	public void		clearOutput()
	{
		if(this.jump_slope_object != null) {

			GameObject.Destroy(this.jump_slope_object);

			this.jump_slope_object = null;

			this.place = this.place_min;

			this.is_created = false;
		}
	}

	// 设置跳台的位置
	public void	setPlace(float place)
	{
		this.place = place;
		this.place = Mathf.Clamp(this.place, this.place_min, this.place_max);

		if(this.is_created) {

			Vector3		position = this.road_creator.getPositionAtPlace(this.place);
			Quaternion	rotation = this.road_creator.getSmoothRotationAtPlace(this.place);

			this.jump_slope_object.transform.position = position;
			this.jump_slope_object.transform.rotation = rotation;

			//

			Vector3		screen_position = this.main_camera.GetComponent<Camera>().WorldToScreenPoint(position);

			screen_position -= new Vector3(Screen.width/2.0f, Screen.height/2.0f, 0.0f);

			screen_position.y += this.jump_icon.texture.height/2.0f*this.jump_icon.getScale().y;

			this.jump_icon.setPosition(screen_position);
		}
	}

	// 设置图标的显示／隐藏
	public void		setIsDrawIcon(bool sw)
	{
		this.is_draw_icon = sw;
	}
}
