using UnityEngine;
using System.Collections;

public class LevelSelectorEvents : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Level Selects
	public void LoadLevel1 () {
		SceneLoader.self.LoadLevel(0);
	}

	public void LoadLevel2 () {
		SceneLoader.self.LoadLevel(1);
	}

	public void LoadLevel3 () {
		SceneLoader.self.LoadLevel(2);
	}
}
