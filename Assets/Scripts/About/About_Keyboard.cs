using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class About_Keyboard : MonoBehaviour 
{
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
			 SceneManager.LoadSceneAsync(0);
	}
}
