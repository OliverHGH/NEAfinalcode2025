using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CanSeePlayer : BehaviourNode //node that checks if enemy can see player
{
    MathsMethods mathsh = new MathsMethods();
    Transform enemy;
    Transform forward;
    Transform player;
    Vector3 playertop,enemytop;
    private bool couldseelastframe = false;
    BehaviourNode ptoedit;
    private LayerMask walls;
    public float countdown=0f;
    public float timespentchasing=0.75f;
    public  CanSeePlayer(Transform e, Transform f, Transform p, LayerMask w)//constructor assigns variables
    {
        enemy = e;
        forward = f;
        player = p;
        playertop = player.position;
        enemytop = enemy.position;
        playertop.y += 1.3f;
        enemytop.y += 1.3f;
        walls = w;
    }
    public override NodeState Evaluate()
    {
        ptoedit = getroot();
        bool playervisible = mathsh.islookingat(forward,player,80,50,walls,false);
        if(playervisible)
        {
            couldseelastframe = true;
            ptoedit.DataChecker["NeedToSpin"] = false;//dont need to spin to find player if player is visible
            ptoedit.DataChecker["lastseenpos"] = player.position;//makes the player position the target if player is visible
            countdown = timespentchasing;//resets timer if player visible
            return NodeState.SUCCESS;
        }
        if (couldseelastframe == true)
        {
            ptoedit.DataChecker["NeedToSpin"] = true;//if enemy has lost sight if player, it will spin around
            
        }
        if (countdown > 0)
        {//if timer hasnt run out, returns sucess
            return NodeState.SUCCESS;
        }
       

        couldseelastframe = false;
        return NodeState.FAILURE;

    }
}

 
