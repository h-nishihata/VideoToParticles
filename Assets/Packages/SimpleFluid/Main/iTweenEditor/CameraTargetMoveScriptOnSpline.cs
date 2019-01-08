
	using UnityEngine;
	using System.Collections;
	
	public class CameraTargetMoveScriptOnSpline : MonoBehaviour {
		
		
		public int time=100;
		public string PathName="CameraTarget";
		
		void Start () {
			iTween.MoveTo (this.gameObject, iTween.Hash ("path", iTweenPath.GetPath (PathName), "time", time));
		}
		
		
	}

