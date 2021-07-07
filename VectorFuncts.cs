using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;

namespace Fluid {
	public static class VectorFuncts {
		public static Vector2f GetTopLeft(Vector2f[] vertices) {
			float minX = float.MaxValue;
			float minY = float.MaxValue;

			for (int i = 0; i < vertices.Length; i++) {
				if (vertices[i].X <= minX && vertices[i].Y <= minY) {
					minX = vertices[i].X;
					minY = vertices[i].Y;
				}
			}

			return new Vector2f(minX, minY);
		}

		public static Vector2f[] TranslateToOrigin(Vector2f[] vertices, Vector2f origin) {
			Vector2f topLeft = GetTopLeft(vertices);

			for (int i = 0; i < vertices.Length; i++) {
				vertices[i] -= topLeft;
			}

			return vertices;
		}

		public static Vector2f RotateAroundPoint(Vector2f point, Vector2f origin, double radians) {
			float rotX = (float) (Math.Cos(radians) * (point.X - origin.X) - Math.Sin(radians) * (point.Y - origin.Y) + origin.X);
			float rotY = (float) (Math.Sin(radians) * (point.X - origin.X) + Math.Cos(radians) * (point.Y - origin.Y) + origin.Y);
			return new Vector2f(rotX, rotY);
		}

		public static float Area(Vector2f[] vertices) {
			List<Vector2f> vList = vertices.ToList();

			return Math.Abs(vList.Take(vList.Count - 1).Select((p, i) => p.X * vList[i + 1].Y - p.Y * vList[i + 1].X).Sum() / 2);
		}

		public static void TranslateToOrigin(ref Vector2f[] vertices, Vector2f origin) {
			Vector2f topLeft = GetTopLeft(vertices) + origin;

			for (int i = 0; i < vertices.Length; i++) {
				vertices[i] -= topLeft;
			}
		}

		public static Vector2f AngleToVector(float radians) {
			return new Vector2f((float) Math.Cos(radians), (float) Math.Sin(radians));
		}

		public static float AngleBetweenVectors(Vector2f v1, Vector2f v2) {
			return (float) Math.Atan2(v2.Y - v1.Y, v2.X - v1.X);
		}

		public static Vector2f CentroidFromVertices(Vector2f[] vertices) {
			float sumX = 0;
			float sumY = 0;

			for (int i = 0; i < vertices.Length; i++) {
				sumX += vertices[i].X;
				sumY += vertices[i].Y;
			}

			return new Vector2f(sumX / vertices.Length, sumY / vertices.Length);
		}

		public static float DistanceBetweenVectors(Vector2f v1, Vector2f v2) {
			return (float) Math.Sqrt(Math.Pow(v2.X - v1.X, 2) + Math.Pow(v2.Y - v1.Y, 2));
		}

		public static Vector2f LineNormal(Vector2f lineStart, Vector2f lineEnd) {
			float dx = lineEnd.X - lineStart.X;
			float dy = lineEnd.Y - lineStart.Y;

			return new Vector2f(-dy, dx);
		}

		public static bool VectorInsideVertices(Vector2f point, Vector2f[] poly) {
			float x = point.X;
			float y = point.Y;
			int n = poly.Length;
			bool inside = false;
			bool include_edges = true;

			float p1x = poly[0].X;
			float p1y = poly[0].Y;
			for (int i = 1; i < n + 1; i++) {
				float p2x = poly[i % n].X;
				float p2y = poly[i % n].Y;

				if (p1y == p2y) {
					if (y == p1y) {
						if (Math.Min(p1x, p2x) <= x && x <= Math.Max(p1x, p2x)) {
							inside = include_edges;
							break;
						}

						if (x < Math.Min(p1x, p2x)) inside = !inside;
					}
				}
				else {
					if (Math.Min(p1y, p2y) <= y && y <= Math.Max(p1y, p2y)) {
						float xinters = (y - p1y) * (p2x - p1x) / (p2y - p1y) + p1x;
						if (x == xinters) inside = include_edges;

						if (x < xinters) inside = !inside;
					}
				}

				p1x = p2x;
				p1y = p2y;
			}

			return inside;
		}

		public static Vector2f LineIntersection(Vector2f[] line1, Vector2f[] line2) {
			float Slope(Vector2f p1, Vector2f p2) {
				if (p2.X == p1.X)
					return p2.Y - p1.Y;
				return (p2.Y - p1.Y) / (p2.X - p1.X);
			}

			float yIntercept(float slope, Vector2f p1) {
				return p1.Y - 1f * slope * p1.X;
			}

			float m1 = Slope(line1[0], line1[1]);
			float b1 = yIntercept(m1, line1[0]);
			float m2 = Slope(line2[0], line2[1]);
			float b2 = yIntercept(m2, line2[0]);

			float x = 0;
			if (m1 == m2)
				x = b2 - b1;
			else
				x = (b2 - b1) / (m1 - m2);

			float y = m1 * x + b1;

			return new Vector2f(x, y);
		}

		public static Vector2f? LineSegmentIntersection(Vector2f[] line1, Vector2f[] line2) {
			Vector2f intersectionPoint = LineIntersection(line1, line2);

			if (Math.Min(line1[0].X, line1[1].X) - 0.001 <= intersectionPoint.X && intersectionPoint.X <= Math.Max(line1[0].X, line1[1].X) + 0.001)
				if (Math.Min(line1[0].Y, line1[1].Y) - 0.001 <= intersectionPoint.Y && intersectionPoint.Y <= Math.Max(line1[0].Y, line1[1].Y) + 0.001)
					if (Math.Min(line2[0].X, line2[1].X) - 0.001 <= intersectionPoint.X && intersectionPoint.X <= Math.Max(line2[0].X, line2[1].X) + 0.001)
						if (Math.Min(line2[0].Y, line2[1].Y) - 0.001 <= intersectionPoint.Y && intersectionPoint.Y <= Math.Max(line2[0].Y, line2[1].Y) + 0.001)
							return intersectionPoint;

			return null;
		}
	}
}