using PA4E4F_ADT_2023241.Client;

const string SERVER_URI = "http://127.0.0.1:5000";

Console.WriteLine(SERVER_URI.GetType().Name);
GradingApp App = new GradingApp(SERVER_URI, new HttpClient());
if(App.EnsureConnection())
{
    App.Run();
}