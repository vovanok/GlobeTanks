using UnityEngine;

namespace GlobeTanks.Common
{
	public class PolarVector3
	{
		public float R { get; set; }

		public float Teta { get; set; }

		public float Fi { get; set; }

		public Vector3 DecardVector
		{
			get
			{
				return new Vector3(
					R * Mathf.Sin(Teta) * Mathf.Cos(Fi),
					R * Mathf.Sin(Teta) * Mathf.Sin(Fi),
					R * Mathf.Cos(Teta));
			}
		}

		public PolarVector3()
		{
			R = 0f;
			Teta = 0f;
			Fi = 0f;
		}

		public PolarVector3(Vector3 decardVector3)
		{
			R = decardVector3.magnitude;
			Teta = Mathf.Acos(decardVector3.z / R);
			Fi = decardVector3.x != 0f ? Mathf.Atan(decardVector3.y / decardVector3.x) : 0f;
		}
	}
}