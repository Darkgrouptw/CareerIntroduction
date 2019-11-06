using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;                                       //Stack需求
using UnityEngine.SceneManagement;
public class Road_Move : MonoBehaviour
{
    #region "Road"
    public GameObject Road_1,Road_2,Traffic_Light;
    private Vector3 Road_pos_1,Road_pos_2;
    public static float Road_move_Speed = 3;                            //跑步的速度       

    public GameObject Background_Groups;
    private Sprite [] Items_Sprite;                                     //取得他的名稱
    private GameObject Dynamic_Parent;
    private List<GameObject> Dynamic_Items = new List<GameObject>();

    private float boundary_pos_X = 13f;
    private float Building_pos_y = 0.9f;
    private float sprite_scale = 200f;
    private int random_int;

    //Read,Blue Dot說明
    //每個東西隔十秒鐘才會出現
    //而所以總共需要分12分，假設一個東西沒吃到，Red,Blue Dot不會移動
    //UI上的紅點跟藍點
    public Image RedDot, BlueDot;
    private int Red_Dot_pos = 1282;                                     //Y固定132
    private int Red_Dot_final = 270;
    private int Blue_Dot_pos = -790;                                    //Y固定98
    private int Blue_Dot_final = 790;

    private const float ConstOneStageTime = 5;                          //一個階段要幾秒
    private const float ConstWaittingTime = 10;                         //假設一個沒吃到的等待持間
    private static float wait_times = 0;                                //等待的時間 
    private static float times;                                         //時間設定 0 ~ ConstOneStageTime
    private static int stage_index;                                     //告訴她吃了哪些東西

    public GameObject bag;
    private static Image[] Skills_UI;
    public GameObject Skills_groups;
    private GameObject[] Skills_Items;

    public GameObject Designer;                                         //最後結束 要把Designer往前面帶
    private float Designer_run_toEnd = 3;                               //最後要跑去終點的時間
    public GameObject EndingLine;                                       //終點的線
    private GameObject Road_Win;

    public GameObject[] Barriers;
    public GameObject BM_Parent;
    private List<GameObject> BM_Trans = new List<GameObject>();
    private float BarrierTime;                                          //障礙物的間隔時間
    private float CountBarrierTime;                                     //計算BarrierTime的時間

