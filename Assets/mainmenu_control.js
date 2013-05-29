var isquitbutton= false;
var isfootbutton= false;
var isboxbutton= false;
function OnMouseEnter()
{
renderer.material.color=Color.green;

}
function OnMouseExit()
{
renderer.material.color=Color.white;

}
function OnMouseUp()
{
if(isquitbutton)
{
Application.Quit();
}
if(isfootbutton)
{
Application.LoadLevel(1);
}
else
{
Application.LoadLevel(2);
}
}

function Update()
{
PlayerPrefs.SetInt("hit_next",0);
PlayerPrefs.SetInt("score_next",0);
}



