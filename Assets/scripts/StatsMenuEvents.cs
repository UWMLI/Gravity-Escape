using UnityEngine;
using System.Collections;

public class StatsMenuEvents : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showLevelSelect()
	{

	}

	public void nextLevel()
	{
		SceneLoader.self.LoadNextLevel();
	}
}
