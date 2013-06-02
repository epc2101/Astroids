using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {

	public GameObject ufoToSpawn; 
	public GameObject astrToSpawn; 

	public float timer; 
	public float ufoTimer;
	public float spawnPeriod; 
	public float ufoSpawnPeriod; 
	public int numberAstrSpawnedEachPeriod; 
	public int numberUfoSpawnedEachPeriod;
	public int currNumAstr;
	public int currNumUfo; 
	int currNumUfoStatic; 
	public Vector3 originInScreenCoords; 
	
	public int score; 
	public int lives; 
	public int level; 
	public bool newLevel; 
	
	
	bool addedRoids; 
	 // Use this for initialization 
	 void Start () { 
		score = 0; 
		timer = 0; 
		level = 1; 
		lives = 3; 
		newLevel = true; 
		addedRoids = false; 
		
		spawnPeriod = 1.0f; 
		ufoSpawnPeriod = 5.0f; 
		numberAstrSpawnedEachPeriod = 3;
		numberUfoSpawnedEachPeriod = 1;	
		currNumAstr = 0; 
		currNumUfo = 0; 
		currNumUfoStatic = 0; 
		
		 /* 
		 So here's a design point to consider: 
		- is the gameplay constrained by the screen size in any particular way? 
		That might sound like a weird question, but it's actually a significant one for asteroids if you want the 
		game to play like Asteroids on arbitrary screen dimensions. It's mostly here for 
		pedagogical reasons, though. The value that actually matters here is the depth value. Since the gameplay 
		takes place on a XZplane, and we're looking down the Y-axis, 
		we're mainly interested in what the Y value of 0 maps to in the camera's depth. 
		*/ 
		 originInScreenCoords = Camera.main.WorldToScreenPoint(new Vector3(0,0,0)); 
	 } 
	
	// Update is called once per frame 
	void Update () { 
		//Check # of lives & end game
		float deathPauseTime = 2.0f;
		float timeAfterDeath = 0.0f; 
		if (lives < 1 && timeAfterDeath < deathPauseTime) {
			//Check if the user has a new high score
			for (int i = 1; i < 5; i++) {
				string scoreKey = "Score" + i.ToString(); 
				string nameKey = "Name" + i.ToString(); 
				int storedScore = PlayerPrefs.GetInt(scoreKey);
				if (score > storedScore) {
					print ("NEW HIGH SCORE!");
					PlayerPrefs.SetInt(scoreKey, score); 
					PlayerPrefs.SetString(nameKey, "ENTER YOUR NAME HERE"); 
					break; 
				}
			}
			Application.LoadLevel("PlayAgainScene"); 			
		} else if (lives < 1) {
			deathPauseTime += Time.deltaTime;	
		}
		
		//Add asteroids & UFOs if timer is ready
		addNewAsteroids(); 
		addNewUfos(); 
	
		//Reset level based && increase difficulty
		if (addedRoids && currNumAstr > 0) newLevel = false; 
		if (addedRoids && currNumAstr == 0 && currNumUfo == 0) {
			newLevel = true;
			level++; 
			numberAstrSpawnedEachPeriod += 2; 
			numberUfoSpawnedEachPeriod += 1; 
			ufoSpawnPeriod -= 0.5f;
			currNumUfoStatic = 0; 
			if (ufoSpawnPeriod < 0.1) ufoSpawnPeriod = 0.1f;  
			score += 50; 
		}
	} 
	
	void addNewAsteroids()
	{
		//Add new asteroids at the start of each scene
		timer += Time.deltaTime;
		if( timer > spawnPeriod && newLevel ) 
		{ 
			timer = 0; 
			float width = Camera.main.GetScreenWidth(); 
			float height = Camera.main.GetScreenHeight(); 
			addedRoids = true;
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
				a.lifeSpan = 2; 
				currNumAstr++; 
			
			} 
		}
		
	}
	
	public void addBabyRoids(int lifeSpan, GameObject parentRoid)
	{
		Vector3 lastPos = new Vector3(0, 0, 0); 
		for (int i = 0; i < lifeSpan; i++) {
			Vector3 pos = parentRoid.transform.position;
			int randX = Random.Range(4, 10);  
			int randZ = Random.Range(4, 10);  
			pos = new Vector3( pos.x + (float)randX /10.0f, 0 , pos.z + (float)randZ/10.0f); 
			//Make sure 2 new asteroids aren't on top of eachother
			if (i == 1) {
				print ("TESTING SPLIT: " + pos.x + " " + lastPos.x + " " + lastPos.z); 
				float rSqr = 0.95f * 0.95f; //Middle asteroids are 1.9 scale
				float dist = Vector3.Distance(pos, lastPos); 
				float distSqr = dist * dist; 
				if (distSqr < rSqr)
				{
					print ("TOO Close!!"); 
					if (pos.x < lastPos.x) 
						pos.x -= 1; 
					else
						pos.x += 1; 
					if (pos.z < lastPos.z) 
						pos.z -= 1;
					else
						pos.z += 1; 
				}
			}
			GameObject newRoid = Instantiate(astrToSpawn, pos, Quaternion.identity) as GameObject; 
			lastPos = pos; 
			Astroid a = newRoid.GetComponent<Astroid>();
			randX = Random.Range(0, 10);  
			randZ = Random.Range(0, 10);
			a.gameObject.rigidbody.AddRelativeForce(randX, 0, randZ); 
			a.lifeSpan = lifeSpan - 1; 
			currNumAstr++; 
		}	
	}
	
	void addNewUfos() {
		ufoTimer += Time.deltaTime; 
		if (ufoTimer > ufoSpawnPeriod) {
			ufoTimer = 0; 
			float width = Camera.main.GetScreenWidth(); 
			float height = Camera.main.GetScreenHeight(); 
			if (currNumUfoStatic <  numberUfoSpawnedEachPeriod)
			{ 
				float horizontalPos = Random.Range(0.0f, width); 
				float verticalPos = Random.Range(0.0f, height);
				GameObject obj = Instantiate(ufoToSpawn, 
							Camera.main.ScreenToWorldPoint( 
							new Vector3(horizontalPos, verticalPos,originInScreenCoords.z)), 
							Quaternion.identity ) as GameObject; 
				Vector3 vel = new Vector3(Random.Range(-20.0f, 20.0f), 0, Random.Range(-20.0f, 20.0f)); 
				UFO ufo = obj.GetComponent<UFO>(); 
				ufo.gameObject.rigidbody.AddRelativeForce(vel); 
				currNumUfo++; 
				currNumUfoStatic++;
			}			
		}	
	}
	
	//Checks that all the objects in the scene are within the screen
	/*public void checkScreenBounds () 
	{
		object[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) ;
		foreach(object thisObject in allObjects) {
			//Check all active objects and compare them to the screen boundary
	   		if (((GameObject) thisObject).activeInHierarchy) {
				
				Transform trans = ((GameObject) thisObject).transform; 
				Vector3 pos = trans.position;  
				Vector3 screenPos = Camera.main.WorldToViewportPoint(pos); 
				float x = screenPos.x; 
				float y = screenPos.y; 
				//print (thisObject.ToString() + screenPos.x + " " + screenPos.y); 
				if (x < -0.1f) {
					print ("Under the x");
					screenPos.x = Screen.width;
					pos.x =  Camera.main.ScreenToWorldPoint(screenPos).x; 
					((GameObject) thisObject).transform.position = pos; 
			    }
				if (screenPos.x > 1.1f) {
					print ("Over the x");
					screenPos.x = 0; 
					pos.x = Camera.main.ScreenToWorldPoint(screenPos).x; 
					((GameObject) thisObject).transform.position = pos;
				} 
				if (screenPos.y <-0.1f) {
					print ("Under the y");
					screenPos.y = Screen.height; 
					pos.y =  Camera.main.ScreenToWorldPoint(screenPos).y; 
					((GameObject) thisObject).transform.position = pos; 
				} 
				if (y > 1.1f) {
					print ("Over the y");
					screenPos.y = 0; 
					pos.y = Camera.main.ScreenToWorldPoint(screenPos).y; 
					((GameObject) thisObject).transform.position = pos;
				} 		
			}
		}
	}*/
}
