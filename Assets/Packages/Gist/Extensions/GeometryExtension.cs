using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nobnak.Gist.Extensions.GeometryExt {

	public static class GeometryExtension {
		public const float DEF_EPS = 0.001f;

		public static IList<int> RenameMapForMergedIndices(this Mesh mesh, float eps = DEF_EPS) {
			var size = mesh.bounds.size;
			var minlength = Mathf.Min(Mathf.Min(size.x, size.y), size.z);
			eps *= minlength;

			var vertices = mesh.vertices;
			var triangles = mesh.triangles;

			var renameMap = Enumerable.Range(0, vertices.Length).ToArray();
			for (var i = 0; i < vertices.Length; i++) {
				var vi = vertices[i];
				var minsq = float.MaxValue;
				for (var j = 0; j < i; j++) {
					var vj = vertices[j];
					var tmpsq = (vi - vj).sqrMagnitude;
					if (tmpsq < minsq) {
						minsq = tmpsq;
						if (tmpsq < (eps * eps))
							renameMap[i] = j;
					}
				}
			}

			return renameMap;
		}

		public static IList<int> MergeIndices(this Mesh mesh, float eps = DEF_EPS) {
			var triangles = mesh.triangles;
			var rem = mesh.RenameMapForMergedIndices(eps);
			for (var i = 0; i < triangles.Length; i++)
				triangles[i] = rem[triangles[i]];
			return triangles;
		}

		public static int[,] CountEdgesOnTriangles(this IList<int> triangles, int vertexCount) {
			var counter = new int[vertexCount, vertexCount];
			for (var t = 0; t < triangles.Count; t += 3) {
				for (var o = 0; o < 3; o++) {
					var i0 = triangles[t + o];
					var i1 = triangles[t + (o + 1) % 3];
					counter[i0, i1]++;
				}
			}
			return counter;
		}

		public static bool Raycast(this Ray ray,
			Vector3 center, Vector3 forward,
			out float distance, float eps = DEF_EPS) {

			distance = default(float);

			var det = Vector3.Dot((Vector3)forward, ray.direction);
			if (-eps < det && det < eps)
				return false;

			distance = Vector3.Dot((Vector3)forward, (Vector3)(center - ray.origin)) / det;
			return true;
		}
		public static bool Raycast(this Transform tr,
			Ray ray, out float distance, float eps = DEF_EPS) {

			distance = default(float);
			return ray.Raycast(tr.position, tr.forward, out distance, eps);
		}
		public static bool Raycast(this Component c,
			Ray ray, out float distance, float eps = DEF_EPS) {
			return c.transform.Raycast(ray, out distance, eps);
		}
	}
}