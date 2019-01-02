using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
	Transform target;
	public float speed = 1.0f;
	public float maxDeltaRotate = 100f;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update (){		
		Vector3 to = target.transform.position - transform.position;
		Quaternion toRot = Quaternion.LookRotation (to, Vector3.up);

		transform.rotation = Quaternion.RotateTowards (transform.rotation, toRot, maxDeltaRotate * Time.deltaTime);
		//if ((Mathf.Abs(to.x) > 0.5f) && (Mathf.Abs(to.y) > 0.5f) && (Mathf.Abs(to.z) > 0.5f)) {
		transform.position += transform.forward * speed * Time.deltaTime;
		//}
	}
}