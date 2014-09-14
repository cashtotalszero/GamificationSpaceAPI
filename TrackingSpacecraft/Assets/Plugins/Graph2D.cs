using UnityEngine;
using System;
using System.Collections;

public delegate float GraphFunction(float x);

public class Graph2D{
	
	protected Texture2D texture;
	
	public Color backgroundColor = Color.gray;  
	public Color gridColor = Color.black;        
	public Color colorX = Color.green;			
	public Color colorY = Color.green;			
	public Color fontColor = Color.black;		
		
	
	
	protected int texWidth;		
	protected int texHeight;
	
	public Vector2 fontScale = Vector3.one;
	
	public int TexWidth{
			get{return texWidth;}
	}
	
	public int TexHeight{
			get{return texHeight;}
	}
	
	
	
	protected FontNumbers fontNumbers = new FontNumbers();
	
	protected Graph2D(){
		
	}
	
	public Graph2D(int width, int height){	
		
		texWidth = width;
		texHeight = height;
		
		texture = new Texture2D(texWidth, texHeight, TextureFormat.RGB24, false);
	}
	
	private bool IsPointOnTexture(int x, int y, int tWidth, int tHeight){
		bool result = false;
		if(x >= 0 && x< tWidth && y>=0 && y<tHeight){
			result = true;	
		}		
		return result;
	}
		
	private void DrawLineTex(int x1, int y1, int x2, int y2, Color color, int tWidth, int tHeight){
		
		bool isx2y2 = IsPointOnTexture(x2,y2,tWidth,tHeight); 
		bool isx1y1 = IsPointOnTexture(x1,y1,tWidth,tHeight);
		
		if(!isx1y1 && !isx2y2)
			return;
		
		int Dx = Mathf.Abs(x2 - x1);
		int Dy = Mathf.Abs(y2 - y1);
		int Sx = x1 < x2 ? 1 : -1;
		int Sy = y1 < y2 ? 1 : -1;
		
		int error = Dx - Dy;
		
		if(isx2y2)
			texture.SetPixel(x2,y2,color);		
		
		while(x1 != x2 || y1 != y2){
			if(IsPointOnTexture(x1,y1,tWidth,tHeight))
				texture.SetPixel(x1,y1,color);
			
			int error2 = error * 2;
			
			if(error2 > -Dy){
				error -= Dy;
				x1 += Sx;				
			}
			if(error2 < Dx){
				error+=Dx;	
				y1 += Sy;
			}			
		}		
	}
	
	private void DrawHorisontalLineTex(int y, Color color, int tWidth){  
		int x = 0;
		while (x < tWidth){
			texture.SetPixel(x, y, color);
            ++x;
		}
	}
	
	
	private void DrawVerticalLineTex(int x, Color color, int tHeight){ 
		int y = 0;
		while (y < tHeight){
			texture.SetPixel(x, y, color);
            ++y;
		}
	}
	
	private bool IsRectOnTexture(int rx, int ry,int rw,int rh,int texW, int texH){ 
		
		int rx1=rx;
		
		if(IsPointOnTexture(rx,ry,texW,texH)) //top left point
			return true;
		
		rx+=rw;
		
		if(IsPointOnTexture(rx,ry,texW,texH)) //top right point
			return true;
		
		ry-=rh;
		
		if(IsPointOnTexture(rx,ry,texW,texH)) //bottom right point
			return true;
		
				
		if(IsPointOnTexture(rx1,ry,texW,texH)) //bottom left point
			return true;
		
		
		
		return false;
	}
	
