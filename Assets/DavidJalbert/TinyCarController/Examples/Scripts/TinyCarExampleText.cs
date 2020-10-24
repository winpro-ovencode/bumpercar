using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DavidJalbert
{
	public class TinyCarExampleText : MonoBehaviour
	{
		private TinyCarController carController;
		private TinyCarCamera carCamera;
		private Text textDebug;

		void Start()
		{
			carController = FindObjectOfType<TinyCarController>();
			carCamera = FindObjectOfType<TinyCarCamera>();
			textDebug = GetComponent<Text>();
		}

		void Update()
		{
			if (textDebug == null) return;

			textDebug.text = "Tiny Car Controller Demo v1.4.0\n\nUse the arrow keys or WASD keys to control the car.\n\nPress C to toggle camera modes.\n\n";

			if (carController != null)
			{
				textDebug.text += "Speed : " + (int)carController.getForwardVelocity() + " m/s\n";
				textDebug.text += "Drift speed : " + (int)carController.getLateralVelocity() + " m/s\n";
				textDebug.text += "Is grounded : " + carController.isGrounded() + "\n";
				textDebug.text += "Ground type : " + carController.getSurfaceParameters().getName() + "\n";
				textDebug.text += "Is braking : " + carController.isBraking() + "\n";
			}

			if (carCamera != null)
			{
				if (Input.GetKeyDown(KeyCode.C))
				{
					switch (carCamera.viewMode)
					{
						case TinyCarCamera.CAMERA_MODE.TopDown:
							carCamera.viewMode = TinyCarCamera.CAMERA_MODE.ThirdPerson;
							break;
						case TinyCarCamera.CAMERA_MODE.ThirdPerson:
							carCamera.viewMode = TinyCarCamera.CAMERA_MODE.TopDown;
							break;
					}
					
				}
			}
		}
	}
}