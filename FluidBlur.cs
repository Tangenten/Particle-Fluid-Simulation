using System;
using ChipmunkBinding;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;

namespace Fluid {
	public struct FluidBlur {
		public Fluid fluid;

		private RenderTexture renderTexture;
		private Shader blurShader;
		private Shader thresholdShader;
		private RenderStates renderStates;
		private Sprite sprite;

		private int width;
		private int height;

		public FluidBlur(ref Space physicsSpace, int width, int height) {
			this.width = width;
			this.height = height;

			this.fluid = new Fluid(ref physicsSpace);
			this.renderTexture = new RenderTexture((uint) this.width, (uint) this.height);

			blurShader = new Shader(null, null, "C:\\Users\\Tangent\\Dropbox\\Programming\\RiderProjects\\Fluid\\blur.frag");
			thresholdShader = new Shader(null, null, "C:\\Users\\Tangent\\Dropbox\\Programming\\RiderProjects\\Fluid\\threshold.frag");
			renderStates = new RenderStates(BlendMode.None);

			this.sprite = new Sprite(this.renderTexture.Texture);
			this.sprite.Position = new Vector2f(0, 0);
			this.sprite.TextureRect = new IntRect(new Vector2i(0,0), (Vector2i) this.renderTexture.Size);
		}

		public void Draw(ref RenderWindow window) {
			this.renderTexture.Clear(Color.Transparent);

			CircleShape circleShape = new CircleShape();
			for (int i = 0; i < this.fluid.fluidParticles.Length; i++) {
				Vector2f pos = this.fluid.fluidParticles[i].GetPosition();
				float rad = this.fluid.fluidParticles[i].GetRadius() * 1.5f;
				Color color = this.fluid.fluidParticles[i].color;

				circleShape.Radius = rad;
				circleShape.Position = pos - new Vector2f( rad,rad);
				circleShape.FillColor = color;
				this.renderTexture.Draw(circleShape, renderStates);
			}

			this.sprite.Texture = this.renderTexture.Texture;

			blurShader.SetUniform("blur_radius", new Vec2(0, 0.0075f));
			blurShader.SetUniform("texture", this.sprite.Texture);
			renderStates.Shader = blurShader;
			this.renderStates.Texture = this.sprite.Texture;
			this.sprite.Draw(this.renderTexture, this.renderStates);

			blurShader.SetUniform("blur_radius", new Vec2(0.0075f, 0));
			blurShader.SetUniform("texture", this.sprite.Texture);
			renderStates.Shader = blurShader;
			this.renderStates.Texture = this.sprite.Texture;
			this.sprite.Draw(this.renderTexture, this.renderStates);

			thresholdShader.SetUniform("threshold", 0.05f);
			thresholdShader.SetUniform("texture", sprite.Texture);
			renderStates.Shader = thresholdShader;
			this.sprite.Draw(this.renderTexture, this.renderStates);

			blurShader.SetUniform("blur_radius", new Vec2(0, 0.00075f));
			blurShader.SetUniform("texture", this.sprite.Texture);
			renderStates.Shader = blurShader;
			this.renderStates.Texture = this.sprite.Texture;
			this.sprite.Draw(this.renderTexture, this.renderStates);

			blurShader.SetUniform("blur_radius", new Vec2(0.00075f, 0));
			blurShader.SetUniform("texture", this.sprite.Texture);
			renderStates.Shader = blurShader;
			this.renderStates.Texture = this.sprite.Texture;
			this.sprite.Draw(this.renderTexture, this.renderStates);

			this.renderTexture.Display();
			this.renderStates.Shader = null;
			this.renderStates.Texture = null;
			window.Draw(this.sprite, new RenderStates(BlendMode.Add));
		}
	}
}
