using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathsMethods //mathematical methods to be used in project
{
    public Vector2 normaliser(Vector2 vtnorm)
    {
        float dist = Mathf.Sqrt(vtnorm.x * vtnorm.x + vtnorm.y * vtnorm.y);
        Vector2 final = new Vector2(vtnorm.x / dist, vtnorm.y / dist);
        return final;//divides 2d transformation by total distance

    }
    public float findDif(Vector3 a, Vector3 b)
    {
        float xdif = a.x - b.x;
        float zdif = a.z - b.z;
        return (Mathf.Sqrt(xdif * xdif + zdif * zdif));//simply pythag finds differecnt between two vectors
    }
    public bool islookingat(Transform a, Transform b, int maxanglex, int maxangley, LayerMask walls, bool fullcheck)
    {

        bool PathClear()
        {
            Vector3 rightofrecipient = b.position + a.transform.right * 0.7f;
            Vector3 leftofrecepient = b.position - a.transform.right * 0.7f;
            Vector3 topofrecipient = b.position;
            Vector3 bottomofrecipient = b.position;
            topofrecipient.y += 1.4f;
            bottomofrecipient.y -= 1.4f;
            bool E = lineofsightblocked(a.position, b.position - a.transform.forward * 0.3f);
            bool anylinesofisght = !E;
            if (fullcheck == true)//checks line of sight more thoroughly by checking if all sides of B are blocked
            {
                bool A = lineofsightblocked(a.position, leftofrecepient);
                bool B = lineofsightblocked(a.position, rightofrecipient);
                bool C = lineofsightblocked(a.position, topofrecipient - a.transform.forward * 0.3f);
                bool D = lineofsightblocked(a.position, bottomofrecipient - a.transform.forward * 0.3f);
                anylinesofisght = !A || !B || !C || !D || !E;
            }
            return anylinesofisght;
        }

        bool lineofsightblocked(Vector3 one ,Vector3 two)
        {
        return Physics.Linecast(one, two, walls);//checks if line of sight between two objects is clear
         }
        bool ObservedY()
       {
        float eypos = b.position.y;
        float bottom = eypos - 1.5f;//bottom of b
        float top = eypos + 1.5f;//top of b
        float dif = findDif(a.position, b.position);//finds difference between A and B in x and y
        float topdif = (top-a.position.y);
        float topangle= Mathf.Rad2Deg * Mathf.Atan(topdif / dif);//finds the angle between A and the top of B
        float angle = a.transform.eulerAngles.x;//finds angle that A is looking at in y
        
        if (angle <= 360 && angle >= 270)
        {
            angle = ((270 - angle) + 90);//ensures angle is in correct range

        }
        else
        {
            angle = -angle;//ensures ang
        }
        if (Mathf.Abs(angle - topangle) < maxangley + (8/dif))
        {
            return true;//checks if difference between angles is in allowed range
        }
        float botdif = (a.position.y-bottom);
        float botangle =-( Mathf.Rad2Deg * Mathf.Atan(botdif / dif));
        if (Mathf.Abs(angle - botangle) < maxangley + (8 / dif))//does the same for angle between A and the bottom of B
        {
            return true;
        }
        
        return false;

    }

        bool ObservedX()
        {
        Vector2 pp = new Vector2(a.position.x, a.position.z);
        Vector2 ep = new Vector2(b.position.x, b.position.z);
        Vector2 dahead = new Vector2(a.forward.x, a.forward.z);//represents position infront of a

        dahead = normaliser(dahead);
        pp = normaliser(ep - pp);
        float dotProduct = (dahead.x * pp.x + dahead.y * pp.y);//gets dot product of the two normalised vectors
        float angle = Mathf.Rad2Deg * Mathf.Acos(dotProduct);// Acos the dot product to get angle
        float dif= (findDif(a.position, b.position));
        if (angle < maxanglex + (20/dif))//checks if angle is within paramaters
        {
            return true;
        }
        else
        {
            return false;
        }
    }
        return PathClear() && ObservedX() && ObservedY();//checks if all conditions are met
    }

    public Vector3 halfway(Vector3 A, Vector3 B)
    {
        float newX = (A.x + B.x) / 2;
        float newY = (A.y + B.y) / 2;
        float newZ = (A.z + B.z) / 2;
        return new Vector3 (newX, newY, newZ);//finds midway point between two vectors
    }


}
