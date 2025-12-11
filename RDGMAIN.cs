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







        static void ErstelleDungeon(int hoehe, int laenge)
        {
                char[,] dungeon = new char[hoehe, laenge];
                Random rand = new Random();

                for (int i = 0; i < hoehe; i++)
                    for (int j = 0; j < laenge; j++)
                        dungeon[i, j] = '#';

                int startX = rand.Next(1, hoehe - 1);
                int startY = rand.Next(1, laenge - 1);

                int endeX, endeY;
                do
                {
                    endeX = rand.Next(1, hoehe - 1);
                    endeY = rand.Next(1, laenge - 1);
                } while (endeX == startX && endeY == startY);

                dungeon[startX, startY] = 'S';
                dungeon[endeX, endeY] = 'E';

                int x = startX;
                int y = startY;

                while (x != endeX || y != endeY)
                {
                    bool horizontal = rand.Next(2) == 0;

                    if (horizontal)
                    {
                        if (y < endeY) y++;
                        else if (y > endeY) y--;
                    }
                    else
                    {
                        if (x < endeX) x++;
                        else if (x > endeX) x--;
                    }
                    if (!(x == startX && y == startY) && !(x == endeX && y == endeY))
                        dungeon[x, y] = '.';
                }

                for (int i = 0; i < hoehe; i++)
                {
                    Console.Write("    ");
                    for (int j = 0; j < laenge; j++)
                    {
                        if (dungeon[i, j] == 'S')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("S");
                            Console.ResetColor();
                        }
                        else if (dungeon[i, j] == 'E')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("E");
                            Console.ResetColor();
                        }
                        else
                            Console.Write(dungeon[i, j]);
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
                    Console.Write("Bitte geben Sie die Höhe (Min. 10, Max 25) des RDG an: ");
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
