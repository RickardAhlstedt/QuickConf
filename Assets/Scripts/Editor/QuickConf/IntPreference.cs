using UnityEngine;
using UnityEditor;
using System.Collections;

public class IntPreference : Preference {

	string key = "";
	int value = 0;

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
		show = EditorGUILayout.Foldout(show, "Integer: " + key + ", value: " + value);
		if(show) {
			key = EditorGUILayout.TextField("Key:", key);
			value = EditorGUILayout.IntField("Value:", value);
			if(GUILayout.Button("Load")) {
				if(PlayerPrefs.HasKey(key))
					value = PlayerPrefs.GetInt(key);
			}
			if(GUILayout.Button("Delete")) {
				window.removePreference(key);
			}
		}
	}

	public int getValue() {
		return value;
	}

	public override void save() {
		PlayerPrefs.SetInt(key, value);
		PlayerPrefs.Save();
	}

	public override string getKey() {
		return key;
	}
}
