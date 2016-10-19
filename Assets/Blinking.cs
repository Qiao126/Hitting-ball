using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class Blinking : MonoBehaviour
{
    public Text display;
    private bool isTargetOn;
    public int total;
    private int trial;
    public int block;
    public float distance;
    public float width;
    private double timer;
    public int hits;
    private GameObject[] walls;
    private float flash;

    public int mode;
    private float speed_a;
    private float speed_b;
    private float current_speed;
    private float enter_speed;
    private bool hasEntered;
    private string speed_report;

    private int i, j, k;
    float[] D = { 0.9f, 1.5f };
    float[] W = { 0.08f, 0.16f, 0.24f };
    int[] M = { 0, 1, 2, 3 };

    // Use this for initialization
    void Start()
    {
        isTargetOn = false;
        trial = 1;
        block = 1;
        hasEntered = false;
        flash = 1;
        hits = 0;
        i = 0;
        j = 0;
        k = 0;
        distance = D[i];
        width = W[j];
        mode = k;
    }
    // Update is called once per frame
    void Update()
    {
        if (trial < total+1 && block < 25)
        {
            Blink();
            this.display.text = "Block: " + block.ToString() + " Trial: " + trial.ToString() + " Count: " + hits.ToString();
        } else {
            block++;
            trial = 1;
            hits = 0;
            k++;
            if (k > 3) { k = 0; j++; }
            if (j > 2) { j= 0; i++; }
            distance = D[i];
            width = W[j];
            mode = k;
        }
        if (mode == 1 || mode == 3)
        {
            if (flash < 1)
            {
                flash = flash + 0.1f;
            }
            else
            {
                flash = 1;
            }
            walls = GameObject.FindGameObjectsWithTag("marked_wall");
            foreach (GameObject wall in walls)
            {
                wall.GetComponent<MeshRenderer>().material.color = new Color(0.67f, 0.96f, 0.65f, flash);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        enter_speed = other.attachedRigidbody.GetPointVelocity(this.gameObject.GetComponent<SphereCollider>().center).magnitude;

        if (mode == 0)
        {
            if (other.gameObject.name == "myPalm")
            {
                
                if (isTargetOn)
                {
                    hits++;
                    print(hits + "," + trial);
                    this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                    flash = 0;
                }
            }
        }
        speed_a = enter_speed;
        speed_b = enter_speed;
        hasEntered = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (mode == 1)
        {
            current_speed = other.attachedRigidbody.GetPointVelocity(this.gameObject.GetComponent<SphereCollider>().center).magnitude;
            if (current_speed != 0 && hasEntered)
            {
                if (speed_a > speed_b && current_speed > speed_b)
                {
                    if (other.gameObject.name == "myPalm")
                    {
                        if (isTargetOn)
                        {
                            hits++;
                            print(hits + "," + trial);
                            speed_report += "\n" + 999999999999999;
                            //print(speed_a +" , " + speed_b +" , "+current_speed);
                            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                            flash = 0;
                        }
                        hasEntered = false;
                    }
                }
                speed_a = speed_b;
                speed_b = current_speed;
            }

            if (current_speed != 0)
            {
                speed_report += "\n" + current_speed;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        File.WriteAllText("./report.txt", speed_report);
    }

    void Blink()
    {
        if (Time.time > timer)
        {
            if (isTargetOn)
            {
                timer = Time.time + distance;
                isTargetOn = false;
                this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
            else
            {
                timer = Time.time + width;
                isTargetOn = true;
                this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                trial++;
            }
        }
    }

}