    public GameObject[] Monster;
    public bool[] MonsterFly;
    #endregion
    #region SLEEP SETTING
    public GameObject[] GrayUIPart;
    public GameObject[] ColorUIPart;
    public GameObject[] RoadSleep;
    public static int SleepWordShow = -1;                               //告訴要顯示哪一個字
    public static bool SleepWordEat = true;
    private bool SleepBackgroundAnim = false;                           //背景動畫的bool
    public int SleepStageIndex = 0;                                     //該出現哪一個字
    //public 
    #endregion
    #region "TestModeMonnster 讓他們要不要出現"
    public bool TestModeSeenBM;
    #endregion
    void Start()
    {
        Road_pos_1 = Road_1.transform.localPosition;
        Road_pos_2 = Road_2.transform.localPosition;
        
        Transform[] Back_Items;
        Back_Items = Background_Groups.GetComponentsInChildren<Transform>();
        Items_Sprite = new Sprite[Back_Items.Length - 1];

        for (int i = 1; i < Back_Items.Length; i++)
        {
            Items_Sprite[i - 1] = Back_Items[i].gameObject.GetComponent<SpriteRenderer>().sprite;
            Destroy(Back_Items[i].gameObject);
        }
        Dynamic_Parent = Back_Items[0].gameObject;
        Dynamic_Parent.name = "Dynamic_Items";
        random_int = Random.Range(0, Items_Sprite.Length );

        //Bag Items
        Transform[] temp_tran = bag.GetComponentsInChildren<Transform>();
        Skills_UI = new Image[temp_tran.Length - 1];
        for (int i = 0; i < Skills_UI.Length; i++)
            Skills_UI[i] = temp_tran[i + 1].GetComponent<Image>();

        temp_tran = Skills_groups.GetComponentsInChildren<Transform>(true);
        Skills_Items = new GameObject[temp_tran.Length - 1];
        for (int i = 0; i < Skills_Items.Length; i++)
            Skills_Items[i] = temp_tran[i + 1].gameObject;

        //Ending Line
        EndingLine.transform.localPosition = new Vector3(-EndingLine.transform.localPosition.x,
            EndingLine.transform.localPosition.y, EndingLine.transform.localPosition.z);

        //設定障礙物時間
        SetBarrierTime();
    }
    void Update()
    {
        if (stage_index == 12)
        {
            //GameObject Road_back = new GameObject();
            //if (Road_1.transform.localPosition.x < Road_2.transform.localPosition.x)
            //    Road_back = Road_2;
            //else
            //    Road_back = Road_1;

            #region "要預設終點線的位置"
            if (EndingLine.transform.localPosition.x < 0)
            {
                Road_Win = Instantiate(Road_1);
                Road_Win.name = "Road_Win";
                Road_Win.transform.localPosition = new Vector3(Road_Win.transform.localPosition.x + Road_Win.GetComponent<SpriteRenderer>().sprite.rect.width / sprite_scale * 3,
                    Road_Win.transform.localPosition.y, Road_Win.transform.localPosition.z);
                Road_Win.transform.parent = Road_1.transform.parent;
                EndingLine.SetActive(true);
                EndingLine.transform.localPosition = new Vector3(Road_Win.transform.localPosition.x + Road_Win.GetComponent<SpriteRenderer>().sprite.rect.width / sprite_scale,
                    EndingLine.transform.localPosition.y, EndingLine.transform.localPosition.z);
            }
            #endregion
            #region "當看到終點要往前衝之前 和之後判斷"
            if (Road_Win.transform.localPosition.x > 0)
            {
                Road_1.transform.localPosition -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
                Road_2.transform.localPosition -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
                Road_Win.transform.localPosition -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
                EndingLine.transform.localPosition -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
                for (int i = 0; i < Dynamic_Items.Count; i++)
                    Dynamic_Items[i].transform.position -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
            }
            else
            {
                if (times > Designer_run_toEnd)
                {
                    stage_index++;
                    times = 0;
                    Designer.GetComponent<Animator>().enabled = false;
                    SceneManager.LoadSceneAsync(6);
                }
                else
                {
                    times += Time.deltaTime;
                    Designer.transform.localPosition += new Vector3(-2 * Teaching_Page.Running_point_X * Time.deltaTime / Designer_run_toEnd, 0, 0);
                }
            }
            #endregion
            #region "要讓路上的東西消失"
            //if (Dynamic_Items[0].transform.position.x < -boundary_pos_X)
            //{
            //    Destroy(Dynamic_Items[0]);
            //    Dynamic_Items.RemoveAt(0);
            //}
            //for (int i = 0; i < Dynamic_Items.Count; i++)
            //    Dynamic_Items[i].transform.position -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
            #endregion

            if (BM_Trans.Count > 0)
                BM_Trans.Clear();
        }
        else if (stage_index < 12)
        {
            #region "路面上的東西"
            if (Traffic_Light != null)
                if (Traffic_Light.transform.localPosition.x < -boundary_pos_X)
                    Destroy(Traffic_Light);
                else
                    Traffic_Light.transform.localPosition -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);

            if (Road_2.transform.localPosition.x <= Road_pos_1.x && Road_1.transform.localPosition.x < Road_2.transform.localPosition.x)
                Road_1.transform.localPosition = Road_pos_2;
            else if (Road_1.transform.localPosition.x <= Road_pos_1.x && Road_2.transform.localPosition.x < Road_1.transform.localPosition.x)
                Road_2.transform.localPosition = Road_pos_2;
            Road_1.transform.localPosition -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
            Road_2.transform.localPosition -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);

