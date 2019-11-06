using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Menu_Keyboard : MonoBehaviour {
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
			SceneManager.LoadSceneAsync (2);
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		    SceneManager.LoadSceneAsync (1);
	}
}
