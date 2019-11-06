using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class Career_Keyboard : MonoBehaviour 
{
	// Update is called once per frame
	public Image[] Career_Set;
	private int gap_x = 200;                      // X 的間距
	private float gap_times = 1;				// 間距時間
	private float times = 0;                    // 計數時間
    private int do_Anim = 0;                  //-1 是向左移，0 是不做，1是向右移

	public static int Career_Index = 0;			//設定賄選到哪一個角色 
	
	private	const float scale_num = 0.2f;
	private const float scale_org = 0.4f;
	private const float tran_offset = 60f;
	void Start ()
	{
		Career_Index = 0;
        Career_Set[1].transform.localScale = new Vector3(scale_num, scale_num); 
		Career_Set [1].transform.localPosition -= new Vector3 (0, tran_offset, 0);
        Career_Set[2].transform.localScale = new Vector3(scale_num, scale_num); 
		Career_Set [2].transform.localPosition -= new Vector3 (0, tran_offset, 0);
	}
	void Update () 
	{
        //0可以按按鍵，做動畫
        if (do_Anim == 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                SceneManager.LoadSceneAsync(3);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                do_Anim = -1;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                do_Anim = 1;
        }
        else
            if (times >= gap_times)
            {
                times = 0;
                Career_Index = Num_do(Career_Index - do_Anim);
                do_Anim = 0;
            }
            else if (times < gap_times)
            {
                times += Time.deltaTime;
                Career_Animation(do_Anim);
            }
	}
	void Career_Animation(int num)
	{
		if (num == 1)           //Move to right
		{
            Career_Set[Career_Index].transform.localPosition +=
                new Vector3(gap_x * Time.deltaTime / gap_times, -tran_offset * Time.deltaTime / gap_times, 0);
            Career_Set[Num_do(Career_Index + 1)].transform.localPosition +=
                new Vector3(gap_x * Time.deltaTime / gap_times, 0, 0);
            Career_Set[Num_do(Career_Index + 2)].transform.localPosition +=
                new Vector3(gap_x * Time.deltaTime / gap_times, tran_offset * Time.deltaTime / gap_times, 0);

            //Scale
            Career_Set[Career_Index].transform.localScale = new Vector3(scale_org - (scale_org - scale_num) * times / gap_times,
                scale_org - (scale_org - scale_num) * times / gap_times);
            Career_Set[Num_do(Career_Index + 2)].transform.localScale = new Vector3((scale_org - scale_num) * times / gap_times + scale_num,
                (scale_org - scale_num) * times / gap_times + scale_num);

            if (Career_Set[Num_do(Career_Index + 1)].transform.localPosition.x >= gap_x * 3 / 2)
                Career_Set[Num_do(Career_Index + 1)].transform.localPosition -= new Vector3 (gap_x * 3,0,0); 
		} 
		else                //Move to left
		{
            Career_Set[Career_Index].transform.localPosition -=
                new Vector3(gap_x * Time.deltaTime / gap_times, tran_offset * Time.deltaTime / gap_times, 0);
            Career_Set[Num_do(Career_Index + 2)].transform.localPosition -=
                new Vector3(gap_x * Time.deltaTime / gap_times, 0, 0);
            Career_Set[Num_do(Career_Index + 1)].transform.localPosition -=
                new Vector3(gap_x * Time.deltaTime / gap_times, -tran_offset * Time.deltaTime / gap_times, 0);

            //Scale
            Career_Set[Career_Index].transform.localScale = new Vector3((scale_num - scale_org) * times / gap_times + scale_org,
                (scale_num - scale_org) * times / gap_times + scale_org);
            Career_Set[Num_do(Career_Index + 1)].transform.localScale = new Vector3((scale_org - scale_num) * times / gap_times + scale_num,
                (scale_org - scale_num) * times / gap_times + scale_num);

            if (Career_Set[Num_do(Career_Index + 2)].transform.localPosition.x <= gap_x * -3 / 2)
                Career_Set[Num_do(Career_Index + 2)].transform.localPosition += new Vector3(gap_x * 3, 0, 0); 
		}
	}
	
	int Num_do(int a)
	{
        return (a + Career_Set.Length) % Career_Set.Length;
	}
}
