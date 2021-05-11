using UnityEngine;
using UniRx;
using Zenject;

namespace GlobeTanks.Common
{
	public class SphereTerrain : MonoBehaviour
	{
		[SerializeField] private float notDestroyableRadius = 0.1f;
		[SerializeField] private float terrainAmplitude = 0.1f;
		[SerializeField] private float samplesScaleFactor = 1f;

		public ReactiveCommand OnTransformate { get; } = new ReactiveCommand();

		private MeshCollider meshCollider;
		private MeshAdapter mesh;

		[Inject]
		public void Construct()
		{
			mesh = new MeshAdapter(GetComponent<MeshFilter>().mesh);

			foreach (MeshAdapter.MeshPoint point in mesh.Points)
			{
                PolarVector3 pointPolarPos = new PolarVector3(point.Position);
				float deltaCoords = terrainAmplitude * Mathf.PerlinNoise(pointPolarPos.Teta * samplesScaleFactor, pointPolarPos.Fi * samplesScaleFactor) + 1;
				point.Position *= deltaCoords;
			}

			mesh.Flush();

			RegenerateCollider();
			OnTransformate.Execute();
		}

		public void SubstractSphere(Vector3 position, float radius)
		{
            GameObject boomGhost = new GameObject("BoomGhost");
			boomGhost.transform.position = position;
            SphereCollider boomGhostCoolider = boomGhost.AddComponent<SphereCollider>();
			boomGhostCoolider.radius = radius;

			RaycastHit hit;
			foreach (var terrainPoint in mesh.Points)
			{
				var rayFromCore = new Ray(transform.position, terrainPoint.Position - transform.position);
				var distanceToCore = Vector3.Distance(terrainPoint.Position, transform.position);

				if (boomGhostCoolider.Raycast(rayFromCore, out hit, distanceToCore))
				{
					terrainPoint.Position = Vector3.Distance(hit.point, transform.position) <= notDestroyableRadius
						? Vector3.ClampMagnitude(terrainPoint.Position, notDestroyableRadius)
						: hit.point;
				}
			}

			Destroy(boomGhost);

			mesh.Flush();
			RegenerateCollider();
			OnTransformate.Execute();
		}

		public Vector3 GetPointOnSurface(Vector3 positionDir)
		{
			var rayToCore = new Ray(positionDir, transform.position - positionDir);
			var distanceToCore = Vector3.Distance(positionDir, transform.position);

			if (meshCollider.Raycast(rayToCore, out RaycastHit hit, distanceToCore))
				return hit.point;

			return transform.position;
		}

		public float DistanceToSurface(Vector3 point)
		{
			return Vector3.Distance(point, GetPointOnSurface(point));
		}

		public bool Linecast(Vector3 start, Vector3 end, out Vector3 hitPosition)
		{
			hitPosition = Vector3.zero;
			RaycastHit hitInfo;
			if (Physics.Linecast(start, end, out hitInfo) && hitInfo.collider == meshCollider)
			{
				hitPosition = hitInfo.point;
				return true;
			}

			return false;
		}

		private void RegenerateCollider()
		{
			if (meshCollider != null)
			{
				Destroy(meshCollider);
			}

			meshCollider = gameObject.AddComponent<MeshCollider>();
		}
	}
}