            if (Dynamic_Items.Count == 0 ||
                (Dynamic_Items[Dynamic_Items.Count - 1].transform.localPosition.x + (Dynamic_Items[Dynamic_Items.Count - 1].GetComponent<SpriteRenderer>().sprite.rect.width + Items_Sprite[random_int].rect.width) / sprite_scale)
                <
                boundary_pos_X - 0.1f)
            {
                GameObject temp = new GameObject();
                temp.transform.parent = Dynamic_Parent.transform;
                temp.transform.localPosition = new Vector3(boundary_pos_X, Building_pos_y + Items_Sprite[random_int].rect.height / sprite_scale);
                temp.name = Items_Sprite[random_int].name;
                temp.AddComponent<SpriteRenderer>().sprite = Items_Sprite[random_int];
                Dynamic_Items.Add(temp);
                random_int = Random.Range(0, Items_Sprite.Length);
            }

            if (Dynamic_Items[0].transform.position.x < -boundary_pos_X)
            {
                Destroy(Dynamic_Items[0]);
                Dynamic_Items.RemoveAt(0);
            }
            for (int i = 0; i < Dynamic_Items.Count; i++)
                Dynamic_Items[i].transform.position -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
            #endregion
            #region "Red & Blue Dot 設定"
            if (times >= ConstOneStageTime)
            {
                if (wait_times == 0)
                {
                    //產生一個物品出來
                    float rand_high = Random.value;
                    Skills_Items[stage_index].transform.localPosition = new Vector3(boundary_pos_X, Building_pos_y + rand_high * 2, 0);
                    Skills_Items[stage_index].SetActive(true);
                }
                if (wait_times >= ConstWaittingTime)                         //產生那個物品
                    wait_times = 0;
                else
                {
                    wait_times += Time.deltaTime;
                    Skills_Items[stage_index].transform.localPosition -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
                }
            }
            else
            {
                times += Time.deltaTime;
                RedDot.transform.localPosition = RedBot_Vec3();
                HeroControll.Liver_Num -= Time.deltaTime;
                BlueDot.transform.localPosition = new Vector3((Blue_Dot_final - Blue_Dot_pos) * (times / ConstOneStageTime + stage_index) / 12 + Blue_Dot_pos,
                    BlueDot.transform.localPosition.y, BlueDot.transform.localPosition.z);
            }
            #endregion
            #region "障礙物出現"
            if (TestModeSeenBM)
            {
                if (CountBarrierTime > BarrierTime)
                {
                    CountBarrierTime = 0;
                    SetBarrierTime();

                    //創立一個新的怪物，或障礙物
                    GameObject temp;
                    if (Random.value > 0.5)                         //產生障礙物
                    {
                        int index = Random.Range(0, Barriers.Length);
                        temp = Instantiate<GameObject>(Barriers[index]);
                        temp.transform.parent = BM_Parent.transform;
                        temp.name = Barriers[index].name;
                        temp.transform.localPosition = new Vector3(boundary_pos_X, 0.34f, 0);
                    }
                    else                                            //產生怪物
                    {
                        int index = Random.Range(0, Monster.Length);
                        temp = Instantiate<GameObject>(Monster[index]);
                        temp.transform.parent = BM_Parent.transform;
                        temp.name = Monster[index].name;
                        if (MonsterFly[index] == true)
                            temp.transform.localPosition = new Vector3(boundary_pos_X, 2.15f, 0);
                        else
                            temp.transform.localPosition = new Vector3(boundary_pos_X, 1.15f, 0);
                    }
                    BM_Trans.Add(temp);
                }
                CountBarrierTime += Time.deltaTime;

                if (BM_Trans.Count > 0)
                    if (BM_Trans[0].transform.position.x < -boundary_pos_X)
                    {
                        Destroy(BM_Trans[0]);
                        BM_Trans.RemoveAt(0);
                    }

                //讓他移動
                for (int i = 0; i < BM_Trans.Count; i++)
                    BM_Trans[i].transform.position -= new Vector3(Road_move_Speed * Time.deltaTime, 0, 0);
            }
            #endregion

