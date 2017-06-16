using UnityEngine;
using System.Collections;
namespace AssemblyCSharp
{
	public class Message 
	{

		public Vector3f force;
		public int sender;

		public Message(Vector3f force,int sender){
			this.force = force;
			this.sender = sender;
		}
	}
}