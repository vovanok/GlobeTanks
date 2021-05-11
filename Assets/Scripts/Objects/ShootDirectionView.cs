using UnityEngine;

namespace GlobeTanks.Objects
{
    public class ShootDirectionView : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;

        private const float VECTOR_MAGNITUDE = 0.5f;

        public void Refresh(Vector3 startPoint, Vector3 direction)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, startPoint + direction.normalized * VECTOR_MAGNITUDE);
        }
    }
}
