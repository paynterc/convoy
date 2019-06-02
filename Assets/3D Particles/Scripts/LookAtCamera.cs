using UnityEngine;

public class LookAtCamera:MonoBehaviour{
    public Camera lookAtCamera;
    public bool lookOnlyOnAwake;

	public void Start() {

        GameObject playercam = GameObject.Find("PlayerCamera");

        if (lookAtCamera == null){
            if (playercam)
            {
                lookAtCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
            }
            else
            {
                lookAtCamera = Camera.main;
            }
    		
    	}
    	if(lookOnlyOnAwake){
			LookCamera();
    	}
    }
    
    public void Update() {
    	if(!lookOnlyOnAwake){
			LookCamera();
    	}
    }
    
    public void LookCamera() {
        if (lookAtCamera)
        {
            transform.LookAt(lookAtCamera.transform);
        }
    
    }
}