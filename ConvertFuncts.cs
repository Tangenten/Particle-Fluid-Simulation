using ChipmunkBinding;
using SFML.System;

namespace Fluid {
	public static class ConvertFuncts {
		public static Vect Vector2fToVect(Vector2f v) {
			return new Vect(v.X, v.Y);
		}

		public static Vect[] Vector2fToVect(Vector2f[] v) {
			Vect[] vects = new Vect[v.Length];
			for (int i = 0; i < v.Length; i++) {
				vects[i].X = v[i].X;
				vects[i].Y = v[i].Y;
			}

			return vects;
		}

		public static Vector2f VectToVector2f(Vect v) {
			return new Vector2f((float) v.X, (float) v.Y);
		}

		public static Vector2f[] VectToVector2f(Vect[] v) {
			Vector2f[] vector2Fs = new Vector2f[v.Length];

			for (int i = 0; i < v.Length; i++) {
				vector2Fs[i].X = (float) v[i].X;
				vector2Fs[i].Y = (float) v[i].Y;
			}

			return vector2Fs;
		}
	}
}