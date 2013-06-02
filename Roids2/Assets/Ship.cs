using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour { 
	public Vector3 forceVector; 
	public float rotationSpeed; 
	public float rotation; 
	
	public GameObject deathExplosion; 
	public AudioClip deathKnell; 
	
	//variables when a ship is hit, but has more lives
	public bool isDead; 

	// Use this for initialization 
	void Start () { 
		// Vector3 default initializes all components to 0.0f 
		forceVector.x = 1.0f; 
		rotationSpeed = 2.0f; 	
		isDead = false;
	
	} 
	
/* forced changes to rigid body physics parameters should be done 
 through the FixedUpdate() method, not the Update() method 
*/ 
 void FixedUpdate() 
 { 
	// force thruster 
	if( Input.GetAxisRaw("Vertical") > 0 ) 
	{ 
		forceVector = new Vector3(0.0f, 2.0f, 2.0f);  
		gameObject.rigidbody.AddRelativeForce(forceVector); 
	} 
	if( Input.GetAxisRaw("Horizontal") > 0 ) 
	{ 
		rotation += rotationSpeed; 
		Quaternion rot = Quaternion.Euler(new Vector3(90, rotation, 0)); 
		//Vector3 newRot = new Vector3(0,rotation,0); 
		gameObject.rigidbody.MoveRotation(rot); 
		//gameObject.transform.Rotate(0.0f, 0.0f, 2.0f ); 
	} 
	else if( Input.GetAxisRaw("Horizontal") < 0 ) 
	{ 
		rotation -= rotationSpeed; 
		Quaternion rot = Quaternion.Euler(new Vector3(90, rotation, 0)); 
		//Vector3 newRot = new Vector3(0,rotation,0); 
		gameObject.rigidbody.MoveRotation(rot); 
		//gameObject.transform.Rotate( 0.0f, 0.0f,  -2.0f ); 
	} 
 } 
	
	public GameObject bullet; // the GameObject to spawn 
	void Update () { 	
		KeepObjOnScreen(); 
			
		if(Input.GetButtonDown("Fire1")) 
			{ 
				Debug.Log ("Fire! " + rotation); 
				/* we don’t want to spawn a Bullet inside our ship, so some 
				Simple trigonometry is done here to spawn the bullet 
				at the tip of where the ship is pointed. 
				*/ 
				Vector3 spawnPos = gameObject.transform.position; 
				//spawnPos.x += 1.0f * Mathf.Cos(rotation * Mathf.PI/180); 
				//spawnPos.z += 1.0f * Mathf.Sin(rotation * Mathf.PI/180); 
				// instantiate the Bullet 
				GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
			
				// get the Bullet Script Component of the new Bullet instance 
				Bullet b = obj.GetComponent<Bullet>(); 
				Transform bulletTrans = b.transform; 
				Physics.IgnoreCollision(bulletTrans.collider, collider);
				// set the direction the Bullet will travel in 
				Quaternion rot = Quaternion.Euler(new Vector3(0,rotation-90,0)); 
				b.heading = rot; 
			} 
	 }  
	
	public void Die() 
	{ 
		//Create explosion effect
		AudioSource.PlayClipAtPoint(deathKnell, gameObject.transform.position ); 
		Instantiate(deathExplosion, gameObject.transform.position,Quaternion.AngleAxis(-90, Vector3.right) );
		//Update globals to keep score 
		GameObject obj = GameObject.Find("GlobalObject");
		Global g = obj.GetComponent<Global>(); 
		g.lives--; 
		GameObject shipRent = GameObject.Find("ShipParent");
        VisibilityChangeScript r = shipRent.GetComponent<VisibilityChangeScript>();
		r.isDead = true; 
		gameObject.SetActive(false);
		if (g.lives < 1) 
			Destroy (gameObject); 
	} 
	
	//Makes sure that the ship stays on the screen
	void KeepObjOnScreen () {
		Vector3 pos = gameObject.transform.position; 
		Vector3 screenPos = Camera.main.WorldToViewportPoint(pos); 
			
		//make sure that obj remains on the y axis
		if (Mathf.Abs(pos.y) > 0.01f) {
			pos.y = 0.0f; 	
			gameObject.transform.position = pos; 
		}
		
		if (screenPos.x < -0.1f) {
			print ("Under the x");
			screenPos.x = Screen.width;
			pos.x =  Camera.main.ScreenToWorldPoint(screenPos).x; 
			gameObject.transform.position = pos; 
		} else if (screenPos.x > 1.1f) {
			print ("Over the x");
			screenPos.x = 0; 
			pos.x = Camera.main.ScreenToWorldPoint(screenPos).x; 
			gameObject.transform.position = pos;
		} 
		if (screenPos.y <-0.1f) {
			print ("Under the y");
			screenPos.y = Screen.height; 
			pos.z =  Camera.main.ScreenToWorldPoint(screenPos).z; 
			gameObject.transform.position = pos; 
		} else if (screenPos.y > 1.1f) {
			print ("Over the y");
			screenPos.y = 0; 
			pos.z = Camera.main.ScreenToWorldPoint(screenPos).z;
			gameObject.transform.position = pos;
		} 	
	}
} 
