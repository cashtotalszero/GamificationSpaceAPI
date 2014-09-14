using UnityEngine;
using System.Collections;
 
public class Nullable {
	public static implicit operator bool(Nullable o) {
		return (object)o != null;
	}
}