	private void DrawChar(char symbol, Vector2 offset, Vector2 scale,  Color color ,int tWidth, int tHeight,  out int cWidth, out int cHeight){  //Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜ Ð Ñ•Ð Ò‘Ð Ñ‘Ð Ð… Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â»; Ð Ñ—Ð Â°Ð¡Ð‚Ð Â°Ð Ñ˜Ð ÂµÐ¡â€šÐ¡Ð‚Ð¡â€¹: symbol - Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜Ð¡â€¹Ð â„– Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â», offset - Ð¡ÐƒÐ Ñ˜Ð ÂµÐ¡â€°Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð Â»Ð ÂµÐ Ð†Ð Ñ•Ð Ñ–Ð Ñ• Ð Ð†Ð ÂµÐ¡Ð‚Ð¡â€¦Ð Ð…Ð ÂµÐ Ñ–Ð Ñ• Ð¡Ñ“Ð Ñ–Ð Â»Ð Â° Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â»Ð Â° Ð Ñ•Ð¡â€šÐ Ð…Ð Ñ•Ð¡ÐƒÐ Ñ‘Ð¡â€šÐ ÂµÐ Â»Ð¡ÐŠÐ Ð…Ð Ñ• Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð Â° Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹,
		cWidth = 0;																																//scale - Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡â‚¬Ð¡â€šÐ Â°Ð Â± Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â»Ð Â°, color - Ð¡â€ Ð Ð†Ð ÂµÐ¡â€š, Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹, cWidth - Ð Ð†Ð Ñ•Ð Â·Ð Ð†Ð¡Ð‚Ð Â°Ð¡â€°Ð Â°Ð ÂµÐ Ñ˜ Ð¡Ð‚Ð ÂµÐ Â°Ð Â»Ð¡ÐŠÐ Ð…Ð¡Ñ“Ð¡Ð‹ Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð¡Ñ“ Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â»Ð Â°, cHeight - Ð Ð†Ð Ñ•Ð Â·Ð Ð†Ð¡Ð‚Ð Â°Ð¡â€°Ð Â°Ð ÂµÐ Ñ˜ Ð¡Ð‚Ð ÂµÐ Â°Ð Â»Ð¡ÐŠÐ Ð…Ð¡Ñ“Ð¡Ð‹ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ¡Ñ“ Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â»Ð Â°
		cHeight = 0;
		
		foreach(Symbol sl in fontNumbers.Symbols){
			if(sl.Sign == symbol){
				cWidth = (int)(sl.sWidth*scale.x);
				cHeight = (int)(sl.sHeight*scale.y);
				
				if(!IsRectOnTexture((int)offset.x,(int)offset.y,cWidth,cHeight,tWidth,tHeight))//Ð ÂµÐ¡ÐƒÐ Â»Ð Ñ‘ Rect Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â»Ð Â° Ð Ð…Ð Â°Ð¡â€¦Ð Ñ•Ð Ò‘Ð Ñ‘Ð¡â€šÐ¡ÐƒÐ¡Ð Ð Ð†Ð Ð…Ð Âµ Ð Ñ—Ð¡Ð‚Ð Ñ•Ð¡ÐƒÐ¡â€šÐ¡Ð‚Ð Â°Ð Ð…Ð¡ÐƒÐ¡â€šÐ Ð†Ð Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹, Ð Ð†Ð¡â€¹Ð¡â€¦Ð Ñ•Ð Ò‘Ð Ñ‘Ð Ñ˜ Ð Ñ‘Ð Â· Ð¡â€žÐ¡Ñ“Ð Ð…Ð Ñ”Ð¡â€ Ð Ñ‘Ð Ñ‘
					return;				
				
				int l = sl.Vectors.Length - 1;
				for(int i = 0; i< l; i++){
					Vector2 v1 = Vector2.Scale(sl.Vectors[i],scale) + offset;
					Vector2 v2 = Vector2.Scale(sl.Vectors[i+1],scale) + offset;
					DrawLineTex((int)v1.x,(int)v1.y,(int)v2.x,(int)v2.y,color, tWidth, tHeight);					
				}				
			}
		}		
	}
	
	
	
	private void DrawString(string str, Vector2 offset, Vector2 scale, int chSpacing, int tWidth, int tHeight, Color color, bool alignLeft){ //Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜ Ð¡ÐƒÐ¡â€šÐ¡Ð‚Ð Ñ•Ð Ñ”Ð¡Ñ“ Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â»Ð Ñ•Ð Ð†; Ð Ñ—Ð Â°Ð¡Ð‚Ð Â°Ð Ñ˜Ð ÂµÐ¡â€šÐ¡Ð‚Ð¡â€¹: str - Ð¡ÐƒÐ¡â€šÐ¡Ð‚Ð Ñ•Ð Ñ”Ð Â°, offset - Ð¡ÐƒÐ Ñ˜Ð ÂµÐ¡â€°Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð Â»Ð ÂµÐ Ð†Ð Ñ•Ð Ñ–Ð Ñ• Ð Ð†Ð ÂµÐ¡Ð‚Ð¡â€¦Ð Ð…Ð ÂµÐ Ñ–Ð Ñ• Ð¡Ñ“Ð Ñ–Ð Â»Ð Â°, scale - Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡â‚¬Ð¡â€šÐ Â°Ð Â±
		int sl = str.Length;																												 //chSpacing - Ð¡Ð‚Ð Â°Ð¡ÐƒÐ¡ÐƒÐ¡â€šÐ Ñ•Ð¡ÐÐ Ð…Ð Ñ‘Ð Âµ Ð Ñ˜Ð ÂµÐ Â¶Ð Ò‘Ð¡Ñ“ Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ð†Ð Ñ•Ð Â»Ð Â°Ð Ñ˜Ð Ñ‘ Ð Ð† Ð Ñ—Ð Ñ‘Ð Ñ”Ð¡ÐƒÐ ÂµÐ Â»Ð¡ÐÐ¡â€¦, Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹, Ð¡â€ Ð Ð†Ð ÂµÐ¡â€š, alignLeft - Ð Ð†Ð¡â€¹Ð¡Ð‚Ð Â°Ð Ð†Ð Ð…Ð Ñ‘Ð Ð†Ð Â°Ð¡â€šÐ¡ÐŠ Ð Â»Ð Ñ‘ Ð¡ÐƒÐ¡â€šÐ¡Ð‚Ð Ñ•Ð Ñ”Ð¡Ñ“ Ð Ñ—Ð Ñ• Ð Â»Ð ÂµÐ Ð†Ð Ñ•Ð Ñ˜Ð¡Ñ“ Ð Ð†Ð ÂµÐ¡Ð‚Ð¡â€¦Ð Ð…Ð ÂµÐ Ñ˜Ð¡Ñ“ Ð¡Ñ“Ð Ñ–Ð Â»Ð¡Ñ“
		
		int chWidth;
		int chHeight;	
		
		int strHeight = (int)(fontNumbers.StrHeight*scale.y);
		int strWidth = (int)(FontNumbers.CharsPxWidth(str)*scale.x+((chSpacing*(sl-1))*Mathf.Sign(scale.x)));
		
		if(alignLeft){
			offset.x-=strWidth;			
		}
		
		//Vector2 chStep = offset;
		if(!IsRectOnTexture((int)offset.x,(int)offset.y,strWidth,strHeight,tWidth,tHeight))   //Ð â€¢Ð¡ÐƒÐ Â»Ð Ñ‘ Rect Ð¡ÐƒÐ¡â€šÐ¡Ð‚Ð Ñ•Ð Ñ”Ð Ñ‘ Ð Ð…Ð Â°Ð¡â€¦Ð Ñ•Ð Ò‘Ð Ñ‘Ð¡â€šÐ¡ÐƒÐ¡Ð Ð Ð†Ð Ð…Ð Âµ Ð Ñ—Ð¡Ð‚Ð Ñ•Ð¡ÐƒÐ¡â€šÐ¡Ð‚Ð Â°Ð Ð…Ð¡ÐƒÐ¡â€šÐ Ð†Ð Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹, Ð Ð†Ð¡â€¹Ð¡â€¦Ð Ñ•Ð Ò‘Ð Ñ‘Ð Ñ˜ Ð Ñ‘Ð Â· Ð¡â€žÐ¡Ñ“Ð Ð…Ð Ñ”Ð¡â€ Ð Ñ‘Ð Ñ‘
			return;
		
		
		
		for(int i = 0; i< sl; i++){			
		 	DrawChar(str[i],offset, scale,color,tWidth,tHeight,out chWidth, out chHeight);
			offset.x+=chWidth+(chSpacing*Mathf.Sign(chWidth));					
		}
			
	}
	
