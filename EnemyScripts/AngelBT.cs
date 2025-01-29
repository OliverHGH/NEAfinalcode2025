using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace GameManage
{
    public class AngelBT : Btree
    {
        public Transform player,playercamera, enemy, forward;
        public Transform halfway;
        public GameObject Astar,GameManageObject;
        public LayerMask walllayer;
        GameManagerScript manager;
        pathfinder pfinder;
        GridNav grid;
        public NewPathToPlayer playerpath;
        public PatrolScript patrol;
        public SpinToLook spinning;
        public GoLastPos golast;
        CanSeePlayer playervisible;
        int encounters=0;
        float timer, damagetimer;
        public bool paused = false;
        MathsMethods visiblecalc= new MathsMethods();
        public int Encounters
        {
            get { return encounters; }
        }
        bool observed, halfwayobserved,observedlastiteration =false;

        public bool haskilledplayer,survivalmode;

        SelectorNode easyroot,normalroot;
            
        public override void Start()
        {
            manager = GameManageObject.GetComponent<GameManagerScript>();
            grid = Astar.GetComponent<GridNav>();
            pfinder = Astar.GetComponent<pathfinder>();
            StartCoroutine(checkobserved());
            timer = 0; //asigns variables
            base.Start();
        }

        public override BehaviourNode CreateTree()
        {

            playerpath = new NewPathToPlayer(pfinder, enemy, player,grid,halfway);
            KillPlayer kill = new KillPlayer(manager,this);
            playervisible = new CanSeePlayer(enemy, forward, player, walllayer);
            AtPlayerNode atplayer = new AtPlayerNode(enemy, player,walllayer);
            patrol = new PatrolScript(pfinder, grid, enemy);
            spinning = new SpinToLook(enemy);
            SpinCondition needspin = new SpinCondition();
            VisitedLastPosCond vlastpos = new VisitedLastPosCond();
            golast = new GoLastPos(enemy, pfinder,grid); //creats behaviour nodes

            List<BehaviourNode> KillList = new List<BehaviourNode>();
            KillList.Add(atplayer);
            KillList.Add(kill);
            List<BehaviourNode> FollowList = new List<BehaviourNode>();
            FollowList.Add(playervisible);
            FollowList.Add(playerpath);
            SequenceNode KillSequence = new SequenceNode(KillList);
            SequenceNode FollowSequence = new SequenceNode(FollowList);
            List<BehaviourNode> ChaseList = new List<BehaviourNode>(); //creates sequence/series nodes for behaviours
            ChaseList.Add(KillSequence);
            ChaseList.Add(FollowSequence);
            SelectorNode HuntSelector = new SelectorNode(ChaseList);

            List<BehaviourNode> spinlist = new List<BehaviourNode>();
            spinlist.Add(needspin);
            spinlist.Add(spinning);
            SequenceNode SpinSelector = new SequenceNode(spinlist);

            List<BehaviourNode> lastposlist = new List<BehaviourNode>();
            lastposlist.Add(vlastpos);
            lastposlist.Add(golast);
            SequenceNode LPosSelector = new SequenceNode(lastposlist);

            List<BehaviourNode> FinalList = new List<BehaviourNode>();
            FinalList.Add(HuntSelector);
            FinalList.Add(SpinSelector);
            FinalList.Add(LPosSelector);
            FinalList.Add(patrol);
            List<BehaviourNode> EasyList = new List<BehaviourNode>();
            EasyList.Add(HuntSelector);
            EasyList.Add(patrol);
            SelectorNode root = new SelectorNode(FinalList); //puts lists together to make a tree
            easyroot = new SelectorNode(EasyList);//creates a tree without more complex behaviours for an easier experience
            normalroot = root;
            return root; //sets up the tree as lists of nodes that each have a list of nodes
        }
        IEnumerator checkobserved()//will execute every 50 ms
        {

            while (true)
            {
                observedlastiteration = observed;//checks if the enemy was observed last check
                observed = visiblecalc.islookingat(playercamera,enemy,58,35,walllayer,true);//checks if enemy is observed
                float timetowait = 0.05f;
                if (manager.currentdifficulty == Difficulty.Difficult)//will checks the visibility of halfway point if difficulty is hard;
                {
                    halfway.transform.position = visiblecalc.halfway(player.position, enemy.position);//finds position halfway between player and enemy;
                    halfwayobserved = visiblecalc.islookingat(playercamera, halfway, 58, 35, walllayer, true);//determines if halfway point is visible
                }
                if (!observed && observedlastiteration)//returns true if the enemy was observed last iteration
                {
                    if (manager.currentdifficulty!=Difficulty.Easy)                    
                    {
                        root.DataChecker["NeedToSpin"] = true;//when enemy is unfrozen, it will now spin around to look for player;
                    }
                    encounters++; //encounters is incremented
                }

                yield return new WaitForSeconds(timetowait);
            }
        }
        public override void Update()
        {
            if (!paused)
            {
                timer += Time.deltaTime;
                if (damagetimer > 0)
                {
                    damagetimer-= Time.deltaTime;
                    observed = true;//while damage timer is bigger than 0, enemy will be frozen
                }
                if (observed == false)
                {
                    playerpath.besneaky = halfwayobserved;
                    playerpath.dtime = Time.deltaTime;
                    playerpath.timer += Time.deltaTime;
                    patrol.dtime = Time.deltaTime;
                    spinning.dtime = Time.deltaTime;
                    spinning.timer += Time.deltaTime;
                    golast.dtime = Time.deltaTime;
                    playervisible.countdown-= Time.deltaTime;//time since last frame added to timers in beheavior nodes
                    base.Update();
                }
            }
            if (!survivalmode)
            {
                if (encounters < 4 && timer < 100)
                {
                    playerpath.chasespeed = 5;
                }
                if (encounters > 3)
                {
                    playerpath.chasespeed = 15;
                }
                if (encounters > 10 || timer > 150)
                {
                    playerpath.chasespeed = 30;//controls speed of enemy
                }
            }
        }

        public void ChangeDifficulty(Difficulty d)//changes the enemies difficulty
        {
            switch (d)
            {
                case Difficulty.Easy:
                    root = easyroot;//changes root to easy root
                    break;
                case Difficulty.Normal:
                    root = normalroot;
                    //sets root to normal
                    break;
                case Difficulty.Difficult:
                    root = normalroot;
                    playerpath.chasespeed = 50f;
                    playervisible.timespentchasing = 2f;//increase speed and time spend chasing player
                    break;

            }

        }
        public void MakeSneaky()
        {
            pfinder.sneakymode = true;//ensures pathfinding node weighs the nodes in front of player, so that enemy tries to sneak around player
        }
        public void HuntBetter()
        {
            pfinder.sneakymode = false;
            playervisible.timespentchasing = 2f; //after player runs out of sight, will spend longer chasing after them before giving up
        }
        public void TakeDamage()
        {
            Debug.Log("im shot");
            damagetimer = 2.5f; //freezes enemy for 2.5 secs
        }
        public void speedincrease()
        {
            playerpath.chasespeed *= 1.1f;
        }

    }

}
