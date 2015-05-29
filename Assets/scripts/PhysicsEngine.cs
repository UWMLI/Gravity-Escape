using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsEngine : MonoBehaviour {
	public bool physicsStarted;
	public List<Star> stars;

	void Awake(){
		physicsStarted = false;
		GameObject[] starArr = GameObject.FindGameObjectsWithTag("Star");
		foreach(GameObject g in starArr){
			stars.Add(g.GetComponent<Star>());
		}
		GameControl.physics = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	void FixedUpdate(){
		if(!physicsStarted){
			return;
		}
		Rocket rocket = GameControl.rocket;
		rocket.combinedGravitation = Vector2.zero;
		foreach(Star s in stars){
			Vector2 forceOnRocket = s.CalculateGravity();
			rocket.combinedGravitation += forceOnRocket;

			rocket.GetComponent<Rigidbody2D>().AddForce(forceOnRocket);
		}
		//rotation always perpendicular to combined force
		Vector2 combinedForceDir = rocket.combinedGravitation + rocket.currentThrust;
		
		Vector2 vel = rocket.GetComponent<Rigidbody2D>().velocity;
		rocket.transform.up = vel.normalized;
		//float angle = Mathf.Rad2Deg * Mathf.Atan2(vel.y,vel.x);
		//float angle = Mathf.Rad2Deg * Mathf.Atan2(combinedForceDir.y,combinedForceDir.x);
		//rocket.transform.eulerAngles = new Vector3(0,0, angle);

	}
	// Update is called once per frame
	void Update () {
	
	}
}
