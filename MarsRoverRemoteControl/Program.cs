using API;

namespace MarsRoverRemoteControl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var marsRoverController = new MarsRoverController();
            var earthHQ = new EarthHQ(marsRoverController);

            earthHQ.InitializeRover();

            while (true) 
            {
                Console.Write("Select the operation to perform:\n" +
                    "1 -> Chart a route for the Rover\n" +
                    "2 -> End the transmission\n\n" +
                    "Operation number: ");

                var optionSelected = Console.ReadLine();

                switch (optionSelected)
                {
                    case("1"):
                        earthHQ.ChartRoverRoute();
                        break;
                    case("2"):
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Write("Wrong input. Select the operation to perform by writing the corresponding option number\n\n");
                        break;
                }
                
            }
        }
    }
}
