/// This behavior script changest the GUI text on the object it is attached to to display 
// a frames per second counter.

var xPos : int;
var yPos : int;
var wide : int;
var high : int;

private var fps : float;

function Update () {
	fps = 1.0 / Time.smoothDeltaTime;
}

function OnGUI () {
	GUI.Label(Rect(Screen.width - wide, yPos, wide, high), fps.ToString());
}