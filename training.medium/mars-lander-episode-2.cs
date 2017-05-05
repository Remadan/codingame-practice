using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static void Main(string[] args)
    {
        MarsMap marsMap = new MarsMap();

        const double marsGravity = -3.711;

        string[] inputs;
        int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
        for (int i = 0; i < surfaceN; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
            int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.

            marsMap.AddPoint(landX, landY);
        }
        Console.Error.WriteLine(marsMap.LandingZoneStart);
        Console.Error.WriteLine(marsMap.LandingZoneEnd);

        // horizontal move - accelerating, cruise speed, decelerating 
        // vertical move - free fall, decelerating, land

        // A = acos(3.711 / 4) = acos(0.9275) = 21.9 from https://forum.codingame.com/t/mars-lander-puzzle-discussion/32/14
        int horizontalMoveDefaultAngle = 21; 

        // game loop
        while (true)
        {
            int newAngle = 0;
            int newPower = 0;

            inputs = Console.ReadLine().Split(' ');
            int X = int.Parse(inputs[0]);
            int Y = int.Parse(inputs[1]);
            int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
            int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
            int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
            int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
            int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

            Position currentPosition = new Position(X, Y);
            if (marsMap.AboveLandingZone(currentPosition))
            {
                // vertical move

                // do we need to stop?
                if (Math.Abs(hSpeed) > 10)
                {
                    newAngle = 90 * ((hSpeed > 0) ? 1 : -1);
                    newPower = 4;
                }
                else
                {
                    newAngle = 0;

                    newPower = 500;

                    if (marsMap.HeightAboveLanding(currentPosition) > 200)
                    {
                        newPower = 0;
                    }
                    else
                    {
                        newPower = 4;
                    }
                }

            }
            else
            {
                // horizontal move
                int cruiseDirection = (marsMap.LandingZoneStart.Longitude < currentPosition.Longitude) ? 1 : -1; // 1 - west, -1 - east

                // how fast are we going?
                if (hSpeed> 60) // to fast
                {

                }

                // which direction?
                newAngle = horizontalMoveDefaultAngle * cruiseDirection;


                newPower = 4;




            }


            // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
            // To debug: Console.Error.WriteLine("Debug messages...");
            Console.WriteLine(newAngle + " " + newPower);
        }
    }
}

struct Position
{
    public int Height { get; set; }
    public int Longitude { get; set; }

    public Position(int latitude, int height)
    {
        Height = height;
        Longitude = latitude;
    }
}

class MarsMap
{
    public List<Position> Points { get; set; }
    public Position LandingZoneStart { get; set; }
    public Position LandingZoneEnd { get; set; }

    public MarsMap()
    {
        Points = new List<Position>();
        LandingZoneStart = new Position(0, 0);
        LandingZoneEnd = new Position(0, 0);
    }

    public void AddPoint(int x, int y)
    {
        Points.Add(new Position(x, y));
        FindLandingZone();
    }

    private void FindLandingZone()
    {
        if (!(LandingZoneStart.Longitude == 0 && LandingZoneEnd.Longitude == 0))
            return;

        Position prevPoint = new Position(-1, -1);
        foreach (Position point in Points)
        {
            if (prevPoint.Height == point.Height)
            {
                LandingZoneStart = prevPoint;
                LandingZoneEnd = point;
                break;
            }
            else
                prevPoint = point;
        }
    }

    public bool AboveLandingZone(Position currentPosition)
    {
        return (currentPosition.Longitude >= LandingZoneStart.Longitude && currentPosition.Longitude <= LandingZoneEnd.Longitude);
    }

    public int HeightAboveLanding(Position currentPosition)
    {
        return currentPosition.Height - LandingZoneStart.Height;
    }
}