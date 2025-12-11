using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDGMAIN
{

            class Program
        {
            static void Main(string[] args)
            {
                        
            int hoehe = EingabeH();     // Eingabe der Höhe
            int laenge = EingabeL();    // Eingabe der Länge

            Console.Write("-------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Der Random Dungeon Generator");
            Console.ResetColor();
            Console.WriteLine("-------------------");

            ErstelleDungeon(hoehe, laenge);
            Console.ReadKey();



        }







        static void ErstelleDungeon(int hoehe, int laenge)      // Methode mit zweidimensionalem Arrai
        {
            char[,] dungeon = new char[hoehe, laenge];          // Zwei Dimensionalen arrai erstellen
            Random rand = new Random();

            int startX, startY, endeX, endeY;                   // Koordinaten Erstellen als int

            // Startpunkt Zufällig Generieren bzw die int einen wert geben
            startX = rand.Next(1, hoehe - 1);
            startY = rand.Next(1, laenge - 1);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Endpunkt Zufällig Generieren bzw die int einen wert geben
            do
            {
                endeX = rand.Next(1, hoehe - 1);
                endeY = rand.Next(1, laenge - 1);
            } while (endeX == startX && endeY == startY);      // Danit Ende niht auf Start gesetzt wird

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Dungeon füllen
            for (int i = 0; i < hoehe; i++)
            {
                for (int j = 0; j < laenge; j++)
                {
                    dungeon[i, j] = '#';   // einfach überall Wand setzen
                }
            }


            dungeon[startX, startY] = 'S';          // Start und Ende an die Koordinaten Platzieren
            dungeon[endeX, endeY] = 'E';

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
            // Ausgabe von S und E
            for (int i = 0; i < hoehe; i++)
            {
                Console.Write("    ");
                for (int j = 0; j < laenge; j++)
                {
                    if (dungeon[i, j] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('S');
                        Console.ResetColor();
                    }
                    else if (dungeon[i, j] == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('E');
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(dungeon[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }
       
        
        
        
        
        
        
        
        //  Eingaben
       
        static int EingabeH()
        {
            while (true)
            {
                try
                {
                    Console.Write("Bitte geben Sie die Höhe des RDG an: ");
                    int hoehe = Convert.ToInt32(Console.ReadLine());

                    if (hoehe >= 10 && hoehe <= 25)
                        return hoehe;
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
    }
}

