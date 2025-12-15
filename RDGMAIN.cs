using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDG
{
   
    class Program
    {
                
        static Random zufall = new Random();
        static char wand = '#';
        static char gang = '.';
        static char Start = 'S';
        static char End = 'E';
        static char Falle = 'F';
        static char Schatz = 'T';

        static void Main(string[] args)
        {
            int Hoehe = EingabeH();
            int Laenge = EingabeL();

            Console.Write("-------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Der Random Dungeon Generator");
            Console.ResetColor();
            Console.WriteLine("-------------------");
            char[,]Karte = Karte_erstellen(Laenge , Hoehe);

            Start_Ende(Karte, Laenge, Hoehe, out int Startx, out int Starty, out int Endx, out int Endy);
            for (int y = 0; y < Hoehe; y++)
            {
                for (int x = 0; x < Laenge; x++)
                {
                    char Farbe = Karte[y,x];
                    if (Farbe == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (Farbe == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                    Console.Write(Farbe);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        static int EingabeH()
        {
            while (true)
            {
                try
                {
                    Console.Write("Bitte geben Sie die Höhe des RDG an: ");
                    int Hoehe = Convert.ToInt32(Console.ReadLine());

                    if (Hoehe >= 10 && Hoehe <= 25)
                        return Hoehe;
                    else
                        Console.WriteLine("Die Höhe muss zwischen 10 und 25 liegen.");
                }
                catch
                {
                    Console.WriteLine("Ungültige Eingabe, bitte eine Zahl eingeben.");
                }
            }
        }

        static int EingabeL()
        {
            while (true)
            {
                try
                {
                    Console.Write("Bitte geben Sie die Länge des RDG an: ");
                    int laenge = Convert.ToInt32(Console.ReadLine());

                    if (laenge >= 10 && laenge <= 50)
                        return laenge;
                    else
                        Console.WriteLine("Die Länge muss zwischen 10 und 50 liegen.");
                }
                catch
                {
                    Console.WriteLine("Ungültige Eingabe, bitte eine Zahl eingeben.");
                }
            }
        }

        static char[,] Karte_erstellen(int Laenge, int Hoehe)
        {
            char[,] Karte = new char[Hoehe, Laenge];
            for (int y = 0; y < Hoehe; y++)
            {
                for (int x = 0; x < Laenge; x++)
                {
                    Karte[y, x] = wand;

                }
            }
            return Karte;
        }
        static void Start_Ende(char[,] Karte, int Laenge, int Hoehe, out int Startx,out int Starty,out int Endx,out int Endy)
        {
            int Mindestabstandy = zufall.Next(1, Math.Max(2, Hoehe / 5));
            int Mindestabstandx = zufall.Next(1, Math.Max(2, Laenge / 5));
            Starty = zufall.Next(1,Hoehe - 1);
            Startx = zufall.Next(1,Laenge - 1);
            Karte[Starty, Startx] = Start;
          do
          {
           Endy = zufall.Next(1, Hoehe - 1);
           Endx = zufall.Next(1, Laenge - 1);
          } while (Math.Abs(Endy - Starty) < Mindestabstandy || Math.Abs(Endx - Startx) < Mindestabstandx) ;
           Karte[Endy, Endx] = End;
            
           
        }
        Console.Readkey();
    }
}
    
