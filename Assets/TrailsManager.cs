using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailsManager : MonoBehaviour {
    
    public int numInstances;
    public GameObject trails;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numInstances; i++)
        {
            var trail = Instantiate(trails, trails.transform.position + Random.insideUnitSphere * 10, Random.rotation);
            trail.transform.parent = gameObject.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
