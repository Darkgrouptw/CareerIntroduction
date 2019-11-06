using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sun_and_Cloud_Move : MonoBehaviour
{
    public GameObject sun;
    public Sprite[] cloud;
    public Road_Move scripts;
    private List<GameObject> cloud_list;
    private float speed;
    private const float boundary_x = 10;

    private const float sun_pos_X = 6;
    private const float sun_pos_Y = 8;              //上 0.5，下0.5                    
    void Start()
    {
        sun.SetActive(true);
        sun.transform.localPosition = new Vector3(sun_pos_X, sun_pos_Y + Random.value - 0.5f, 0);
        cloud_list = new List<GameObject>();
        for (int i = -6; i <= 12; i += 6)
        {
            GameObject temp = new GameObject();
            temp.transform.localPosition = new Vector3(i - Random.value - 0.5f, sun_pos_Y - Random.value , 0);
            temp.transform.localScale = new Vector3(0.5f, 0.5f, 0);
            SpriteRenderer temp_sprite = temp.AddComponent<SpriteRenderer>();
            temp_sprite.sprite = cloud[Random.Range(0, cloud.Length)];
            temp_sprite.sortingOrder = -1;
            temp.transform.parent = this.gameObject.transform;
            temp.name = "Cloud";
            cloud_list.Add(temp);
        }

        speed = Road_Move.Road_move_Speed * 0.01f;
    }

	void Update () 
    {
        for (int i = 0; i < cloud_list.Count; i++)
        {
            cloud_list[i].transform.localPosition -= new Vector3((speed + 
                ((scripts.enabled == true) ? speed : 0)) * Time.deltaTime, 0, 0);
            if (cloud_list[i].transform.localPosition.x < -sun_pos_X * 2)
            {
                cloud_list[i].transform.localPosition = new Vector3(sun_pos_X * 2 - Random.value - 0.5f, sun_pos_Y - Random.value, 0);
                cloud_list[i].GetComponent<SpriteRenderer>().sprite = cloud[Random.Range(0, cloud.Length)];
            }
        }
	}
}
