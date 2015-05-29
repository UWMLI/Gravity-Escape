using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour {
	public float mass;
	public float thrustMagnitude; //assign in inspector
	public float fuel;
	public float initialVelocityAngle; //assign in inspector
	public float initialVelocityMagnitude; // >= 0
	public float initialVelocityAppliedTime;
	[HideInInspector]public float initialVelocityTimeLeft;
	public Vector2 flyingDirection;
	public bool thrusting;
	public Vector2 currentThrust;
	public Vector2 combinedGravitation;
	[HideInInspector]public Vector2 rocketLastPosition;
	public int state;
	public enum State{
		preLaunch,
		flying,
		thrusting,
		crashing
	};
	[HideInInspector]Vector2 initialPosition;
	[HideInInspector]public GhostTrail ghostTrail;
	void Awake(){
		GameControl.rocket = this;
		if(thrustMagnitude <= 0f){
			thrustMagnitude = 50f;
		}
		initialPosition = transform.position;
		rocketLastPosition = initialPosition;
		ghostTrail = GetComponent<GhostTrail>();
		if(ghostTrail != null){
			ghostTrail.enabled = false;
		}
		GetComponent<Rigidbody2D>().mass = mass;
		Init();
	}
	public void Init(){
		fuel = 1000f;
		initialVelocityTimeLeft = initialVelocityAppliedTime;
		state = (int)State.preLaunch;
		flyingDirection = Vector2.up;
		currentThrust = Vector2.zero;
		combinedGravitation = Vector2.zero;
		transform.position = initialPosition;
		transform.rotation = Quaternion.identity;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		
	}
	public void Crash(){
		state = (int)State.crashing;
		GameControl.rocket.state = (int)State.crashing;
		ghostTrail.disable_new_segments();
	}

	public void ApplyThrust(int positive = 1){
		if(thrustMagnitude > 0f){
			currentThrust = flyingDirection * Mathf.Sign(positive) * thrustMagnitude;
			GetComponent<Rigidbody2D>().AddForce(currentThrust);
			flyingDirection = GetComponent<Rigidbody2D>().velocity;
			flyingDirection.Normalize();
			fuel -= 1f;
		}else{
			thrusting = false;
			currentThrust = Vector2.zero;
		}
	}
	public void ApplyInitialVelocity(){
		if(initialVelocityMagnitude == 0)return;
		transform.Rotate(0,0,initialVelocityAngle);
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * initialVelocityMagnitude;
		initialVelocityTimeLeft -= Time.deltaTime;
		fuel -= 1f;
	}
	//called in physics engine
	public void ContinueApplyingInitialVelocity(){
		
		if(initialVelocityMagnitude == 0)return;
		Vector2 currentVelocity = GetComponent<Rigidbody2D>().velocity;
		GetComponent<Rigidbody2D>().velocity 
			= new Vector3(currentVelocity.x, currentVelocity.y)
			 + transform.up.normalized * initialVelocityMagnitude;
		initialVelocityTimeLeft -= Time.fixedDeltaTime;
		fuel -= 1f;

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameControl.self.totalDistance += (new Vector2(transform.position.x, transform.position.y) - rocketLastPosition).magnitude;
	}
}
