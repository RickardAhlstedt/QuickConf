using UnityEngine;
using UnityEditor;
using System.Collections;

public class BoolPreference : Preference {
	string key = "";
	int value = 0;
	bool temp = false;
	PrefsEditorWindow window = null;
	
	public override void Start(PrefsEditorWindow window) {
		this.window = window;
		if(PlayerPrefs.HasKey(key)) {
			value = PlayerPrefs.GetInt(key);
		}
	}

	public override void Start(PrefsEditorWindow window, string key) {
		this.window = window;
		this.key = key;
		if(PlayerPrefs.HasKey(key)) {
			value = PlayerPrefs.GetInt(key);
		}
	}

	bool show = true;

	public override void OnGUI() {
		if(temp)
			value = 1;
		else
			value = 0;

		show = EditorGUILayout.Foldout(show, "Bool: " + key + ", value: " + value);
		if(show) {
			key = EditorGUILayout.TextField("Key:", key);
			temp = EditorGUILayout.Toggle("Value:", temp);
			if(GUILayout.Button("Load")) {
				if(PlayerPrefs.HasKey(key)) {
					value = PlayerPrefs.GetInt(key);
					if(value == 1)
						temp = true;
					else
						temp = false;
				}
				
				
			}
			if(GUILayout.Button("Delete")) {
				window.removePreference(key);
			}
		}
	}

	public bool getValue() {
		return temp;
	}
	
	public override void save() {
		PlayerPrefs.SetInt(key, value);
		PlayerPrefs.Save();
	}
	
	public override string getKey() {
		return key;
	}
}
