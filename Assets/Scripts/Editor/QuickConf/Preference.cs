using UnityEngine;
using UnityEditor;
using System.Collections;

public abstract class Preference {

	PrefsEditorWindow window = null;
	string key = "";
	public virtual void Start(PrefsEditorWindow window){
		this.window = window;
	}
	public virtual void Start(PrefsEditorWindow window, string key) {
		this.window = window;
		this.key = key;
	}
	public virtual void OnGUI(){}
	public virtual void save(){}
	public virtual string getKey() {
		return null;
	}	

}
