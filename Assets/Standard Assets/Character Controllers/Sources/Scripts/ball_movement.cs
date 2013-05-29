using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ball_movement : MonoBehaviour {
	public Rigidbody obstacle,obstacle_duck;
	public GUIText score_text;
	public float speed = 10f;
	Transform bullitPrefab;	
	public GameObject spawnpoint, spawnpoint1,spawnpoint2,spawnpoint3,spawnpoint4;
	public float timer = 1f;
	public AudioClip audio;
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("Player Score",0);
	}

	// Update is called once per frame
	void Update () {
		var fileName = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), "alpha.txt");
		timer  =timer - Time.deltaTime;
		if(timer<0){
		FileInfo theSourceFile = new FileInfo (fileName);
        StreamReader reader = theSourceFile.OpenText();
        string text = reader.ReadLine();
		reader.Close();
		if(text!=null){
			StreamWriter file = new System.IO.StreamWriter(fileName);
			file.WriteLine("");
			file.Close();
			if(text == "1<EOF>"){
				fireball();
			}
			if(text == "2<EOF>"){
				fireball2();
			}
			if(text == "3<EOF>"){
				fireball3();
			}
			if(text == "4<EOF>"){
				fireball4();
			}
			if(text == "5<EOF>"){
				fireball5();
			}
			timer = 1f;
			
		}}
		var clone = GameObject.Find("fireball(Clone)");
		if(clone!=null){
		if(clone.transform.position.z<-10){
				Destroy(clone);
				int score = PlayerPrefs.GetInt("Player Score");
				PlayerPrefs.SetInt("Player Score", score+500);
				score_text.text = "Score "+PlayerPrefs.GetInt("Player Score");
		}}
		var clone_duck = GameObject.Find("fireball_duck(Clone)");
		if(clone_duck!=null){
		if(clone_duck.transform.position.z<-10){
				Destroy(clone_duck);
				int score = PlayerPrefs.GetInt("Player Score");
				PlayerPrefs.SetInt("Player Score", score+500);
				score_text.text = "Score "+PlayerPrefs.GetInt("Player Score");
		}}
	}
	void fireball(){
//Vector3 pos = new Vector3(0,0,-100);
		//Transform coordintes = new Transform(0,0,-1);
		//Vector3 dir_cmd1= new Vector3(0,0,-1);
		Rigidbody obstacleClone = (Rigidbody) Instantiate(obstacle, spawnpoint.transform.position, Quaternion.identity);
		//obstacleClone.velocity =  dir_cmd1* speed;
		//obstacleClone.velocity =  coordintes* speed;
		//obstacleClone.velocity= transform.forward*speed;
		obstacleClone.rigidbody.AddForce(transform.forward*-2000);	
	AudioSource.PlayClipAtPoint(audio,new Vector3(0,0,0));
	}
	
	void fireball2(){
		Vector3 direction = new Vector3(-0.13f,0f,1f);
		Rigidbody obstacleClone = (Rigidbody) Instantiate(obstacle, spawnpoint1.transform.position, Quaternion.identity);
		obstacleClone.rigidbody.AddForce(direction*-2000);
	AudioSource.PlayClipAtPoint(audio,new Vector3(0,0,0));
	}
		
	void fireball3(){
		Vector3 direction = new Vector3(-0.082f,0.234f,1f);
		Rigidbody obstacleClone = (Rigidbody) Instantiate(obstacle_duck, spawnpoint2.transform.position, Quaternion.identity);
		obstacleClone.rigidbody.AddForce(direction*-2000);
		AudioSource.PlayClipAtPoint(audio,new Vector3(0,0,0));
	}

		void fireball4(){
		Vector3 direction = new Vector3(-0.134f,-0.085f,1f);
		Rigidbody obstacleClone = (Rigidbody) Instantiate(obstacle_duck, spawnpoint3.transform.position, Quaternion.identity);
		obstacleClone.rigidbody.AddForce(direction*-2000);
	AudioSource.PlayClipAtPoint(audio,new Vector3(0,0,0));
	}

	void fireball5(){
		Vector3 direction = new Vector3(0.05f,0f,1f);
		Rigidbody obstacleClone = (Rigidbody) Instantiate(obstacle, spawnpoint4.transform.position, Quaternion.identity);
		obstacleClone.rigidbody.AddForce(direction*-2000);
	AudioSource.PlayClipAtPoint(audio,new Vector3(0,0,0));
	}

}

	