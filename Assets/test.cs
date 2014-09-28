using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	LoadPrefs loadPrefs;

	void Awake() {
		loadPrefs = GetComponent<LoadPrefs>();
		Debug.Log (loadPrefs.getString().ToString());
	}

}
