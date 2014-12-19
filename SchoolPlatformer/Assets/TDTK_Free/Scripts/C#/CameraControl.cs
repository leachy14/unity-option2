using UnityEngine;
using System.Collections;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

public class CameraControl : MonoBehaviour {

	//public enum _Platform{Hybird, Mouse&Keyboard, Touch}
	//public _Platform platform;

	public float panSpeed=5;
	public float zoomSpeed=5;
	
	private float initialMousePosX;
	private float initialMousePosY;
	
	private float initialRotX;
	private float initialRotY;
	
	
	public float minPosX=-10;
	public float maxPosX=10;
	
	public float minPosZ=-10;
	public float maxPosZ=10;
	
	public float minRadius=8;
	public float maxRadius=30;
	
	public float minRotateAngle=10;
	public float maxRotateAngle=89;

	//calculated deltaTime based on timeScale so camera movement speed always remain constant
	private float deltaT;
	
	private Transform cam;
	private Transform thisT;

	void Awake(){
		thisT=transform;
		
		cam=Camera.main.transform;
	}
	
	// Use this for initialization
	void Start () {
		minRotateAngle=Mathf.Max(10, minRotateAngle);
		maxRotateAngle=Mathf.Min(89, maxRotateAngle);
		
		minRadius=Mathf.Max(1, minRadius);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Time.timeScale==1) deltaT=Time.deltaTime;
		else deltaT=Time.deltaTime/Time.timeScale;
		
		
		#if UNITY_EDITOR || (!UNITY_IPHONE && !UNITY_ANDROID)
		
		//mouse and keyboard
		if(Input.GetMouseButtonDown(1)){
			initialMousePosX=Input.mousePosition.x;
			initialMousePosY=Input.mousePosition.y;
			initialRotX=thisT.eulerAngles.y;
			initialRotY=thisT.eulerAngles.x;
		}

		if(Input.GetMouseButton(1)){
			float deltaX=Input.mousePosition.x-initialMousePosX;
			float deltaRotX=(.1f*(initialRotX/Screen.width));
			float rotX=deltaX+deltaRotX;
			
			float deltaY=initialMousePosY-Input.mousePosition.y;
			float deltaRotY=-(.1f*(initialRotY/Screen.height));
			float rotY=deltaY+deltaRotY;
			float y=rotY+initialRotY;
			
			//limit the rotation
			if(y>maxRotateAngle){
				initialRotY-=(rotY+initialRotY)-maxRotateAngle;
				y=maxRotateAngle;
			}
			else if(y<minRotateAngle){
				initialRotY+=minRotateAngle-(rotY+initialRotY);
				y=minRotateAngle;
			}
			
			thisT.rotation=Quaternion.Euler(y, rotX+initialRotX, 0);
		}
		
		
		Quaternion direction=Quaternion.Euler(0, thisT.eulerAngles.y, 0);
		
		if(Input.GetButton("Horizontal")) {
			Vector3 dir=transform.InverseTransformDirection(direction*Vector3.right);
			thisT.Translate (dir * panSpeed * deltaT * Input.GetAxisRaw("Horizontal"));
		}

		if(Input.GetButton("Vertical")) {
			Vector3 dir=transform.InverseTransformDirection(direction*Vector3.forward);
			thisT.Translate (dir * panSpeed * deltaT * Input.GetAxisRaw("Vertical"));
		}
		
		//cam.Translate(Vector3.forward*zoomSpeed*Input.GetAxis("Mouse ScrollWheel"));
		
		if(Input.GetAxis("Mouse ScrollWheel")<0){
			if(Vector3.Distance(cam.position, thisT.position)<maxRadius){
				cam.Translate(Vector3.forward*zoomSpeed*Input.GetAxis("Mouse ScrollWheel"));
			}
		}
		else if(Input.GetAxis("Mouse ScrollWheel")>0){
			if(Vector3.Distance(cam.position, thisT.position)>minRadius){
				cam.Translate(Vector3.forward*zoomSpeed*Input.GetAxis("Mouse ScrollWheel"));
			}
		}
		
		//thisT.Translate(cam.forward*zoomSpeed*Input.GetAxis("Mouse ScrollWheel"), Space.World);
		
		#endif
		
		float x=Mathf.Clamp(thisT.position.x, minPosX, maxPosX);
		float z=Mathf.Clamp(thisT.position.z, minPosZ, maxPosZ);
		//float y=Mathf.Clamp(thisT.position.y, verticalLimitBottom, verticalLimitTop);
		
		thisT.position=new Vector3(x, thisT.position.y, z);
		
	}
	
	
	public bool showGizmo=true;
	void OnDrawGizmos(){
		if(showGizmo){
			Vector3 p1=new Vector3(minPosX, transform.position.y, maxPosZ);
			Vector3 p2=new Vector3(maxPosX, transform.position.y, maxPosZ);
			Vector3 p3=new Vector3(maxPosX, transform.position.y, minPosZ);
			Vector3 p4=new Vector3(minPosX, transform.position.y, minPosZ);
			
			Gizmos.color=Color.green;
			Gizmos.DrawLine(p1, p2);
			Gizmos.DrawLine(p2, p3);
			Gizmos.DrawLine(p3, p4);
			Gizmos.DrawLine(p4, p1);
		}
	}
	
}
