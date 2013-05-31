using UnityEngine; 
using System.Collections; 
public class Bullet : MonoBehaviour { 
	 public Vector3 thrust; 
	 public Quaternion heading; 
	 float deathTimer; 
	
	 // Use this for initialization 
	 void Start () { 
		 // travel straight in the X-axis 
		 thrust.x = 400.0f; 
		 // do not passively decelerate 
		 gameObject.rigidbody.drag = 0; 
		 // set the direction it will travel in 
		 gameObject.rigidbody.MoveRotation(heading); 
		 // apply thrust once, no need to apply it again since 
		 // it will not decelerate 
		 gameObject.rigidbody.AddRelativeForce(thrust); 
	 } 
	
	 // Update is called once per frame 
	 void Update () { 
		deathTimer += Time.deltaTime;
		KeepObjOnScreen(); 
		if (deathTimer > 3.0f) {
			Destroy(gameObject); 
		}
	} 
	
	 void OnCollisionEnter( Collision collision ) 
	 { 
		// the Collision contains a lot of info, but it’s the colliding 
		// object we’re most interested in. 
		Collider collider = collision.collider; 
		 if( collider.CompareTag("Astroids")) 
		 { 
			 Astroid roid = collider.gameObject.GetComponent< Astroid >(); 
			// let the other object handle its own death throes
			 roid.Die(); 
			 // Destroy the Bullet which collided with the Asteroid 
			 Destroy(gameObject); 
		 } 
		 else if( collider.CompareTag("UFO")) 
		 { 
			 UFO foe = collider.gameObject.GetComponent< UFO >(); 
			 foe.Die(); 
			 Destroy(gameObject); 
		 } 
		 else 
		 { 
			//TODO
			 // if we collided with something else, print to console 
			 // what the other thing was 
			 Debug.Log ("Collided with " + collider.tag); 
		 } 
	 } 
	
	//Keeps the bullet on the screen
	void KeepObjOnScreen () {
		Vector3 pos = gameObject.transform.position; 
		Vector3 screenPos = Camera.main.WorldToViewportPoint(pos); 
		
		//make sure that obj remains on the y axis
		if (Mathf.Abs(pos.y) > 0.1f) {
			pos.y = 0.0f; 	
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