using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class boxer : MonoBehaviour {
	private double timer;
	private bool isTargetOn;
	private int count;
	private Vector3 ang_col;
	private Vector3 ang_boxer;

	private float speed_a;
	private float speed_b;
	private float current_speed;
	private float enter_speed;
	public bool hasEntered;
    public bool hasHit;

    public int mode;     // 0 no haptic   1 haptic
    public int detection;   // 0 continuous dynamic         1 like a boxer
    public Text display;
    private float dist;

    // Use this for initialization
    void Start () {
		isTargetOn = true;
        dist = 0;
		hasEntered=false;
        hasHit = false;
	
	}

	void Update () {
        if (count < 51)
        {
            this.display.text = "Trail: " + count + " Distance from center: " + this.dist.ToString("#.00");

            if (!isTargetOn && Time.time > timer)
            {

                this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                this.gameObject.transform.position = new Vector3(0.0f, 0.9f, 0.25f);
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                this.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                this.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
                isTargetOn = true;
            }
            if (detection == 1 && hasEntered && !hasHit)
            {
                this.gameObject.transform.position = new Vector3(0.0f, 0.9f, 0.25f);
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                this.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            }
        }
    }

	void OnCollisionEnter (Collision col)
	{
        
		if (col.gameObject.name == "myPalm") {    //add LIKE_A_BOXER algorithm
			
			timer = Time.time + 1.5;
			ang_col = col.contacts [0].normal;
            if (detection == 1)
            {
                enter_speed = col.gameObject.GetComponent<Rigidbody>().GetPointVelocity(this.gameObject.GetComponent<SphereCollider>().center).magnitude;
                speed_a = enter_speed;
                speed_b = enter_speed;
                hasEntered = true;
            }
            else { isTargetOn = false; }
		}
		if(col.gameObject.name == "backwall")// 
		{
			this.gameObject.GetComponent<MeshRenderer> ().material.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
			this.gameObject.GetComponent<Rigidbody> ().detectCollisions = false;
			ContactPoint contact = col.contacts [0];
			print(contact.point+" trial: "+count);
			dist = Vector3.Distance (contact.point, col.gameObject.transform.position);
            print(dist);
			isTargetOn = false;
            hasHit = false;
			count++;
		}

	}
	void OnCollisionStay(Collision col){
       
            if (col.gameObject.name == "myPalm" && detection==1) {
        
                current_speed = col.gameObject.GetComponent<Rigidbody> ().GetPointVelocity (this.gameObject.GetComponent<SphereCollider> ().center).magnitude;
			if (current_speed != 0 && hasEntered) {
				if (speed_a > speed_b && current_speed > speed_b) {
                    ang_boxer = col.contacts [0].normal;
                    print(speed_a + " " + speed_b + " " + current_speed);
                    hasHit = true;
					hasEntered = false;
				} 
				speed_a = speed_b;
				speed_b = current_speed;
			}

			isTargetOn = false;
		}
	}

}
