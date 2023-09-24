using ConvexHull.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace ConvexHullApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const string HINT = "Enter the individual points on each line separately.\nSeparate the X and Y values with the ';' symbol.\nFor example:\n-1;5\n2.3;-4\n0; 1.8";

		private bool HintShown { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			PointsTB.Text = HINT;
			HintShown = true;
		}

		private void PointsTB_GotFocus(object sender, RoutedEventArgs e)
		{
			if (HintShown)
			{
				PointsTB.Text = string.Empty;
				HintShown = false;
			}
		}

		private void UploadBtn_Click(object sender, RoutedEventArgs e)
		{
			var ofd = new OpenFileDialog
			{
				Title = "Select a Text file",
				Filter = "txt files (*.txt)|*.txt"
			};

			if (ofd.ShowDialog() == true)
			{
				using (var sr = new StreamReader(ofd.OpenFile()))
				{
					PointsTB.Text = sr.ReadToEnd();
					HintShown = false;
				}
			}
		}

		private void CalcBtn_Click(object sender, RoutedEventArgs e)
		{
			var (success, lineNumber, msg) = Utils.GetPointsFromText(PointsTB.Text, out List<(double X, double Y)> points);
			if(!success)
			{
				MessageBox.Show($"Input string was not in a correct format.\n Line: {lineNumber} -> {msg} ",
					"Warning", MessageBoxButton.OK, MessageBoxImage.Warning);		
			}
			else
			{
				if (points != null && !(points.Count < 3))
				{
					try
					{
						PlotPoints(points);
						var (X, Y) = GrahamScan.Compute(points);
						PlotResult((X, Y));
						ResultTB.Text = Utils.GetResultPoints((X, Y));
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				else if (points == null)
					MessageBox.Show("The supplied data is null.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				else
					MessageBox.Show("The convex hull cannot be calculated from less than three points.", 
						"Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void PlotPoints(List<(double X, double Y)> points)
		{
			Graph.Plot.Clear();
			foreach (var (X, Y) in points)
				Graph.Plot.AddPoint(X, Y, System.Drawing.Color.Blue);
			Graph.Refresh();
		}

		private void PlotResult((List<double> X, List<double> Y) result)
		{
			(IEnumerable<double> X, IEnumerable<double> Y) = result;
			X = X.Append(result.X.First());
			Y = Y.Append(result.Y.First());
			Graph.Plot.AddScatter(X.ToArray(), Y.ToArray(), System.Drawing.Color.Red);
			Graph.Refresh();
		}
	}
}
