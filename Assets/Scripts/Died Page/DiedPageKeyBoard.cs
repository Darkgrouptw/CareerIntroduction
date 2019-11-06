using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class DiedPageKeyBoard : MonoBehaviour {
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SceneManager.LoadSceneAsync(0);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            SceneManager.LoadSceneAsync(4);
	}
}