	private delegate void DrawHVLine(int coord, Color color, int tWH);  //Ð Ò‘Ð ÂµÐ Â»Ð ÂµÐ Ñ–Ð Â°Ð¡â€š Ð Ò‘Ð Â»Ð¡Ð Ð Ñ—Ð Ñ•Ð Ò‘Ð¡â€¦Ð Ð†Ð Â°Ð¡â€šÐ Â° Ð¡â€žÐ¡Ñ“Ð Ð…Ð Ñ”Ð¡â€ Ð Ñ‘Ð Ñ‘ Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ Ñ•Ð Ð†Ð Â°Ð Ð…Ð Ñ‘Ð¡Ð Ð Ð†Ð ÂµÐ¡Ð‚Ð¡â€šÐ Ñ‘Ð Ñ”Ð Â°Ð Â»Ð¡ÐŠÐ Ð…Ð Ñ•Ð â„– Ð Ñ‘Ð Â»Ð Ñ‘ Ð Ñ–Ð Ñ•Ð¡Ð‚Ð Ñ‘Ð Â·Ð Ñ•Ð Ð…Ð¡â€šÐ Â°Ð Â»Ð¡ÐŠÐ Ð…Ð Ñ•Ð â„– Ð Â»Ð Ñ‘Ð Ð…Ð Ñ‘Ð Ñ‘
	
