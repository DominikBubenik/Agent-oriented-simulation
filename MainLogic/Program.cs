namespace MainLogic;

class Program
{
    static void Main(string[] args)
    {
        // var air = new AirportSimulationCore(1, 2, 4, 6, endSimulationTime: 86400, systemCapacity: 5725,
        //     timeIntervalSeconds: 86400, turboMode: true);
        // air.RunSimulation(10_000);
        // Console.WriteLine("gfgfdsg");
        // return;
        // var sim = new PortSimulationCore(0, 0,  1_000_000_000);
        // sim.RunSimulation(1);
        // return;
        var tester = new GeneratorTester();
        // tester.TestExponentionalGenerator(10000);
        // tester.TestTriangularGenerator(10000);
        tester.TestGammaGenerator(10000);
    }
}