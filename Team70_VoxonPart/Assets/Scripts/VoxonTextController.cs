using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxonTextController : MonoBehaviour
{
	private bool isHacked = false;

	Voxon.VXTextComponent text;
	string number = "";						// The current input by the guest.
	string password = "1234";					// The true password.

	void Start()
	{
		text = GetComponent<Voxon.VXTextComponent>();
	}


	void Update()
	{
        if (isHacked || !text.forceUpdatePerFrame)
        {
			return;
        }

		if(Voxon.Input.GetKeyDown("Enter") || number.Length > 3)
        {
			if(number == password)
            {
				text.text = "Hacked!";					// Password matched.
				isHacked = true;
				GameEvents.instance.EveSetActiveCube();
			}
            else
            {
				text.text = "Password: ";			// Not matched.
				number = "";
			}
        }
        else
        {
			for(int i = 0; i < 10; i++)
            {
                if (Voxon.Input.GetKeyDown(i.ToString()))		// If there's an input match.
                {
					text.text += i.ToString();					// Add it to the number.
					number += i.ToString();
                }
            }
        }

	}
}
