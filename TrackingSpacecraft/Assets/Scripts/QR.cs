
using UnityEngine;
using System.Collections;
using System.Threading;

using com.google.zxing.qrcode;

public class QR : MonoBehaviour {
	
	
	private WebCamTexture camTexture;
	private Thread qrThread;
	private Color32[] c;
	private sbyte[] d;
	private int W, H, WxH;
	private int x, y, z;
//	private int requestSent = 0;
	//private string spacecraft_id;
	private string str;
	private int cameraIndex;
	private bool isQuit;
	

	IEnumerator WaitAndPrint(float waitTime) {
        yield return new WaitForSeconds(waitTime);
    }
	
	
	void OnGUI () {
		GUI.Label(new Rect(100, 100, 500, 20), str);
		if (str!=null) {
			Screen.orientation = ScreenOrientation.Portrait;
			StartCoroutine(WaitAndPrint(0.25F)); 
			//Caching_Device Dev_cache = GetComponent<Caching_Device>();
			//Dev_cache.AddSpacecraft("1");
			//Caching_Device.AddSpacecraft("1");
			//spacecraft_id = str.Substring(str.length-1,1);
			
			// Sends message to Addspacecraft with the spacecraft id attached
			gameObject.SendMessage("AddSpacecraft", str[40].ToString());
			Application.LoadLevel("MainMenus");
		}
		
		//back button
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			if(GUI.Button(new Rect(20,50,80,50), "Back")) {
				Screen.orientation = ScreenOrientation.Portrait;
				StartCoroutine(WaitAndPrint(0.25F)); 
				Application.LoadLevel("MainMenus");
			}
		}
		else {
			if(GUI.Button(new Rect(20,30,50,30), "Back")) {
				Screen.orientation = ScreenOrientation.Portrait;
				StartCoroutine(WaitAndPrint(0.25F)); 
				Application.LoadLevel("MainMenus");
			}
		}
		
	}
	
	void OnEnable () {
		if(camTexture != null) {
			camTexture.Play();
			W = camTexture.width;
			H = camTexture.height;
			WxH = W * H;
		}
	}
	
	void OnDisable () {
		if(camTexture != null) {
			camTexture.Pause();
		}
	}
	
	void OnDestroy () {
		qrThread.Abort();
		camTexture.Stop();
	}
	
	// It's better to stop the thread by itself rather than abort it.
	void OnApplicationQuit () {
		isQuit = true;
	}
	
	void Start () {
		//check platform
		//if on Android device:
//		QR_AddCraft(1);
		if(Application.platform == RuntimePlatform.Android) {
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			// Checks how many and which cameras are available on the device
	        for (cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++) {
	            //if the selected camera is facing the back (what we want)
	            if (!WebCamTexture.devices[cameraIndex].isFrontFacing) {
	                camTexture = new WebCamTexture(cameraIndex, Screen.width, Screen.height);
	                //apply a localScale transformation to adjust to the difference for the mobile device camera
	                transform.localScale = new Vector3(1,1,1);
					guiTexture.texture = camTexture;
					transform.Translate(0.5f,0.5f,0f);
	            }
	        }
		}
		//if on iOS device:
		else if(Application.platform == RuntimePlatform.IPhonePlayer) {
			Screen.orientation = ScreenOrientation.LandscapeLeft;
			// Checks how many and which cameras are available on the device
	        for (cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++) {
	            //if the selected camera is facing the back (what we want)
	            if (!WebCamTexture.devices[cameraIndex].isFrontFacing) {
	                camTexture = new WebCamTexture(cameraIndex, Screen.width, Screen.height);
	                //apply a localScale transformation to adjust to the difference for the mobile device camera
	                transform.localScale = new Vector3(1,-1,1);
					guiTexture.texture = camTexture;
					transform.Translate(0.5F,0.5F,0F);
	            }
	        }
		}
		//if it is a windows or mac-based platform:
		else {
			camTexture = new WebCamTexture(Screen.width, Screen.height);
			transform.localScale = new Vector3(-1,1,1);
			transform.Translate(0.5f, 0.5f, 0f);
			guiTexture.texture = camTexture;
		}
		
		guiTexture.enabled = true;
		OnEnable();
		qrThread = new Thread(DecodeQR);
		qrThread.Start();
	}
		
	void Update () {
		//Decode once every 60 framses for smoothness
		if(Time.frameCount % 60 == 0){
			c = camTexture.GetPixels32();
		}
	}
	
	void DecodeQR () {
		while(true) {
			
			if(isQuit) break;
			
			try {
				d = new sbyte[WxH];
				z = 0;
				
				for(y = H - 1; y >= 0; y--) { // This is flipped vertically because the Color32 array from Unity is reversed vertically,
												// it means that the top most row of the image would be the bottom most in the array.
					for(x = 0; x < W; x++) {
						d[z++] = (sbyte)(((int)c[y * W + x].r) << 16 | ((int)c[y * W + x].g) << 8 | ((int)c[y * W + x].b));
					}
				}

                str = (new QRCodeReader().decode(d, W, H).Text);
			}
			catch {
				continue;
			}
		}
	}
}