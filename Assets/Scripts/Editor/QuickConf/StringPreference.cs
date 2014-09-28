using UnityEngine;
using UnityEditor;
using System.Collections;

public class StringPreference : Preference {

	string key = "";
	string value = "";
	
	PrefsEditorWindow window = null;
	
	public override void Start(PrefsEditorWindow window) {
		this.window = window;
		if(PlayerPrefs.HasKey(key)) {
			value = PlayerPrefs.GetString(key);
		}
	}

	public override void Start(PrefsEditorWindow window, string key) {
		this.window = window;
		this.key = key;
		if(PlayerPrefs.HasKey(key)) {
			value = PlayerPrefs.GetString(key);
		}
	}

	bool show = true;

	public override void OnGUI() {
		show = EditorGUILayout.Foldout(show, "String: " + key + ", value: " + value);
		if(show) {
			key = EditorGUILayout.TextField("Key:", key);
			value = EditorGUILayout.TextField("Value:", value);
			if(GUILayout.Button("Load")) {
				if(PlayerPrefs.HasKey(key))
					value = PlayerPrefs.GetString(key);
			}
			if(GUILayout.Button("Delete")) {
				window.removePreference(key);
			}
		}
	}

	public string getValue() {
		return value;
	}

	public override void save() {
		PlayerPrefs.SetString(key, value);
		PlayerPrefs.Save();
	}
	
	public override string getKey() {
		return key;
	}
}
