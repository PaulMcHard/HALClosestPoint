using System;


namespace HAL_ClosestPoint
{
    class Line
    {
      
        private Coordinate[] coords;
        private double[] vector;
       public Line(){
            //Setting the default as a 2D [1 1] unit vector in each X,Y
            coords = new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 1)};
            vector = new double[] {coords[1].X() - coords[0].X(), coords[1].Y() - coords[0].Y() };
       } 
       public Line(Coordinate start, Coordinate end){
            //Construtor using Start and End Coordinates
            coords = new Coordinate[] { start, end };
            vector = new double[] { end.X() - start.X(), end.Y() - start.Y() };
            }
    
       public Line(double x1, double y1, double x2, double y2){
           //Overload constructor to take 4 ints as input instead.
           coords = new Coordinate[] {new Coordinate(x1,y1), new Coordinate(x2,y2)}; 
           vector = new double[] {x2 - x1, y2 - y1};
       } 
        //Accessor and Mutator Methods
        public Coordinate getStart() {   return coords[0]; }
        public Coordinate getEnd() {    return coords[1]; }
        public Coordinate[] GetCoordinates() { return coords; }
        public double[] getVector() { return vector; }
        public void setCoords(Coordinate[] c) { this.coords = c; }

        public virtual (Coordinate, double) getClosestPointDistance(Coordinate P)
        {
            //this Line, closest point to P
            Coordinate A = this.getStart();
            Line AP = new Line(A, P);

            //Squared Magnitude of AB
            double ABSquare = Math.Pow(this.vector[0], 2) + Math.Pow(this.vector[1], 2);
            
            //Dot Product of AP and AB 
            double dotProduct = (AP.vector[0] * this.vector[0]) + (AP.vector[1] * this.vector[1]);

            //Normalise the distance from A to our CP
            double normalisedDelta = dotProduct / ABSquare;

            Coordinate CP = new Coordinate(A.X() + (normalisedDelta * this.vector[0]), A.Y() + (normalisedDelta * this.vector[1]));
            
            //Point to CP distance
            double shortestDistance = Math.Sqrt( Math.Pow( (P.X() - CP.X()) ,2) + Math.Pow( (P.Y() - CP.Y()) , 2) );

            return (CP, shortestDistance);
        }
        public override string ToString() => $"Line from ({coords[0].X()}, {coords[0].Y()}) to ({coords[1].X()}, {coords[1].Y()})";
    }
}