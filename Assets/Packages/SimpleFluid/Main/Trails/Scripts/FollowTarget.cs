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

        if (gameObject.name == "Trail(Clone)")
        {
            speed = Random.Range(5f, 30.0f);
            maxDeltaRotate = Random.Range(2f, 100f);

            var r = Random.Range(0f, 1f);
            var g = Random.Range(0f, 1f);
            var b = Random.Range(0f, 1f);
            var col = new Color(r, g, b, 1f);

            var trail = gameObject.GetComponent<TrailRenderer>();
            var mat = trail.material;
            trail.widthCurve = new AnimationCurve(new Keyframe(0f, r), new Keyframe(0.3f, g * 5f), new Keyframe(1f, b * 2f));
            mat.SetColor("_TintColor", col);
        }
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