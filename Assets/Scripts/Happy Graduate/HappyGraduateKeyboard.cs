using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class HappyGraduateKeyboard : MonoBehaviour
{
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SceneManager.LoadSceneAsync(0);
	}
}
