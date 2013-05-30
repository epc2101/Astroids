using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {

	public GameObject ufoToSpawn; 
	public GameObject astrToSpawn; 

	public float timer; 
	public float ufoTimer;
	public float spawnPeriod; 
	public int numberAstrSpawnedEachPeriod; 
	public int numberUfoSpawnedEachPeriod;
	public int currNumAstr;
	public int currNumUfo; 
	public Vector3 originInScreenCoords; 
	
	public int score; 
	public int lives; 
	public int level; 
	
	 // Use this for initialization 
	 void Start () { 
		score = 0; 
		timer = 0; 
		level = 1; 
		lives = 3; 
		
		spawnPeriod = 100.0f; 
		numberAstrSpawnedEachPeriod = 3;
		numberUfoSpawnedEachPeriod = 1;	
		currNumAstr = 0; 
		currNumUfo = 0; 
		
		 /* 
		 So here's a design point to consider: 
		- is the gameplay constrained by the screen size in any particular way? 
		That might sound like a weird question, but it's actually a significant one for asteroids if you want the 
		game to play like Asteroids on arbitrary screen dimensions. It's mostly here for 
		pedagogical reasons, though. The value that actually matters here is the depth value. Since the gameplay 
		takes place on a XZplane, and we're looking down the Y-axis, 
		we're mainly interested in what the Y value of 0 maps to in the camera's depth. 
		*/ 
		 originInScreenCoords = 
		 Camera.main.WorldToScreenPoint(new Vector3(0,0,0)); 
	 } 
	
	// Update is called once per frame 
	void Update () { 
		timer += Time.deltaTime; 
		if( timer > spawnPeriod ) 
		{ 
			timer = 0; 
			float width = Camera.main.GetScreenWidth(); 
			float height = Camera.main.GetScreenHeight(); 
			for( int i = 0; i < numberAstrSpawnedEachPeriod; i++ ) 
			{ 
				float horizontalPos = Random.Range(0.0f, width); 
				float verticalPos = Random.Range(0.0f, height);
				GameObject obj = Instantiate(astrToSpawn, 
							Camera.main.ScreenToWorldPoint( 
							new Vector3(horizontalPos, verticalPos,originInScreenCoords.z)), 
							Quaternion.identity ) as GameObject; 
				
			 	//Get the asteroid component & set a random rotation & speed
				float x = Random.Range(-20.0f, 20.0f);
				//float y = Random.Range(0.0f, 10.0f); //TODO - use this for 3D
				float z = Random.Range(-20.0f, 20.0f);
				Astroid a = obj.GetComponent<Astroid>();
				//float rotation = Random.Range(0.0f,1.0f); 
				//Quaternion rot = Quaternion.Euler(new Vector3(0,rotation,0)); 
				a.gameObject.rigidbody.AddRelativeForce(x, 0, z); 
			} 
		} 
	} 
	
	//Checks that all the objects in the scene are within the screen
	public void checkScreenBounds () 
	{
		object[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) ;
		foreach(object thisObject in allObjects) {
			//Check all active objects and compare them to the screen boundary
	   		if (((GameObject) thisObject).activeInHierarchy) {
	      		
				Transform trans = ((GameObject) thisObject).transform; 
				Vector3 pos = trans.position;  
				Vector3 screenPos = Camera.main.WorldToViewportPoint(pos); 
				
				if (screenPos.x <-0.1) {
					screenPos.x = Screen.width;
					pos.x =  Camera.main.ScreenToWorldPoint(screenPos).x; 
					((GameObject) thisObject).transform.position = pos; 
				} else if (screenPos.x > 1.1) {
					screenPos.x = 0; 
					pos.x = Camera.main.ScreenToWorldPoint(screenPos).x; 
					((GameObject) thisObject).transform.position = pos;
				} else if (screenPos.y <-0.1) {
					screenPos.y = Screen.height; 
					pos.y =  Camera.main.ScreenToWorldPoint(screenPos).y; 
					((GameObject) thisObject).transform.position = pos; 
				} else if (screenPos.y > 1.1) {
					screenPos.y = 0; 
					pos.y = Camera.main.ScreenToWorldPoint(screenPos).y; 
					((GameObject) thisObject).transform.position = pos;
				}
			}
		}
	}
}



			/* if you want to verify that this method works, uncomment 
			this code. What will happen when it runs is that one object will be spawned 
			at each corner of the screen, regardless of the size of the screen. If you 
			pause the Scene and inspect each object, you will see that each has a Ycoordinate value of 0. 
			*/ 
			/* 
			Vector3 botLeft = new Vector3(0,0,originInScreenCoords.z); 
			Vector3 botRight = new Vector3(width, 0, 
			originInScreenCoords.z); 
			Vector3 topLeft = new Vector3(0, height, 
			originInScreenCoords.z); 
			Vector3 topRight = new Vector3(width, height, 
			originInScreenCoords.z); 
			Instantiate(objToSpawn, 
			Camera.main.ScreenToWorldPoint(topLeft), Quaternion.identity ); 
			Instantiate(objToSpawn, 
			Camera.main.ScreenToWorldPoint(topRight), Quaternion.identity ); 
			Instantiate(objToSpawn, 
			Camera.main.ScreenToWorldPoint(botLeft), Quaternion.identity ); 
			Instantiate(objToSpawn,  Camera.main.ScreenToWorldPoint(botRight), Quaternion.identity ); 
			*/ 