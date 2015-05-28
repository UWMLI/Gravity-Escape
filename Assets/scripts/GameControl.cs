using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
	public GameObject rocketPrefab;
	public static Rocket rocket;
	public static PhysicsEngine physics;
	public static UIControl uiControl;
	public static GameControl self;
	[HideInInspector]public Camera mainCamera;
	public float prepTime;
	public float maxOutOfViewportTime;
	public float outOfViewportTime;
	public float crashTime;

	void Awake(){
		maxOutOfViewportTime = 3f;
		self = this;
		mainCamera = Camera.main;
		Init();
	}
	void Init(){
		crashTime = 2.0f;
		prepTime = 5f;
		outOfViewportTime = 0f;
		Time.timeScale = 1.0f;
	}
	// Use this for initialization
	void Start () {
		uiControl.UpdateTimer(prepTime);
	}
	void RocketInViewportTest(){
		Vector2 rocketInViewport 
			= mainCamera.WorldToViewportPoint(rocket.transform.position);
		if(rocketInViewport.x < 0f || rocketInViewport.x > 1f 
			|| rocketInViewport.y < 0f || rocketInViewport.y > 1f){
			//start counting 3 seconds
			outOfViewportTime += Time.deltaTime;
		}else{
			outOfViewportTime = 0f;
		}
	}
	void OnGameOver(){
		Time.timeScale = 0f;
		physics.physicsStarted = false;
		
		//reset current scene
		Destroy(rocket.gameObject);
		//destroy all line renderers
		
		LineRenderer[] lineRenderers 
			= GameObject.FindObjectsOfType<LineRenderer>() as LineRenderer[];
		foreach(LineRenderer lr in lineRenderers){
			Destroy(lr.gameObject);
		}
		
		GameObject newRocket = Instantiate(rocketPrefab) as GameObject;
		rocket = newRocket.GetComponent<Rocket>();
		rocket.gameObject.name = "rocket";

		uiControl.Init();
		Init();
		
		

	}
	void HandleThrusting(){
		if(Input.GetButton("Fire1")){
			rocket.thrusting = true;
			rocket.state = (int)Rocket.State.thrusting;
			rocket.ApplyThrust();
		}else {
			rocket.state = (int)Rocket.State.flying;
			rocket.thrusting = false;
		}
	}
	void MoveCameraWithRocket(){
		Vector3 newCameraPosition = rocket.transform.position;
		newCameraPosition.y = mainCamera.transform.position.y;
		newCameraPosition.z = -10f;
		mainCamera.transform.position = newCameraPosition;
	}
	void Steer(){
		//right: clockwise
		//left: counterclockwise
		Vector2 vel = rocket.GetComponent<Rigidbody2D>().velocity;
		float angle = Mathf.Rad2Deg * Mathf.Atan2(vel.y,vel.x);
		//float angle = Mathf.Rad2Deg * Mathf.Atan2(combinedForceDir.y,combinedForceDir.x);
		//rocket.transform.eulerAngles = new Vector3(0,0, angle);
	
		float steer = Input.GetAxis("Horizontal");
		float rotation = -steer * /*Mathf.Rad2Deg **/ 10f * Time.deltaTime;
		rocket.transform.Rotate(0,0,rotation);

		//Debug.Log("steer rotation: " + rotation);
	}
	// Update is called once per frame
	void Update () {
		//Count 5
		if(rocket.state == (int)Rocket.State.preLaunch){
			prepTime -= Time.deltaTime;
			uiControl.UpdateTimer(Mathf.Max(0f,prepTime));
			if(prepTime <= 0){
				rocket.state = (int)Rocket.State.flying;
				//before starting physics
				uiControl.HideTimerPanel();
				rocket.state = (int)Rocket.State.flying;
				rocket.ghostTrail.enabled = true;
				//start physics
				physics.physicsStarted = true;
			}
		}
		else if(rocket.state == (int)Rocket.State.flying
			|| rocket.state == (int)Rocket.State.thrusting){
			//if rocket is out of sight for more than 3 seconds
			//game over
			/*
			RocketInViewportTest();

			if(outOfViewportTime >= maxOutOfViewportTime){
				OnGameOver();
			}else{
				HandleThrusting();
			}
			//update: camera moves with rocket
			//rocket is always at the center of the screen
			*/
			Steer();
			HandleThrusting();
			MoveCameraWithRocket();
			
		}
		else if(rocket.state == (int)Rocket.State.crashing){
			physics.physicsStarted = false;
			if(crashTime > 0f){
				crashTime = Mathf.Max(0f, crashTime-Time.deltaTime);
			}else{
				//crash done
				OnGameOver();
				return;
			}
		}
		
	}
}
