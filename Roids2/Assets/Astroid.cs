using UnityEngine;
using System.Collections;

public class Astroid : MonoBehaviour {

	int pointValue; 
	// Use this for initialization
	void Start () {
		pointValue = 10; 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public GameObject deathExplosion; 
	public AudioClip deathKnell; 
	public void Die() 
	{ 
		AudioSource.PlayClipAtPoint(deathKnell, 
		gameObject.transform.position ); 
		Instantiate(deathExplosion, gameObject.transform.position, 
		Quaternion.AngleAxis(-90, Vector3.right) ); 
		GameObject obj = GameObject.Find("GlobalObject");
		Global g = obj.GetComponent<Global>(); 
		g.score += pointValue; 
		Destroy (gameObject); 
	} 
}
