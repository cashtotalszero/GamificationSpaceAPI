/*Pocket Mission Control - a JA initiative - a cross platform application for
the review and management of personal and professional spacecraft

Questions and comments: support@PocketMissionControl.org

Written in 2012-2013 by:

Ashley Whetter
Claudiu Octavian Soare
Dominic Moylett
Francis Xavier Kwong
Fraser McQueen-Govan
Ioannis Kaoullas
James Savage
Jamie Daniell
Michael Johnson
Nicholas Phillips
Pakizat Masgutova
William Colaluca
Zahra Dasht Bozorgi

To the extent possible under law, the author(s) have dedicated all
copyright and related and neighboring rights to this software to the
public domain worldwide. This software is distributed without any
warranty.

You should have received a copy of the CC0 Public Domain Dedication
along with this software. If not, see
<http://creativecommons.org/publicdomain/zero/1.0/>.

If you contribute to this file and are not listed above, please
complete the following waiver
<http://creativecommons.org/choose/zero/waiver>, email the
confirmation to rights@PocketMissionControl.org and add your name and
email address to the author list. If you are not willing/able to
complete the waiver, please do not contribute to this file.
*/

using UnityEngine;
using System;
using System.Collections;
[System.Serializable]

/////////////////////////////////////////////////////////////////////////////
//THIS CODE IS BASED ON THE PLAN-13 BBC BASIC PROGRAM BY J.R.MILLIER      //  
//IT IS AVALIABLE AT http://WWW.amsat.org/amsat/articles/g3ruh/111.html  //
//PLEASE NOTE THIS IS NOT A COMPLETE AND ACCURATE TRANSLATION           //
/////////////////////////////////////////////////////////////////////////

//stores data from tle string
public class TLE_DATA {
	public int YE; //Epcoh year , year
    public double TE; //epoch time, days
    public double IN; //inclination, deg
    public double RA; //R.A.A.N, deg
    public double EC; //eccentricity, -
    public double WP; //Arg perigee, deg
    public double MA; //Mean anomoly, deg
    public double MM; //Mean motion, rev/day
    public double M2; // deacy rate, rev/day/day
    public int RV; // orbit mumber, -
	
	//define base century for tle epoch
	private static int century = 2000;
	
	//caculate PI to high precision
	private static double PI = 3.14159265359; 
	
	//converts angle in degrees to radians
	double degtorad(double deg) {
        return deg*PI/180;
	}
	
	//convert angles to radians etc
	public void toRads() {
        RA = degtorad(RA);
        MA = degtorad(MA);
        IN = degtorad(IN);
        WP = degtorad(WP);
        MM = MM*2*PI;
        M2 = M2*2*PI;
	}
	// initilaizer for TLE_DATA
	public void TLE_DATA_INIT(string line1, string line2) {
		YE = century + int.Parse (line1.Substring(18,2));
		TE = double.Parse (line1.Substring(20,12));
		IN = double.Parse (line2.Substring(8,8));
		RA = double.Parse (line2.Substring(17,8));
		EC = double.Parse ("0." + line2.Substring(26,7));
		WP = double.Parse (line2.Substring(34,8));
		MA = double.Parse (line2.Substring(43,8));
		MM = double.Parse (line2.Substring(52,11));
		//deal with strange encoding
		string temp = line1.Substring(33,10);
		//if positive
		if(temp[0] == ' '){
			M2 = double.Parse ("0." + temp.Substring(2,8));
		}
		//if negative
		else{
			M2 = double.Parse ("-0." + temp.Substring(2,8));
		}
        
		RV = int.Parse (line2.Substring(63,5));
		//convert angles to radians
		toRads();
	}
}

//a vector in 3D space
class Vector {
	public double x;
    public double y;
    public double z;
	public Vector(double X, double Y, double Z){
		x = X;
		y = Y;
		z = Z;
	}
}

//a collection of Position and Velocity Vectors
class Vectors {
	public Vector position;
	public Vector velocity;
	public Vectors(Vector p, Vector v){
		position = p;
		velocity = v;	
	}
}

//provides Time functionality
class TimeUtils {
	//constants
	static double YM = 365.25; //mean year days
	
	//converts date to day-number
	public int FNday(int year, int month, int day){
        if( month <= 2) {
                year = year - 1;
                month = month + 12;
        }
        double output = year*YM + (month + 1)*30.6 + day-428;
        return Convert.ToInt32(output);
	}
	
