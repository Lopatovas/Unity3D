using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {


    // Acceleration value
    public float acceleration;
    //Interaction with oil spills
    private bool oilHit = false;
    private float speedOnOil = 1f;
    //Interaction with offroad
    private bool offroad = false;
    private float speedWhileOffroad = 1f;
    //Break
    public float breakValue;
    // turning value
    public float torqueForce;
    public float turning;
    // Drift
    public float driftSticky;
    public float driftSlip;
    private float maxSticky = 2.5f;

    //Lap triggers
    private bool lapStart = false;
    private bool crossedLapLine1 = false;
    private bool crossedLapLine2 = false;
    private bool crossedLapLine3 = false;
    private bool crossedLapLine4 = false;
    private bool crossedLapLine5 = false;

    //Amount of laps
    private float lapsDone = 0;
    public float numberOfLaps;
    private int score;
    //UI control
    public Text InfoText;
    public string position;
    public AISteering[] ai;

    public GameManager manager;
    // Use this for initialization
    void Start () {
        
	}

    void FixedUpdate ()
    {
        position = Position();
        UpdateInfoText();
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();

        //Default drift value
        float driftFactor = driftSticky;

        if(SidewayVelocity().magnitude > maxSticky)
        {
            driftFactor = driftSlip;
        }

        rb2D.velocity = ForwardVelocity() + SidewayVelocity() * driftFactor;

        //To get it moving.
        if (Input.GetButton("Accelerate"))
        {
            rb2D.AddForce(transform.up * acceleration * speedOnOil * speedWhileOffroad);
        }

        if (Input.GetButton("Break"))
        {
            rb2D.AddForce(transform.up * -acceleration/breakValue);
        }

        //the faster you go the more turning you get.
        float turning = Mathf.Lerp(0, torqueForce, rb2D.velocity.magnitude / 5);
        //Turns
        rb2D.angularVelocity = Input.GetAxis("Horizontal") * torqueForce * turning;

        if(oilHit == true)
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
        if(numberOfLaps - lapsDone == 0)
        {
            manager.EndGame();
        }
        
    }

    //For sliding reduction
    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }

    Vector2 SidewayVelocity()
    {
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
    }

    public void UpdateInfoText()
    {
        InfoText.text = "Laps left:" + (numberOfLaps - lapsDone);
    }

    //Upon collision sets values.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Register oilhit.
        if(collision.CompareTag("oil") == true)
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
            Debug.Log("1");
            score++;
        }

        else if (collision.CompareTag("Lap line 1") == true && lapStart == true)
        {
            lapStart = false;
            crossedLapLine1 = true;
            Debug.Log("2");
            score++;
        }

        else if (collision.CompareTag("Lap line 2") == true && crossedLapLine1 == true)
        {
            crossedLapLine1 = false;
            crossedLapLine2 = true;
            Debug.Log("3");
            score++;
        }

        else if (collision.CompareTag("Lap line 3") == true && crossedLapLine2 == true)
        {
            crossedLapLine2 = false;
            crossedLapLine3 = true;
            Debug.Log("4");
            score++;
        }

        else if (collision.CompareTag("Lap line 4") == true && crossedLapLine3 == true)
        {
            crossedLapLine3 = false;
            crossedLapLine4 = true;
            Debug.Log("5");
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
            lapsDone++;
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
    
    private string Position()
    {
        int place = 4;
        for (int i = 0; i < ai.Length; i++)
        {
            if(ai[i].score <= score)
            {

                place--;
            }
            else
            {
                for(int j = i; j < ai.Length ; j++)
                {
                    if (ai[j].score <= score)
                    {

                        place--;
                    }
                }
            }

        }

        if(place == 4)
        {
            position = "4th";
        }
        else if(place == 3)
        {
            position = "3rd";
        }
        else if (place == 2)
        {
            position = "2nd";
        }
        else if (place == 1)
        {
            position = "1st";
        }

        return position;
    }
}
