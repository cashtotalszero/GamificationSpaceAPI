using UnityEngine;
using System.Collections;

public class SplashSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(SceneChange());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel("MainMenus");
		}
	}
	
	IEnumerator SceneChange() {
    	yield return new WaitForSeconds(4.0f);
   		Application.LoadLevel("MainMenus");
    }
}