	private void DrawGridLines(ref float grStep,float offset, float scale, Color color, int max, int tWH , DrawHVLine HVLine, out float pxStep, out float offs){ // Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜ Ð Â»Ð Ñ‘Ð Ð…Ð Ñ‘Ð Ñ‘ Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘, Ð Ð†Ð ÂµÐ¡Ð‚Ð¡â€šÐ Ñ‘Ð Ñ”Ð Â°Ð Â»Ð¡ÐŠÐ Ð…Ð¡â€¹Ð Âµ Ð Ñ‘Ð Â»Ð Ñ‘ Ð Ñ–Ð Ñ•Ð¡Ð‚Ð Ñ‘Ð Â·Ð Ñ•Ð Ð…Ð¡â€šÐ Â°Ð Â»Ð¡ÐŠÐ Ð…Ð¡â€¹Ð Âµ
		//Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Â°								//Ð Ñ—Ð Â°Ð¡Ð‚Ð Â°Ð Ñ˜Ð ÂµÐ¡â€šÐ¡Ð‚Ð¡â€¹: grStep	Ð¡â‚¬Ð Â°Ð Ñ– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘, Ð Ñ—Ð ÂµÐ¡Ð‚Ð ÂµÐ Ò‘Ð Â°Ð ÂµÐ¡â€šÐ¡ÐƒÐ¡Ð Ð Ñ•Ð¡Ð‚Ð Ñ‘Ð Ñ–Ð Ñ‘Ð Ð…Ð Â°Ð Â»Ð¡ÐŠÐ Ð…Ð¡â€¹Ð â„–, Ð Ð†Ð Ñ•Ð Â·Ð Ð†Ð¡Ð‚Ð Â°Ð¡â€°Ð Â°Ð ÂµÐ¡â€šÐ¡ÐƒÐ¡Ð Ð Ñ•Ð Ñ”Ð¡Ð‚Ð¡Ñ“Ð Ñ–Ð Â»Ð ÂµÐ Ð…Ð Ð…Ð¡â€¹Ð â„–, offset - Ð¡ÐƒÐ Ñ˜Ð ÂµÐ¡â€°Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð Â° Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š, scale - Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡â‚¬Ð¡â€šÐ Â°Ð Â±, Ð¡â€ Ð Ð†Ð ÂµÐ¡â€š, max - Ð Ñ˜Ð Â°Ð Ñ”Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Â»Ð¡ÐŠÐ Ð…Ð Â°Ð¡Ð Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘Ð Â»Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹ (Ð Ð† Ð Â·Ð Â°Ð Ð†Ð Ñ‘Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ñ•Ð¡ÐƒÐ¡â€šÐ Ñ‘ Ð Ñ•Ð¡â€š Ð Ð…Ð Â°Ð Ñ—Ð¡Ð‚Ð Â°Ð Ð†Ð Â»Ð ÂµÐ Ð…Ð Ñ‘Ð¡Ð Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ Ñ•Ð Ð†Ð Â°Ð Ð…Ð Ñ‘Ð¡Ð Ð Â»Ð Ñ‘Ð Ð…Ð Ñ‘Ð â„–),																	
											//tWH - Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘Ð Â»Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° (Ð ÂµÐ¡ÐƒÐ Â»Ð Ñ‘ max Ð Â±Ð¡â€¹Ð Â»  Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â°, Ð Â·Ð Ð…Ð Â°Ð¡â€¡Ð Ñ‘Ð¡â€š Ð Ò‘Ð Ñ•Ð Â»Ð Â¶Ð Ð…Ð Â° Ð Â±Ð¡â€¹Ð¡â€šÐ¡ÐŠ Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â°), HVLine Ð¡â€žÐ¡Ñ“Ð Ð…Ð Ñ”Ð¡â€ Ð Ñ‘Ð¡Ð Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ Ñ•Ð Ð†Ð Â°Ð Ð…Ð Ñ‘Ð¡Ð Ð Ñ–Ð Ñ•Ð¡Ð‚Ð Ñ‘Ð Â·Ð Ñ•Ð¡â€šÐ Â°Ð Â»Ð¡ÐŠÐ Ð…Ð Ñ•Ð â„– Ð Â»Ð Ñ‘Ð Â±Ð Ñ• Ð Ð†Ð ÂµÐ¡Ð‚Ð¡â€šÐ Ñ‘Ð Ñ”Ð Â°Ð Â»Ð¡ÐŠÐ Ð…Ð Ñ•Ð â„– Ð Â»Ð Ñ‘Ð Ð…Ð Ñ‘Ð Ñ‘,pxStep Ð Ð†Ð Ñ•Ð Â·Ð Ð†Ð¡Ð‚Ð Â°Ð¡â€°Ð Â°Ð ÂµÐ Ñ˜ Ð Ñ—Ð Ñ‘Ð Ñ”Ð¡ÐƒÐ ÂµÐ Â»Ð¡ÐŠÐ Ð…Ð¡â€¹Ð â„– Ð¡â‚¬Ð Â°Ð Ñ– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘, offs - Ð Ð†Ð Ñ•Ð Â·Ð Ð†Ð¡Ð‚Ð Â°Ð¡â€°Ð Â°Ð ÂµÐ Ñ˜ Ð Ñ”Ð¡Ð‚Ð Â°Ð â„–Ð Ð…Ð Ñ‘Ð â„– Ð Â»Ð ÂµÐ Ð†Ð¡â€¹Ð â„– Ð Ñ—Ð Ñ• x, Ð Â»Ð Ñ‘Ð Â±Ð Ñ• Ð Ð…Ð Ñ‘Ð Â¶Ð Ð…Ð Ñ‘Ð â„– Ð Ñ—Ð Ñ• y Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð¡ÐŠÐ Ð…Ð¡â€¹Ð â„– Ð Ñ—Ð Ñ‘Ð Ñ”Ð¡ÐƒÐ ÂµÐ Â»Ð¡ÐŠ Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘.
											
		grStep = Mathf.Abs(grStep);			 
		grStep = Mathf.Floor(grStep/0.5f)*0.5f;  //Ð Ñ•Ð Ñ”Ð¡Ð‚Ð¡Ñ“Ð Ñ–Ð Â»Ð¡ÐÐ ÂµÐ Ñ˜ Ð¡â‚¬Ð Â°Ð Ñ– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘ Ð Ò‘Ð Ñ• 0.5		
		if(grStep == 0f)grStep=0.5f;
		
		// removed the * gridStep so that it scale and not just create bigger blocks
		pxStep = (scale); //Ð¡â‚¬Ð Â°Ð Ñ– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘ Ð Ð† Ð Ñ—Ð Ñ‘Ð Ñ”Ð¡ÐƒÐ ÂµÐ Â»Ð¡ÐÐ¡â€¦		
				
		if(pxStep < 3f)pxStep = 3f;	
		
		offs = Mathf.Repeat(offset,pxStep); 
				
		float way = offs;
		
		while(way < max){      
			HVLine((int)way , color, tWH);				
			way += pxStep;			
		}		
		
	}
	
