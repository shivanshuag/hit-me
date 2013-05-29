//var HITS=0;
//var hits : GUIText;
//var soccer_ball : Rigidbody;
//var shoot : Transform;
public var score_text : GUIText;
var man : GameObject;
//var sound : AudioClip;
function OnCollisionEnter(theCollision : Collision)
{
// if(theCollision.gameObject.name=="fireball(Clone)")
// {
 //SCORE=SCORE+1;
// Debug.Log(SCORE);
 var clone = GameObject.Find("fireball(Clone)");
  Destroy(clone);
  var clone_duck= GameObject.Find("fireball_duck(Clone)");
  Destroy(clone_duck);
  var score = PlayerPrefs.GetInt("Player Score");
  PlayerPrefs.SetInt("Player Score", score-100);
  score_text.text = "Score "+(score-20).ToString();
  man.audio.Play();
 //  var hit=PlayerPrefs.GetInt("hit_next")+1;
 //PlayerPrefs.SetInt("hit_next",hit);
 //hits.text=""+ hit;
 
 
// DontDestroyOnLoad (GUIText.goal_text);
// Application.LoadLevel(1);
 
 
 
 
 
 
 //if( ball == null)
 //ball = GameObject.Find("soccer");
 
 //goal_text.text=SCORE+"";

 
 
 /*
 yield WaitForSeconds (3);
 var soccerball : Rigidbody;
 soccerball = Instantiate(soccer_ball,shoot.position,Quaternion.identity);
 soccerball.velocity = transform.TransformDirection (Vector3.forward * 0);
 HITS=HITS+1;
 hits.text=HITS+"";
 //soccerball.RigidBody.AddForce(0);
 */
// }
 }
 