using ChipmunkBinding;
using SFML.Graphics;
using SFML.System;

namespace Fluid {
	public struct FluidParticle {
		public Body shapeBody;
		public Circle shapePoly;

		public Color color;

		public FluidParticle(ref Space physicsSpace, Vector2f pos, Color color) {
			this.shapeBody = new Body(1, 1666, BodyType.Dynamic);
			this.shapeBody.Position = new Vect(pos.X, pos.Y);

			this.shapePoly = new Circle(this.shapeBody, 10);
			this.shapePoly.Friction = 0.0f;
			this.shapePoly.Elasticity = 0.85f;
			this.shapePoly.Density = 0.5f;
			this.shapePoly.MomentForMass(this.shapeBody.Mass);

			physicsSpace.AddBody(this.shapeBody);
			physicsSpace.AddShape(this.shapePoly);

			this.color = color;
		}

		public Vector2f GetPosition() {
			return ConvertFuncts.VectToVector2f(this.shapeBody.Position);
		}

		public float GetRadius() {
			return (float) this.shapePoly.Radius;
		}
	}
}
