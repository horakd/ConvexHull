namespace ConvexHull.Algorithms
{
	public static class GrahamScan
	{
		/// <summary>
		/// Get orientation of last tre point in hull
		/// </summary>
		/// <param name="p">The index of the last added point in the result list.</param>
		/// <param name="q">The index of the next point that is counter-clockwise to the last added point.</param>
		/// <param name="r">The index of the point that can be the next counter-clockwise point.</param>
		/// <returns>If result is &lt; 0 then the orientation is counterclockwise. If result is &gt; 0 then orientation is clockwise. If result is = 0 then orientation is collinear.</returns>
		static double PointsOrientation((double X, double Y) p, (double X, double Y) q, (double X, double Y) r)
		{
			return (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
		}

		/// <summary>
		/// Gets the convex hull of a set of point using Graham scan algorithm.
		/// </summary>
		/// <param name="points">List of point in a set.</param>
		/// <returns>Lists of x and y coordinates.</returns>
		/// <exception cref="ArgumentNullException">Thrown when supplied data is null.</exception>
		/// <exception cref="ArgumentException">Thrown when supplied data count is &lt; 3.</exception>
		public static (List<double> X, List<double> Y) Compute(List<(double X, double Y)> points)
		{
			int n = points != null ? points.Count : throw new ArgumentNullException(nameof(points), "The supplied data is null.");
			if (n < 3)
				throw new ArgumentException("The convex hull cannot be calculated from less than three points.", nameof(points));

			(List<double> X, List<double> Y) hull = new(new List<double>(), new List<double>());
			var pivot = points.IndexOf(points.OrderBy(p => (p.Y, p.X)).First());
			int p = pivot;
			int q;
			do
			{
				hull.X.Add(points[p].X);
				hull.Y.Add(points[p].Y);

				q = p + 1;

				for (int i = 0; i < n; i++)
				{
					if (PointsOrientation(points[p], points[i], points[q]) < 0)
						q = i;
				}

				p = q;

			} while (p != pivot);  

			return hull;
		}
	}
}
