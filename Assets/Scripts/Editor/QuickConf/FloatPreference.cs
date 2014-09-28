using UnityEngine;
using UnityEditor;
using System.Collections;

public class FloatPreference : Preference {

	string key = "";
	float value = 0;
	
	PrefsEditorWindow window = null;
	
	public override void Start(PrefsEditorWindow window) {
		this.window = window;
		if(PlayerPrefs.HasKey(key)) {
			value = PlayerPrefs.GetFloat(key);
		}
	}

	public override void Start(PrefsEditorWindow window, string key) {
		this.window = window;
		this.key = key;
		if(PlayerPrefs.HasKey(key)) {
			value = PlayerPrefs.GetFloat(key);
		}
	}

	bool show = true;

	public override void OnGUI() {
		show = EditorGUILayout.Foldout(show, "Float: " + key + ", value: " + value);
		if(show) {
			key = EditorGUILayout.TextField("Key:", key);
			value = EditorGUILayout.FloatField("Value:", value);
			if(GUILayout.Button("Load")) {
				if(PlayerPrefs.HasKey(key))
					value = PlayerPrefs.GetFloat(key);
			}
			if(GUILayout.Button("Delete")) {
				window.removePreference(key);
			}
		}
	}

	public float getValue() {
		return value;
	}

	public override void save() {
		PlayerPrefs.SetFloat(key, value);
		PlayerPrefs.Save();
	}
	
	public override string getKey() {
		return key;
	}
}
