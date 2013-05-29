#pragma strict
@script RequireComponent(CharacterController)
public var object : Transform;
var count=1;
 var initial = Vector3.zero;
function Start () {


}

function Update () {

if( count==01)
	{
	count=0;
	
	initial=object.position;
	}
	else
	{
	 var moveDirection = object.position  - initial;
	var distance :float = moveDirection.x * moveDirection.x + moveDirection.y * moveDirection.y + moveDirection.z * moveDirection.z;
	var dir :float = Mathf.Sqrt(distance);
	 var moveSpeed=dir/Time.deltaTime;
	var movement = moveDirection * moveSpeed*Time.deltaTime;
	var controller : CharacterController = GetComponent(CharacterController);
	controller.Move(movement);
	initial = object.position;
	}
}