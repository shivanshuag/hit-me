//var SCORE=0;
//var HITS=0;
//var ball : GameObject;
//var s1 : Transform;
var shoot : Transform;
//var s2 : Transform;
//var s3 : Transform;
//var s4 : Transform;

var goal_text : GUIText;
var hits : GUIText;
var soccer_ball : Rigidbody;

function OnCollisionEnter(theCollision : Collision)
{
 if(theCollision.gameObject.name=="soccer_pre(Clone)")
 {
 //SCORE=SCORE+1;
 Debug.Log(goal_text.text);
 var score=PlayerPrefs.GetInt("score_next")+1;
 PlayerPrefs.SetInt("score_next",score);
 goal_text.text=""+ score;
 var ball2 = GameObject.Find("soccer_pre(Clone)");
 //if( ball == null)
 //ball = GameObject.Find("soccer");
 
 //goal_text.text=SCORE+"";
 Destroy(ball2);
 DontDestroyOnLoad (GUIText.goal_text);
 Application.LoadLevel(1);
 
 
 }}
 
 function Update()
 {
 //var score=PlayerPrefs.GetInt("score_next")+1;
 
 var score=PlayerPrefs.GetInt("score_next");
 PlayerPrefs.SetInt("score_next",score);
 goal_text.text=""+ score;
 
var hit=PlayerPrefs.GetInt("hit_next");
 PlayerPrefs.SetInt("hit_next",hit);
 hits.text=""+ hit;
 

 }
 
 /*yield WaitForSeconds (3);
 var soccerball : Rigidbody;
 soccerball = Instantiate(soccer_ball,shoot.position,Quaternion.identity);
 soccerball.velocity = transform.TransformDirection (Vector3.forward * 0);
 HITS=HITS+1;
 hits.text=HITS+"";
 //soccerball.RigidBody.AddForce(0);
 }
 }
 */
// function doa(){ yield WaitForSeconds(5);}
 /*
 function Update()
 {
 var ball = GameObject.Find("soccer_pre(Clone)");
// if(ball == null)
  //  ball = GameObject.Find("soccer");
 if(ball!= null)
 {
 if(ball.transform.position.z > s1.position.z || ball.transform.position.x < s1.position.x || ball.transform.position.z < s4.position.z ||
 ball.transform.position.x > s4.position.x)
 {
 
 Destroy (ball);
 //yield WaitForSeconds (5);
 var soccerball : Rigidbody;
 doa();
 soccerball = Instantiate(soccer_ball,shoot.position,Quaternion.identity);
 soccerball.velocity = transform.TransformDirection (Vector3.forward * 0);
 HITS=HITS+1;
 hits.text=HITS+"";
 //soccerball.RigidBody.AddForce(0);
 HITS++;
 
 }
 }
 }*/