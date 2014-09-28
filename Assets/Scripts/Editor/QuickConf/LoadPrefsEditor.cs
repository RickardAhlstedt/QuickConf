using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LoadPrefs))]
public class LoadPrefsEditor : Editor {

	PrefsEditorWindow window = null;

	string[] options = new string[]{"Float", "String", "Integer", "Boolean"};
	int index = 0;

	string key = "";

	Preference pref;

	public void Awake() {
		window = (PrefsEditorWindow)EditorWindow.GetWindow(typeof(PrefsEditorWindow));
	}

	bool showFoldout = false;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		LoadPrefs loadPrefs = (LoadPrefs)target;

		index = EditorGUILayout.Popup("Preference-type:", index, options);
		key = EditorGUILayout.TextField("Preference-name:", key);

		if(GUILayout.Button("Load")) {
			if(PlayerPrefs.HasKey(key)) {
				if(options[index].Equals("String")) {
					StringPreference stringPref = new StringPreference();
					stringPref.Start(window, key);
					loadPrefs.setString(stringPref.getValue().ToString());
					pref = stringPref;
				} else if(options[index].Equals("Float")) {
					
				} else if(options[index].Equals("Integer")) {
					
				} else if(options[index].Equals("Boolean")) {
					
				} else {
					EditorUtility.DisplayDialog("Error!", "An exception happened, " +
					                            "the item that was added doesn't exists." +
					                            "\nOr the index is out of range", "Mkay'");
				}	
			}
		}
		showFoldout = EditorGUILayout.Foldout(showFoldout, "Values:");
		if(showFoldout) {
			if(this.pref != null) {
				this.pref.OnGUI();
			}
		}
	}
}
