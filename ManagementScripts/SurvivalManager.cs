using UnityEngine;
using GameManage;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor.Overlays;

public class SurvivalManager : GameManagerScript//manager for survival mode
{
    public Text TimeSurvived;
    private string TimeMessage,BestScoreFile;
    private int enemiesactive;
    private float spawninterval,spawntimer;
    public torchbatteryscript batterytospawn;
    public GameObject e3, e4, e5, e6, e7, e8, e9, e10;
    protected override void TogglePaused()
    {
        gamepaused = !gamepaused;
        pausemenu.SetActive(gamepaused);
        if(gamepaused )
        {
            lookpauser.paused = true;
            movepauser.paused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            lookpauser.paused = false;
            movepauser.paused = false;
            pausemenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    protected override void Start()
    {
        BestScoreFile = Application.persistentDataPath + "/BestScoreJSON";//file where the best score will be saved
        enemiesactive = 2;
        spawninterval = 80;
        spawntimer = 0;
        pausemenu.SetActive(false);
        enemybodylist.Add(enemy1obj);
        enemybodylist.Add(enemy2obj);
        enemybodylist.Add(e3);
        enemybodylist.Add(e4);
        enemybodylist.Add(e5);
        enemybodylist.Add(e6);
        enemybodylist.Add(e7);
        enemybodylist.Add (e8);
        enemybodylist.Add(e9);
        enemybodylist.Add(e10);//creates list of enemies
        foreach(GameObject b in enemybodylist)
        {
            enemylist.Add(b.GetComponent<AngelBT>());
        }
        foreach(AngelBT e in enemylist)
        {
            e.survivalmode = true;//sets all enemies to survival mode behaviour
        }
        q.onClick.AddListener(Quit);//assigns function to button
        timer = 0;
        endtimer = 0;
        currentdifficulty = Difficulty.Difficult;
        gamepaused = false;
        lookpauser = cameratopause.GetComponent<playerlook>();
        movepauser = playertopause.GetComponent<playermovement>();
        for(int i = 0; i < 20; i++)
        {
            Instantiate(batterytospawn, randomlocation(), Quaternion.identity);//puts battery in a random location
        }
    }
    protected override void Update()
    {
        if (!gameover)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePaused();
            }
        }
        timer += Time.deltaTime;
        foreach (AngelBT x in enemylist)
        {
            if (timer / (x.Encounters / 3) < 20 && timer > 120)
            {
                Debug.Log("this angel has been seen a lot");
                x.MakeSneaky();
            }
            else if (x.Encounters / 3 > 0 && timer / (x.Encounters / 3) > 100 && timer > 120)
            {
                Debug.Log("you are hidey");
                x.HuntBetter();
            }
        }
        if (gameover)
        {
            endgame();
            endtimer += Time.deltaTime;
            if (endtimer > 2.5)
            {
                Debug.Log("game over");
                SceneManager.LoadScene(0);
            }

        }
        string minutestring =Mathf.RoundToInt(timer/60).ToString();
        string secondstring = Mathf.RoundToInt(timer%60).ToString();
        TimeMessage = " Time Surived " + minutestring+":"+secondstring;
        TimeSurvived.text = TimeMessage;

        spawntimer += Time.deltaTime;
        if (spawntimer > spawninterval)
        {
            Debug.Log("new enemy");
            enemybodylist[enemiesactive].gameObject.SetActive(true);//sets next enemy to active
            enemybodylist[enemiesactive].gameObject.transform.position = randomlocation();//puts it in a random location
            AngelBT enemy = enemybodylist[enemiesactive].gameObject.GetComponent<AngelBT>();
            enemy.Start();
            enemiesactive++;
            spawntimer = 0;
            spawninterval *= 0.75f;//time between spawns gets exponentially smaller
            foreach(AngelBT x in enemylist)
            {
                x.speedincrease();//makes all enemies go faster
            }
        }

    }

    Vector3 randomlocation()//chooses a random location
    {
        int x = Random.Range(-25, 25);
        int y = Random.Range(-25, 25);
        return new Vector3(x*5, 1.5f,y*5);
    }

    void SaveScore()
    {
        if (LoadScore())//if new score is better than old one, it will be deleted
        {
            timesave Saver = new timesave();
            Saver.minutes = (Mathf.RoundToInt(timer / 60).ToString());//turns time in seconds into minutes and seconds
            Saver.seconds = (Mathf.RoundToInt(timer % 60).ToString());
            string Timestring = JsonUtility.ToJson(Saver);
            Debug.Log(Timestring);
            File.WriteAllText(BestScoreFile, Timestring);
        }

    }

    bool LoadScore()
    {
        if(File.Exists(BestScoreFile))
        {
            string LoadData = File.ReadAllText(BestScoreFile);
            timesave bestscore = JsonUtility.FromJson<timesave>(LoadData);
            int besttime = (int.Parse(bestscore.minutes)*60)+ int.Parse(bestscore.seconds);
            if (timer > besttime)
            {
                return true;
            }
            return false;
        }
        return true;//checks if the new score is the highest
    }

    protected override void Quit()
    {
        SaveScore();//score will be saved when you quit
        base.Quit();
    }

    protected override void endgame()
    {
        base.endgame();
        SaveScore() ;
    }


}
public class timesave//saves the time in minutes and seconds
{
    public string minutes;
    public string seconds;
}



