using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseLevelPicker : MonoBehaviour {
	public Text levNameShow;
	private string levName = "";
	private string levNameStart;

	void Start () {
		levNameStart = levNameShow.text;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		RaycastHit rhInfo;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		if(Physics.Raycast(mouseRay, out rhInfo, 50000.0f) &&
		   rhInfo.collider.name == "MegaShipCenter") {

			levName = rhInfo.collider.transform.parent.name;
			// Debug.Log(levName);
			if(Input.GetKeyDown(KeyCode.X)) {
				PlayerPrefs.SetInt(levName,1);
			}

			if(Input.GetMouseButton(0)) {
				Application.LoadLevel(levName);
			}
			levNameShow.text = levNameStart + levName;
		} else {
			levNameShow.text = "";
		}
	}
}
