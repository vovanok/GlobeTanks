using UnityEngine;
using Zenject;

namespace GlobeTanks.Objects
{
	public class ObjectOnEarth : MonoBehaviour
	{
		private Rigidbody rigidbody;

		[Inject]
		public void Construct()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
            Vector3 g = Vector3.ClampMagnitude(-transform.position, 0.001f) * 1000f * 9.8f;
            rigidbody.AddForce(rigidbody.mass * g, ForceMode.Acceleration);
        }
	}
}