            if (SleepStageIndex <= 6)
            {
                switch (SleepStageIndex)
                {
                    #region S
                    case 0:
                        if (HeroControll.Liver_Num <= HeroControll.Liver_Top * 0.9)
                            if (SleepWordEat)
                            {
                                float rand_high = Random.value;
                                GameObject temp = Instantiate<GameObject>(RoadSleep[0]);
                                temp.transform.position = new Vector3(boundary_pos_X, Building_pos_y + rand_high * 2, 0);
                                BM_Trans.Add(temp);

                                SleepWordEat = false;
                                SleepStageIndex++;
                            }
                            else
                                SleepStageIndex = -1;
                        break;
                    #endregion
                    #region L
                    case 1:
                        if (HeroControll.Liver_Num <= HeroControll.Liver_Top * 0.7)
                            if (SleepWordEat)
                            {
                                float rand_high = Random.value;
                                GameObject temp = Instantiate<GameObject>(RoadSleep[1]);
                                temp.transform.position = new Vector3(boundary_pos_X, Building_pos_y + rand_high * 2, 0);
                                BM_Trans.Add(temp);

                                SleepWordEat = false;
                                SleepStageIndex++;
                            }
                            else
                                SleepStageIndex = -1;
                        break;
                    #endregion
                    #region E
                    case 2:
                        if (HeroControll.Liver_Num <= HeroControll.Liver_Top * 0.5)
                            if (SleepWordEat)
                            {
                                float rand_high = Random.value;
                                GameObject temp = Instantiate<GameObject>(RoadSleep[2]);
                                temp.transform.position = new Vector3(boundary_pos_X, Building_pos_y + rand_high * 2, 0);
                                BM_Trans.Add(temp);

                                SleepWordEat = false;
                                SleepStageIndex++;
                            }
                            else
                                SleepStageIndex = -1;
                        break;
                    #endregion
                    #region E
                    case 3:
                        if (HeroControll.Liver_Num <= HeroControll.Liver_Top * 0.3)
                            if (SleepWordEat)
                            {
                                float rand_high = Random.value;
                                GameObject temp = Instantiate<GameObject>(RoadSleep[3]);
                                temp.transform.position = new Vector3(boundary_pos_X, Building_pos_y + rand_high * 2, 0);
                                BM_Trans.Add(temp);

                                SleepWordEat = false;
                                SleepStageIndex++;
                            }
                            else
                                SleepStageIndex = -1;
                        break;
                    #endregion
                    #region P
                    case 4:
                        if (HeroControll.Liver_Num <= HeroControll.Liver_Top * 0.1)
                            if (SleepWordEat)
                            {
                                float rand_high = Random.value;
                                GameObject temp = Instantiate<GameObject>(RoadSleep[4]);
                                temp.transform.position = new Vector3(boundary_pos_X, Building_pos_y + rand_high * 2, 0);
                                BM_Trans.Add(temp);

                                SleepWordEat = false;
                                SleepStageIndex++;
                            }
                            else
                                SleepStageIndex = -1;
                        break;
                    #endregion
                }

                //吃到顯示字
                if(SleepWordShow != -1)
                {
                    Image temp = GrayUIPart[SleepWordShow].GetComponent<Image>();
                    temp.color = new Color(
                        temp.color.r,
                        temp.color.g,
                        temp.color.b,
                        0);
                    ColorUIPart[SleepWordShow].SetActive(true);

                    if (SleepWordShow == 4)
                        SleepBackgroundAnim = true;
                    SleepWordShow = -1;
                }
            }
        }
    }

    private Vector3 RedBot_Vec3()
    {
        float temp = HeroControll.Liver_Num;
        if (temp <= 0)
            temp = 0;

        return new Vector3(-(Red_Dot_pos - Red_Dot_final) * (HeroControll.Liver_Top - temp) / HeroControll.Liver_Top + Red_Dot_pos,
                    RedDot.transform.localPosition.y, RedDot.transform.localPosition.z);
    }
    private void SetBarrierTime()
    {
        BarrierTime = 1.5f + Random.Range(0, 2) + Random.value; 
    }
    public static void add_stage_index()
    {
        Skills_UI[stage_index].enabled = true;
        stage_index++;
        wait_times = 0;
        times = 0;
    }
}