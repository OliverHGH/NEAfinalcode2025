using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


namespace GameManage 
{
    public class Datatosave//class that represents data to be saved
    {
        public string stringtest;
        public int LevelNum;
        public float[] Ppos = new float[3];
        public float[] E1pos = new float[3];
        public float[] E2pos = new float[3];
        public Difficulty dlevel;
        public string[] inventoryslot1 = new string[10];
        public string[] inventoryslot2 = new string[10];
        public string[] inventoryslot3 = new string[10];
        public string[] inventoryslot4 = new string[10];
        public string[] inventoryslot5 = new string[10]; // saves each inventory slot as array as JOSN has no 2d arrays
        public int thingscollectedforlevel1;
    }
    class Loadinfo//used when loading a new scene to determine if a new game or saved game is being loaded
    {
        public bool loading; //true if loading save, false if new game
        public string slotpath;//path to load if a save is being loaded
    }
 

    public class JSONSAVELOAD : MonoBehaviour
    {
        Datatosave SavedData;
        string FilePath, FilePath2;
        public Transform player;
        Transform enemy1, enemy2;
        public Button save1,load1, save2,load2;
        public GameObject gmanagerobj;
        GameManagerScript gamemanager;
        InventoryManager inventorymanage;
        playermovement setasct;
        void Start()
        {
            setasct = player.GetComponent<playermovement>();
            gamemanager = gmanagerobj.GetComponent<GameManagerScript>();
            inventorymanage = player.GetComponent<InventoryManager>();
            SavedData = new Datatosave();
            FilePath = Application.persistentDataPath + "/PlayerData1.json";//creates paths for different save slots
            FilePath2 = Application.persistentDataPath + "/PlayerData2.json";
            if (File.Exists(Application.persistentDataPath + "loadinfo"))
            {
                string LoadData = File.ReadAllText(Application.persistentDataPath + "loadinfo");//reads all the data from the JSON file
                Loadinfo loadcheck = JsonUtility.FromJson<Loadinfo>(LoadData);
                if (loadcheck.loading)
                { 
                    LoadGame(loadcheck.slotpath);//if players has contunued a saved game, the saved game will be loaded
                }
            }
            save1.onClick.AddListener(SaveSlot1);
            load1.onClick.AddListener(LoadSlot1);
            save2.onClick.AddListener(SaveSlot2);
            load2.onClick.AddListener(LoadSlot2);//assigns functions to buttons
        }


