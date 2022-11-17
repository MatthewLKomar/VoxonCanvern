using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxonTextController : MonoBehaviour
{

	Voxon.VXTextComponent text;
	string number = "";						// The current input by the guest.
	[SerializeField] string password = "ETCOS";                  // The true password.
	private char[] charList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

	void Start()
	{
		text = GetComponent<Voxon.VXTextComponent>();
	}


	void Update()
	{
        if (MapController.instance.hackStatus != 0)
        {
			return;
        }

		if(Voxon.Input.GetKeyDown("Enter") || number.Length > 3)
        {
			if(number == password)
            {
				text.text = "";					// Password matched.
				GameEvents.instance.EveSetActiveCube(true);
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
                if (Voxon.Input.GetKeyDown(charK.ToString()))		// If there's an input match.
                {
					if (text.text[0] != 'P')
					{
						text.text = "Password: " + charK.ToString();

					}
					else
					{
						text.text += charK.ToString();                  // Add it to the number.
					}
					number += charK.ToString();
				}
            }
        }

	}
}
