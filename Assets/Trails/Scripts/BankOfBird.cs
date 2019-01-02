using UnityEngine;
using System.Collections;

public class BankOfBird : MonoBehaviour
{

	[SerializeField] Transform parent;
	float parentYaxis;
	float go_Zaxis;


	void Update (){
		
		parentYaxis = parent.transform.rotation.eulerAngles.y;
		go_Zaxis = transform.rotation.eulerAngles.z;

		if ((parentYaxis < 360) && (parentYaxis > 180)) {
			
			if ((go_Zaxis < 180) && ((int)go_Zaxis > 35)) {
				return;
			}
				transform.Rotate (0, 0, Time.deltaTime * 8);
			
		} else if ((parentYaxis >= 0) && (parentYaxis < 180)) {
			
			if ((go_Zaxis > 180) && ((int)go_Zaxis < 315)) {
				return;
			}
				transform.Rotate (0, 0, Time.deltaTime * -8);
			
		}

	}

}