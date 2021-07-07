namespace Fluid {
	public static class MathFuncts {
		public static int Mod(int x, int m) {
			int r = x % m;
			return r < 0 ? r + m : r;
		}
	}
}