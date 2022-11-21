using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxonTextController : MonoBehaviour
{

	Voxon.VXTextComponent text;
	string number = "";						// The current input by the guest.
	[SerializeField] string password = "ETCOS";                  // The true password.
	private char[] charList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();		// An array of available input char.

	void Start()
	{
		text = GetComponent<Voxon.VXTextComponent>();
	}


	void Update()
	{
        if (MapController.instance.hackStatus != 0)				// If not started.
        {
			return;
        }

		if(Voxon.Input.GetKeyDown("Enter") || number.Length > 3)		// If reach limit or is entered.
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
                if (Voxon.Input.GetKeyDown(charK.ToString()))		// If there's a char in the array matches to the user's input.
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
}
