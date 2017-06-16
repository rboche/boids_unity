using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace AssemblyCSharp
{
	public class BoidAgent : MonoBehaviour
	{
		private int envHeight{ get; set; }
		private int envWidth{ get; set; }

		private int id;

		private GameObject environment;

		private PerceivedBoidBody myBody;

		public Vector3f position;

		private Vector3 inflFinal3;

		private LinkedList<GameObject> walls = new LinkedList<GameObject> ();

		Vector3f acceleration;
		Vector3f force;

		public Vector3f speed;
		public Vector3f Speed{
			get{
				return speed;
			}
			set{
				this.speed = value;
			}
		}

		void Start(){
			Debug.Log ("Agent Start");
			GameObject[] wallObj = GameObject.FindGameObjectsWithTag ("room");
			foreach (GameObject w in wallObj) {
				walls.AddLast (w);
			}
		}

		void Update(){
//			
			Vector3f forceOld = force;
			force = think (perceive ());

//			if (force.length () > environment.GetComponent<BoidsSettings>().maxForce) {
//				force.normaliser ();
//				force.multiply(environment.GetComponent<BoidsSettings>().maxForce);
//			}
//			if (force.x != 0 || force.y != 0 || force.z != 0) {
//				
//				Vector3f acceleration = myBody.Acceleration;
//				acceleration.setXYZ (force);
//				Vector3f vitesse = myBody.Speed;
//				vitesse.plus (acceleration);
//
//				if (vitesse.length () > environment.GetComponent<BoidsSettings> ().maxSpeed) {
//					vitesse.normaliser ();
//					vitesse.multiply (environment.GetComponent<BoidsSettings> ().maxSpeed);
//				}
//
//				Vector3f position = myBody.Position;
//				//Debug.Log ("Vitesse boid " + id + " : " + vitesse.x + " " + vitesse.y + " " + vitesse.z);
//				position.plus (vitesse);
//				this.speed = vitesse;
//				this.position = position;
//				myBody.Acceleration = acceleration;
//				myBody.Speed = vitesse;
//				myBody.Position = position;
//				Debug.Log ("vitesse " + vitesse.x + " " + vitesse.y + " " + vitesse.z);
//				Debug.Log ("vitesse " + position.x + " " + position.y + " " + position.z);
//				myBody.GetComponent<Rigidbody> ().velocity = vitesse.ToVector3 ();
//				myBody.GetComponent<Rigidbody> ().position = position.ToVector3 ();
//				myBody.GetComponent<Rigidbody> ().detectCollisions = true;
//				Debug.Log ("pos agent " + id + " : " + this.position.x + "," + this.position.y +","+ this.position.z);
//
//				//			Vector3f acceleration = myBody.Acceleration;
//				//			acceleration.setXYZ(force);
//				//			Vector3f vitesse = myBody.Speed;
//				//			vitesse.plus(acceleration);
//				//
//				//			if(vitesse.length() > GetComponent<BoidsSettings>().maxSpeed){
//				//				vitesse.normaliser();
//				//				vitesse.multiply(GetComponent<BoidsSettings>().maxSpeed);
//				//			}
//
//				inflFinal3 = force.ToVector3 ();
//				Debug.Log ("force " + force.x + " " + force.y + " " + force.z);
//				//Debug.Log ("influence boid "+ id + " : "+inflFinal3);
//				//gameObject.GetComponent<Rigidbody>().AddRelativeForce(inflFinal3);
//				gameObject.transform.localPosition = position.ToVector3();

//			}
			//Debug.Log ("Position body " + id + " : " + this.position.x + " " + this.position.y + " " + this.position.z);
			if(force == forceOld){
				Debug.Log ("CEST LA MEME PUTAIN DE FORCE A CHAQUE FOIS !!!!!!!!");
			}
			Debug.Log ("force appliqué : " + force.x + " " + force.y + " " + force.z);
			Vector3 force3 = force.ToVector3 ();
			Vector3 speed3 = speed.ToVector3 ();
			gameObject.GetComponent<Rigidbody> ().AddForce (force3* Time.deltaTime * 8);
			Vector3 velocity = new Vector3 (force3.x * speed3.x, force3.y * speed3.y, force3.z * speed3.z);
			gameObject.GetComponent<Rigidbody> ().velocity = velocity;
			this.position = new Vector3f (gameObject.transform.position);
			this.myBody.Position = this.position;
			this.speed = new Vector3f (gameObject.GetComponent<Rigidbody> ().velocity);
			this.myBody.Speed = speed;
		}

		public void initBoid(int p_envWidth, int p_envHeight,Vector3f initialPos, Vector3f initialSpeed,PerceivedBoidBody boidBody,int i_id){
			this.envWidth = p_envWidth;
			this.envHeight = p_envHeight;
			this.speed = initialSpeed;
			this.position = initialPos;
			this.myBody = boidBody;
			this.id = i_id;
			this.environment = GameObject.FindGameObjectWithTag ("environment");
			Debug.Log ("Agent Initialize" + position.x +" " +position.y+" "+position.z);
			Debug.Log ("speed " + speed.x + " " + speed.y + " " + speed.z);
		}

		private LinkedList<PerceivedBoidBody> perceive(){
			LinkedList<PerceivedBoidBody> perceivables = new LinkedList<PerceivedBoidBody> ();
			GameObject[] perceived = GameObject.FindGameObjectsWithTag ("boid");
			foreach(GameObject obj in perceived){
				perceivables.AddLast (obj.GetComponent<PerceivedBoidBody>());
			}
			//Debug.Log ("Perceived :" + perceivables.Count);
			return perceivables;
		}

		private Vector3f think(LinkedList<PerceivedBoidBody> perception){
			if (perception != null) {
				Vector3f force = new Vector3f ();
				Vector3f influence= new Vector3f();
				influence.setZero ();

				foreach (var e in perception) {
					if (e.ID == this.id) {
						//Debug.Log ("Update agent position");
						this.position = e.Position;
						this.speed = e.Speed;
					}
				}

				if (environment.GetComponent<BoidsSettings>().alignementOn) {
					force = (alignement (perception));
					force.multiply (environment.GetComponent<BoidsSettings>().alignementForce);
					influence.plus (force);
				}
				if (environment.GetComponent<BoidsSettings>().cohesionOn) {
					force = (cohesion (perception));
					force.multiply (environment.GetComponent<BoidsSettings>().cohesionForce);
					influence.plus (force);

				}
				if (environment.GetComponent<BoidsSettings>().repulsionOn) {
					force = (repulsion (perception));
					force.multiply (environment.GetComponent<BoidsSettings>().repulsionForce);
					influence.plus (force);
				}
				if (environment.GetComponent<BoidsSettings>().separationOn) {
					force = (separation (perception));
					force.multiply (environment.GetComponent<BoidsSettings>().separationForce);
					influence.plus (force);
				}

				force = (repulsionWall (walls));
				force.multiply (environment.GetComponent<BoidsSettings> ().wallRepulsion);
				influence.plus (force);

				if (influence.length () > environment.GetComponent<BoidsSettings>().maxForce) {
					influence.normalize ();
					influence.multiply (environment.GetComponent<BoidsSettings>().maxForce);
				}
				influence.plus (force);
				return influence;
			}
			return null;
		}


		private Boolean near(PerceivedBoidBody otherBoid,double distance){
			Vector3f tmp = new Vector3f (otherBoid.Position);
			tmp.minus (position);
			double len;
			len = tmp.length();
			if (len> distance) {
				return false;
			}
			return true;
		}

		private Boolean visible(PerceivedBoidBody otherBoid,double distance){
			Vector3f tmp = new Vector3f(otherBoid.Position);
			Debug.Log ("position this : " + position.x + " " + position.y + " " + position.z);
			tmp.minus (position);
			Debug.Log ("otherBoid position : " + otherBoid.Position.x + " "+ otherBoid.Position.y + " "+otherBoid.Position.z);

			double length = tmp.length();
			Debug.Log ("distance " + distance + ", length " + length);
			if (length > distance) {
				return false;
				//Debug.Log ("false");
			}
			//Debug.Log ("speed " +otherBoid.Speed.x + " " + otherBoid.Speed.y + " " + otherBoid.Speed.z);
			Vector3f tmp2 = new Vector3f (otherBoid.Speed);
			tmp2.normaliser();
			if (tmp2.point(tmp) < environment.GetComponent<BoidsSettings>().visibleAngleCos) {
				return false;
			}

			Debug.Log ("true");
			return true;
		}

		// FORCES CALCUL //

		//Séparation
		int po = 0;

		private Vector3f separation(LinkedList<PerceivedBoidBody> otherBoids){
			
			Vector3f tmp = new Vector3f();
			Vector3f force = new Vector3f();
			force.setZero ();
			tmp.setZero ();
			double len = 0;
			Debug.Log ("others boids count : " + otherBoids.Count);
			foreach(PerceivedBoidBody e in otherBoids){
				if(e!= null  && visible(e,environment.GetComponent<BoidsSettings>().separationDistance) && e.ID != this.id){
					tmp.setXYZ (position);
					tmp.minus(e.Position);
					len = tmp.length();
					tmp.multiply (1 / len);
					force.plus (tmp);
					Debug.Log ("Ca calcul");
					po++;
					Debug.Log (po++);
				}
			}
			//Debug.Log ("Force separation :" + force.x +"," +force.y+","+force.z);
			return force;
		}

		//Cohésion

		private Vector3f cohesion(LinkedList<PerceivedBoidBody> otherBoids){
			Vector3f force = new Vector3f ();
			force.setZero ();
			int i = 0;
			foreach (var e in otherBoids) {
				if (e != null && e.ID != this.id && visible (e, environment.GetComponent<BoidsSettings>().cohesionDistance)) {
					force.plus (e.Position);
					i++;
					Debug.Log ("Calcul cohesion");
				}
			}
			if (i != 0) {
				force.multiply (1 / i);
				force.minus(position);
			}
			//Debug.Log ("Force Cohésion :" + force.x +"," +force.y+","+force.z +" boids " + id);
			return force;
		}

		//Alignement

		private Vector3f alignement(LinkedList<PerceivedBoidBody> otherboids){
			Vector3f force= new Vector3f ();
			force.setZero ();
			int i = 0;
			foreach (var e in otherboids) {
				if (e != null && e.ID != this.id && visible (e, environment.GetComponent<BoidsSettings>().alignementDistance)) {
					Vector3f ispeed = e.Speed;
					ispeed.multiply (1 / ispeed.length());
					force.plus (ispeed);
					i++;
				}
			}
			if (i != 0) {
				force.multiply(1 / i);
			}
			//Debug.Log ("Force alignement :" + force.x +"," +force.y+","+force.z+" boids " + id);
			return force;
		}

		//répulsion

		private Vector3f repulsion(LinkedList<PerceivedBoidBody> otherBoids){
			Vector3f force= new Vector3f ();
			force.setZero ();
			Vector3f tmp= new Vector3f ();
			tmp.setZero ();
			double len = 0;

			foreach (var e in otherBoids) {
				//Debug.Log ("Id perceived " +e.ID +" my id :" + id);
				if (e != null && near (e, environment.GetComponent<BoidsSettings>().repulsionDistance) && e.ID != this.id) {
					tmp.setXYZ (position);
					tmp.minus(e.Position);
					len = tmp.length();
					tmp.multiply(1/(len*len));
					force.plus(tmp);
				}
			}
			//Debug.Log ("Force repulsion :" + force.x +"," +force.y+","+force.z);
			return force;
		}

		private Vector3f repulsionWall(LinkedList<GameObject> walls){
			Vector3f force = new Vector3f ();
			force.setZero ();
			Vector3f tmp = new Vector3f ();
			tmp.setZero ();
			double len = 0;
			foreach(GameObject w in walls) {
				if(wallIsNear(w,environment.GetComponent<BoidsSettings>().wallDistance)){
					tmp.setXYZ (this.position);
					Vector3f wallPos = new Vector3f (w.transform.position);
					tmp.minus (wallPos);
					len = tmp.length ();
					tmp.multiply (1 / (len * len));
					force.plus (tmp);
					//Debug.Log ("Calcul wall répulsion");
				}
			}
			//Debug.Log ("force sortie repuls " + force.x + " " + force.y+" " + force.z);
			return force;
		}

		private Boolean wallIsNear(GameObject wall, double distance){
			Vector3f tmp = new Vector3f (wall.transform.localPosition);
			//Debug.Log ("tmp  wall body "+ id + ": " + tmp.x + " " + tmp.y + " " + tmp.z);
			tmp.minus (position);
			//Debug.Log ("tmp minus body "+ id + ": " + tmp.x + " " + tmp.y + " " + tmp.z);
			double len = 0;
			len = tmp.length();
			//Debug.Log ("len body " + id +" " + len);
			if (len > distance) {
				//Debug.Log ("false");
				return false;
			}
			//Debug.Log ("true");
			return true;
		}
	
	}
}

