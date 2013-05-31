using UnityEngine;
using System.Collections;

public class ScoreUI : MonoBehaviour {
	Global globalObj; 
	GUIText scoreText; 
	//int lastScore; 
	
	// Use this for initialization 
	void Start () { 
		GameObject g = GameObject.Find ("GlobalObject"); 
		
		globalObj = g.GetComponent< Global >(); 
		//lastScore = 0; 
		scoreText = gameObject.GetComponent<GUIText>(); 
	} 
	
	// Update is called once per frame 
	void Update () { 
		string scoreLabel = "\nSCORE: " + globalObj.score.ToString(); 
		string livesLabel = "\nLIVES: " + globalObj.lives.ToString(); 
		string levelLabel = "LEVEL: " + globalObj.level.ToString();
		string label = levelLabel + scoreLabel + livesLabel; 
		scoreText.text = label ; 		 
	} 
}
