using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11._12._25_Projektus
{

            class Program
        {
            static void Main(string[] args)
            {
                int hoehe = EingabeH();     // Eingabe der Höhe
                int laenge = EingabeL();    // Eingabe der Länge

                Console.Write("-------------------");
                Console.ForegroundColor = ConsoleColor.Red;                 // Titel "Der Random DUngeon Generator" in Rot :)
                Console.Write("Der Random Dungeon Generator");
                Console.ResetColor();
                Console.WriteLine("-------------------");

                Random rand = new Random();                                       // Für Random Position von S und E
            int startX, startY, endeX = -1, endeY = -1;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // FÜr den Random Start
            // Für den Startpunkt
            do
            {
                startX = rand.Next(1, hoehe - 1); // S wird zwischen 1 und hoehe-1 gewählt
                startY = rand.Next(1, laenge - 1); // S wird zwischen 1 und laenge-1 gewählt
            } while (startX == endeX && startY == endeY); // Startpunkt darf nicht auf dem Endpunkt liegen

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////          
            // FÜr das Random Ende
            do
            {
                endeX = rand.Next(1, hoehe - 1); // Ende zwischen 1 und hoehe - 1
                endeY = rand.Next(1, laenge - 1); // Ende zwischen 1 und laenge - 1
            } while (endeX == startX && endeY == startY); // Endpunkt wird nicht auf dem Startpunkt liegen

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            // Dungeon Ausgabe
            for (int i = 0; i < hoehe; i++)
                {
                    Console.Write("    "); // Damit die Wände zentriert sind
                    for (int j = 0; j < laenge; j++)
                    {
                        if (i == 0 || i == hoehe - 1 || j == 0 || j == laenge - 1)
                        {
                            Console.Write("#"); // Rand
                        }
                        else
                        {
                            if (i == startX && j == startY)
                            {
                                Console.ForegroundColor = ConsoleColor.Green; // S in grün
                                Console.Write("S");
                                Console.ResetColor();
                            }
                            else if (i == endeX && j == endeY)
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // E in rot
                                Console.Write("E");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.Write("#");
                            }
                        }
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
                        int hoehe = Convert.ToInt32(Console.ReadLine());                    // Exception Handling für Höphe

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
                        int laenge = Convert.ToInt32(Console.ReadLine());                   // Exception Handling für Länge

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

    
