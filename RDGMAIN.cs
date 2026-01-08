using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RDGMAIN
{
    class Program
    {
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
            int minNebenwege = 7 + (hoehe - 10) * 13 / 15;
            int maxNebenwege = 14 + (laenge - 10) * 20 / 40;

            // Sicherstellen, dass min <= max
            int min = Math.Min(minNebenwege, maxNebenwege);
            int max = Math.Max(minNebenwege, maxNebenwege);

            // Zufällige Anzahl an Nebenwegen bestimmen
            int anzahlNebenwege = zufall.Next(min, max + 1);

            // Je nach Dungeon-Größe unterschiedliche Nebenweg-Algorithmen nutzen
            if (hoehe <= 15 && laenge <= 25)
            {
                ErzeugeNebenWegeKlein(dungeon, anzahlNebenwege, startX, startY, endeX, endeY, hoehe, laenge);
            }
            else
            {
                ErzeugeNebenWegeGroß(dungeon, anzahlNebenwege, startX, startY, endeX, endeY, hoehe, laenge);
            }
            Schatz_Falle(dungeon, laenge, hoehe);

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

        static void Schatz_Falle(char[,] dungeon, int Laenge, int Hoehe)
        {
            for (int y = 0; y < Hoehe; y++)
            {
                for (int x = 0; x < Laenge; x++)
                {
                    if (dungeon[y, x] == gang)
                    {
                        int Zahl = zufall.Next(1, 101);

                        if (Zahl <= 5)
                        {
                            int Zahl2 = zufall.Next(0, 2);
                            if (Zahl2 == 0)
                            {
                                dungeon[y, x] = Schatz;
                            }
                            else
                            {

                                dungeon[y, x] = Falle;
                            }

                        }


                    }
                }
            }
        }

        static void ErzeugeNebenWegeKlein(char[,] karte, int anzahlNebenwege, int startX, int startY, int endX, int endY, int hoehe, int breite)
        {
            // Schleife für alle Nebenwege
            for (int i = 0; i < anzahlNebenwege; i++)
            {
                // Startpunkt des Nebenwegs (immer vom Start ausgehend)
                int festX = startX;
                int festY = startY;

                // Zufällige Länge des Nebenwegs (3–14 Felder)
                int laenge = zufall.Next(3, 15);

                // Zufällige Richtung: 0=oben, 1=unten, 2=links, 3=rechts
                int richtung = zufall.Next(0, 4);

                // Nebenweg Schritt für Schritt erzeugen
                for (int l = 0; l < laenge; l++)
                {
                    // Bewegung entsprechend der gewählten Richtung
                    if (richtung == 0) festY--; // nach oben
                    if (richtung == 1) festY++; // nach unten
                    if (richtung == 2) festX--; // nach links
                    if (richtung == 3) festX++; // nach rechts

                    // Wenn der Weg die Karte verlassen würde → abbrechen
                    if (festX <= 0 || festX >= breite - 1 || festY <= 0 || festY >= hoehe - 1)
                    {
                        break;
                    }

                    // Wenn das Feld kein Wand ist → überspringen
                    if (karte[festY, festX] != wand)
                    {
                        continue;
                    }

                    // Feld als Gang setzen
                    karte[festY, festX] = gang;
                }
            }
        }
        static void ErzeugeNebenWegeGroß(char[,] karte, int anzahlNebenwege, int startX, int startY, int endX, int endY, int hoehe, int breite)
        {
            // Schleife für alle Nebenwege
            for (int i = 0; i < anzahlNebenwege; i++)
            {
                int festX, festY;

                // Startpunkt des Nebenwegs: zufälliger Gang auf der Karte
                do
                {
                    festX = zufall.Next(1, breite - 1);
                    festY = zufall.Next(1, hoehe - 1);
                }
                while (karte[festY, festX] != gang);

                // Länge des Nebenwegs
                int laenge = zufall.Next(30, 70);

                // Nebenweg Schritt für Schritt erzeugen
                for (int l = 0; l < laenge; l++)
                {
                    // Zufällige Richtung wählen
                    int richtung = zufall.Next(0, 4);

                    // Bewegung nur durchführen, wenn innerhalb der Karte
                    if (richtung == 0 && festY > 1) // oben
                        festY--;
                    else if (richtung == 1 && festY < hoehe - 2) // unten
                        festY++;
                    else if (richtung == 2 && festX > 1) // links
                        festX--;
                    else if (richtung == 3 && festX < breite - 2) // rechts
                        festX++;

                    // Wenn der Weg die Karte verlassen würde → abbrechen
                    if (festX <= 0 || festX >= breite - 1 || festY <= 0 || festY >= hoehe - 1)
                    {
                        break;
                    }

                    // Keine Überschreibung von bestehenden Gängen oder dem Endpunkt
                    if (karte[festY, festX] == gang || (festX == endX && festY == endY))
                    {
                        continue;
                    }

                    // Prüfen, wie viele angrenzende Felder bereits Gänge sind
                    // → verhindert große offene Flächen oder Schleifen
                    int angrenzendeGange = 0;

                    if (karte[festY - 1, festX] == gang) angrenzendeGange++;
                    if (karte[festY + 1, festX] == gang) angrenzendeGange++;
                    if (karte[festY, festX - 1] == gang) angrenzendeGange++;
                    if (karte[festY, festX + 1] == gang) angrenzendeGange++;

                    // Wenn mehr als 1 Gang angrenzend ist → kein neuer Weg
                    if (angrenzendeGange > 1)
                    {
                        continue;
                    }

                    // Feld als Gang markieren
                    karte[festY, festX] = gang;
                }
            }
        }
    }
}
