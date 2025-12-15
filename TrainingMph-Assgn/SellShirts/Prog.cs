using System;

public struct Time
{
    private readonly int minutes;

    public Time(int hh, int mm)
    {
        this.minutes = 60 * hh + mm;
    }

    public int Hour
    {
        get { return minutes / 60; }
    }

    public int Minute
    {
        get { return minutes % 60; }
    }


    public override string ToString()
    {
        //return minutes.ToString();   //--> this is for 1.1
        return String.Format("{0:D2}:{1:D2}", Hour, Minute);   //---> this is for 1.2
    }


    // Question 1.3
    public Time(int minutes)
    {
        this.minutes = minutes;
    

    public static Time operator +(Time t1, Time t2)
    {
        return new Time(t1.minutes + t2.minutes);
    }

    public static Time operator -(Time t1, Time t2)
    {
        return new Time(t1.minutes - t2.minutes);
    }


    // Question 1.4
    public static implicit operator Time(int minutes)
    {
        return new Time(minutes);
    }
    public static explicit operator int(Time t)
    {
        return t.minutes;
    }

}

class Program
{
    static void Main(string[] args)
    {
        //Time t1 = new Time(10, 5);   
        //Time t2 = new Time(0, 45);   
        //Time t3 = new Time(23, 30);  

        //Console.WriteLine(t1);
        //Console.WriteLine(t2);
        //Console.WriteLine(t3);

        //Console.WriteLine(t3.Hour);     // Question 1.2
        //Console.WriteLine(t3.Minute);   //  Question 1.2


        /// Question 1.3
        Time t1 = new Time(9, 30);

        Console.WriteLine(t1 + new Time(1, 15));
        Console.WriteLine(t1 - new Time(1, 15));

        //Question 1.4
        Time t2 = 120;          
        int m1 = (int)t1;       

        Console.WriteLine("t1={0} and t2={1} and m1={2}", t1, t2, m1);

        Time t3 = t1 + 45;      
        Console.WriteLine("t3={0}", t3);

        Console.ReadLine();
    }
}
