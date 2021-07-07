using System;
using System.Threading.Tasks;
using ChipmunkBinding;
using SFML.Graphics;
using SFML.System;

namespace Fluid {
	public class FluidGrid {
		public Fluid fluid;

		private readonly int gridSize;
		private readonly Vertex[,] grid;

		private readonly int width;
		private readonly int height;

		private readonly VertexBuffer vertexBuffer;
		private readonly Vertex[] vertices;

		public FluidGrid(ref Space physicsSpace, int gridSize, int width, int height) {
			this.width = width;
			this.height = height;

			this.fluid = new Fluid(ref physicsSpace);

			this.gridSize = gridSize;
			this.grid = new Vertex[this.gridSize, this.gridSize];

			float xWidth = (float) this.width / (this.gridSize - 1);
			float yWidth = (float) this.height / (this.gridSize - 1);
			for (int i = 0; i < this.gridSize; i++) {
				for (int j = 0; j < this.gridSize; j++) {
					this.grid[i, j] = new Vertex(new Vector2f(i * xWidth, j * yWidth), new Color(0, 0, 0, 0));
				}
			}

			this.vertexBuffer = new VertexBuffer((uint) (this.gridSize * this.gridSize * 4), PrimitiveType.Quads, VertexBuffer.UsageSpecifier.Stream);
			this.vertices = new Vertex[this.gridSize * this.gridSize * 4];
		}

		public void ResetGrid() {
			Color rColor = new Color(0, 0, 0, 0);
			for (int i = 0; i < this.gridSize; i++) {
				for (int j = 0; j < this.gridSize; j++) {
					this.grid[i, j].Color = rColor;
				}
			}
		}

		private void Color(Vector2f pos, Color color, float rad) {
			if (pos.X > 0 && pos.X < this.width && pos.Y > 0 && pos.Y < this.height) {
				int gridIndexX = (int) (pos.X / (this.width / (float) (this.gridSize - 1)));
				int gridIndexY = (int) (pos.Y / (this.height / (float) (this.gridSize - 1)));


				this.grid[gridIndexX, gridIndexY].Color += color;
				this.grid[gridIndexX, gridIndexY].Color.A = (byte) Math.Clamp(this.grid[gridIndexX, gridIndexY].Color.A + 32, 0, 255);

				Color lColor = new Color((byte) (color.R / 4),(byte) (color.G / 4),(byte) (color.B / 4),0);

				int xSize = (int) ((int) rad * ((float)this.gridSize / (float)this.width) * 2f);
				int ySize = (int) ((int) rad * ((float)this.gridSize / (float)this.height) * 2f);

				for (int j = gridIndexX - xSize; j <= gridIndexX + xSize; j++) {
					for (int k = gridIndexY - ySize; k <= gridIndexY + ySize; k++) {
						this.grid[MathFuncts.Mod(j, this.gridSize), MathFuncts.Mod(k, this.gridSize)].Color += lColor;
						this.grid[MathFuncts.Mod(j, this.gridSize), MathFuncts.Mod(k, this.gridSize)].Color.A += (byte) Math.Clamp(this.grid[gridIndexX, gridIndexY].Color.A + 16, 0, 255);
					}
				}
			}
		}

		public void ColorGrid() {
			for (int i = 0; i < this.fluid.fluidParticles.Length; i++) {
				Vector2f pos = this.fluid.fluidParticles[i].GetPosition();
				Color color = this.fluid.fluidParticles[i].color;
				float rad = (float) this.fluid.fluidParticles[i].shapePoly.Radius;
				this.Color(pos, color, rad);
				/*pos.X -= rad;
				this.Color(pos, color);
				pos.X += (rad * 2);
				this.Color(pos, color);
				pos.X -= rad;
				pos.Y -= rad;
				this.Color(pos, color);
				pos.Y += (rad * 2);
				this.Color(pos, color);*/
			}
		}

		public void Draw(ref RenderWindow window) {
			float xWidth = (float) this.width / (this.gridSize - 1);
			float yWidth = (float) this.height / (this.gridSize - 1);

			int vertexIndex = 0;

			for (int i = 0; i < this.gridSize; i++) {
				for (int j = 0; j < this.gridSize; j++) {
					Vertex vertex = this.grid[i, j];
					this.vertices[vertexIndex++] = vertex;
					vertex.Position.X += xWidth;
					this.vertices[vertexIndex++] = vertex;
					vertex.Position.Y += yWidth;
					this.vertices[vertexIndex++] = vertex;
					vertex.Position.X -= xWidth;
					this.vertices[vertexIndex++] = vertex;
				}
			}

			this.vertexBuffer.Update(this.vertices);
			window.Draw(this.vertexBuffer);
		}
	}
}
