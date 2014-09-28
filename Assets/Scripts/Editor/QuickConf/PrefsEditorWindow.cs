using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class PrefsEditorWindow : EditorWindow {

	List<Preference> prefs = new List<Preference>();

	PrefsData prefsData = new PrefsData();

	string[] options = new string[]{"Float", "String", "Integer", "Boolean"};
	int index = 0;
	public static PrefsEditorWindow window = null;

	[MenuItem("QuickConfig/Preferences Editor Window")]
	static void init() {
		window = (PrefsEditorWindow)EditorWindow.GetWindow(typeof(PrefsEditorWindow));
	}
	[MenuItem("QuickConfig/Load preferences")]
	static void load() {
		window.loadPrefs();
	}

	void Awake() {
		window = (PrefsEditorWindow)EditorWindow.GetWindow(typeof(PrefsEditorWindow));
		loadPrefs();
	}

	Vector2 scrollPos = new Vector2(0,0);

	void OnGUI() {
		index = EditorGUILayout.Popup("Preference:", index, options);
		if(GUILayout.Button("Add preference")) {
			addPreference(index);
		}

		EditorGUILayout.BeginVertical();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			for(int i = 0; i < prefs.Count; i++) {
				prefs[i].OnGUI();
			}
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();

		if(GUILayout.Button("Save all")) {
			if(prefs.Count > 0) {
				for(int j = 0; j < prefs.Count; j++) {
					prefs[j].save();
					if(prefs[j].Equals(typeof (StringPreference))) {
						prefsData.addPrefs(prefs[j].getKey().ToString(), "string");
					} else if(prefs[j].Equals(typeof (BoolPreference))) {
						prefsData.addPrefs(prefs[j].getKey().ToString(), "bool");
					} else if(prefs[j].Equals(typeof (FloatPreference))) {
						prefsData.addPrefs(prefs[j].getKey().ToString(), "float");
					} else if(prefs[j].Equals(typeof (IntPreference))) {
						prefsData.addPrefs(prefs[j].getKey().ToString(), "integer");
					}
				}
				savePrefs();
			} else {
				Debug.Log ("Well, add a few preferences to get started.");
			}
		}
		if(GUILayout.Button("Clear all and delete")) {
			if(EditorUtility.DisplayDialogComplex("Warning!", 
			                                      "This will clear all your set preferences, and delete them PERMANENTLY!" +
												  "\nAre you sure that you know what you're doing?" +
			                                      "\n(This action cannot be undone!)", 
			                                      "Yes, delete all my preferences.",
			                                      "Cancel", 
			                                      null) == 0) {
				prefs.Clear();
				prefsData.prefs.Clear();
				removeSaveFile();
				PlayerPrefs.DeleteAll();
			}
		}	
	}

	void OnInspectorUpdate() {
		this.Repaint();
	}

	void Update() {
		window = (PrefsEditorWindow)EditorWindow.GetWindow(typeof(PrefsEditorWindow));
	}

	public void removePreference(string key) {
		for(int i = 0; i < prefs.Count; i++) {
			if(prefs[i].getKey().Equals(key)) {
				PlayerPrefs.DeleteKey(key);
				prefs.RemoveAt(i);
				break;
			}
		}
	}

	void addPreference(int index) {
		if(options[index].Equals("String")) {
			StringPreference stringPref = new StringPreference();
			stringPref.Start(this);
			prefs.Add(stringPref);
		} else if(options[index].Equals("Float")) {
			FloatPreference floatPref = new FloatPreference();
			floatPref.Start(this);
			prefs.Add(floatPref);
		} else if(options[index].Equals("Integer")) {
			IntPreference intPref = new IntPreference();
			intPref.Start(this);
			prefs.Add(intPref);
		} else if(options[index].Equals("Boolean")) {
			BoolPreference boolPref = new BoolPreference();
			boolPref.Start(this);
			prefs.Add(boolPref);
		} else {
			EditorUtility.DisplayDialog("Error!", "An exception happened, " +
										"the item that was added doesn't exists." +
			                            "\nOr the index is out of range", "Mkay'");
		}
	}

	public void loadPrefs() {
		if(File.Exists(Application.dataPath + "/" + PlayerSettings.productName + "_quickConf.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.dataPath + "/" + PlayerSettings.productName + "_quickConf.dat", FileMode.Open);

			this.prefsData = bf.Deserialize(file) as PrefsData;

			foreach(string key in this.prefsData.prefs.Keys) {
				string type = this.prefsData.getType(key);
				if(type.Equals("string")) {
					StringPreference stringPref = new StringPreference();
					stringPref.Start(this, key);
					prefs.Add(stringPref);
				} else if(type.Equals("bool")) {
					BoolPreference boolPref = new BoolPreference();
					boolPref.Start(this, key);
					prefs.Add(boolPref);
				} else if(type.Equals("float")) {
					FloatPreference floatPref = new FloatPreference();
					floatPref.Start(this, key);
					prefs.Add(floatPref);
				} else if(type.Equals("integer")) {
					IntPreference intPref = new IntPreference();
					intPref.Start(this, key);
					prefs.Add(intPref);
				} else {
					EditorUtility.DisplayDialog("Error!", "An exception happened, " +
					                            "the item that was added doesn't exists.",
					                            "Mkay'");
				}
			}

			file.Close();
			prefsData.prefs.Clear();
		} else {
			Debug.Log ("Couldn't find a file containing the keyset for this project.");
		}
	}

	public void removeSaveFile() {
		if(File.Exists(Application.dataPath + "/" + PlayerSettings.productName + "_quickConf.dat")) {
			File.Delete(Application.dataPath + "/" + PlayerSettings.productName + "_quickConf.dat");
		}
	}

	public void savePrefs() {
		if(prefs == null) {
			Debug.Log ("Prefs is null");
		} else {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = null;
			if(File.Exists(Application.dataPath + "/" + PlayerSettings.productName + "_quickConf.dat")) {
				file = File.Open(Application.dataPath + "/" + PlayerSettings.productName + "_quickConf.dat", FileMode.Append);
			} else {
				file = File.Open(Application.dataPath + "/" + PlayerSettings.productName + "_quickConf.dat", FileMode.OpenOrCreate);
			}

			for(int i = 0; i < prefs.Count; i++) {
				Preference pref = prefs[i];
				if(pref.GetType().Equals(typeof(StringPreference))) {
					prefsData.addPrefs(pref.getKey().ToString(), "string");
				} else if(pref.GetType().Equals(typeof(IntPreference))) {
					prefsData.addPrefs(pref.getKey().ToString(), "integer");
				} else if(pref.GetType().Equals(typeof(FloatPreference))) {
					prefsData.addPrefs(pref.getKey().ToString(), "float");
				} else if(pref.GetType().Equals(typeof(BoolPreference))) {
					prefsData.addPrefs(pref.getKey().ToString(), "bool");
				}
			}

			bf.Serialize(file, prefsData);
			file.Close();
			prefsData.prefs.Clear();
		}
	}
}

[System.Serializable]
class PrefsData {
	public Dictionary<string, string> prefs = new Dictionary<string, string>();

	public void addPrefs(string key, string dataType) {
		prefs.Add(key, dataType);
	}

	public Dictionary<string, string> getPrefs() {
		return prefs;
	}

	public string getType(string key) {
		return prefs[key];
	}

	public int getSize() {
		return prefs.Count;
	}

	public void printData() {
		foreach(string key in prefs.Keys) {
			Debug.Log (key + getType(key));
		}
	}

}