	private void DrawNumbers(int max, int tWtH , float pxStep, float offset, float grStep,float offs, int XorY, Vector2 fntScale, Color color, int borderGrPadding, int grPadding){ //Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜ Ð Ð…Ð Ñ•Ð Ñ˜Ð ÂµÐ¡Ð‚Ð Â° (Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€šÐ¡â€¹) Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘
							//max - Ð Ñ˜Ð Â°Ð Ñ”Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Â»Ð¡ÐŠÐ Ð…Ð Â°Ð¡Ð Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘Ð Â»Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹ (Ð Ð† Ð Â·Ð Â°Ð Ð†Ð Ñ‘Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Ñ•Ð¡ÐƒÐ¡â€šÐ Ñ‘ Ð Ñ•Ð¡â€š Ð Ð…Ð Â°Ð Ñ—Ð¡Ð‚Ð Â°Ð Ð†Ð Â»Ð ÂµÐ Ð…Ð Ñ‘Ð¡Ð Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ Ñ•Ð Ð†Ð Â°Ð Ð…Ð Ñ‘Ð¡Ð Ð Ð…Ð Ñ•Ð Ñ˜Ð ÂµÐ¡Ð‚Ð Ñ•Ð Ð†), tWtH - Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘Ð Â»Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° (Ð ÂµÐ¡ÐƒÐ Â»Ð Ñ‘ max Ð Â±Ð¡â€¹Ð Â»  Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â°, Ð Â·Ð Ð…Ð Â°Ð¡â€¡Ð Ñ‘Ð¡â€š Ð Ò‘Ð Ñ•Ð Â»Ð Â¶Ð Ð…Ð Â° Ð Â±Ð¡â€¹Ð¡â€šÐ¡ÐŠ Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â°),
							//pxStep - Ð Ñ—Ð Ñ‘Ð Ñ”Ð¡ÐƒÐ ÂµÐ Â»Ð¡ÐŠÐ Ð…Ð¡â€¹Ð â„– Ð¡â‚¬Ð Â°Ð Ñ– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘, offset - Ð¡ÐƒÐ Ñ˜Ð ÂµÐ¡â€°Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð Â° Ð Ñ”Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š Ð Ñ—Ð Ñ• Ð¡â€¦ Ð Â»Ð Ñ‘Ð Â±Ð Ñ• Ð Ñ—Ð Ñ• y, grStep - Ð¡â‚¬Ð Â°Ð Ñ– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘, offs - Ð Ñ”Ð¡Ð‚Ð Â°Ð â„–Ð Ð…Ð Ñ‘Ð â„– Ð Â»Ð ÂµÐ Ð†Ð¡â€¹Ð â„– Ð Ñ—Ð Ñ• x, Ð Â»Ð Ñ‘Ð Â±Ð Ñ• Ð Ð…Ð Ñ‘Ð Â¶Ð Ð…Ð Ñ‘Ð â„– Ð Ñ—Ð Ñ• y Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð¡ÐŠÐ Ð…Ð¡â€¹Ð â„– Ð Ñ—Ð Ñ‘Ð Ñ”Ð¡ÐƒÐ ÂµÐ Â»Ð¡ÐŠ Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘, XorY - Ð Ñ—Ð Ñ• x Ð Ñ‘Ð Â»Ð Ñ‘ Ð Ñ—Ð Ñ• y Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð¡Ð‹Ð¡â€šÐ¡ÐƒÐ¡Ð Ð Ð…Ð Ñ•Ð Ñ˜Ð ÂµÐ¡Ð‚Ð Â°, 
							//fntScale - Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡â‚¬Ð¡â€šÐ Â°Ð Â± Ð¡â‚¬Ð¡Ð‚Ð Ñ‘Ð¡â€žÐ¡â€šÐ Â°, Ð¡â€ Ð Ð†Ð ÂµÐ¡â€š, borderGrPadding - Ð Ñ•Ð¡â€šÐ¡ÐƒÐ¡â€šÐ¡Ñ“Ð Ñ— Ð Ñ•Ð¡â€š Ð Ñ–Ð¡Ð‚Ð Â°Ð Ð…Ð Ñ‘Ð¡â€ Ð¡â€¹ Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹, grPadding - Ð Ñ•Ð¡â€šÐ¡ÐƒÐ¡â€šÐ¡Ñ“Ð Ñ— Ð Ñ•Ð¡â€š Ð Â»Ð Ñ‘Ð Ð…Ð Ñ‘Ð â„– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘.
		XorY =(int) Mathf.Clamp01(XorY); 
		
		int tHeight = XorY==1 ? max : tWtH;
		int tWidth = XorY==0 ? max : tWtH;
		
		bool alignLeft = XorY==1 ? false : true;
		
		int length = (int)(max/pxStep)+1; //Ð Ñ”Ð Ñ•Ð Â»Ð Ñ‘Ð¡â€¡Ð ÂµÐ¡ÐƒÐ¡â€šÐ Ð†Ð Ñ• Ð Ò‘Ð ÂµÐ Â»Ð ÂµÐ Ð…Ð Ñ‘Ð â„– Ð Ð…Ð Â° Ð¡â‚¬Ð Ñ”Ð Â°Ð Â»Ð Âµ;
		
		int dLength = (int)(offset/pxStep); //Ð Ñ”Ð Ñ•Ð Â»Ð Ñ‘Ð¡â€¡Ð ÂµÐ¡ÐƒÐ¡â€šÐ Ð†Ð Ñ• Ð Ò‘Ð ÂµÐ Â»Ð ÂµÐ Ð…Ð Ñ‘Ð â„– Ð Ð…Ð Ñ‘Ð Â¶Ð Âµ Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð Â° Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š	
		
		float lowerVal = -(dLength*grStep); //Ð Ð…Ð Ñ‘Ð Â¶Ð Ð…Ð ÂµÐ Âµ Ð Â·Ð Ð…Ð Â°Ð¡â€¡Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð¡â‚¬Ð Ñ”Ð Â°Ð Â»Ð¡â€¹
		
		if(offset < 0) lowerVal+=grStep;		
		
		float val = lowerVal;
		
		
		Vector2 fOffset = Vector2.zero; //new Vector2(3,offs-2);		
		fOffset[XorY] = offs+grPadding;			
		int XY = XorY==1 ? 0 : 1;
		fOffset[XY] = borderGrPadding;
		
		for(int i = 0; i<length; i++){			
			DrawString(val.ToString(),fOffset,fntScale,2,tWidth,tHeight,color,alignLeft);
			fOffset[XorY] += pxStep;
			val += grStep;
		}
		
	}
	
