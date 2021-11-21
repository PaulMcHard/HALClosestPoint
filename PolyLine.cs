using System;

namespace HAL_ClosestPoint
{
    class Polyline : Line
    //Polyline object inherits from Line object in order to reuse some of the logic in the parent class
    {
        public List<Line> lines;

        public Polyline(Coordinate[] c){
             //Polyline from n coordinates.
             this.setCoords(c);
             this.lines = new List<Line>();
             for (int j = 1; j < c.Length; j++){
                this.lines.Add(new Line(c[j - 1], c[j]));
             }
            }
        
        public override (Coordinate, double) getClosestPointDistance(Coordinate P)
        {
            //Override behaviour of inherited method. 
            List<Coordinate> CPs = new List<Coordinate>();
            double shortestDistance = 0;
            int shortestIndex = 0;
            foreach (Line l in lines){
                (Coordinate thisCP, double thisDist) = l.getClosestPointDistance(P);
                CPs.Add(thisCP);
                //shortestDistance could be nulled to start. 
                //For demo, dist is always +ve, unlikely a true dist is ever exactly 0.
                //Only falls over if multiple lines were to intercept the target point.
                //Not the spec of this exercise.  
                if (thisDist < shortestDistance || shortestDistance == 0 ){
                    shortestDistance = thisDist;
                    shortestIndex = lines.IndexOf(l);
                }
            }
            Coordinate closestCP = CPs[shortestIndex];
            //Console.WriteLine($"{lines.Count} lines were given. The nearest line was line {shortestIndex + 1}, which had a CP at ({closestCP.x_pos}, {closestCP.y_pos}), with a distance {shortestDistance} from the given point.");
            return (closestCP, shortestDistance);
        }

        public override string ToString() => $"This is a polyline made up of {lines.Count} line segments starting from ({lines[0].getStart().X()},{lines[0].getStart().Y()}) and going to ({lines.Last().getEnd().X()},{lines.Last().getEnd().Y()})";
    }
    
}