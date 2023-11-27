using PA4E4F_ADT_2023241.Client;
using System.Net;

const string SERVER_URI = "http://127.0.0.1:8080";

Console.WriteLine(SERVER_URI.GetType().Name);
GradingApp App = new GradingApp(SERVER_URI, new HttpClient());
App.EnsureConnection();
App.Run();
