using UnityEngine;
using UnityEditor;
using System.Collections;

public class LoadPrefs : MonoBehaviour {

	float floatVal = 0f;
	int intVal = 0;
	string stringVal = "";
	bool boolVal = false;

	public void setInteger(int value) {
		this.intVal = value;
	}
	public int getInteger() {
		return intVal;
	}

	public void setFloat(float value) {
		this.floatVal = floatVal;
	}
	public float getFloat() {
		return floatVal;
	}

	public void setString(string value) {
		this.stringVal = value;
		Debug.Log (value);
	}
	public string getString() {
		return stringVal;
	}

	public void setBool(bool value) {
		this.boolVal = value;
	}
	public bool getBool() {
		return boolVal;
	}

}
