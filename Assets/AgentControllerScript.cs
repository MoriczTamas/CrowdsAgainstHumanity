using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AgentControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject participant_prefab;
    public GameObject[] WorkingRooms;
    public Transform SnackBar;
    public Transform[] spawnPoints;
    public int numOfAgents;
    public GameObject[] participants;
    public Color[] colors;
    public GameObject[] Toilets;
    public GameObject obstaclePrefab;
    public GameObject targetPrefab;
    public GameObject target;
    public float evacuationStartTime;
    public bool evacuating = false;
    public bool allDead = false;
    public Text[] texts = new Text[4];
    public GameObject Lights;
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        participants = new GameObject[numOfAgents];
        for (int i = 0; i < numOfAgents; i++)
        {
            GameObject participant = Instantiate(participant_prefab, spawnPoints[Random.Range(0, spawnPoints.Length)]);
            int index = Random.Range(0, 24);
            int toiletIndex = Random.Range(0, 6);
            Transform[] newGoals = new Transform[3];
            newGoals[0] = WorkingRooms[Random.Range(0,2)].transform.GetChild(index).gameObject.transform;
            newGoals[1] = Toilets[Random.Range(0, 2)].transform.GetChild(toiletIndex).gameObject.transform;
            newGoals[2] = SnackBar;
            participant.GetComponent<ParticipantScript>().setCommandGoal(newGoals[0]);
            participant.GetComponent<ParticipantScript>().setGoals(newGoals);
            //participant.GetComponent<ParticipantScript>().setColor(colors[index]);
            int newSpeed = Random.Range(3, 10);
            participant.GetComponent<ParticipantScript>().setSpeed(newSpeed);
            participants[i] = participant;
        }
        texts[2].text = "";
        texts[3].text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            //Initialize();
            SceneManager.LoadScene("SampleScene");
        }
        int num_of_people = 0;
        for(int i = 0; i < participants.Length; ++i)
        {
            if (participants[i] != null)
                num_of_people++;
        }
        texts[1].text = "Number of people at the venue: " + num_of_people;
        if (allDead && evacuating)
        {
            int currentTime = (int)(Mathf.Floor(Time.time) - Mathf.Floor(evacuationStartTime));
            texts[3].text = "Time to evacuate: " + currentTime/60 + ":" + (currentTime%60).ToString("D2");
            evacuating = false;
        }
        if (evacuating)
        {
            int currentTime = (int)(Mathf.Floor(Time.time) - Mathf.Floor(evacuationStartTime));
            texts[3].text = "Time to evacuate: " + currentTime / 60 + ":" + (currentTime % 60).ToString("D2");
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            Vector3 wordPos = new Vector3(0,0,0);
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1000f);
            if (hit.collider.gameObject.tag != "Obstacle")
            {
                wordPos = hit.point;
            }
            else
            {
                //wordPos = Camera.main.ScreenToWorldPoint(mousePos);
                Destroy(hit.transform.gameObject);
            }
            wordPos.y += 0.5f;
            Instantiate(obstaclePrefab, wordPos, Quaternion.identity);
            //or for tandom rotarion use Quaternion.LookRotation(Random.insideUnitSphere)
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
            Vector3 wordPos = new Vector3(0, 0, 0);
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1000f);
            if (hit.collider.gameObject.tag == "Ground")
            {
                wordPos = hit.point;
                wordPos.y += 0.5f;
                target = Instantiate(targetPrefab, wordPos, Quaternion.identity);
                texts[0].text = "Event placed";
            }
            
            
        }
        if (Input.GetMouseButtonDown(2) && target != null)
        {
            evacuating = true;
            evacuationStartTime = Time.time;
            for (int i = 0; i < participants.Length; ++i)
            {
                participants[i].GetComponent<ParticipantScript>().setSuicideGoal(target.transform);
            }
            texts[2].text = "FIRE ALARM IS ACTIVE!";
            for(int i = 0; i < 44; ++i)
            {
                Lights.transform.GetChild(i).gameObject.GetComponent<Light>().color = new Color32(255, 0, 0, 255);
            }
        }
        if(Input.GetKey(KeyCode.Space) && target!= null)
        {
            for(int i = 0; i < participants.Length; ++i)
            {
                participants[i].GetComponent<ParticipantScript>().setCommandGoal(target.transform, 15);
            }
            texts[0].text = "Event triggered";
        }
        allDead = true;
        for(int i = 0; i < participants.Length; ++i)
        {
            if (participants[i] != null)
                allDead = false;
        }
    }
}
