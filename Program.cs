using System;


namespace HAL_ClosestPoint
{
    class ProgUtils{
        //Helper functions stored within an embedded class.
        public static List<Coordinate> VertexSetFromString(string s){
            string[] vertices_in = s.Split(";");
            List<Coordinate> coords = new List<Coordinate>();
                    foreach (string c in vertices_in){
                        string[] xy = c.Split(",");
                        coords.Add(new Coordinate(Double.Parse(xy[0]), Double.Parse(xy[1])));
                    }
            return coords;
        }

        public static (Coordinate, int, double) CPInLineSet(List<Line> lines, Coordinate point)
        {
            List<Coordinate> CPs = new List<Coordinate>();
            double shortestDistance = 0;
            int shortestIndex = 0;
            foreach (Line l in lines){
                (Coordinate thisCP, double thisDist) = l.getClosestPointDistance(point);
                CPs.Add(thisCP);  
                if (thisDist < shortestDistance || shortestDistance == 0 ){
                    shortestDistance = thisDist;
                    shortestIndex = lines.IndexOf(l);
                }
            }
            Coordinate closestCP = CPs[shortestIndex];
            //Console.WriteLine($"{lines.Count} lines were given. The nearest line was line {shortestIndex + 1}, which had a CP at ({closestCP.x_pos}, {closestCP.y_pos}), with a distance {shortestDistance} from the given point.");
            return (closestCP, shortestIndex, shortestDistance);
        }
    
    }

    class TestsCli
    {
        //These can largely be ignored, just tests I'd written to run inside of Main.
        private static void testOne(){
             //Test closest point to a single line
             Console.WriteLine("\nTest One:");
            Line lineOne = new Line(2, 4, 6, 6);
            Coordinate point = new Coordinate(4,1); //Leave as origin for test
            (Coordinate CP, double CPdist) = lineOne.getClosestPointDistance(point);
            Console.WriteLine($"Closest Point to Line is at {CP.X()}, {CP.Y()} at a distance of {CPdist}");
        }

        private static void testTwo(){
            //Test on a pair of lines
            Console.WriteLine("\nTest Two:");
             List<Line> SingleLines = new List<Line>();
            SingleLines.Add(new Line(6,0,4,2));
            SingleLines.Add(new Line(2,1,3,4));
            Coordinate point = new Coordinate(1,3);
            (Coordinate CP, int index, double dist) = ProgUtils.CPInLineSet(SingleLines, point);
            Console.WriteLine($"{SingleLines.Count} lines were given. The nearest line was line {index + 1}, which had a CP at ({CP.X()}, {CP.Y()}), with a distance {dist} from the given point.");
        }

        private static void testThree(){
            //Now try a list of single lines
            Console.WriteLine("\nTest Three:");
            List<Line> SingleLines = new List<Line>();
            SingleLines.Add(new Line(6,0,4,2));
            SingleLines.Add(new Line(2,1,3,4));
            SingleLines.Add(new Line(3,6,9,12));
            SingleLines.Add(new Line(4, 1, 3, 1));
            
            Coordinate point = new Coordinate(4,4);
            (Coordinate CP, int index, double dist) = ProgUtils.CPInLineSet(SingleLines, point);
            Console.WriteLine($"{SingleLines.Count} lines were given. The nearest line was line {index + 1}, which had a CP at ({CP.X()}, {CP.Y()}), with a distance {dist} from the given point.");
        }
        private static void testFour(){
            //Test 4: Generate individual coordinates, pass into a polyline and get CP from a new point to the PL
            Coordinate One = new Coordinate(0,0);
            Coordinate Two = new Coordinate(1,2);
            Coordinate Three = new Coordinate(3,2);
            Coordinate Four = new Coordinate(5,1);
            Polyline liner = new Polyline(new Coordinate[] {One, Two, Three, Four});
            Console.WriteLine(liner.ToString());
            (Coordinate PolyCP, double PolyDist) = liner.getClosestPointDistance(new Coordinate(2,3));
            Console.WriteLine($"{liner.lines.Count} lines were given. The CP is at ({PolyCP.X()}, {PolyCP.Y()}), with a distance {Math.Round(PolyDist, 2)} from the given point.");
        }

