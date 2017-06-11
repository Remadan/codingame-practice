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

        string[] inputs;
        int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
        for (int i = 0; i < surfaceN; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
            int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.

            marsMap.AddPoint(landX, landY);
        }

        // desired horizontal speed 
        int horizontalMaxSpeed = 60; 
        int horizontalCruiseSpeed = 40; // for getting above landing zone
        int horizontalLandingSpeed = 19; // for landing

        // desired vertical speed 
        int verticalMaxSpeed = 10; // for getting above landing zone
        int verticalLandingSpeed = 38; // for landing

        int safeHeightAboveGround = 200; // we shouldn't go lower unless touching down

        int horizontalMoveDefaultAngle = 20;
        int horizontalEmergencyBrakeAngle = 45;

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
            Direction cruiseDirection = marsMap.GetDirectionToLandingZone(currentPosition);
            Position nextPeak = marsMap.GetNextPeak(currentPosition, cruiseDirection);

            int directionalSpeed = hSpeed * (int)cruiseDirection;
            int directionalAngle = rotate * -(int)cruiseDirection;

            bool aboveLandingZone = marsMap.AboveLandingZone(currentPosition);

            int currentHorizLimit = aboveLandingZone ? horizontalLandingSpeed : horizontalMaxSpeed;
            int currentVertLimit = aboveLandingZone ? verticalLandingSpeed : verticalMaxSpeed;

            bool touchMode = aboveLandingZone && marsMap.HeightAboveLanding(currentPosition) < safeHeightAboveGround && directionalSpeed <= currentHorizLimit;
            bool dungerousHeight = (currentPosition.Height - nextPeak.Height < safeHeightAboveGround);

            Console.Error.WriteLine("directionalSpeed: " + directionalSpeed);
            Console.Error.WriteLine("cruiseDirection: " + cruiseDirection);
            Console.Error.WriteLine("directionalAngle: " + directionalAngle);
            Console.Error.WriteLine("dungerousHeight: " + dungerousHeight);

            if (touchMode)
            {
                newAngle = 0;
                newPower = (-vSpeed > currentVertLimit) ? 4: 3;
                Console.Error.WriteLine("Current mode: TOUCH");
            }
            else if (dungerousHeight) // need to get UP
            {
                newAngle = 0;
                newPower = 4;
                Console.Error.WriteLine("Current mode: UP - Height");
            }
            else if (directionalSpeed < 0 || directionalSpeed > currentHorizLimit) // emergency brake
            {
                bool highPriorityBrake = directionalSpeed < 0 || directionalSpeed > horizontalMaxSpeed;
                newAngle = horizontalEmergencyBrakeAngle * (int)cruiseDirection * (directionalSpeed < 0 ? -1 : 1) / (highPriorityBrake ? 1 : 2);

                if (directionalAngle > 60)
                    newPower = 0; // we really don't need more speed
                else
                    newPower = 4;

                Console.Error.WriteLine("Current mode: BRAKE");
            }
            else if (-vSpeed > currentVertLimit) // need to get UP
            {
                newAngle = 0;
                newPower = 4;
                Console.Error.WriteLine("Current mode: UP - vert speed");
            }
            else if (!aboveLandingZone && directionalSpeed < horizontalCruiseSpeed) // cruising to landing zone
            {
                newAngle = horizontalMoveDefaultAngle * -(int)cruiseDirection / (dungerousHeight ? 2 : 1);
                newPower = 4;
                Console.Error.WriteLine("Current mode: CRUISE");
            }
            else // above landing with a height to spare or in cruise
            {
                newAngle = 0;
                if (Math.Abs(rotate) < 45)
                    newPower = 2;
                else
                    newPower = 0;

                Console.Error.WriteLine("Current mode: DESCENT");
            }

            // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
            // To debug: Console.Error.WriteLine("Debug messages...");
            Console.WriteLine(newAngle + " " + newPower);
        }
    }
}

enum Direction
{
    WEST = -1,
    EAST = 1
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

    public Position GetNextPeak(Position currentPosition, Direction cruiseDirection)
    {
        Position maxHeightPeak = new Position(0, 0);

        if (AboveLandingZone(currentPosition))
            return maxHeightPeak;

        foreach (Position point in Points)
        {
            if ((cruiseDirection == Direction.EAST && (point.Longitude < currentPosition.Longitude || point.Longitude > LandingZoneEnd.Longitude)) 
                || (cruiseDirection == Direction.WEST && (point.Longitude > currentPosition.Longitude || point.Longitude < LandingZoneStart.Longitude)))
                continue;

            if (point.Height > maxHeightPeak.Height)
                maxHeightPeak = point;
        }

        return maxHeightPeak;
    }

    public Direction GetDirectionToLandingZone(Position currentPosition)
    {
        int landingLongitude = (LandingZoneStart.Longitude + LandingZoneEnd.Longitude) / 2;

        if (currentPosition.Longitude < landingLongitude)
            return Direction.EAST;
        else
            return Direction.WEST;
    }

    public int HeightAboveLanding(Position currentPosition)
    {
        return currentPosition.Height - LandingZoneStart.Height;
    }
}