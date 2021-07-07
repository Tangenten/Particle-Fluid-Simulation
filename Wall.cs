using System.Linq;
using ChipmunkBinding;
using SFML.Graphics;
using SFML.System;

namespace Fluid {
	public struct Wall {
		public Body shapeBody;
		public Polygon shapePoly;

		public Color color;

		public Wall(ref Space physicsSpace, Vector2f pos, Vector2f side, Color color) {
			this.shapeBody = new Body(BodyType.Static);
			this.shapeBody.Position = new Vect(pos.X, pos.Y);
			this.shapePoly = new Polygon(this.shapeBody, new Vect[4] {new Vect(0, 0), new Vect(side.X, 0), new Vect(side.X, side.Y), new Vect(0, side.Y)}, 1f);

			physicsSpace.AddBody(this.shapeBody);
			physicsSpace.AddShape(this.shapePoly);

			this.color = color;
		}

		public Vector2f GetTopLeft() {
			return ConvertFuncts.VectToVector2f(this.shapeBody.Position);
		}

		public Vector2f GetCenter() {
			return VectorFuncts.CentroidFromVertices(ConvertFuncts.VectToVector2f(this.shapePoly.Vertices.ToArray())) + this.GetTopLeft();
		}

		public Vector2f[] GetVertices() {
			Vector2f[] vertices = ConvertFuncts.VectToVector2f(this.shapePoly.Vertices.ToArray());

			for (int i = 0; i < vertices.Length; i++) {
				vertices[i].X += this.GetTopLeft().X;
				vertices[i].Y += this.GetTopLeft().Y;
				vertices[i] = VectorFuncts.RotateAroundPoint(vertices[i], this.GetTopLeft(), this.shapeBody.Angle);
			}

			return vertices;
		}

		public VertexArray GetVertexArray() {
			Vector2f[] vertices = this.GetVertices();
			VertexArray vertexArray = new VertexArray(PrimitiveType.Quads, (uint) vertices.Length);

			for (int i = 0; i < vertices.Length; i++) {
				vertexArray[(uint) i] = new Vertex(vertices[i], this.color);
			}

			return vertexArray;
		}
	}
}
