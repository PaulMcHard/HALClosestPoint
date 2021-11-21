using System;


namespace HAL_ClosestPoint
{class Coordinate
    //Coordinate object is our basic point on a 2D Cartesian Plane. 
    {
       private double x { get; } //X Coordinate
       private double y { get; } //Y Coordinate

       public Coordinate(){
           //Default Constructor
           x = 0;
           y = 0;
       }

       public Coordinate(double x_in, double y_in){
           x = x_in;
           y = y_in;
       } 

       //Providing Accessor but not mutator functions. Design intent is that a point is set once created for the purposes of the exercise.
        public double X{ get {return x;} }
        public double Y{ get {return y;} }

       public override string ToString() => $"{x}, {y}"; //Override inherited method from System.Object Class

    }
}