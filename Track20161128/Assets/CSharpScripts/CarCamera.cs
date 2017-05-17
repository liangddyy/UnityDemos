using UnityEngine;
using System.Collections;

public class CarCamera : MonoBehaviour
{
	public Transform	target = null;

	public float		height = 1f;
	public float		positionDamping = 3f;
	public float		velocityDamping = 3f;
	public float		distance = 4f;

	public LayerMask	ignoreLayers = -1;

	private RaycastHit	hit = new RaycastHit();

	private Vector3		prevVelocity = Vector3.zero;
	private LayerMask	raycastLayers = -1;
	
	private Vector3		currentVelocity = Vector3.zero;
	
	void Start()
	{
		raycastLayers = ~ignoreLayers;
	}

	void FixedUpdate()
	{
		Vector3		target_velocity;

		target_velocity = target.root.GetComponent<Rigidbody>().velocity;

		// 当赛车�?驶时，防止摄像机跑到赛车的前斸�		//
		if(Vector3.Dot(target_velocity, target.TransformDirection(Vector3.forward)) < 0.0f) {

			target_velocity = -target_velocity;
		}

		currentVelocity   = Vector3.Lerp(prevVelocity, target_velocity, velocityDamping*Time.deltaTime);
		currentVelocity.y = 0;
		prevVelocity = currentVelocity;
	}
	
	void LateUpdate()
	{
		this.calcPosture();
	}

	public void	calcPosture()
	{
		float	speedFactor = Mathf.Clamp01(target.root.GetComponent<Rigidbody>().velocity.magnitude / 70.0f);

		GetComponent<Camera>().fieldOfView = Mathf.Lerp(55, 72, speedFactor);

		float	currentDistance = Mathf.Lerp(7.5f, 6.5f, speedFactor);
		
		currentVelocity = currentVelocity.normalized;
		
		Vector3	newTargetPosition = target.position + Vector3.up * height;

		Vector3	newPosition       = newTargetPosition - (currentVelocity * currentDistance);
		newPosition.y = newTargetPosition.y;
		
		Vector3	targetDirection = newPosition - newTargetPosition;

		if(Physics.Raycast(newTargetPosition, targetDirection, out hit, currentDistance, raycastLayers))
			newPosition = hit.point;
		
		transform.position = newPosition;
		transform.LookAt(newTargetPosition);
	}

	// 重置（赛轥�生成后立刻调�?��
	public void	reset()
	{
		// 生成赛车名�rigidbody.velocity 值为0,因�?使用 rotation 
		// 来求出摄像机的方吐�		//
		this.currentVelocity   = this.target.TransformDirection(Vector3.forward);
		this.currentVelocity.y = 0.0f;

		this.prevVelocity = this.currentVelocity;
	}


	public void	setEnable(bool sw)
	{
		this.enabled = sw;
	}
}
