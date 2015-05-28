using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour {
	public float mass;
	public float thrustMagnitude; //assign in inspector
	public float fuel;
	public Vector2 flyingDirection;
	public bool thrusting;
	public Vector2 currentThrust;
	public Vector2 combinedGravitation;
	public float steerSpeed;
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
			thrustMagnitude = 10f;
		}
		if(steerSpeed <= 0f){
			steerSpeed = 5.0f;
		}
		initialPosition = transform.position;
		ghostTrail = GetComponent<GhostTrail>();
		if(ghostTrail != null){
			ghostTrail.enabled = false;
		}
		GetComponent<Rigidbody2D>().mass = mass;
		Init();
	}
	public void Init(){
		fuel = 1000f;
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
		ghostTrail.disable_new_segments();
	}
	public void ApplyThrust(){
		if(thrustMagnitude > 0f){
			currentThrust = flyingDirection * thrustMagnitude;
			GetComponent<Rigidbody2D>().AddForce(currentThrust);
			flyingDirection = GetComponent<Rigidbody2D>().velocity;
			flyingDirection.Normalize();
		}else{
			thrusting = false;
			currentThrust = Vector2.zero;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
