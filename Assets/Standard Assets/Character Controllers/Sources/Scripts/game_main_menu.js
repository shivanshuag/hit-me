var menubutton= false;

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

if(menubutton)
{
Application.LoadLevel(0);
}
}