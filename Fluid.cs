using System.Collections.Generic;
using ChipmunkBinding;
using SFML.Graphics;
using SFML.System;

namespace Fluid {
	public struct Fluid {
		public FluidParticle[] fluidParticles;
		private int fluidIndex;
		private int fluidSize;
		private Space physicsSpace;

		public Fluid(ref Space physicsSpace) {
			this.fluidIndex = 0;
			this.fluidSize = 1024;

			this.fluidParticles = new FluidParticle[this.fluidSize];
			this.physicsSpace = physicsSpace;

			this.initFluid();
		}

		private void initFluid() {
			Color color = new Color((byte) RandomFuncts.GetRandomInt(0, 255), (byte) RandomFuncts.GetRandomInt(0, 255), (byte) RandomFuncts.GetRandomInt(0, 255), 96);
			for (int i = 0; i < this.fluidSize; i++) {
				this.fluidParticles[i] = new FluidParticle(ref this.physicsSpace, new Vector2f(RandomFuncts.GetRandomInt(0, 1920), RandomFuncts.GetRandomInt(0, 1080)), color);
			}
		}

		public void CreateFluid(Vector2f pos, int fluidCount, Color color) {
			for (int i = 0; i < fluidCount; i++) {
				Vector2f rPos = new Vector2f(pos.X + RandomFuncts.GetRandomFloat(-10, 10), pos.Y + RandomFuncts.GetRandomFloat(-10, 10));
				FluidParticle fluidParticle = new FluidParticle(ref this.physicsSpace, rPos, color);
				this.AddFluid(fluidParticle);
			}
		}

		public void AddFluid(FluidParticle fluidParticle) {
			this.physicsSpace.RemoveBody(this.fluidParticles[MathFuncts.Mod(fluidIndex-fluidSize, fluidSize)].shapeBody);
			this.physicsSpace.RemoveShape(this.fluidParticles[MathFuncts.Mod(fluidIndex-fluidSize, fluidSize)].shapePoly);

			this.fluidParticles[fluidIndex % fluidSize] = fluidParticle;
			this.fluidIndex++;
		}

	}
}
