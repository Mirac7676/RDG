using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDG
{
    class Program
    {
        // Zufallsgenerator für die Erzeugung zufälliger Werte
        static Random zufall = new Random();

        // Zeichen für verschiedene Objekte auf der Karte
        static char wand = '#';    // Wand zeichen
        static char gang = '.';    // Gang zeichen
        static char Start = 'S';   // Startpunkt zeichen
        static char End = 'E';     // Endpunkt zeichen
        static char Falle = 'F';   // Falle zeichen
        static char Schatz = 'T';  // Schatz zeichen

        static void Main(string[] args)
        {
            // Eingabe der Höhe und Länge für die Karte
            int Hoehe = EingabeH();  // vertikale Höhe der Karte
            int Laenge = EingabeL(); // horizontale Länge der Karte 

            // Ausgabe des Titels
            Console.Write("-------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Der Random Dungeon Generator");
            Console.ResetColor();
            Console.WriteLine("-------------------");

            // Erstellen der Karte mit der angegebenen Länge und Höhe
            char[,] Karte = Karte_erstellen(Laenge, Hoehe);

            // Start- und Endpunkte auf der Karte platzieren
            Start_Ende(Karte, Laenge, Hoehe, out int Startx, out int Starty, out int Endx, out int Endy);

            // Ausgabe der Karte in der Konsole
            for (int y = 0; y < Hoehe; y++)
            {
                for (int x = 0; x < Laenge; x++)
                {
                    char Farbe = Karte[y, x]; // Aktuelles Zeichen auf der Karte

                    // Setze die Farbe der Konsole je nach Symbol (Start = grün, Ende = rot)
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
                        Console.ResetColor(); // Für alle anderen Zeichen die Standardfarbe
                    }

                    // Zeichen auf der Konsole ausgeben
                    Console.Write(Farbe);
                    Console.ResetColor(); // Reset der Farbe nach jeder Ausgabe
                }
                Console.WriteLine(); // Zeilenumbruch nach einer Reihe
            }

            // Warten auf eine Tasteneingabe, damit das Programm nicht sofort schließt
            Console.ReadKey();
        }

        // Methode zur Eingabe der Höhe der Karte
        static int EingabeH()
        {
            while (true)
            {
                try
                {
                    // Benutzer nach der Höhe der Karte fragen
                    Console.Write("Bitte geben Sie die Höhe des RDG an: ");
                    int Hoehe = Convert.ToInt32(Console.ReadLine());

                    // Überprüfen, ob die Höhe im gültigen Bereich liegt
                    if (Hoehe >= 10 && Hoehe <= 25)
                        return Hoehe; // Gültige Eingabe, Wert zurückgeben
                    else
                        Console.WriteLine("Die Höhe muss zwischen 10 und 25 liegen."); // Fehlermeldung
                }
                catch
                {
                    // Fehlerbehandlung für ungültige Eingaben
                    Console.WriteLine("Ungültige Eingabe, bitte eine Zahl eingeben.");
                }
            }
        }

        // Methode zur Eingabe der Länge der Karte
        static int EingabeL()
        {
            while (true)
            {
                try
                {
                    // Benutzer nach der Länge der Karte fragen
                    Console.Write("Bitte geben Sie die Länge des RDG an: ");
                    int laenge = Convert.ToInt32(Console.ReadLine());

                    // Überprüfen, ob die Länge im gültigen Bereich liegt
                    if (laenge >= 10 && laenge <= 50)
                        return laenge; // Gültige Eingabe, Wert zurückgeben
                    else
                        Console.WriteLine("Die Länge muss zwischen 10 und 50 liegen."); // Fehlermeldung
                }
                catch
                {
                    // Fehlerbehandlung für ungültige Eingaben
                    Console.WriteLine("Ungültige Eingabe, bitte eine Zahl eingeben.");
                }
            }
        }

        // Methode zur Erstellung der Karte mit Wänden
        static char[,] Karte_erstellen(int Laenge, int Hoehe)
        {
            // Erstellen eines zweidimensionalen Arrays (Karte)
            char[,] Karte = new char[Hoehe, Laenge];

            // Füllen der Karte mit Wänden ('#')
            for (int y = 0; y < Hoehe; y++)
            {
                for (int x = 0; x < Laenge; x++)
                {
                    Karte[y, x] = wand; // Setze jede Zelle auf eine Wand
                }
            }

            return Karte; // Rückgabe der generierten Karte
        }

        // Methode zur Platzierung des Start- und Endpunkts auf der Karte
        static void Start_Ende(char[,] Karte, int Laenge, int Hoehe, out int Startx, out int Starty, out int Endx, out int Endy)
        {
            // Mindestabstand zwischen Start- und Endpunkt für ein realistischeres Labyrinth
            int Mindestabstandy = zufall.Next(1, Math.Max(2, Hoehe / 5));
            int Mindestabstandx = zufall.Next(1, Math.Max(2, Laenge / 5));

            // Zufällige Positionen für den Startpunkt (S)
            Starty = zufall.Next(1, Hoehe - 1);
            Startx = zufall.Next(1, Laenge - 1);
            Karte[Starty, Startx] = Start; // Setze Startpunkt auf der Karte

            // Platzierung des Endpunkts (E), wobei der Mindestabstand zum Start eingehalten wird
            do
            {
                // Zufällige Positionen für den Endpunkt (E)
                Endy = zufall.Next(1, Hoehe - 1);
                Endx = zufall.Next(1, Laenge - 1);
            } while (Math.Abs(Endy - Starty) < Mindestabstandy || Math.Abs(Endx - Startx) < Mindestabstandx);

            // Endpunkt auf der Karte setzen
            Karte[Endy, Endx] = End;
        }

        static void Schatz_Falle(char[,] Karte, int Laenge, int Hoehe)
        {
            for (int y = 0; y < Hoehe; y++)
            {
                for (int x = 0; x < Laenge; x++)
                {
                    if (Karte[y, x] == gang)
                    {
                        int Zahl = zufall.Next(1, 101);

                        if (Zahl <= 5)
                        {
                            int Zahl2 = zufall.Next(0, 2);
                            if (Zahl2 == 0)
                            {
                                Karte[y, x] = Schatz;
                            }
                            else
                            {

                                Karte[y, x] = Falle;
                            }
                        
                        }
                            
                    
                    }
                }
            }


        }

    }
}