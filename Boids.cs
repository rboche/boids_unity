using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Boids : MonoBehaviour {

	public int height;

	public int width;

	Vector3f speed = new Vector3f(0,0.2,0);

	public Transform boidPrefab;

	private SortedDictionary<int,PerceivedBoidBody> boids;

	public Message receive;

	// Use this for initialization
	void Start () {
		Transform parentObj = GameObject.Find ("environment").transform;
		boids = new SortedDictionary<int,PerceivedBoidBody>();
		boids.Clear();
		for (int i = 1; i<5;++i) {
			speed.normaliser ();
			speed.multiply (1);
			speed.plus (new Vector3f (0,0.025, 0));
			speed.multiply (this.GetComponent<BoidsSettings> ().maxSpeed);
			Vector3 speed3 = speed.ToVector3 ();
			Vector3 position = new Vector3 (Random.Range(-2,2), Random.Range(-1,1),Random.Range(-2,2));
			//Random.Range(-3,3), Random.Range(-2,2),Random.Range(-3,3)
			Transform boidBody = Instantiate (boidPrefab);
			boidBody.localPosition = position;
			boidBody.name = "BoidBody." + i;
			boidBody.tag = "boid";
			//boidBody.parent = parentObj;
			Vector3f pos = new Vector3f (boidBody.localPosition);
			boidBody.gameObject.AddComponent<BoidAgent> ();
			boidBody.GetComponent<Rigidbody> ().useGravity = false;
			boidBody.gameObject.AddComponent<PerceivedBoidBody> ();
			boidBody.GetComponent<PerceivedBoidBody> ().Position = pos;
			boidBody.GetComponent<PerceivedBoidBody> ().ID = i;
			boidBody.GetComponent<PerceivedBoidBody> ().Speed = speed;
			boidBody.GetComponent<PerceivedBoidBody> ().Acceleration = new Vector3f ();
			boidBody.GetComponent<PerceivedBoidBody> ().bodyObject = boidBody.gameObject;
			boidBody.GetComponent<BoidAgent>().initBoid (width, height, pos, speed, boidBody.GetComponent<PerceivedBoidBody>(), i);
			Debug.Log ("Agent " + i + " created");
			Debug.Log ("id " + i);
			boids.Add(i,boidBody.GetComponent<PerceivedBoidBody>());
			Debug.Log ("count " + boids.Count);
		}
	}

	// Update is called once per frame
	void Update (){
		
		
	}

	public void applyChanges(Message mess){
		Debug.Log ("force reçu");
		Vector3f force = mess.force;
		int senderId = mess.sender;
		Debug.Log ("values " + boids.Values);
		Debug.Log ("senderId " + senderId);
		PerceivedBoidBody b = boids[senderId];
		if (force.length () > this.GetComponent<BoidsSettings> ().maxForce) {
			force.normaliser ();
			force.multiply(GetComponent<BoidsSettings>().maxForce);
		}

		Debug.Log ("b accel " + b.Acceleration);
		Vector3f acceleration = b.Acceleration;
		acceleration.setXYZ(force);
		Vector3f vitesse = b.Speed;
		vitesse.plus(acceleration);

		if(vitesse.length() > GetComponent<BoidsSettings>().maxSpeed){
			vitesse.normaliser();
			vitesse.multiply(GetComponent<BoidsSettings>().maxSpeed);
		}

		Vector3f position = b.Position;
		position.plus(vitesse);
		b.Acceleration = acceleration;
		b.Speed = vitesse;
		b.Position = position;

		Debug.Log ("Application du déplacement au body " + b.ID);

		//b.bodyObject.transform.Rotate (vitesse.ToVector3 ());
	}
}
