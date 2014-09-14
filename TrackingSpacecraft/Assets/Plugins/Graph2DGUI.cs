
using UnityEngine;
using System.Collections;
// added this to support the abs function
using System;
// added this to support max min functions
using System.Linq;

public class Graph2DGUI : MonoBehaviour {
	
	
	
	public int width = System.Convert.ToInt32(Screen.width * 0.9);
	public int height = System.Convert.ToInt32(Screen.height * 0.6);
	
	public float scaleSpeed = 2f;
	
	public float updateRate = 0.05f;
	
	private GraphFunction[] gf;
	//private Color[] fColors;
	
	
	private Graph2D graph;
	private Texture2D graphTex;
	
	private Vector2 offset = Vector2.zero;
	private Vector2 scale = new Vector2(20f,20f);
	Vector2 gridStep = Vector2.one;
	
	private string lastTooltip = "";
	
	private bool focus = false;
	private float lastUpdateTime;

	private float[] datapoints;
	//private string dataname;
	
	public void plot(float[] dPoints, string dName) {
		datapoints = dPoints;
		//dataname = dName;
		offset.x = (float)(width * 0.07);
		offset.y = offset.x;
		if (datapoints != null && datapoints.Length != 0) {	// if the minimum data point is above 0 then it sets the x-axis at the bottom of the screen
			if (datapoints.Min () == 0 && datapoints.Max () == 0) {	//if all values are 0 put the x-axis in the middle of the screen
				offset.y = height/2;
			}
			else if (datapoints.Min() >= 0) {
				offset.y = offset.x;
			}
			else if (datapoints.Max() > 0 && datapoints.Min() < 0) {
				offset.y = height - (datapoints.Max () / (datapoints.Max () - datapoints.Min())*height);
			}
			else offset.y = height - offset.x;	// otherwise it sets the x-axis at the top of the graph
		}
			
		// if datapoints is not empty it sets the gridstep according to the range of values relative to y = 0
		if (datapoints != null && datapoints.Length != 0) {	
			float gridx = (float)Math.Ceiling((datapoints.Length + 3) / 18f);	// sets grid density on x axis according to the number of elements on the graph
			if (datapoints.Min() >= 0)	gridStep = new Vector2(gridx,(datapoints.Max() + 10f)/16f);
			else if (datapoints.Max() <= 0)	gridStep = new Vector2(gridx,Math.Abs((datapoints.Min() - 10f)/16f));
			else  gridStep = new Vector2(gridx,(datapoints.Max()-datapoints.Min() + 10f)/16f);
		}
	}
	
 
	void Awake(){
		//fColors = new Color[]{Color.green,Color.red};
		offset.x = (float)(height * 0.1);
		
		graph = new Graph2D(width,height);
		graphTex = graph.Draw(datapoints,offset,scale,Color.red,gridStep,1);
		//gf = new GraphFunction[]{Mathf.Sin,Mathf.Cos};	
		
		//fColors = new Color[]{Color.green,Color.red};
		
		//offset.x = width/2f;
		
		graph = new Graph2D(width,height);	
		//graphTex = graph.Draw(foo,offset,scale,Color.green,gridStep,1);
		graphTex = graph.Draw(datapoints,offset,scale,Color.red,gridStep,1);
		
	}
	

	public void OnGUI(){
		//Rect graph2DRect = new Rect(boxOffset.x,boxOffset.y,width,height);
		
		//GUIStyle style = new GUIStyle();
		
		SetOffset();
		SetScale();
		
		int boxX = System.Convert.ToInt32(Screen.width * 0.1 / 2); 
		int boxY = System.Convert.ToInt32(Screen.height * 0.2 / 2);
		
		GUILayout.BeginArea(new Rect(boxX, boxY,width,height));
				GUILayout.Box(new GUIContent(graphTex,"Graph1" ));
		GUILayout.EndArea();

		
		
		if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip) {
            if (lastTooltip != "")
                SendMessage(lastTooltip + "OnMouseOut", SendMessageOptions.DontRequireReceiver);
            
            if (GUI.tooltip != "")
                SendMessage(GUI.tooltip + "OnMouseOver", SendMessageOptions.DontRequireReceiver);
            
        	lastTooltip = GUI.tooltip;
		}
		
		GraphUpdate();

	}
	
	public void GraphUpdate(){
		if (Time.time>(lastUpdateTime + updateRate)){			
			/*if(toggleHour){
				gf = new GraphFunction[]{Mathf.Sin, Mathf.Cos};
				fColors = new Color[]{Color.green,Color.red};
			}else if(toggle12Hours){
				gf = new GraphFunction[]{Mathf.Sin};	
				fColors = new Color[]{Color.green};
			}else if(toggleDay){
				gf = new GraphFunction[]{Mathf.Cos};
				fColors = new Color[]{Color.red};	
			}else if(toggleWeek){
				gf = new GraphFunction[0]{};
			}else if(toggleMonth){
			}*/
			graphTex = graph.Draw(datapoints,offset,scale,Color.red,gridStep,1);
			
			lastUpdateTime = Time.time;
		}
	}
	
	
	
	//moves graph contents 
	private void SetOffset(){
		if(focus && Input.GetKey(KeyCode.Mouse0)){			
				offset.x += Input.GetAxis("Mouse X");
				offset.y += Input.GetAxis("Mouse Y");				        			

				GraphUpdate();		
		}	
	}
	
	private void SetScale(){
		/*float[] mx = new float[2];
		mx = minmax(datapoints);
		
		while (scale.y > mx[0])	scale.y -= 1;
		Debug.Log (datapoints.Length);
		while (scale.x > (50f / datapoints.Length)) scale.x -= 1f;
			
		gridStep = new Vector2(50f / scale.x,50f / scale.y);
		
		GraphUpdate();*/
		/*float wheel =  Input.GetAxis ("Mouse ScrollWheel")*scaleSpeed;
		
		if(focus && (wheel != 0f)){
						
			//Debug.Log(wheel);
		
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				scale.y += wheel;	
			else
			if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
				scale.x += wheel;
			else{
			
				scale.x +=wheel;
				scale.y +=wheel;
			}
			
			
			if(scale.x <= 0f) scale.x = 0.0001f;
			if(scale.y <= 0f) scale.y = 0.0001f;
			
			gridStep = new Vector2(50f / scale.x,50f / scale.y);
			
			
			//GraphUpdate();
		}*/
		
	}
	
	
	/*void Graph1OnMouseOver() {
       Debug.Log("got focus");
		focus = true;
    }
    void Graph1OnMouseOut() {
        Debug.Log("lost focus");
		focus = false;
    }*/
	
			
	/*private float foo(float x){
		return Mathf.Sin(x)*2f;
	}*/
	
}