        void SaveSlot1()
        {
            Debug.Log("slot one saving");
            Save(FilePath);//saves the game to the first path
        }
        void SaveSlot2()
        {
            Save(FilePath2);//saves data to 2nd path
        }
        void LoadSlot1()
        {//retrives data at 1st slot
            string LoadData = File.ReadAllText(Application.persistentDataPath + "/PlayerData1.json");
            Datatosave d = JsonUtility.FromJson<Datatosave>(LoadData);
            int scenetoload = d.LevelNum;
            Loadinfo info = new Loadinfo();
            info.loading = true;
            info.slotpath = Application.persistentDataPath + "/PlayerData1.json";
            File.WriteAllText(Application.persistentDataPath + "loadinfo", JsonUtility.ToJson(info));
            SceneManager.LoadScene(scenetoload);//loads the scene saved in slot, and when scene is loaded the data in 1st slot will be applied to scene
        }
        void LoadSlot2()
        {//retrives data from 2nd slot
            string LoadData = File.ReadAllText(Application.persistentDataPath + "/PlayerData2.json");
            Datatosave d = JsonUtility.FromJson<Datatosave>(LoadData);
            int scenenum = d.LevelNum;
            Loadinfo info = new Loadinfo();
            info.loading = true;
            info.slotpath = Application.persistentDataPath + "/PlayerData2.json";
            File.WriteAllText(Application.persistentDataPath + "loadinfo", JsonUtility.ToJson(info));
            SceneManager.LoadScene(scenenum);//loads the scene saved in slot, and when scene is loaded the data in 2nd slot will be applied to scene
        }
        public void Save(string WhichPath)
        {
            enemy1 = gamemanager.enemybodylist[0].transform;
            if (gamemanager.enemybodylist.Count>1)//if level 2, both enemies will have position saved
            {
                Debug.Log("two enemies");
                enemy2= gamemanager.enemybodylist[1].transform;
                SavedData.E2pos[0] = enemy2.position.x;
                SavedData.E2pos[1] = enemy2.position.y;
                SavedData.E2pos[2] = enemy2.position.x;
            }
            Debug.Log("saving " + SceneManager.GetActiveScene().buildIndex);
            SavedData.thingscollectedforlevel1 = gamemanager.thingscollectedforlevel1;
            SavedData.stringtest = "level1test";
            SavedData.Ppos[0] = player.position.x;
            SavedData.Ppos[1] = player.position.y;
            SavedData.Ppos[2] = player.position.z;
            SavedData.E1pos[0] = enemy1.position.x;
            SavedData.E1pos[1] = enemy1.position.y;
            SavedData.E1pos[2] = enemy1.position.z;//saves the positon of enemies
            SavedData.dlevel = gamemanager.currentdifficulty;
            SavedData.LevelNum = (SceneManager.GetActiveScene().buildIndex);
            WriteSlot(SavedData.inventoryslot1, 0);
            WriteSlot(SavedData.inventoryslot2, 1);
            WriteSlot(SavedData.inventoryslot3, 2);
            WriteSlot(SavedData.inventoryslot4, 3);
            WriteSlot(SavedData.inventoryslot5, 4);//saves inventory
            string TestDatastring = JsonUtility.ToJson(SavedData);
            File.WriteAllText(WhichPath, TestDatastring); // loads data into the file

        }
        public void LoadGame(string WhichPath)
        {
         
            if (File.Exists(FilePath))
            {
                string LoadData = File.ReadAllText(WhichPath);
                SavedData = JsonUtility.FromJson<Datatosave>(LoadData);
                setasct.paused = true;
                enemy1 = gamemanager.enemybodylist[0].transform;
                if (gamemanager.enemybodylist.Count > 1)
                {
                    enemy2 = gamemanager.enemybodylist[1].transform; 
                    Debug.Log("loading two enemies");
                    enemy2.transform.position = new Vector3(SavedData.E2pos[0], SavedData.E2pos[1], SavedData.E2pos[2]);
                    gamemanager.enemybodylist[1].transform.position = enemy2.position;
                }
                player.transform.position = new Vector3(SavedData.Ppos[0], SavedData.Ppos[1], SavedData.Ppos[2]);//puts player and enemies in saved positions
                enemy1.transform.position = new Vector3(SavedData.E1pos[0], SavedData.E1pos[1], SavedData.E1pos[2]);
                gamemanager.enemybodylist[0].transform.position= enemy1.position;
                gamemanager.ChangeDifficulty(SavedData.dlevel);
                Debug.Log(SavedData.dlevel);
                ReadSlot(SavedData.inventoryslot1, 0);
                ReadSlot(SavedData.inventoryslot2, 1);
                ReadSlot(SavedData.inventoryslot3, 2);
                ReadSlot(SavedData.inventoryslot4, 3);
                ReadSlot(SavedData.inventoryslot5, 4);//loads inventory
                gamemanager.thingscollectedforlevel1 = SavedData.thingscollectedforlevel1;

            }


        }
        public void Delete()
        {
            Debug.Log("deleting");
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
        public void WriteSlot(string[] slot, int number)//saves a specific slot in the inventory array
        {
            for(int y = 0; y < 9; y++)
            {
                if (inventorymanage.loadlist[number, y] !=null)//
                {
                    slot[y] = inventorymanage.loadlist[number, y].ID;
                }
            }
        }//saved the inventory objects by their IDs
        public void ReadSlot(string[] slot, int number)
        {
            for (int y = 0; y < 9; y++)//loads items into a specific inventory slot
            {
                if (slot[y] != null)
                {
                    foreach (InventoryObj o in inventorymanage.itemstoload)
                    {
                        if (o.ID == slot[y])//loads items with matching id into inventory
                        {
                            inventorymanage.loadlist[number, y] = o;
                            inventorymanage.loadlist[number, y].Pickup();
                        }
                    }
                }
            }
        }

        private void Update()
        {
        }
    }

}
