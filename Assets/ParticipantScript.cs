using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParticipantScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] goals;
    NavMeshAgent agent;
    public bool onTheWay = false;
    public float waitStart;
    public int waitLength;
    public Transform currentGoal;
    public NavMeshPathStatus status;
    public NavMeshPath path;
    public float distance;
    public Vector3 destination;
    private float elapsed = 0.0f;
    private bool commanded = false;
    private int targetDist;
    private bool suicide = false;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        byte rand = (byte)Random.Range(100, 200);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color32(rand, rand, rand, 255));
    }

    // Update is called once per frame
    void Update()
    {
        status = agent.pathStatus;
        //path = agent.path;
        distance = agent.remainingDistance;
        destination = agent.destination;

        //Debug.Log(agent.remainingDistance);

        if (((transform.position - currentGoal.position).magnitude < targetDist) && onTheWay)
        {
            if (suicide)
                Destroy(gameObject);
            else
            {
                onTheWay = false;
                waitStart = Time.time;
                waitLength = Random.Range(5, 20);
                targetDist = 5;
                // currentGoal = transform;
                //agent.destination = currentGoal.position;
            }
        }
        if(((Time.time - waitStart) > waitLength) && !onTheWay)
        {
            int index = Random.Range(0, goals.Length);
            if (index == 0)
            {
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color32(0, (byte)Random.Range(100,200), 0, 255));
            }
            else if(index == 1)
            {
                byte rando = (byte)Random.Range(100, 200);
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color",new Color32(rando,rando,0,255));
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color32(0, 0, (byte)Random.Range(100, 200), 255));
            }
            currentGoal = goals[index];
           // agent.destination = currentGoal.position;
            onTheWay = true;
        }
        if (onTheWay)
        {
            int chance = Random.Range(0, 6);

            elapsed += Time.deltaTime;
            if (elapsed > 1.0f)
            {
                elapsed -= 1.0f;
                if (chance == 0)
                {
                    NavMesh.CalculatePath(transform.position, currentGoal.position, NavMesh.AllAreas, path);
                    agent.SetPath(path);
                }
            }
        }
        
    }

    public void setGoals(Transform[] newGoal) {
        
        goals = newGoal;
    }

    public void setColor(Color newColor)
    {
        GetComponent<Light>().color = newColor;
    }

    public void setSpeed(int newSpeed)
    {
        agent.speed = newSpeed;
    }

    public void setCommandGoal(Transform goal, int targetDistance = 5)
    {
        if (commanded)
        {
            byte rando = (byte)Random.Range(100, 200);
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color32(rando, 0, rando, 255));
        }
        commanded = true;
        targetDist = targetDistance;
        currentGoal = goal;
        agent.destination = currentGoal.position;
        onTheWay = true;
    }

    public void setSuicideGoal(Transform goal)
    {
        //gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color32(254, 173, 185, 255));
        byte rando = (byte)Random.Range(100, 200);
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color32(rando, 0, 0, 255));
        suicide = true;
        currentGoal = goal;
        agent.destination = currentGoal.position;
        onTheWay = true;

    }
}
