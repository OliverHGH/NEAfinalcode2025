using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
namespace GameManage
{
    public enum Difficulty//enum that determines the game's difficulty
    { 
        Easy,
        Normal,
        Difficult
    }


    public class GameManagerScript : MonoBehaviour//manages gameplay 
    {
        public Difficulty currentdifficulty;
        public List<AngelBT> enemylist = new List<AngelBT>();
        public List<GameObject> enemybodylist = new List<GameObject>();
        public GameObject playertopause,cameratopause, pausemenu, enemy1obj,enemy2obj;
        public Button d1,d2,d3, q;
        protected bool gamepaused = false;
        public bool gameover = false;
        protected float timer, endtimer, wontimer;
        protected playerlook lookpauser;
        protected playermovement movepauser;
        public int thingscollectedforlevel1 =0;
        protected virtual void Start()
        {
            pausemenu.SetActive(false);
            enemylist.Add(enemy1obj.GetComponent<AngelBT>());
            lookpauser = cameratopause.GetComponent<playerlook>();
            movepauser = playertopause.GetComponent<playermovement>(); 
            enemybodylist.Add(enemy1obj);
            if (enemy2obj != null)
            {
                enemylist.Add(enemy2obj.GetComponent<AngelBT>());
                enemybodylist.Add(enemy2obj);//gets list of enemies ais and enemy bodies
            }//assigns variables to starting values
            currentdifficulty = Difficulty.Normal;
            timer = 0;
            d1.onClick.AddListener(MakeEasy);
            d2.onClick.AddListener(MakeNormal);
            d3.onClick.AddListener(MakeHard);
            q.onClick.AddListener(Quit);//asings functions to be called when buttons pressed
            endtimer = 0;
            wontimer = 0;
            gamepaused = false;

        }

        protected virtual void Quit()
        {
            SceneManager.LoadScene(0);//loads title scene
        }
        public void ChangeDifficulty(Difficulty d)
        {
            currentdifficulty=d;
            foreach(AngelBT x in enemylist)
            {
                x.ChangeDifficulty(d);
            }//changes difficult settings of all the enemies
        }
        void MakeEasy()
        {
            ChangeDifficulty(Difficulty.Easy);
        }
        void MakeNormal()
        {
            ChangeDifficulty(Difficulty.Normal);
        }
        void MakeHard()
        {
            ChangeDifficulty(Difficulty.Difficult);
        }

        protected virtual void TogglePaused()
        {
            gamepaused =! gamepaused;
            if (gamepaused == true)
            {
                pausemenu.SetActive(true);
                lookpauser.paused = true;//sets pause menu active and pauses player
                movepauser.paused = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                foreach(AngelBT x in enemylist)
                {
                    x.paused = true;//pauses enemies
                }
            }
            else
            {
                lookpauser.paused = false;
                movepauser.paused = false;//unpasues enemies and players, hides pause menu
                pausemenu.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                foreach (AngelBT x in enemylist)
                {
                    x.paused = false;
                }
            }

        }
        protected virtual void Update()
        {
            if (!gameover)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    TogglePaused();//while game not over, pressing esc will let you
                }
            }
            if (!gamepaused)
            {
                timer += Time.deltaTime;
                if (thingscollectedforlevel1 == 3)//if level one has been won, you will be loaded into next level after timer
                {
                    Debug.Log("Won level one");
                    wontimer += Time.deltaTime;
                }
                if (wontimer > 2)
                {
                    Loadinfo loadinfo = new Loadinfo();
                    loadinfo.loading = false;//makes it clear you are loading into a new level and not into a previous save
                    File.WriteAllText(Application.persistentDataPath + "loadinfo", JsonUtility.ToJson(loadinfo));//loads data into JSON file
                    SceneManager.LoadScene("TestC2");//loads you into new scene
                }
                foreach (AngelBT x in enemylist)
                {
                    if (timer / (x.Encounters / 3) < 20 && timer > 120)
                    {
                        Debug.Log("this angel has been seen a lot");
                        x.MakeSneaky();//changes enemy behaviour if player interacts with it a lot
                    }
                    else if (x.Encounters / 3 > 0 && timer / (x.Encounters / 3) > 100 && timer > 120)
                    {
                        Debug.Log("you are hidey");
                        x.HuntBetter();//changes angel behaviour if player has not seen enemy a lot
                    }
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
                }//loads player into title screen if they are killed
               
            }
        }
        protected virtual void endgame()
        {
           if(endtimer==0)
            {
                lookpauser.paused = true;
                movepauser.paused = true;
                int pos = 0;
                foreach (AngelBT x in enemylist)
                {
                    x.paused = true;
                    if (x.haskilledplayer)
                    {
                        cameratopause.transform.position = enemybodylist[pos].transform.position + enemybodylist[pos].transform.forward * 2f+ enemybodylist[pos].transform.up*1f;
                        cameratopause.transform.LookAt(enemybodylist[pos].transform.position + enemybodylist[pos].transform.up * 1f);
                    }
                    pos++;//makes player face enemy when enemy kills them
                }

            }

        }
    }

}