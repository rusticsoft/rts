﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnits : MonoBehaviour
{
	// public Material selectedMaterial;
	public List<GameObject> selectedUnits;

	private Vector3 lastClickPoint;
	private GUIStyle selectBoxStyle;
	private Texture2D selectBoxTexture;
	private Color selectBoxColor;
	
	// Use this for initialization
	void Start ()
	{
		selectBoxStyle = new GUIStyle();
		selectBoxTexture = new Texture2D(1, 1);
		selectBoxColor = new Color(0, 1.0f, 0, 0.3f);
		selectBoxTexture.SetPixel(0, 0, selectBoxColor);
		selectBoxTexture.Apply();
		selectBoxStyle.normal.background = selectBoxTexture;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo = new RaycastHit ();

		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hitInfo) && hitInfo.collider.tag == "Unit") {
				GameObject obj = hitInfo.collider.gameObject;

				if (!selectedUnits.Contains (obj)) {
					if (!Input.GetKey (KeyCode.LeftShift)) {
						ClearSelectedUnits ();
					}
					selectedUnits.Add(obj);
					obj.renderer.material.color = Color.green;
				}
			} else {
				ClearSelectedUnits ();
			}

			lastClickPoint = Input.mousePosition;
		}

		
		if (Input.GetMouseButtonDown (1) && selectedUnits.Count > 0) {
			if (Physics.Raycast (ray, out hitInfo)) {
				Mover seeker;
				if (hitInfo.collider.tag == "Unit") {
					GameObject target = hitInfo.collider.gameObject;
					selectedUnits.ForEach (delegate(GameObject o) {
						seeker = (Mover)o.GetComponent<Mover> ();
						seeker.follow (target);
					});
				} else {
					selectedUnits.ForEach (delegate(GameObject o) {
						seeker = (Mover)o.GetComponent<Mover> ();
						seeker.moveTo (hitInfo.point);
					});
				}
			}
		}
	}


	void OnGUI () {
		if (Input.GetMouseButton(0)) {
			Vector2 selectBoxPos = new Vector2(lastClickPoint.x, Screen.height - lastClickPoint.y);
			Vector2 selectBoxSize = new Vector2(
				Input.mousePosition.x - selectBoxPos.x,
				Screen.height - Input.mousePosition.y - selectBoxPos.y);
			Rect selectBox = new Rect(selectBoxPos.x, selectBoxPos.y, selectBoxSize.x, selectBoxSize.y);
			GUI.Label(selectBox, GUIContent.none, selectBoxStyle);

			GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
			foreach (GameObject obj in allUnits) {
				Vector2 pos = Camera.main.WorldToScreenPoint(obj.transform.position);
				if (selectBox.Contains(pos)) {
					selectedUnits.Add(obj);
				}
			}
		}
	}

	void ClearSelectedUnits ()
	{
		selectedUnits.ForEach (delegate(GameObject obj) {
			obj.renderer.material.color = Color.white;
		});
		selectedUnits.Clear ();
	}
}