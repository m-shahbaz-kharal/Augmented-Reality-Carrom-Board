using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerScript : MonoBehaviour {
	public enum ColorPickerModeEnum {FromPicker, FromPlayerPrefs}
	public ColorPickerModeEnum ColorPickerMode;
	public GameObject ModelParent;
	void Update () {
		switch (ColorPickerMode) {
		case ColorPickerModeEnum.FromPicker:
			if (ModelParent != null) {
				Color C = GetComponent<ColorPickerTriangle> ().TheColor;
				ModelParent.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Renderer> ().material.color = C;
				PlayerPrefs.SetString ("Color", C.ToString ());
			}
			break;
		case ColorPickerModeEnum.FromPlayerPrefs:
			if (ModelParent != null) {
				string color = PlayerPrefs.GetString("Color");
				//Remove the header and brackets
				color = color.Replace("RGBA(", "");
				color = color.Replace(")", "");

				//Get the individual values (red green blue and alpha)
				string []parts = color.Split(","[0] );

				Color outputcolor = new Color ();
				outputcolor.r = System.Single.Parse (parts [0]);
				outputcolor.g = System.Single.Parse (parts [1]);
				outputcolor.b = System.Single.Parse (parts [2]);
				outputcolor.a = System.Single.Parse (parts [3]);
				//apply the color to a gameobject
				ModelParent.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Renderer> ().material.color = outputcolor;
			}
			break;
		}
	}
}
