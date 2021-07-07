using System;
using ChipmunkBinding;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shape = ChipmunkBinding.Shape;

namespace Fluid {
	public class Game : IDebugDraw {
		private int windowHeight;
		private int windowWidth;

		private RenderWindow window;

		private readonly Clock clock;
		private float deltaTime;

		private Vector2f mouseCurrentPosition;
		private Vector2f mousePressPosition;
		private Vector2f mouseReleasedPosition;

		private readonly Space physicsSpace;
		private readonly float physicsTimeStep;
		private float physicsTimeStepCarryOver;

		private readonly Wall[] walls;
		private FluidBlur fluidBlur;

		public Game() {
			this.windowWidth = 1920;
			this.windowHeight = 1080;

			this.window = new RenderWindow(new VideoMode((uint) this.windowWidth, (uint) this.windowHeight), "Metaballs Fluid", Styles.Default);
			this.window.SetVerticalSyncEnabled(true);

			this.window.Closed += this.OnClose;
			this.window.Resized += this.OnResize;
			this.window.MouseButtonPressed += this.OnMousePressed;
			this.window.MouseButtonReleased += this.OnMouseReleased;
			this.window.MouseMoved += this.OnMouseMove;

			this.clock = new Clock();
			this.deltaTime = 0f;

			this.physicsSpace = new HastySpace();
			this.physicsSpace.Iterations = 1;
			this.physicsSpace.Gravity = new Vect(0, 1000);

			this.physicsTimeStep = 0.01f;
			this.physicsTimeStepCarryOver = 0;

			this.walls = new Wall[5];

			this.walls[0] = new Wall(ref this.physicsSpace, new Vector2f(0, 0), new Vector2f(10f, this.windowHeight), Color.White);
			this.walls[1] = new Wall(ref this.physicsSpace, new Vector2f(this.windowWidth - 10, 0), new Vector2f(10f, this.windowHeight), Color.White);
			this.walls[2] = new Wall(ref this.physicsSpace, new Vector2f(0, this.windowHeight - 10), new Vector2f(this.windowWidth, 10f), Color.White);
			this.walls[3] = new Wall(ref this.physicsSpace, new Vector2f(0, 0), new Vector2f(this.windowWidth, 10f), Color.White);
			this.walls[4] = new Wall(ref this.physicsSpace, new Vector2f(this.windowWidth / 4f, this.windowHeight / 2f), new Vector2f(this.windowWidth / 2f, 10f), Color.White);

			this.fluidBlur = new FluidBlur(ref this.physicsSpace, this.windowWidth, this.windowHeight);
		}

		private void OnResize(object sender, SizeEventArgs e) {
			this.windowWidth = (int) e.Width;
			this.windowHeight = (int) e.Height;
		}

		private void OnClose(object sender, EventArgs e) {
			this.window.Close();
		}

		private void OnMouseMove(object? sender, MouseMoveEventArgs e) {
			this.mouseCurrentPosition.X = e.X;
			this.mouseCurrentPosition.Y = e.Y;
		}

		private void OnMouseReleased(object? sender, MouseButtonEventArgs e) {
			this.mouseReleasedPosition.X = e.X;
			this.mouseReleasedPosition.Y = e.Y;
		}

		private void OnMousePressed(object? sender, MouseButtonEventArgs e) {
			this.mousePressPosition.X = e.X;
			this.mousePressPosition.Y = e.Y;
		}

		public void Run() {
			while (this.window.IsOpen) {
				this.deltaTime = this.clock.Restart().AsSeconds();
				Console.WriteLine(1f / this.deltaTime);

				this.window.Clear(Color.Black);

				this.window.DispatchEvents();

				if (Mouse.IsButtonPressed(Mouse.Button.Left)) this.fluidBlur.fluid.CreateFluid(this.mouseCurrentPosition, 10, new Color(37, 89, 215 ,96));
				if (Mouse.IsButtonPressed(Mouse.Button.Middle)) this.fluidBlur.fluid.CreateFluid(this.mouseCurrentPosition, 10, new Color(215, 146, 37 ,96));
				if (Mouse.IsButtonPressed(Mouse.Button.Right)) this.fluidBlur.fluid.CreateFluid(this.mouseCurrentPosition, 10, new Color(37, 215, 86, 96));

				this.physicsTimeStepCarryOver += this.deltaTime;
				while (this.physicsTimeStepCarryOver > this.physicsTimeStep) {
					this.physicsSpace.Step(this.physicsTimeStep);
					this.physicsTimeStepCarryOver -= this.physicsTimeStep;
				}

				RenderStates renderStates = new RenderStates(BlendMode.Alpha);
				for (int i = 0; i < this.walls.Length; i++) {
					this.window.Draw(this.walls[i].GetVertexArray(), renderStates);
				}

				this.fluidBlur.Draw(ref this.window);
				//this.physicsSpace.DebugDraw(this);
				this.window.Display();
			}
		}

		public void DrawCircle(Vect pos, double angle, double radius, DebugColor outlineColor, DebugColor fillColor) {
			CircleShape circleShape = new CircleShape((float) radius);
			circleShape.Position = ConvertFuncts.VectToVector2f(pos) - new Vector2f((float) radius, (float) radius);
			this.window.Draw(circleShape);
		}

		public void DrawSegment(Vect a, Vect b, DebugColor color) {
			VertexArray vertexArray = new VertexArray(PrimitiveType.Lines, 2);
			vertexArray[0] = new Vertex(ConvertFuncts.VectToVector2f(a));
			vertexArray[1] = new Vertex(ConvertFuncts.VectToVector2f(b));
			this.window.Draw(vertexArray);
		}

		public void DrawFatSegment(Vect a, Vect b, double radius, DebugColor outlineColor, DebugColor fillColor) {
			VertexArray vertexArray = new VertexArray(PrimitiveType.Lines, 2);
			vertexArray[0] = new Vertex(ConvertFuncts.VectToVector2f(a));
			vertexArray[1] = new Vertex(ConvertFuncts.VectToVector2f(b));
			this.window.Draw(vertexArray);
		}

		public void DrawPolygon(Vect[] vectors, double radius, DebugColor outlineColor, DebugColor fillColor) {
			VertexArray vertexArray = new VertexArray(PrimitiveType.LineStrip, (uint) vectors.Length + 1);

			for (int i = 0; i < vectors.Length + 1; i++) {
				vertexArray[(uint) i] = new Vertex(ConvertFuncts.VectToVector2f(vectors[i % vectors.Length]));
			}

			this.window.Draw(vertexArray);
		}

		public void DrawDot(double size, Vect pos, DebugColor color) {
			CircleShape circleShape = new CircleShape((float) size);
			circleShape.Position = ConvertFuncts.VectToVector2f(pos);
			this.window.Draw(circleShape);
		}

		public DebugColor ColorForShape(Shape shape) {
			return new DebugColor(1, 1, 1);
		}
	}
}
