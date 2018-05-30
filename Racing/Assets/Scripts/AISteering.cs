using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISteering : MonoBehaviour {


    public Movement movement;
    //Ai guidence
    public Transform path;
    public float maxSteerAngle = 45f;
    private List<Transform> nodes;
    private int currentNode = 0;


    // Acceleration value
    public float acceleration;
    //Interaction with oil spills
    private bool oilHit = false;
    private float speedOnOil = 1f;
    //Interaction with offroad
    private bool offroad = false;
    private float speedWhileOffroad = 1f;
    //Amount of laps
    public float lapsDone = 0;
    private float numberOfLaps;
    public int score;

    //Lap triggers
    private bool lapStart = false;
    private bool crossedLapLine1 = false;
    private bool crossedLapLine2 = false;
    private bool crossedLapLine3 = false;
    private bool crossedLapLine4 = false;
    private bool crossedLapLine5 = false;

    // Use this for initialization
    void Start () {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        numberOfLaps = movement.numberOfLaps;
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        CheckWaypointDistance();
        transform.Rotate(Vector3.forward * - ApplySteer());
        rb2D.AddForce(transform.up * acceleration * speedOnOil * speedWhileOffroad);

        if (oilHit == true)
        {
            speedOnOil = 0.2f;
        }
        else
        {
            speedOnOil = 1f;
        }

        if (offroad == true)
        {
            speedWhileOffroad = 0.5f;
        }
        else
        {
            speedWhileOffroad = 1f;
        }

        //EndGame screen
        if (numberOfLaps - lapsDone == 0)
        {
            acceleration = 0;
            score = score + 10;
        }
    }

    private float ApplySteer()
    {
        Vector2 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        return newSteer;
    }

    private void CheckWaypointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 5f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
                lapsDone++;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Register oilhit.
        if (collision.CompareTag("oil") == true)
        {
            oilHit = true;
        }

        //Register if driving off road.
        else if (collision.CompareTag("OffRoad") == true)
        {
            offroad = true;
        }

        //Lap triggers

        if (collision.CompareTag("StartLine") == true)
        {
            lapStart = true;
            score++;
        }

        else if (collision.CompareTag("Lap line 1") == true && lapStart == true)
        {
            lapStart = false;
            crossedLapLine1 = true;
            score++;
        }

        else if (collision.CompareTag("Lap line 2") == true && crossedLapLine1 == true)
        {
            crossedLapLine1 = false;
            crossedLapLine2 = true;
            score++;
        }

        else if (collision.CompareTag("Lap line 3") == true && crossedLapLine2 == true)
        {
            crossedLapLine2 = false;
            crossedLapLine3 = true;
            score++;
        }

        else if (collision.CompareTag("Lap line 4") == true && crossedLapLine3 == true)
        {
            crossedLapLine3 = false;
            crossedLapLine4 = true;
            score++;
        }

        else if (collision.CompareTag("Lap line 5") == true && crossedLapLine4 == true)
        {
            crossedLapLine4 = false;
            crossedLapLine5 = true;
            score++;
        }

        else if (collision.CompareTag("Finish line") == true && crossedLapLine5 == true)
        {
            crossedLapLine5 = false;
            score++;

        }
    }

    //Exiting triggers

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Register exit of oil area
        if (collision.CompareTag("oil") == true)
        {
            oilHit = false;
        }
        //Register exit of offroad area
        else if (collision.CompareTag("OffRoad") == true)
        {
            offroad = false;
        }
    }



}
