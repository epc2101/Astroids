using UnityEngine;
using System.Collections;

public class Astroid : MonoBehaviour {

	public int lifeSpan; 
	public GameObject deathExplosion; 
	public AudioClip deathKnell; 
	int pointValue; 
	
	// Use this for initialization
	void Start () {
		pointValue = 10; 
		if (lifeSpan == 2) gameObject.transform.localScale = new Vector3(2.85f, 2.85f, 2.85f); 
		if (lifeSpan == 1) gameObject.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f); 
		if (lifeSpan == 0) gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); 
	}
	
	// Update is called once per frame
	void Update () {
		KeepObjOnScreen();
	}
	
 
	public void Die() 
	{ 
		AudioSource.PlayClipAtPoint(deathKnell, 
		gameObject.transform.position ); 
		Instantiate(deathExplosion, gameObject.transform.position, 
		Quaternion.AngleAxis(-90, Vector3.right) ); 
		GameObject obj = GameObject.Find("GlobalObject");
		Global g = obj.GetComponent<Global>(); 
		g.score += pointValue; 
		g.currNumAstr--; 
		Destroy (gameObject); 
		g.addBabyRoids(lifeSpan, gameObject); 
	} 
	
	//Used to destroy ships 
	void OnCollisionEnter( Collision collision ) 
	 { 
		// the Collision contains a lot of info, but it’s the colliding 
		// object we’re most interested in. 
		GameObject obj = GameObject.Find("GlobalObject");
		Global g = obj.GetComponent<Global>(); 
		Collider collider = collision.collider; 
		 if( collider.CompareTag("Ship")) 
		 { 
			Ship ourShip = collider.gameObject.GetComponent< Ship >(); 
			ourShip.Die(); 
			//g.score += pointValue; 
			g.currNumAstr--; 
			Destroy(gameObject); 
		 } else if( collider.CompareTag("UFO")) 
		 { 
			 UFO foe = collider.gameObject.GetComponent< UFO >(); 
			 foe.Die(); 
			 Destroy(gameObject);
			 g.currNumAstr--; 
		 } 
	}
	
	void KeepObjOnScreen () {

		
		Vector3 pos = gameObject.transform.position; 
		Vector3 screenPos = Camera.main.WorldToViewportPoint(pos); 
		
		//make sure that obj remains on the y axis
		if (Mathf.Abs(pos.y) > 0.1f) {
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
