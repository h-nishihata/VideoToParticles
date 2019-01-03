using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
	Transform target;
	public float speed;
	public float maxDeltaRotate;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;

        if (gameObject.name == "Flying Object") return;

        speed = Random.Range(1.6f, 10.0f);
        maxDeltaRotate = Random.Range(2f, 100f);

        var col = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        var mat = gameObject.GetComponent<TrailRenderer>().material;
        mat.SetColor("_TintColor", col);
    }

    void Update ()
    {		
		Vector3 to = target.transform.position - transform.position;
		Quaternion toRot = Quaternion.LookRotation (to, Vector3.up);

		transform.rotation = Quaternion.RotateTowards (transform.rotation, toRot, maxDeltaRotate * Time.deltaTime);
		//if ((Mathf.Abs(to.x) > 0.5f) && (Mathf.Abs(to.y) > 0.5f) && (Mathf.Abs(to.z) > 0.5f)) {
		transform.position += transform.forward * speed * Time.deltaTime;
		//}
	}
}