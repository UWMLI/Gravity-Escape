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
		return dir * gravityConst * mass * (GameControl.rocket.mass) / (Mathf.Pow(10f,2f) *dist * dist);
	}
	void OnCollisionEnter2D(Collision2D collision){
		if(gameObject.tag == "Star" && collision.gameObject.tag == "Rocket"){
			Debug.Log("crashed!");
			collision.gameObject.GetComponent<Rocket>().Crash();
		}
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