	private void DrawGrid(Vector2 scale, Vector2 offset, Color color, float grStepX, float grStepY, int tWidth,int tHeight, Vector2 fntScale){  //Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜ Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð¡Ñ“ Ð Ñ‘ Ð Ð…Ð Ñ•Ð Ñ˜Ð ÂµÐ¡Ð‚Ð Â° Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š, Ð¡Ðƒ Ð Â·Ð Â°Ð Ò‘Ð Â°Ð Ð…Ð Ð…Ð¡â€¹Ð Ñ˜ Ð¡â‚¬Ð Â°Ð Ñ–Ð Ñ•Ð Ñ˜ Ð Ñ‘ Ð¡Ð‚Ð Â°Ð Â·Ð Ñ˜Ð ÂµÐ¡Ð‚Ð Ñ•Ð Ñ˜
							//scale Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡â‚¬Ð¡â€šÐ Â°Ð Â± Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘, offset - Ð¡ÐƒÐ Ñ˜Ð ÂµÐ¡â€°Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð Â° Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š, Ð¡â€ Ð Ð†Ð ÂµÐ¡â€š, grStepX - Ð¡â‚¬Ð Â°Ð Ñ– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘ Ð Ñ—Ð Ñ• X, grStepY - Ð¡â‚¬Ð Â°Ð Ñ– Ð¡ÐƒÐ ÂµÐ¡â€šÐ Ñ”Ð Ñ‘ Ð Ñ—Ð Ñ• Y, Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹, Ð¡Ð‚Ð Â°Ð Â·Ð Ñ˜Ð ÂµÐ¡Ð‚ Ð¡â‚¬Ð¡Ð‚Ð Ñ‘Ð¡â€žÐ¡â€šÐ Â°
		float pxStepY;
		float offsY;
		DrawGridLines(ref grStepY,offset.y,scale.y,color,tHeight,tWidth,DrawHorisontalLineTex,out pxStepY,out offsY);
		
		float pxStepX;
		float offsX;
		DrawGridLines(ref grStepX,offset.x,scale.x,color,tWidth,tHeight,DrawVerticalLineTex,out pxStepX,out offsX);
		
		//Ð Â¦Ð Ñ‘Ð¡â€žÐ¡Ð‚Ð¡â€¹
		
		int fontOffs = (int)((fontNumbers.StrHeight*fntScale.y)+3f);
		
		DrawNumbers(tHeight,tWidth,pxStepY,offset.y,grStepY,offsY,1,fntScale,fontColor,3,-3);
		DrawNumbers(tWidth,tHeight,pxStepX,offset.x,grStepX,offsX,0,fntScale,fontColor,fontOffs,-3);
				
	}
	