	//convert satilite epoch to day No
	public int EpochToDay(TLE_DATA tle){
        return FNday(tle.YE,1,0) + (int)tle.TE;
	}
	
	//get day fraction
	public double EpochToFract(TLE_DATA tle){
        return tle.TE - (int)tle.TE;
	}
	
	//converts unix time to C# DateTime object
	private DateTime FromUnixTime(long unixTime){
	    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	    return epoch.AddSeconds(unixTime);
	}
	
	//converts unix time to amsat Day number
	public int UnixTimeToDayNumber(int t){
		DateTime tm = FromUnixTime(t); 
		int year = tm.Year;
		int month = tm.Month;
		int day = tm.Day;
        return FNday(year,month,day);
	}
	
	//converts unix time to amsat day fraction
	public double UnixTimeToDayFrac(int t){
		DateTime tm = FromUnixTime(t);
		double hour = tm.Hour;
		double min = tm.Minute;
		double output = (hour + (min/60))/24;
        return output;
	}
}

//does most of the calculations
public class YAMTRAK{
	TimeUtils timeutil = new TimeUtils();
	//Stores Result of calculation
	Vectors results = null;
	//convert angles to radians etc
	static double PI = Math.PI; //caculate PI to high precision
	//static double YT = 365.2421874;  //Tropical year days
	//static double WW = 2*PI/YT;  //earths rotation rate, rad/whole day
	//static double WE = 2*PI + WW;  //earths rotation rate, rad/day
	//static double WO = WE/86400;  //earths rotation rate, rad/sec
	
	//define some constants
	static double GM = 3.986E5; //Earth's Gravitational constant km^3/s^2
	static double J2 = 1.08263E-3; //2nd Zonal coeff, Earth's Gravity Field
	
	//define WGS-84 earth ellipsoid
	static double RE = 6378.137;
	//static double FL = 1/298.257224;
	
