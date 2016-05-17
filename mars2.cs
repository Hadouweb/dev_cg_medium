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
 
public static class NumericExtensions
{
    public static double ToRadians(this double val)
    {
        return (Math.PI / 180) * val;
    }
}  
 
class Player
{
    static void Main(string[] args)
    {
        string[] inputs;
        int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
        int ply = -1;
        int plx = -1;
        int spX, spY;
        int spXmin = int.MaxValue/4;
        int spXmax = int.MinValue/4;
        for (int i = 0; i < surfaceN; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
            int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
            if (ply == landY) {
                if (spXmin > landX) spXmin=landX;
                if (spXmax < landX) spXmax=landX;
                if (spXmin > plx) spXmin=plx;
                if (spXmax < plx) spXmax=plx;
                spY = landY;
            }
            ply = landY;
            plx = landX;
        }
        
        spX = (spXmin + spXmax)/2;
        
        int dlx = spXmax - spXmin;
        int tlx = dlx/4;
        
        double g=3.711;
        int ag0 = (int)(Math.Acos(g/4)*180/Math.PI);
        
        //As there is no mass nor angular inertia information, let's justjust use tweaking coeficient for horizontal speed / distance.
        int step=0;
        
        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int X = int.Parse(inputs[0]);
            int Y = int.Parse(inputs[1]);
            int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
            int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
            int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
            int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
            int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

            // Write an action using Console.WriteLine()

            int thrust = 0;
            
            //The force of gravity, g = 3.711 m/s2 ...
            //Time to splat: sqrt ( 2 * height / 9.8 ) ...
            //Velocity at splat time: sqrt( 2 * g * height ) ...
            
            //t = [ −vi ± √(vi2 + 2gy) ]/g
            
            double ts = (Math.Sqrt(vSpeed*vSpeed+2*g*Y)+vSpeed)/g;
            double vf=g*ts-vSpeed;
            
            int a=0;
            int dx = spX-X;
            double dvx = (hSpeed==0)? 0 : (double)dx/hSpeed;
            double dvy = (dvx<0)? 0 : dvx*vSpeed;
            double cdy = (Y + dvy - spY) / 200; 
            int avY = (int)(ag0-2+((cdy>10)? 10 : ((cdy<-10)? -10 : cdy)));
            
            double ah = Math.Sin(((double)avY).ToRadians()) * 4;
            
            switch(step) {
                case 0: // go toward landing spot.
                double rc = hSpeed*hSpeed-1.8*Math.Abs(ah*dx);
                if (rc > -5.0*Math.Abs(rc)*Math.Abs(hSpeed)/Math.Abs(dx)) {
                    step=1;
                    goto case 1;
                }
                step_02:
                a = avY * ((dx>0)? -1:1);
                thrust=4;
                break;
                
                case 1:
                if (Math.Abs(hSpeed)<3) {
                    if (Math.Abs(dx-hSpeed*ts) < tlx) {
                        step=2;
                        goto case 2;
                    } else {
                        step=0;
                        goto step_02;
                    }
                }
                //a = -2*aMax*Dir;
                a = avY*((hSpeed>0)? 1:-1);
                thrust=4;
                break;
                
                case 2:
                a=0;
                thrust = (vSpeed<=-32)? 4 : 0;
                break;
            }
    
            Console.Error.WriteLine("dx={0} spx={1} vh2={2} rc={3} dvy={4} cdy={5}", dx, spX, hSpeed*hSpeed, 1.8*Math.Abs(ah*dx), dvy, cdy);
            Console.Error.WriteLine("s={0} ts={1} vf={2} vi={3} th={4}", step, ts, vf,-vSpeed, thrust);
            
            if (thrust<0) { 
                thrust = 0;
            } else if (thrust>4) {
                thrust = 4;
            }
            Console.WriteLine(string.Format("{0} {1}", a, thrust)); // 2 integers: rotate power. rotate is the desired rotation angle (should be 0 for level 1), power is the desired thrust power (0 to 4).
            //Console.WriteLine("-90 4"); // 2 integers: rotate power. rotate is the desired rotation angle (should be 0 for level 1), power is the desired thrust power (0 to 4).
        }
    }
}