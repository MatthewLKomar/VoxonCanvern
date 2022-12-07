using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoxonTextController : MonoBehaviour
{
	public static VoxonTextController instance { private set; get; }

	[SerializeField] TextMeshProUGUI text;
	string number = "";						// The current input by the guest.
	[SerializeField] string password = "ETCOS";                  // The true password.
	private char[] charList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();       // An array of available input char.

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}


	void Start()
	{
	}


	void Update()
	{
        if (MapController.instance.hackStatus != 0)				// If not started.
        {
			return;
        }

		if (Input.GetKeyDown(KeyCode.Return) || number.Length > 3)		// If reach limit or is entered.
        {
			if(number == password)
            {
				text.text = "";					// Password matched.
				MapController.instance.SetActiveCube(true);
			}
            else
            {
				text.text = "Wrong Password";		// Not matched.
				number = "";
				AudioManager.instance.PlayActivateSound(1);
			}
        }
        else
        {
			foreach(char charK in charList)
            {
				KeyCode keycodeK = (KeyCode)System.Enum.Parse(typeof(KeyCode), charK.ToString());
                if (Input.GetKeyDown(keycodeK))		// If there's a char in the array matches to the user's input.
                {
					if (text.text[0] != 'P')							// If the current text is not Password: xxx
					{
						text.text = "Password: " + charK.ToString();

					}
					else
					{
						text.text += charK.ToString();                  // Add it to the button of the text.
					}
					number += charK.ToString();
				}
            }
        }

	}


	public void SetText(string newText) {
		text.text = newText;
	}
}
