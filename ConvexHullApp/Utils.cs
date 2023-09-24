using System;
using System.Collections.Generic;
using System.Globalization;
namespace ConvexHullApp
{
	public static class Utils
	{
		/// <summary>
		/// Parse text to list of points.
		/// </summary>
		/// <param name="text">Text representation of points.</param>
		/// <param name="points">List of points returned after successful text parse.</param>
		/// <returns>Returns true if parsing succeeds or false if parsing fails. If false, it also returns the line number where the problem occurs and a message describing whether the problem occurred with the X or Y coordinate.</returns>
		public static (bool success, int lineNumber, string msg) GetPointsFromText(string text, out List<(double X, double Y)> points)
		{
			points = new();
			var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lines.Length; i++)
			{
				var pointValues = lines[i].Split(';', 2, StringSplitOptions.TrimEntries);

				if (double.TryParse(pointValues[0].Replace(',', '.'), NumberStyles.Number,
					NumberFormatInfo.InvariantInfo, out double x))
				{
					if (double.TryParse(pointValues[1].Replace(',', '.'), NumberStyles.Number,
						NumberFormatInfo.InvariantInfo, out double y))
					{
						points.Add(new(x, y));
					}
					else
						return (false, i + 1, "Y coordinate");
				}
				else
					return (false, i + 1, "X coordinate");
			}

			return (true, 0, string.Empty);
		}

		/// <summary>
		/// Gets the text representation of a list of points.
		/// </summary>
		/// <param name="result">List of points which will be transformed to text.</param>
		/// <returns>String with points int format: (x; y);...</returns>.
		public static string GetResultPoints((List<double> X, List<double> Y) result)
		{
			string resultText = string.Empty;
			for (int i = 0; i < result.X.Count; i++)
			{
				resultText += $"({result.X[i].ToString(NumberFormatInfo.InvariantInfo)}; {result.Y[i].ToString(NumberFormatInfo.InvariantInfo)}); ";
			}
			if (resultText.Length > 2)
				resultText = resultText.Remove(resultText.Length - 2);

			return resultText;
		}
	}
}
