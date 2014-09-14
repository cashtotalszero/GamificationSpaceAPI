using UnityEngine;
using System.Collections;


public class FontNumbers {
	
	private static Symbol[] symbols = new Symbol[]{
		new Symbol('1',2,7, new Vector2[]{
			new Vector2(0f,-1f),
			new Vector2(1f,0f),
			new Vector2(1f,-6f),
			
			
		}),
		
		new Symbol('2',4,7, new Vector2[]{
			new Vector2(0f,-1f),
			new Vector2(1f,0f),
			new Vector2(2f,0f),
			new Vector2(3f,-1f),
			new Vector2(3f,-2f),
			new Vector2(0f,-6f),
			new Vector2(3f,-6f),
		}),
		
		new Symbol('3',4,7, new Vector2[]{
			new Vector2(0f,-1f),
			new Vector2(1f,0f),
			new Vector2(2f,0f),
			new Vector2(3f,-1f),
			new Vector2(3f,-2f),
			new Vector2(2f,-3f),
			new Vector2(3f,-4f),
			new Vector2(3f,-5f),
			new Vector2(2f,-6f),
			new Vector2(1f,-6f),
			new Vector2(0f,-5f),
		}),
			                                          
		new Symbol('4',5,7, new Vector2[]{
			new Vector2(4f,-4f),
			new Vector2(0f,-4f),
			new Vector2(3f,0f),
			new Vector2(3f,-6f),			
		}),
		
		new Symbol('5',4,7, new Vector2[]{
			new Vector2(3f,0f),
			new Vector2(0f,0f),
			new Vector2(0f,-3f),
			new Vector2(1f,-2f),			
			new Vector2(2f,-2f),
			new Vector2(3f,-3f),
			new Vector2(3f,-5f),
			new Vector2(2f,-6f),
			new Vector2(1f,-6f),
			new Vector2(0f,-5f),			
		}),
		
		
		new Symbol('6',4,7, new Vector2[]{
			new Vector2(3f,-1f),
			new Vector2(2f,0f),
			new Vector2(1f,0f),
			new Vector2(0f,-1f),
			new Vector2(0f,-5f),
			new Vector2(1f,-6f),
			new Vector2(2f,-6f),
			new Vector2(3f,-5f),
			new Vector2(3f,-4f),
			new Vector2(2f,-3f),
			new Vector2(1f,-3f),
			new Vector2(0f,-4f),
		}),
		
		new Symbol('7',4,7, new Vector2[]{
			new Vector2(0f,0f),
			new Vector2(3f,0f),
			new Vector2(2f,-3f),
			new Vector2(1f,-6f),			
		}),
		
		new Symbol('8',4,7, new Vector2[]{
			new Vector2(0f,-1f),
			new Vector2(1f,0f),
			new Vector2(2f,0f),
			new Vector2(3f,-1f),
			new Vector2(3f,-2f),
			new Vector2(2f,-3f),
			new Vector2(1f,-3f),
			new Vector2(0f,-4f),
			new Vector2(0f,-5f),
			new Vector2(1f,-6f),
			new Vector2(2f,-6f),
			new Vector2(3f,-5f),
			new Vector2(3f,-4f),
			new Vector2(2f,-3f),
			new Vector2(1f,-3f),
			new Vector2(0f,-2f),
			new Vector2(0f,-1f),
			
		}),
		
		new Symbol('9',4,7, new Vector2[]{
			new Vector2(0f,-5f),
			new Vector2(1f,-6f),
			new Vector2(2f,-6f),
			new Vector2(3f,-5f),
			new Vector2(3f,-1f),
			new Vector2(2f,0f),
			new Vector2(1f,0f),
			new Vector2(0f,-1f),
			new Vector2(0f,-2f),
			new Vector2(1f,-3f),
			new Vector2(2f,-3f),
			new Vector2(3f,-2f),
		}),
		
		
		new Symbol('0',4,7, new Vector2[]{
			new Vector2(0f,-1f),
			new Vector2(1f,0f),
			new Vector2(2f,0f),
			new Vector2(3f,-1f),
			new Vector2(3f,-5f),
			new Vector2(2f,-6f),
			new Vector2(1f,-6f),
			new Vector2(0f,-5f),
			new Vector2(0f,-1f),			
		}),
		
		
		new Symbol('-',3,7, new Vector2[]{
			new Vector2(0f,-3f),			
			new Vector2(3f,-3f),
		}),
		
		new Symbol('.',1,7, new Vector2[]{
			new Vector2(0f,-5.5f),			
			new Vector2(0.5f,-5.5f),
			new Vector2(0.5f,-6f),
			new Vector2(0f,-6f),
			new Vector2(0f,-5.5f),
		}),
		
		
	};
	
	public Symbol[] Symbols{
		get{return symbols;}		
	}
	
	
	private int strHeight = 7;
	
	public int StrHeight{
		get{return strHeight;}		
	}	
	
	public static int CharsPxWidth(string str){
		
		int result = 0;
		
		int sl = str.Length;
		
		for(int i = 0; i<sl; i++){
			
			foreach(Symbol s in symbols){
				if(s.Sign == str[i])	
					result+=s.sWidth;
			}	
			
		}
		
		return result;		
	}
	
}


public class Symbol{
	
	private char sign;
	private Vector2[] vectors;
	private int wPx;
	private int hPx;
	
	public char Sign{
		get{return sign;}	
	}
	
	public Vector2[] Vectors{
		get{return vectors;}	
	}
	
	public int sWidth{
		get{return wPx;}	
	}
	
	public int sHeight{
		get{return hPx;}	
	}
	
	public Symbol(char s, int w, int h, Vector2[] v){
		sign = s;
		wPx = w;
		hPx = h;
		vectors = v;
		
	}
	
	
}
