using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class PerceivedBoidBody : MonoBehaviour
	{
		private Vector3f position;

		public GameObject bodyObject;

		public Vector3f Position {
			get {
				return position;
			}
			set{
				this.position = value;
			}
		}

		public PerceivedBoidBody(Vector3f iposition,Vector3f ispeed,int iid){
			this.speed = ispeed;
			this.position = iposition;
			this.acceleration = new Vector3f ();
			this.id = iid;
		}

		private int id;
		public int ID{
			get{
				return id;
			}
			set {
				this.id = value;
			}
		}

		private Vector3f speed;
		public Vector3f Speed 
		{
			get{
				return speed; 
			} 
			set{
				this.speed = value; 
			}
		}
		private Vector3f acceleration;
		public Vector3f Acceleration{
			get{
				return acceleration; 
			}  
			set{
				this.acceleration = value; 
			}
		}
	}
}

