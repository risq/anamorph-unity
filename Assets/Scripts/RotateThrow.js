//Rotate an object by clicking on it and moving the mouse. When you let go of the mouse, the object will continue to rotate based on how quickly you moved the mouse before letting go. This gives a throwing motion to the rotation.

var rotateSpeed : float = 10;

var rotationDamping : float = 0.8;

private var autoRotate : boolean;
private var oldRotation : Quaternion;
private var newRotation : Quaternion;
private var throwSpeed : float;

function Awake () {
	autoRotate = true;
}

function OnMouseDown () {
	autoRotate = false;
}

function OnMouseUp () {
	autoRotate = true;
}

function LateUpdate () {
	ObjRotate();
}

function ObjRotate () {
	if (!autoRotate) {
		var xMovement : float = Input.GetAxis("Mouse X");
		transform.Rotate(Vector3.up * -xMovement * rotateSpeed);
		//Get new rotation value
		newRotation = transform.rotation;
		//Calculate current throw speed
		if (xMovement < 0) {
			throwSpeed = Quaternion.Angle(newRotation, oldRotation);
		} else {
			throwSpeed = -Quaternion.Angle(newRotation, oldRotation);
		}
		oldRotation = newRotation;			
	} else if (autoRotate && (throwSpeed > 0.01 || throwSpeed < -0.01)) {
		throwSpeed *= rotationDamping;
		transform.Rotate(Vector3.up * throwSpeed);
	}
}