	private void DrawCoordAxis(Vector2 offset, int tWidth, int tHeight){  //Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜ Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€šÐ Ð…Ð¡â€¹Ð Âµ Ð Ñ•Ð¡ÐƒÐ Ñ‘
							//offset - Ð¡ÐƒÐ Ñ˜Ð ÂµÐ¡â€°Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð Â° Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š, Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹
		if(offset.x >= 0 && offset.x < tWidth)
			DrawVerticalLineTex((int) offset.x,colorY, tHeight);	
		
		if(offset.y >= 0 && offset.y < tHeight)		
			DrawHorisontalLineTex((int) offset.y,colorX,tWidth);	
		
	}	
	
	
	private void DrawBackground(Color color, int tWidth, int tHeight){  //Ð Â·Ð Â°Ð Â»Ð Ñ‘Ð Ð†Ð Ñ”Ð Â° Ð¡â€žÐ Ñ•Ð Ð…Ð Â°
								//Ð¡â€ Ð Ð†Ð ÂµÐ¡â€š, Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð Ñ‘ Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹.
		int y = 0;
        while (y < tHeight) {
            int x = 0;
            while (x < tWidth) {
                texture.SetPixel(x, y, color);
                ++x;
            }
            ++y;
        }		
		
	}
	
	private void DrawLine(Vector2 p1, Vector2 p2, Color color, Vector2 offset, Vector2 scale){ //Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜ Ð Â»Ð Ñ‘Ð Ð…Ð Ñ‘Ð¡Ð‹ Ð Ñ•Ð¡â€š Ð¡â€šÐ Ñ•Ð¡â€¡Ð Ñ”Ð Ñ‘ Ð Ò‘Ð Ñ• Ð¡â€šÐ Ñ•Ð¡â€¡Ð Ñ”Ð Ñ‘ Ð Ð† Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Â°Ð¡â€šÐ Ð…Ð Ñ•Ð â„– Ð Ñ—Ð Â»Ð Ñ•Ð¡ÐƒÐ Ñ”Ð Ñ•Ð¡ÐƒÐ¡â€šÐ Ñ‘
							//p1-Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€šÐ Â° Ð Ñ—Ð ÂµÐ¡Ð‚Ð Ð†Ð Ñ•Ð â„– Ð¡â€šÐ Ñ•Ð¡â€¡Ð Ñ”Ð Ñ‘, p2- Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€šÐ Â° Ð Ð†Ð¡â€šÐ Ñ•Ð¡Ð‚Ð Ñ•Ð â„– Ð¡â€šÐ Ñ•Ð¡â€¡Ð Ñ”Ð Ñ‘, offset - Ð¡ÐƒÐ Ñ˜Ð ÂµÐ¡â€°Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð Â° Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š, scale Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡â‚¬Ð¡â€šÐ Â°Ð Â±
		p1=Vector2.Scale(scale,p1);
		p1=p1+offset;
		
		p2=Vector2.Scale(scale,p2);
		p2=p2+offset;
		
		DrawLineTex((int)p1.x,(int)p1.y,(int)p2.x,(int)p2.y,color,texWidth,texHeight);
	}
		
	
	
	
	/*private void DrawGraphFunction(GraphFunction[] graphFunctions, float pxStep, Vector2 offset,Vector2 scale,float tWidth,Color[] colors){ // Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜ Ð Ñ–Ð¡Ð‚Ð Â°Ð¡â€žÐ Ñ‘Ð Ñ” Ð¡â€žÐ¡Ñ“Ð Ð…Ð Ñ”Ð¡â€ Ð Ñ‘Ð Ñ‘
								//graphFunctions - Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡ÐƒÐ Ñ‘Ð Ð† Ð¡â€žÐ¡Ñ“Ð Ð…Ð Ñ”Ð¡â€ Ð Ñ‘Ð â„– Ð Ñ—Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Ñ‘Ð Ñ˜Ð Â°Ð¡Ð‹Ð¡â€°Ð Ñ‘Ð¡â€¦ Ð¡â€¦ Ð Ñ‘ Ð Ð†Ð Ñ•Ð Â·Ð Ð†Ð¡Ð‚Ð Â°Ð¡â€°Ð Â°Ð¡Ð‹Ð¡â€°Ð Ñ‘Ð¡â€¦ y, pxStep - Ð Ò‘Ð Â»Ð Ñ‘Ð Ð…Ð Â° Ð Â»Ð Ñ‘Ð Ð…Ð Ñ‘Ð â„– Ð Ñ‘Ð Â· Ð Ñ”Ð Ñ•Ð¡â€šÐ Ñ•Ð¡Ð‚Ð¡â€¹Ð¡â€¦ Ð¡ÐƒÐ Ñ•Ð¡ÐƒÐ¡â€šÐ Ñ•Ð Ñ‘Ð¡â€š Ð Ñ–Ð¡Ð‚Ð Â°Ð¡â€žÐ Ñ‘Ð Ñ” (Ð Ð† Ð Ñ—Ð Ñ‘Ð Ñ”Ð¡ÐƒÐ ÂµÐ Â»Ð¡ÐÐ¡â€¦), offset - Ð¡ÐƒÐ Ñ˜Ð ÂµÐ¡â€°Ð ÂµÐ Ð…Ð Ñ‘Ð Âµ Ð Ð…Ð Â°Ð¡â€¡Ð Â°Ð Â»Ð Â° Ð Ñ”Ð Ñ•Ð Ñ•Ð¡Ð‚Ð Ò‘Ð Ñ‘Ð Ð…Ð Â°Ð¡â€š, scale - Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡â‚¬Ð¡â€šÐ Â°Ð Â±, tWidth Ð¡â‚¬Ð Ñ‘Ð¡Ð‚Ð Ñ‘Ð Ð…Ð Â° Ð¡â€šÐ ÂµÐ Ñ”Ð¡ÐƒÐ¡â€šÐ¡Ñ“Ð¡Ð‚Ð¡â€¹, colors - Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡ÐƒÐ Ñ‘Ð Ð† Ð¡â€ Ð Ð†Ð ÂµÐ¡â€šÐ Ñ•Ð Ð† Ð¡Ð‚Ð Ñ‘Ð¡ÐƒÐ¡Ñ“Ð ÂµÐ Ñ˜Ð¡â€¹Ð¡â€¦ Ð Ñ–Ð¡Ð‚Ð Â°Ð¡â€žÐ Ñ‘Ð Ñ”Ð Ñ•Ð Ð†
		int cL = colors.Length;
		int gfL = graphFunctions.Length;
		
		if(cL < gfL){
			Array.Resize<Color>(ref colors,gfL);	
			//Ð Ò‘Ð Ñ•Ð Ñ—Ð Ñ•Ð Â»Ð Ð…Ð Ñ‘Ð¡â€šÐ¡ÐŠ Ð Ñ˜Ð Â°Ð¡ÐƒÐ¡ÐƒÐ Ñ‘Ð Ð† Ð¡â€ Ð Ð†Ð ÂµÐ¡â€šÐ Â°Ð Ñ˜Ð Ñ‘
			for (int i=cL;i<gfL;i++){
				colors[i] = Color.red;				
			}
		}
		
		for(int i = 0; i<gfL; i++){
			DrawGraphFunction(graphFunctions[i],pxStep,offset,scale,tWidth,colors[i]);			
		}	
		
	}*/
	
