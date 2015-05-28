using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	//F = GMm/(r^2)
	public float gravityConst;
	public float mass;
	public float maxImpulse; //max impulse allowed without crashing the rocket
	public virtual void Awake(){
		if(maxImpulse <= 0f){
			maxImpulse = 2.0f;
		}
	}
	// Use this for initialization
	public Vector2 CalculateGravity(){
		float dist = Vector2.Distance(
			GameControl.rocket.transform.position,
			transform.position
		);
		Vector2 dir = transform.position - GameControl.rocket.transform.position;
		dir.Normalize();
		return dir * gravityConst * mass * (GameControl.rocket.mass) / (dist * dist);
	}
	public virtual void OnCollisionEnter2D(Collision2D collision){
		if(collision.gameObject.tag == "Rocket"){
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
			//Debug.Log(impulseMagnitude);
			if(impulseMagnitude > maxImpulse){
				Debug.Log("crashed!");
				collision.gameObject.GetComponent<Rocket>().Crash();
			}
				
		}
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
