using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NVComponent : MonoBehaviour {

	static List<NVComponent> nvComponents;
	static bool init=false;

	Transform _tform;
	public Transform tform {get{if(!_tform)_tform=transform;return _tform;}}
	void Awake(){
		if(!init){
			nvComponents = new List<NVComponent>();
			init=true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!pause)
			NVUpdate();
	}

	protected virtual void NVUpdate(){

	}

	bool pause;
}
