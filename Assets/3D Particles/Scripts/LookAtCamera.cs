using UnityEngine;

public class LookAtCamera:MonoBehaviour{
    public Camera lookAtCamera;
    public bool lookOnlyOnAwake;

	public void Start() {
    	if(lookAtCamera == null){
            if (GameObject.Find("PlayerCamera").GetComponent<Camera>())
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