        private static void testFive(){
            //Test 5: Recreate drawing as provided
            Polyline pl = new Polyline(new Coordinate[] {new Coordinate(0,2),new Coordinate(1.5,4), new Coordinate(5.5,3), new Coordinate(3,0.3), new Coordinate(7,0), new Coordinate(10,2.4), new Coordinate(10,5)});
            Line l1 = new Line(new Coordinate(1.3, 0.5), new Coordinate(2.6, 2.3));
            Line l2 = new Line(new Coordinate(6, 8), new Coordinate(8, 3));
            Coordinate P = new Coordinate(2.7, 4.5);
            List<Line> lineSet = new List<Line>(){pl, l1, l2};
            (Coordinate CP, int Index, double Dist) = ProgUtils.CPInLineSet(lineSet, P);
             Console.WriteLine($"{lineSet.Count} lines were given. The nearest line was line {Index}, which had a CP at ({CP.X()}, {CP.Y()}), with a distance {Math.Round(Dist, 2)} from the given point.");
        }
    }
    
    class Program
    {
        public static void Main(string[] args){
            //Test 6. Take User Input from CLI
            List<Line> lineSet = new List<Line>();
            int lineCount = 0;
            Console.WriteLine("Welcome to the Demo, how many lines would you like to generate?");
            
            bool validCount = false;
            while(!validCount){
                try
                {
                    string lineCountIn = Console.ReadLine();
                    lineCount = Int32.Parse(lineCountIn);
                    validCount = true;
                }
                catch (System.FormatException)
                {
                    Console.WriteLine("Try again inserting an integer value only please.");
                    throw;
                }    
            }
            for (int i = 0; i < lineCount; i++){
                Console.WriteLine($"What type is line {i+1}? (Line or Polyline)");
                string lineType = Console.ReadLine().ToLower();
                if (lineType == "line"){
                    Console.WriteLine("Please enter coordinates for the line in the format X1,Y1;X2,Y2");
                    List<Coordinate> coords = ProgUtils.VertexSetFromString(Console.ReadLine());
                    Line l = new Line(coords[0], coords[1]);
                    Console.WriteLine(l.ToString());
                    lineSet.Add(l);
                }
                else if (lineType == "polyline") {
                    Console.WriteLine("Please enter the set of vertices for the Polyline in the format X1,Y1;X2;Y2;...Xn;Yn");
                    List<Coordinate> coords = ProgUtils.VertexSetFromString(Console.ReadLine());
                    Polyline pl = new Polyline(coords.ToArray()); //I kept the constructor as an array, in this test a List is more useful for both line and polyline as no fixed size and easier to append a new item
                    Console.WriteLine(pl.ToString());
                    lineSet.Add(pl);
                }
                else{
                    Console.WriteLine("Invalid Format.");
                    i--; //Don't increment i so loops again
                }
            }
            Console.WriteLine("All lines set, please specify a target point for Closest Point calculation");
            Coordinate target = ProgUtils.VertexSetFromString(Console.ReadLine())[0]; //Simplicity of the operation in this case is just to ignore everything beyond the first point we receive 
            Console.WriteLine($"Target point set at ({target.X()},{target.Y()})");
            (Coordinate CP, int Index, double Dist) = ProgUtils.CPInLineSet(lineSet, target);
            Console.WriteLine($"{lineSet.Count} lines were given. The nearest line was line {Index+1}, which had a CP at ({CP.X()}, {CP.Y()}), with a distance {Math.Round(Dist, 2)} from the given point.");
            Console.WriteLine("Thank you for using this tool. Press Space bar to close...");
            while (Console.ReadKey().Key != ConsoleKey.Spacebar){
                //Included so the program hangs and output can be read.
            }
        }       
    }
}