	// added an additional argument to DrawGraphFunction to take the gridStep
	private void DrawGraphFunction(float[] y, float pxStep, Vector2 offset,Vector2 scale,float tWidth,Color color, Vector2 gridStep){


		if(scale.x <= 0f) scale.x = 0.0001f;
		//if(pxStep <=0f) pxStep = 0.0001f;
		
		//float xLeft = -offset.x/scale.x;  //1)
		//float length = tWidth/pxStep; //2)
		//pxStep = pxStep/scale.x;
		
		
		//float val = xLeft;
		
		Vector2 p1 = Vector2.zero;
		Vector2 p2 = Vector2.zero;
		
		if(y == null){
			p1.x = 0;
			p1.y = 0;
	
			p2.x = 0;
			p2.y = 0;
		}
		else{
			for(int i = 0; i< y.Length - 1; i++){//3
				//p1.x = val;
				//p1.y = graphFunc(val);
				//x is divided by the gridstep to get to the appropriate position on graph
				p1.x = i/gridStep.x;
				//y is divided by the gridstep to get to the appropriate position on graph
				p1.y = y[i]/gridStep.y;
				
				//val+=pxStep;
				
				//p2.x = val;
				//p2.y = graphFunc(val);
				p2.x = (i + 1)/gridStep.x; //y is divided by the gridstep to get to the appropriate position on graph
				//y is divided by the gridstep to get to the appropriate position on graph
				p2.y = y[i+1]/gridStep.y;
				
				DrawLine(p1,p2,color,offset,scale);
			}
		}
		
	}
	
	
	public Texture2D Draw(float[] y,Vector2 offset,Vector2 scale, Color color, Vector2 gridStep, int pixelDetail){
				
		DrawBackground(backgroundColor, texWidth, texHeight);
		
		DrawGrid(scale,offset,gridColor,gridStep.x,gridStep.y,texWidth,texHeight,fontScale);
		
		DrawCoordAxis(offset, texWidth, texHeight);	
		
		// passing gridStep as an additional argument to DrawGraphFunction
		DrawGraphFunction(y,pixelDetail,offset,scale,texWidth,color,gridStep);
			
		texture.Apply();
		
		return texture;
	}
	
	/*public Texture2D Draw(GraphFunction[] functions,Vector2 offset,Vector2 scale, Color[] colors, Vector2 gridStep, int pixelDetail){
				
		DrawBackground(backgroundColor, texWidth, texHeight);
		
		DrawGrid(scale,offset,gridColor,gridStep.x,gridStep.y,texWidth,texHeight,fontScale);
		
		DrawCoordAxis(offset, texWidth, texHeight);	
		
		DrawGraphFunction(functions,pixelDetail,offset,scale,texWidth,colors);
			
		texture.Apply();
		
		return texture;
	}*/
		
	
	/*
	public Texture2D Draw(GraphFunction[] functions, Color[] colors, Vector2 offset ,Vector2 scale){
		if(scale.x <= 0f) scale.x = 0.0001f;
		if(scale.y <= 0f) scale.y = 0.0001f;
		
		Vector2 gridStep = new Vector2(50f / scale.x,50f / scale.y);		
		
		this.Draw(functions,offset,scale,colors,gridStep,1);
		
		
		return texture;
	}
	*/
	
}