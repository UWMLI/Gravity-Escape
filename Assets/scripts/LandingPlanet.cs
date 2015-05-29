using UnityEngine;
using System.Collections;

public class LandingPlanet : Star {
	public override void Awake(){
		base.Awake();
	}
	void OnCollisionEnter2D(Collision2D collision){
		Debug.Log(GameControl.rocket.state);
		if(GameControl.rocket.state == (int)Rocket.State.crashing){
			Debug.Log("already crashed");
			return;
		}
		if(gameObject.tag == "LandingPlanet" && collision.gameObject.tag == "Rocket"){
			
			
			//check if mv is too large
			if(collision.contacts == null || collision.contacts.Length == 0){
				Debug.Log("Error: no contacts!!!");
				return;
			}
			Rigidbody2D rocketRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
			Vector2 contactNormal = collision.contacts[0].normal;
			//m times v, used to meaure impulse
			Vector2 mv = rocketRigidbody.mass * rocketRigidbody.velocity;
			float impulseMagnitude = Mathf.Abs(Vector2.Dot(mv, contactNormal.normalized));
			Debug.Log(impulseMagnitude);
			if(impulseMagnitude > maxImpulse){
				Debug.Log("crashed!");
				collision.gameObject.GetComponent<Rocket>().Crash();
			}else{
				//load next level
				SceneLoader.self.LoadNextLevel();
			}
		}
	}
}