	//converts angle in degrees to radians
	double degtorad(double deg){
        return deg*PI/180;
	}
	
	
	//calculate stalite position at DN, TN
	private Vectors satvec(TLE_DATA sat,int DN, double TN){
        //calculate average precession rates
        double N0 = sat.MM/86400; //Mean Motion rad/s
        double A0 = Math.Pow((GM/N0/N0),(1.0/3.0)); //semi major axis km
        double B0 = A0*Math.Sqrt(1 - sat.EC*sat.EC); //semi minor axis km
        double SI = Math.Sin(sat.IN); 
		double CI = Math.Cos(sat.IN);
        double PC = RE*A0/(B0*B0); //precession constant rad/day
        PC = 1.5*J2*PC*PC*sat.MM; //precession constant rads/day
        double QD = -PC*CI; //precession rate rad/day
        double WD = PC*(5*CI*CI-1)/2; //Perigee precession rate rad/day
        double DC = -2*(sat.M2)/sat.MM/3; //REM Drag coeff. (Angular momentum rate)/(Ang mom)  s^-1
        
        //Sidereal and Solar data. NEVER needs changing. valid until year ~2015
        //double YG = 2000;
		//double G0 = 98.9821; //GHAA, year YG Jan 0.0
        
		// UPDATED BY ALEX: Sidereal and Solar data. Valid to year ~2030
		double YG = 2014;
		double G0 = 99.5828; //GHAA, year YG Jan 0.0


        //Bring sun data to satellite epoch
        double TEG = (timeutil.EpochToDay(sat) - timeutil.FNday(Convert.ToInt32(YG),1,0)) + timeutil.EpochToFract(sat); //Elapsed Time: Epoch - YG
        //there is a wired bug in this line fixed by defining WE explicitly
        double GHAE = degtorad(G0) + TEG*6.300388; //GHA Aries, epoch
        //Time
        double T = (DN - timeutil.EpochToDay(sat)) + (TN - timeutil.EpochToFract(sat)); //Elapsed Time since epoch, days
        
        //Linear drag terms
        double DT = DC*T/2;
        double KD = 1 + 4*DT;
        double KDP = 1 - 7*DT;
        double M  = sat.MA + sat.MM*T*(1- 3*DT); //Mean anomaly at YR,TN
        //orbit number
        int DR = Convert.ToInt32(M/(2*PI)); //strip out whole number of revs;
        M  = M - DR*2*PI; //M now in range 0 - 2pi
        //double RN = sat.RV + DR; //current orbit number

        //solve M = EA - EC*Sin(EA) for EA givne M, using newtons methood
        double EA = M;  //Initial solution
        //declare here for use later
        double D = 1;
        double DNOM = 0; 
        double C = 0;
        double S = 0;
        while((Math.Abs(D) >= 1e-5)){
                C = Math.Cos(EA);
                S = Math.Sin(EA);
                DNOM = 1 - sat.EC*C;
                D = (EA- (sat.EC*S) -M)/DNOM; //change to EA for better solution
                EA = EA - D; //by this ammount
        }
        
        //distances
        double A = A0*KD;
        double B = B0*KD;
        //double RS = A*DNOM;
        
        //calculate satalite position and velocity in plane of elipse
        double Sx = A*(C - sat.EC);
        double Sy = B*S;
        double Vx = -A*S/DNOM*N0;
        double Vy = B*C/DNOM*N0;
        double AP = sat.WP + WD*T*KDP;
        double RAAN = sat.RA + QD*T*KDP;
        
        double CW = Math.Cos(AP);
        double CQ = Math.Cos(RAAN);
        double SW = Math.Sin(AP);
        double SQ = Math.Sin(RAAN);
        
        //plane . celestial coordinate transformtation, [C] = [RAAN]*[IN]*[AP]
        double CXx = CW*CQ - SW*CI*SQ;
        double CYx = CW*SQ + SW*CI*CQ;
        double CZx = SW*SI;
        
        double CXy = -SW*CQ - CW*CI*SQ;
        double CYy = -SW*SQ + CW*CI*CQ;
        double CZy = CW*SI;
        
        //double CXz = SI*SQ;
        //double CYz = -SI*CQ;
        //double CZz = CI;
		
        //Compute satalites position vector and velocity in clestial coordinates
        double SATx = Sx*CXx + Sy*CXy;
        double SATy = Sx*CYx + Sy*CYy;
        double SATz = Sx*CZx + Sy*CZy;
        
        double VELx = Vx*CXx + Vy*CXy;
        double VELy = Vx*CYx + Vy*CYy;
        double VELz = Vx*CZx + Vy*CZy;
        
        //express position and velocity in Geocentric coordinates
        //weired WE def bug also in this line fixed with explicit definition
        double GHAA = GHAE + 6.300388*T; //GHA Aries at elapsed time T
        C = Math.Cos(-GHAA);
        S = Math.Sin(-GHAA);
		
        Sx = SATx*C - SATy*S;
        Sy = SATx*S + SATy*C;
        double Sz = SATz;
        
        Vx = VELx*C - VELy*S;
        Vy = VELx*S + VELy*C;
        double Vz = VELz;       
        
		Vector pos = new Vector(Sx, Sy, Sz);
		Vector vel = new Vector(Vx, Vy, Vz);;
		Vectors output = new Vectors(pos, vel);
        return output;
	}
	
	//perform calculation at time for given TLE lines
	//if you can get at objects from the JS use the commented version
	//this will make GetPos() and GetVel redundant
	
	/*public Vectors calculate(int time, string line1, string line2){
		TLE_DATA sat = new TLE_DATA();
		sat.TLE_DATA_INIT(line1, line2);
		return satvec(sat, timeutil.UnixTimeToDayNumber(time),timeutil.UnixTimeToDayFrac(time));
	}*/
	
	public void calculate(int time, string line1, string line2){
		TLE_DATA sat = new TLE_DATA();
		sat.TLE_DATA_INIT(line1, line2);
		results =  satvec(sat, timeutil.UnixTimeToDayNumber(time),timeutil.UnixTimeToDayFrac(time));
	}
	
	//returns array of floats {x,y,z} represnting location of spacecraft in Geocentric coords
	public double[] GetPos(){
		double[] output = {results.position.x, results.position.y, results.position.z };
		//Debug.Log ("X Pos = " + results.position.x);
		//Debug.Log ("Y Pos = " + results.position.y);
		//Debug.Log ("Z Pos = " + results.position.z);
		return output;
	}
	
	//returns array of floats {x,y,z} represnting velocity of spacecraft in Geocentric coords
	public double[] GetVel(){
		double[] output = {results.velocity.x, results.velocity.y, results.velocity.z };
		return output;
	}
}