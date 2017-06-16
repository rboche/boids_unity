using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Vector3f
	{
		public double x;
		public double y;
		public double z;

		public Vector3f ()
		{
			x = 0;
			y = 0;
			z = 0;
		}
		public Vector3f(double x, double y, double z){
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3f (Vector3 vec){
			x = vec.x;
			y = vec.y;
			z = vec.z;
		}

		public Vector3f (Vector3f vec){
			x = vec.x;
			y = vec.y;
			z = vec.z;
		}
			
		public void normalize(){
			float norm;
			norm = (float)(1.0 / Math.Sqrt (this.x * this.x + this.y * this.y + this.z * this.z));
			this.x *= norm;
			this.y *= norm;
			this.z *= norm;
		}

		public void normaliser(){
			double zero = Math.Pow (10, -9);
			double len = length ();

			if (len * len < zero) return;
			multiply (1 / len);
		}

		public Vector3 ToVector3(){
			Vector3 vec3 = new Vector3 ();
			vec3.x = (float) this.x;
			vec3.y = (float) this.y;
			vec3.z = (float) this.z;
			return vec3;
		}

		public double point(Vector3f vec){
			return(x * vec.x + y * vec.y + z * vec.z);
		}

		public void setZero(){
			x = 0;
			y = 0;
			z = 0;
		}

		public void setXYZ(Vector3f vec){
			x = vec.x;
			y = vec.z;
			z = vec.z;
		}

		public void multiply(double nbr){
			x *= nbr;
			y *= nbr;
			z *= nbr;
		}

		public void plus(Vector3f vec){
			x += vec.x;
			y += vec.y;
			z += vec.z;
		}

		public void minus(Vector3f vec){
			x -= vec.x;
			y -= vec.y;
			z -= vec.z;
		}

		public double length(){
			return (Math.Sqrt (this.x * this.x + this.y * this.y + this.z * this.z));
		}

		public double signedAngle(Vector3f b){
			this.y = this.x;
			this.z = 0;
			double fCrossX, fCrossY, fCrossZ, fCross, fDot;
			fCrossX = this.y * b.z - this.z * b.y;
			fCrossY = this.z * b.x - this.x * b.z;
			fCrossZ = this.x * b.y - this.y * b.x;
			fCross = (float)Math.Sqrt ((fCrossX * fCrossX) + (fCrossY * fCrossY) + (fCrossZ * fCrossZ));
			fDot = this.x * b.x + this.y * b.y + this.z * b.z;

			double xa = Math.Atan2 (fCross, fDot);
			double xb = (xa * 180) / (Math.PI);
			return xb;
		}
	}
}

