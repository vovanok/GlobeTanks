using UnityEngine;
using Zenject;

namespace GlobeTanks.Objects
{
	public class ProjectileTracerView : MonoBehaviour
	{
		[SerializeField] private float MinDistanceForDrawTrace = 0.03f;

		private Vector3 lastPoint;
		private int pointsCount;
		private LineRenderer lineRenderer;

		[Inject]
		public void Construct()
        {
			lineRenderer = GetComponent<LineRenderer>();
		}

		public void AddPoint(Vector3 pointPosition)
		{
			if (pointsCount > 0 && Vector3.Distance(lastPoint, pointPosition) < MinDistanceForDrawTrace)
				return;

			pointsCount++;

			lineRenderer.positionCount = pointsCount;
			lineRenderer.SetPosition(pointsCount - 1, pointPosition);
			lastPoint = pointPosition;
		}

		public void Clear()
		{
			lineRenderer.positionCount = 0;
			pointsCount = 0;
		}
	}
}
