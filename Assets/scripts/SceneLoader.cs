using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour {
	public static SceneLoader self;
	public List<string> scenes; //assign in inspector
	public int currentSceneIdx;
	void Awake(){
		self = this;
        DontDestroyOnLoad(gameObject);
        if(scenes.Count <= 0){
        	Debug.Log("No scenes in scenes list!");
        	Application.Quit();
        }
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	public void LoadNextLevel(){
		if(!Application.isLoadingLevel && currentSceneIdx+1 < scenes.Count){
			Application.LoadLevel(scenes[currentSceneIdx+1]);
			currentSceneIdx++;
		}
	}

	public void LoadLevel(int levelId){
		if(!Application.isLoadingLevel && levelId < scenes.Count){
			Application.LoadLevel(scenes[levelId]);
			currentSceneIdx = levelId+1;
		}
	}
}
