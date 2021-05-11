using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GlobeTanks.Common
{
	public class MeshAdapter
	{
		public class MeshPoint
		{
			private Vector3 position;
			public Vector3 Position
			{
				get => position;
				set
				{
					position = value;
					OwnerMesh.SetVerticesPosition(Vertices, value);
				}
			}

			public List<int> Vertices { get; private set; }
			public MeshAdapter OwnerMesh { get; private set; }

			public MeshPoint(MeshAdapter ownerMesh, Vector3 position, List<int> vertices)
			{
				OwnerMesh = ownerMesh;
				this.position = position;
				Vertices = vertices ?? new List<int>();
			}
		}

		public Mesh Mesh { get; private set; }
		public List<MeshPoint> Points { get; private set; }

		private Vector3[] cachedVertices;

		public MeshAdapter(Mesh mesh)
		{
			Mesh = mesh;
			cachedVertices = mesh.vertices;
			Points = GetPointsCachedVertices();
		}

		public void Flush()
		{
			Mesh.vertices = cachedVertices;
		}

		private void SetVerticesPosition(List<int> verticesNumbers, Vector3 position)
		{
			foreach (int verticeNumber in verticesNumbers)
				cachedVertices[verticeNumber] = position;
		}

		private List<MeshPoint> GetPointsCachedVertices()
		{
            List<MeshPoint> resultPoints = new List<MeshPoint>();

			for (int i = 0; i < cachedVertices.Length; i++)
			{
                MeshPoint existedPoint = resultPoints.FirstOrDefault(point => point.Position == cachedVertices[i]);
				if (existedPoint != null)
				{
					existedPoint.Vertices.Add(i);
				}
				else
				{
					resultPoints.Add(new MeshPoint(this, cachedVertices[i], new List<int> { i }));
				}
			}

			return resultPoints;
		}
	}
}