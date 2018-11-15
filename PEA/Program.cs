using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PEA
{
	class Program
	{
		static void Main (string[] args)
		{
			TspGraph graph = new TspGraph();
			char primaryMenuKey, secondaryMenuKey;
			string input;
			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken ct = cts.Token;

			do
			{
				Console.WriteLine ();
				Console.WriteLine ("ZSIK - problem komiwojażera");
				Console.WriteLine ("-Menu główne");
				Console.WriteLine ("--1. Wczytaj graf z pliku.");
				Console.WriteLine ("--2. Przejrzyj właściwości grafu.");
				Console.WriteLine ("--3. Rozwiąż problem algorytmem genetycznym.");
                Console.WriteLine ("--4. Rozwiąż problem algorytmem genetycznym wielowątkowo.");
                Console.WriteLine ("--5. Stwórz zestaw o określonej ilości miast.");
                Console.WriteLine("--6. Rozwiaz problem algorytmem dynamicznym.");

                primaryMenuKey = Console.ReadKey().KeyChar;

				switch (primaryMenuKey)
				{
					case '1':
						Console.Clear ();
						Console.WriteLine ("Wpisz ścieżkę pliku.");
						string filename = Console.ReadLine();
						filename = filename?.Replace(@"\", @"\\");
						graph.ReadGraphFromFile(filename);
						break;
					case '2':
						Console.Clear ();
						try
						{
							Console.WriteLine("---Graf");
							Console.WriteLine("----Nazwa: " + graph.Name);
							Console.WriteLine("----Typ: " + graph.Type);
							Console.WriteLine("----Wymiary: " + graph.Dimension);
							graph.PrintGraph();
						}
						catch (NullReferenceException)
						{
							Console.WriteLine("Graf albo jego właściwości są null.");
						}
						Console.ReadKey();
						break;
					case '3':
                        Console.Clear();
                        int pop, elites;
                        short startingIndex, endingIndex;
                        double mutrate, crossrate;

                        Console.WriteLine("---Podaj prawdopodobieństwo krzyżowania:");
                        input = Console.ReadLine();

                        while (!double.TryParse(input, out crossrate))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj prawdopodobieństwo mutacji:");
                        input = Console.ReadLine();

                        while (!double.TryParse(input, out mutrate))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj wielkość populacji:");
                        input = Console.ReadLine();

                        while (!Int32.TryParse(input, out pop))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj liczbę elit:");
                        input = Console.ReadLine();

                        while (!Int32.TryParse(input, out elites))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj wierzcholek startowy:");
                        input = Console.ReadLine();

                        while (!short.TryParse(input, out startingIndex) || startingIndex < 0 || startingIndex > graph.Dimension-1)
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj wierzcholek koncowy:");
                        input = Console.ReadLine();

                        while (!short.TryParse(input, out endingIndex) || startingIndex < 0 || startingIndex > graph.Dimension - 1)
                        {
                            input = Console.ReadLine();
                        }

                        cts = new CancellationTokenSource();
                        Console.Clear();
                        Task.Run(() => {
                            while (!cts.IsCancellationRequested)
                            {
                                Console.Write(".");
                                Thread.Sleep(1000);
                            }
                        }, ct);
                        TspGenetic.SolveTsp(graph, elites, crossrate, mutrate, pop, startingIndex, endingIndex, CrossoverType.OX, 300, 1000);
                        cts.Cancel();
                        cts.Dispose();
                        Console.WriteLine();
                        TspGenetic.ShowResults();
                        Console.ReadKey();
                        break;
                    case '4':
                        Console.Clear();
                        int tasksAmount = 0;

                        Console.WriteLine("---Podaj ilosc watkow:");
                        input = Console.ReadLine();

                        while (!int.TryParse(input, out tasksAmount))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj prawdopodobieństwo krzyżowania:");
                        input = Console.ReadLine();

                        while (!double.TryParse(input, out crossrate))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj prawdopodobieństwo mutacji:");
                        input = Console.ReadLine();

                        while (!double.TryParse(input, out mutrate))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj wielkość populacji:");
                        input = Console.ReadLine();

                        while (!Int32.TryParse(input, out pop))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj liczbę elit:");
                        input = Console.ReadLine();

                        while (!Int32.TryParse(input, out elites))
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj wierzcholek startowy:");
                        input = Console.ReadLine();

                        while (!short.TryParse(input, out startingIndex) || startingIndex < 0 || startingIndex > graph.Dimension - 1)
                        {
                            input = Console.ReadLine();
                        }

                        Console.WriteLine("---Podaj wierzcholek koncowy:");
                        input = Console.ReadLine();

                        while (!short.TryParse(input, out endingIndex) || startingIndex < 0 || startingIndex > graph.Dimension - 1)
                        {
                            input = Console.ReadLine();
                        }

                        cts = new CancellationTokenSource();
                        Console.Clear();
                        Task.Run(() => {
                            while (!cts.IsCancellationRequested)
                            {
                                Console.Write(".");
                                Thread.Sleep(1000);
                            }
                        }, ct);
                        TspGenetic.SolveTsp(graph, elites, crossrate, mutrate, pop, startingIndex, endingIndex, CrossoverType.OX, 300, 1000, tasksAmount);
                        cts.Cancel();
                        cts.Dispose();
                        Console.WriteLine();
                        TspGenetic.ShowResults();
                        Console.ReadKey();
                        break;
                    case '5':
						Console.Clear();
						int dim, maxDist;
						Console.WriteLine("---Podaj ilość miast:");
						input = Console.ReadLine();

						while (!Int32.TryParse(input, out dim))
						{
							input = Console.ReadLine();
						}

						Console.WriteLine("---Równe odległości A-B i B-A? (t - tak, n - nie)");
						
						do
						{
							input = Console.ReadLine() ?? "";
						} while (!(input.Equals("t") || input.Equals("n")));

						bool tspOrAtsp = false;

						if (input.Equals("t")) tspOrAtsp = false;
						else if (input.Equals("n")) tspOrAtsp = true;

						Console.WriteLine("---Podaj maksymalną odległość między miastami:");
						input = Console.ReadLine();

						while (!Int32.TryParse(input, out maxDist))
						{
							input = Console.ReadLine();
						}

						graph = new TspGraph(dim, tspOrAtsp, maxDist);
						Console.WriteLine("---Utworzono graf o następujących właściwościach:");
						Console.WriteLine("----Nazwa: " + graph.Name);
						Console.WriteLine("----Typ: " + graph.Type);
						Console.WriteLine("----Wymiary: " + graph.Dimension);
						Console.ReadKey();
						break;
                    case '6':
                        cts = new CancellationTokenSource();
                        Console.Clear();
                        Task.Run(() => {
                            while (!cts.IsCancellationRequested)
                            {
                                Console.Write(".");
                                Thread.Sleep(1000);
                            }
                        }, ct);
                        TspDynamicProgramming.SolveTsp(graph);
                        cts.Cancel();
                        cts.Dispose();
                        Console.WriteLine();
                        TspDynamicProgramming.ShowResults();
                        TspDynamicProgramming.ClearCollections();
                        Console.ReadKey();
                        break;
                    default:
						break;
				}
			} while (primaryMenuKey != 27);
		}

		private static double[,] DpTests(bool tspOrAtsp)
		{
			double[,] timesArray = new double[6,100];
			int numbOfCities = 14;

			for (int i = 0; i < timesArray.GetLength(0); i++, numbOfCities += 2)
			{
				for (int j = 0; j < timesArray.GetLength(1); j++)
				{
					TspGraph graph = new TspGraph(numbOfCities, tspOrAtsp);
					TspDynamicProgramming.SolveTsp(graph);
					timesArray[i, j] = TspDynamicProgramming.TimeMeasured.TotalMilliseconds;
					TspDynamicProgramming.ClearCollections();
				}
			}

			return timesArray;
		}
		//testy dla wybranych instancji z tsplib umieszczonych w folderze ./tsplib
		private static double[,] DpTestsTspLib()
		{
			double[,] timesArray = new double[4, 100];
			int i = 0;

			TspGraph graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\gr17.tsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspDynamicProgramming.SolveTsp(graph);
				timesArray[i, j] = TspDynamicProgramming.TimeMeasured.TotalMilliseconds;
				TspDynamicProgramming.ClearCollections();
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\gr21.tsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspDynamicProgramming.SolveTsp(graph);
				timesArray[i, j] = TspDynamicProgramming.TimeMeasured.TotalMilliseconds;
				TspDynamicProgramming.ClearCollections();
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\gr24.tsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspDynamicProgramming.SolveTsp(graph);
				timesArray[i, j] = TspDynamicProgramming.TimeMeasured.TotalMilliseconds;
				TspDynamicProgramming.ClearCollections();
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\br17.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspDynamicProgramming.SolveTsp(graph);
				timesArray[i, j] = TspDynamicProgramming.TimeMeasured.TotalMilliseconds;
				TspDynamicProgramming.ClearCollections();
			}

			return timesArray;
		}

		private static Tuple<double[,], int[,]> TsTestsTspLib()
		{
			double[,] timesArray = new double[25, 10];
			int[,] distancesArray = new int[25, 10];
			int i = 0;

			TspGraph graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\gr24.tsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 8);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 6);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 4);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 2);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension * 3);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\ftv70.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 8);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 6);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 4);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 2);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension * 3);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;
			
			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\kro124p.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 8);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 6);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 4);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 2);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension * 3);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\ftv170.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 8);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 6);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 4);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 2);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension * 3);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\rbg323.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 8);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 6);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 4);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension / 2);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}
			i++;

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspTabuSearch.SolveTsp(graph, graph.Dimension * 3);
				timesArray[i, j] = TspTabuSearch.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspTabuSearch.PathDistance;
			}

			Tuple<double[,], int[,]> returnTuple = new Tuple<double[,], int[,]>(timesArray, distancesArray);

			return returnTuple;
		}

		/*private static void GaParametersTests()
		{
			TspGraph graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\gr48.tsp");

			double[] mutationRates = { 0.01, 0.05, 0.1, 0.25, 0.5, 0.75 };
			double[] crossoverRates = { 0.25, 0.5, 0.75, 0.9, 1.0 };
			List<double> allAvgDistances = new List<double>(); 

			for (int i = 0; i < crossoverRates.Length; i++)
			{
				for (int j = 0; j < mutationRates.Length; j++)
				{
					double[,] times = new double[1, 11];
					int[,] distances = new int[1, 11];

					for (int k = 0; k < 10; k++)
					{
						TspGenetic.SolveTsp(graph, 28, crossoverRates[i], mutationRates[j], 144, CrossoverType.OX, 30);
						times[0, k] = TspGenetic.TimeMeasured.TotalMilliseconds;
						distances[0, k] = TspGenetic.PathDistance;
					}

					var avgTime = GetAverageTimes(times)[0] * 11.0 / 10.0;
					var avgDistance = GetAverageDistances(distances)[0] * 11.0 / 10.0;
					times[0, times.Length - 1] = avgTime;
					distances[0, distances.Length - 1] = (int)avgDistance;
					string filename = "gr48-CR" + crossoverRates[i].ToString("F2") + "-MR" + mutationRates[j].ToString("F2");
					double[] times1d = new double[11];
					double[] distances1d = new double[11];
					allAvgDistances.Add(avgDistance);

					for (int x = 0; x < times1d.Length; x++)
					{
						times1d[x] = times[0, x];
						distances1d[x] = distances[0, x];
					}

					TimesToFile(filename + "-TimeOX.txt", times1d);
					DistancesToFile(filename + "-DistsOX.txt", distances1d);
				}
			}

			for (int i = 0; i < crossoverRates.Length; i++)
			{
				for (int j = 0; j < mutationRates.Length; j++)
				{
					double[,] times = new double[1, 11];
					int[,] distances = new int[1, 11];

					for (int k = 0; k < 10; k++)
					{
						TspGenetic.SolveTsp(graph, 28, crossoverRates[i], mutationRates[j], 144, CrossoverType.Other, 30);
						times[0, k] = TspGenetic.TimeMeasured.TotalMilliseconds;
						distances[0, k] = TspGenetic.PathDistance;
					}

					var avgTime = GetAverageTimes(times)[0] * 11.0 / 10.0;
					var avgDistance = GetAverageDistances(distances)[0] * 11.0 / 10.0;
					times[0, times.Length - 1] = avgTime;
					distances[0, distances.Length - 1] = (int)avgDistance;
					string filename = "gr48-CR" + crossoverRates[i].ToString("F2") + "-MR" + mutationRates[j].ToString("F2");
					double[] times1d = new double[11];
					double[] distances1d = new double[11];
					allAvgDistances.Add(avgDistance);

					for (int x = 0; x < times1d.Length; x++)
					{
						times1d[x] = times[0, x];
						distances1d[x] = distances[0, x];
					}

					TimesToFile(filename + "-TimeOT.txt", times1d);
					DistancesToFile(filename + "-DistsOT.txt", distances1d);
				}
			}

			DistancesToFile("gr48-AllAverageDists.txt", allAvgDistances.ToArray());
		}*/

		/*private static Tuple<double[,], int[,]> GaTestsTspLib()
		{
			double[,] timesArray = new double[10, 10];
			int[,] distancesArray = new int[10, 10];
			int i = 0;

			TspGraph graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\gr24.tsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 14, 0.5, 0.1, 72, CrossoverType.OX);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\ftv70.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 42, 0.5, 0.1, 212, CrossoverType.OX);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\kro124p.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 60, 0.5, 0.1, 300, CrossoverType.OX);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\ftv170.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 102, 0.5, 0.1, 512, CrossoverType.OX);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\rbg323.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 194, 0.5, 0.1, 968, CrossoverType.OX, 600);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\gr24.tsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 14, 1.0, 0.5, 72, CrossoverType.OX);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\ftv70.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 42, 1.0, 0.5, 212, CrossoverType.OX);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\kro124p.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 60, 1.0, 0.5, 300, CrossoverType.OX);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\ftv170.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 102, 1.0, 0.5, 512, CrossoverType.OX);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}
			i++;

			graph = new TspGraph();
			graph.ReadGraphFromFile(AppDomain.CurrentDomain.BaseDirectory + @"\tsplib\rbg323.atsp");

			for (int j = 0; j < timesArray.GetLength(1); j++)
			{
				TspGenetic.SolveTsp(graph, 194, 1.0, 0.5, 968, CrossoverType.OX, 600);
				timesArray[i, j] = TspGenetic.TimeMeasured.TotalMilliseconds;
				distancesArray[i, j] = TspGenetic.PathDistance;
			}

			Tuple<double[,], int[,]> returnTuple = new Tuple<double[,], int[,]>(timesArray, distancesArray);

			return returnTuple;
		}*/

		private static double[] GetAverageTimes(double[,] times)
		{
			double[] avgTimesArray = new double[times.GetLength(0)];
			double temp = 0;

			for (int i = 0; i < times.GetLength(0); i++)
			{
				for (int j = 0; j < times.GetLength(1); j++)
				{
					temp += times[i, j];
				}

				temp = temp / times.GetLength(1);
				avgTimesArray[i] = temp;
				temp = 0;
			}

			return avgTimesArray;
		}

		private static double[] GetAverageDistances(int[,] distances)
		{
			double[] avgDistArray = new double[distances.GetLength(0)];
			double temp = 0;

			for (int i = 0; i < distances.GetLength(0); i++)
			{
				for (int j = 0; j < distances.GetLength(1); j++)
				{
					temp += distances[i, j];
				}

				temp = temp / distances.GetLength(1);
				avgDistArray[i] = temp;
				temp = 0;
			}

			return avgDistArray;
		}

		private static void TimesToFile(string filename, double[] times)
		{
			string dir = AppDomain.CurrentDomain.BaseDirectory + @"\Times\";
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			StreamWriter file = new StreamWriter(dir + filename);

			file.WriteLine("Czasy [ms]:");

			foreach (double time in times)
			{
				file.WriteLine(time.ToString("F3"));
			}

			file.Close();
		}

		private static void DistancesToFile(string filename, double[] distances)
		{
			string dir = AppDomain.CurrentDomain.BaseDirectory + @"\Distances\";
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			StreamWriter file = new StreamWriter(dir + filename);

			file.WriteLine("Długości ścieżek:");

			foreach (double distance in distances)
			{
				file.WriteLine(distance.ToString("F3"));
			}

			file.Close();
		}
	}//class
}//namespace