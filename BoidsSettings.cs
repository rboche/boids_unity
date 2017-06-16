using UnityEngine;
using System.Collections;

public class BoidsSettings : MonoBehaviour
{

	public float cohesionDistance;
	public float separationDistance;
	public float alignementDistance;
	public float repulsionDistance;

	public float maxForce;

	public double wallDistance;
	public float wallRepulsion;

	public bool cohesionOn;
	public bool separationOn;
	public bool alignementOn;
	public bool repulsionOn;

	public float cohesionForce;
	public float separationForce;
	public float alignementForce;
	public float repulsionForce;

	public double visibleAngleCos;

	public double maxSpeed;

	public float getCohesionDistance(){
		return cohesionDistance;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

