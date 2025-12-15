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

            Console.Write("-------------------"); // Macht Linie als Deko
            Console.ForegroundColor = ConsoleColor.Red; // Schriftfarbe rot
            Console.Write("Der Random Dungeon Generator"); // Titel
            Console.ResetColor(); // Farbe wieder normal
            Console.WriteLine("-------------------"); // Zweite Linie

            ErstelleDungeon(hoehe, laenge); // Ruft die Dungeon-Funktion auf, mit Höhe und Länge
            Console.ReadKey(); 



        }







        static void ErstelleDungeon(int hoehe, int laenge)
        {
            char[,] dungeon = new char[hoehe, laenge]; // 2D. Array für Dungeon
            Random rand = new Random(); // Random wird erstellt

            // Array mit '#' füllen
            for (int i = 0; i < hoehe; i++)
                for (int j = 0; j < laenge; j++)
                    dungeon[i, j] = '#';

            // Startpunkt koordinaten zufällig im Dungeon
            int startX = rand.Next(1, hoehe - 1);
            int startY = rand.Next(1, laenge - 1);

            // Endpunkt auch zufällig, aber nicht gleich wie Start
            int endeX, endeY;
            do
            {
                endeX = rand.Next(1, hoehe - 1);
                endeY = rand.Next(1, laenge - 1);
            } while (endeX == startX && endeY == startY);

            dungeon[startX, startY] = 'S'; // Start mit 'S' markieren
            dungeon[endeX, endeY] = 'E';   // Ende mit 'E' markieren

            int x = startX; 
            int y = startY; 

            // Die Linie muss bis zum Ende gehen
            while (x != endeX || y != endeY)
            {
                bool horizontal = rand.Next(2) == 0; // Zufällig ob wir horizontal oder vertikal gehen

                if (horizontal)
                {
                    if (y < endeY) y++; // nach rechts
                    else if (y > endeY) y--; // nach links
                }
                else
                {
                    if (x < endeX) x++; // runter
                    else if (x > endeX) x--; // hoch
                }

                // Nur Punkte setzen, wenn nicht Start oder Ende
                if (!(x == startX && y == startY) && !(x == endeX && y == endeY))
                    dungeon[x, y] = '.'; // Weg markieren
            }

            // Dungeon ausgeben
            for (int i = 0; i < hoehe; i++)
            {
                Console.Write("    "); // kleine Einrückung
                for (int j = 0; j < laenge; j++)
                {
                    if (dungeon[i, j] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Green; // Start grün
                        Console.Write("S");
                        Console.ResetColor();
                    }
                    else if (dungeon[i, j] == 'E')
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Ende rot
                        Console.Write("E");
                        Console.ResetColor();
                    }
                    else
                        Console.Write(dungeon[i, j]); // Rest normal ausgeben
                }
                Console.WriteLine(); // neue Zeile
            }


        }










        //  Eingaben

        static int EingabeH()
        {
            while (true) // Dauerschleife bis gültige Eingabe kommt
            {
                try
                {
                    Console.Write("Bitte geben Sie die Höhe (Min. 10, Max 25) des RDG an: ");
                    int hoehe = Convert.ToInt32(Console.ReadLine()); // Eingabe lesen und Zahl draus machen

                    if (hoehe >= 10 && hoehe <= 25) // Gucken ob Zahl im Bereich
                        return hoehe; // passt dann zurückgeben
                    else
                        Console.WriteLine("Die Höhe muss zwischen 10 und 25 liegen."); // Fehlermeldung
                }
                catch
                {
                    Console.WriteLine("Ungültige Eingabe, bitte eine Zahl eingeben."); // wenn kein Zahl eingegeben wurde
                }
            }
        }

        static int EingabeL()
        {
            while (true) // gleich wie bei Höhe
            {
                try
                {
                    Console.Write("Bitte geben Sie die Länge (Min. 10, Max. 50) des RDG an: ");
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
