using System;

namespace Fluid {
	public static class RandomFuncts {
		private static readonly Random r = new Random();

		public static float GetRandomFloat(float min, float max) {
			return (float) (r.NextDouble() * (max - min)) + min;
		}

		public static double GetRandomDouble(double min, double max) {
			return r.NextDouble() * (max - min) + min;
		}

		public static int GetRandomInt(int min, int max) {
			return r.Next(min, max);
		}
	}
}