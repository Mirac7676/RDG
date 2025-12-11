using System;
using System.Collections.Generic;

namespace DungeonRPG
{
    class Program
    {
        static Random rand = new Random();
        static bool bossSpawned = false;
        static bool bossAlive = false;

        static void Main(string[] args)
        {
            int playerLife = 5;
            int maxLife = 5;
            Dictionary<string, int> inventory = new Dictionary<string, int>
            {
                ["HealingPotion"] = 0,
                ["StrengthPotion"] = 0
            };

            bool play = true;
            while (play)
            {
                int height = GetHeight();
                int width = GetWidth();

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("----- Interactive Random Dungeon RPG -----");
                Console.ResetColor();

                char[,] dungeon = GenerateDungeon(height, width, out int startX, out int startY, out int endX, out int endY);

                bool survived = PlayerMovement(dungeon, startX, startY, endX, endY, inventory, ref playerLife, maxLife);

                if (!survived) break;

                // Ask for next dungeon with exception handling
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Do you want to enter the next dungeon? (Y/N)");
                        string input = Console.ReadLine().Trim().ToUpper();
                        if (input == "Y") break;
                        else if (input == "N") { play = false; break; }
                        else throw new Exception("Invalid input, please enter Y or N.");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                        Console.ResetColor();
                    }
                }
            }

            Console.WriteLine("Thanks for playing!");
        }

        static int GetHeight()
        {
            while (true)
            {
                Console.Write("Enter dungeon height (10-25): ");
                if (int.TryParse(Console.ReadLine(), out int h) && h >= 10 && h <= 25)
                    return h;
                Console.WriteLine("Invalid input. Try again.");
            }
        }

        static int GetWidth()
        {
            while (true)
            {
                Console.Write("Enter dungeon width (10-50): ");
                if (int.TryParse(Console.ReadLine(), out int w) && w >= 10 && w <= 50)
                    return w;
                Console.WriteLine("Invalid input. Try again.");
            }
        }

        static char[,] GenerateDungeon(int height, int width, out int startX, out int startY, out int endX, out int endY)
        {
            char[,] dungeon = new char[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    dungeon[i, j] = '#';

            int carveX = rand.Next(1, height - 1);
            int carveY = rand.Next(1, width - 1);
            dungeon[carveX, carveY] = '.';
            int tilesToCarve = (int)((height - 2) * (width - 2) * 0.65);

            for (int t = 0; t < tilesToCarve; t++)
            {
                int dir = rand.Next(4);
                switch (dir)
                {
                    case 0: if (carveX > 1) carveX--; break;
                    case 1: if (carveX < height - 2) carveX++; break;
                    case 2: if (carveY > 1) carveY--; break;
                    case 3: if (carveY < width - 2) carveY++; break;
                }
                dungeon[carveX, carveY] = '.';
            }

            List<(int,int)> walkable = new List<(int,int)>();
            for (int i = 1; i < height - 1; i++)
                for (int j = 1; j < width - 1; j++)
                    if (dungeon[i,j] == '.') walkable.Add((i,j));

            (startX, startY) = walkable[rand.Next(walkable.Count)];
            do { (endX, endY) = walkable[rand.Next(walkable.Count)]; } while (endX == startX && endY == startY);

            foreach (var (i,j) in walkable)
                if (rand.NextDouble() < 0.05) dungeon[i,j] = 'F'; // traps

            foreach (var (i,j) in walkable)
                if (rand.NextDouble() < 0.1 && dungeon[i,j] == '.' && !HasAdjacentMonster(dungeon,i,j))
                    dungeon[i,j] = 'M';

            foreach (var (i,j) in walkable)
                if (rand.NextDouble() < 0.05 && dungeon[i,j] == '.') dungeon[i,j] = 'T';

            bossSpawned = false;
            bossAlive = false;

            return dungeon;
        }

        static bool HasAdjacentMonster(char[,] dungeon, int x, int y)
        {
            int[] dx = {-1,0,1,0};
            int[] dy = {0,1,0,-1};
            for(int d=0; d<4; d++)
            {
                int nx = x + dx[d];
                int ny = y + dy[d];
                if(nx >= 0 && nx < dungeon.GetLength(0) && ny >=0 && ny < dungeon.GetLength(1))
                    if(dungeon[nx,ny] == 'M') return true;
            }
            return false;
        }

        static bool PlayerMovement(char[,] dungeon, int startX, int startY, int endX, int endY, Dictionary<string,int> inventory, ref int playerLife, int maxLife)
        {
            int playerX = startX;
            int playerY = startY;
            bool survived = true;

            while (true)
            {
                Console.Clear();
                DrawDungeon(dungeon, playerX, playerY, endX, endY);
                Console.WriteLine($"Your Life: {playerLife}");
                Console.WriteLine($"Inventory: HealingPotion {inventory["HealingPotion"]}, StrengthPotion {inventory["StrengthPotion"]}");

                if (playerLife <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You died! Game Over.");
                    Console.ResetColor();
                    survived = false;
                    break;
                }

                // Spawn Boss if all monsters dead
                if (AllMonstersDead(dungeon) && !bossSpawned)
                {
                    if (rand.NextDouble() < 0.3)
                    {
                        dungeon[endX,endY] = 'B';
                        bossSpawned = true;
                        bossAlive = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("A Boss appeared at the end!");
                        Console.ResetColor();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                    }
                }

                if (playerX == endX && playerY == endY)
                {
                    if (bossAlive)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("A Boss blocks your way! You cannot exit before defeating it!");
                        Console.ResetColor();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        continue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Congratulations! You reached the end!");
                        Console.ResetColor();
                        break;
                    }
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                int nextX = playerX;
                int nextY = playerY;

                switch (key.Key)
                {
                    case ConsoleKey.W: nextX--; break;
                    case ConsoleKey.S: nextX++; break;
                    case ConsoleKey.A: nextY--; break;
                    case ConsoleKey.D: nextY++; break;
                    default: continue;
                }

                if(nextX >=0 && nextX < dungeon.GetLength(0) && nextY >=0 && nextY < dungeon.GetLength(1) && dungeon[nextX,nextY] != '#')
                {
                    if(dungeon[nextX,nextY] == 'F')
                    {
                        int trapDamage = rand.Next(2,5);
                        playerLife -= trapDamage;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"You stepped on a trap! Lost {trapDamage} life. Your life: {Math.Max(playerLife,0)}");
                        Console.ResetColor();
                        dungeon[nextX,nextY] = '.';
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                    }
                    else if(dungeon[nextX,nextY] == 'M')
                    {
                        bool alive = Combat(ref playerLife, dungeon, nextX,nextY, inventory, maxLife);
                        if(!alive){ survived=false; break; }
                        dungeon[nextX,nextY] = '.';
                    }
                    else if(dungeon[nextX,nextY] == 'B')
                    {
                        bool alive = BossCombat(ref playerLife, dungeon, nextX,nextY, inventory, maxLife);
                        if(!alive){ survived=false; break; }
                        dungeon[nextX,nextY] = '.';
                        bossAlive = false;
                    }
                    else if(dungeon[nextX,nextY] == 'T')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("You found a chest!");
                        Console.ResetColor();
                        if(rand.NextDouble()<0.7)
                        {
                            inventory["HealingPotion"]++;
                            Console.WriteLine("You got a Healing Potion (heals 1-5 HP)!");
                        }
                        else
                        {
                            inventory["StrengthPotion"]++;
                            Console.WriteLine("You got a Strength Potion (next attack +6 damage)!");
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        dungeon[nextX,nextY]='.';
                    }

                    playerX=nextX;
                    playerY=nextY;
                }
            }
            return survived;
        }

        static bool Combat(ref int playerLife, char[,] dungeon, int x, int y, Dictionary<string,int> inventory, int maxLife)
        {
            int monsterLife = rand.Next(2,11);
            bool strengthActive=false;
            Console.WriteLine($"A monster attacks! HP: {monsterLife}");

            while(monsterLife>0 && playerLife>0)
            {
                Console.WriteLine("Choose action: 1=Attack,2=Healing Potion,3=Strength Potion");
                string input = Console.ReadLine();
                if(input=="1")
                {
                    int damage = strengthActive?6:2;
                    monsterLife-=damage;
                    strengthActive=false;
                    if(monsterLife<=0)
                    {
                        Console.ForegroundColor=ConsoleColor.Green;
                        Console.WriteLine("You defeated the monster!");
                        Console.ResetColor();
                        break;
                    }
                    else Console.WriteLine($"You attack! Monster HP: {monsterLife}");
                }
                else if(input=="2")
                {
                    if(inventory["HealingPotion"]>0)
                    {
                        int heal = rand.Next(1,6);
                        playerLife=Math.Min(playerLife+heal,maxLife);
                        inventory["HealingPotion"]--;
                        Console.ForegroundColor=ConsoleColor.Blue;
                        Console.WriteLine($"Used Healing Potion! +{heal} life. Your life: {playerLife}");
                        Console.ResetColor();
                    }
                    else{ Console.WriteLine("No Healing Potion!"); continue;}
                }
                else if(input=="3")
                {
                    if(inventory["StrengthPotion"]>0)
                    {
                        strengthActive=true;
                        inventory["StrengthPotion"]--;
                        Console.ForegroundColor=ConsoleColor.Yellow;
                        Console.WriteLine("Used Strength Potion! Next attack +6 damage.");
                        Console.ResetColor();
                        continue;
                    }
                    else {Console.WriteLine("No Strength Potion!"); continue;}
                }
                else{Console.WriteLine("Invalid input!"); continue;}

                int monsterDamage = rand.Next(1,4);
                playerLife-=monsterDamage;
                if(playerLife<=0)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine("The monster killed you!");
                    Console.ResetColor();
                    return false;
                }
                Console.WriteLine($"Monster attacks! You lose {monsterDamage} life. Your life: {playerLife}");
            }
            Console.WriteLine("Press a key to continue...");
            Console.ReadKey(true);
            return true;
        }

        static bool BossCombat(ref int playerLife, char[,] dungeon, int x, int y, Dictionary<string,int> inventory, int maxLife)
        {
            int bossLife = rand.Next(10,21);
            bool strengthActive=false;
            Console.ForegroundColor=ConsoleColor.Yellow;
            Console.WriteLine($"Boss attacks! HP: {bossLife}");
            Console.ResetColor();

            while(bossLife>0 && playerLife>0)
            {
                Console.WriteLine("Choose action: 1=Attack,2=Healing Potion,3=Strength Potion");
                string input = Console.ReadLine();
                if(input=="1")
                {
                    int damage = strengthActive?6:2;
                    bossLife-=damage;
                    strengthActive=false;
                    if(bossLife<=0)
                    {
                        Console.ForegroundColor=ConsoleColor.Green;
                        Console.WriteLine("You defeated the Boss!");
                        Console.ResetColor();
                        break;
                    }
                    else Console.WriteLine($"You attack! Boss HP: {bossLife}");
                }
                else if(input=="2")
                {
                    if(inventory["HealingPotion"]>0)
                    {
                        int heal = rand.Next(1,6);
                        playerLife=Math.Min(playerLife+heal,maxLife);
                        inventory["HealingPotion"]--;
                        Console.ForegroundColor=ConsoleColor.Blue;
                        Console.WriteLine($"Used Healing Potion! +{heal} life. Your life: {playerLife}");
                        Console.ResetColor();
                    }
                    else{Console.WriteLine("No Healing Potion!"); continue;}
                }
                else if(input=="3")
                {
                    if(inventory["StrengthPotion"]>0)
                    {
                        strengthActive=true;
                        inventory["StrengthPotion"]--;
                        Console.ForegroundColor=ConsoleColor.Yellow;
                        Console.WriteLine("Used Strength Potion! Next attack +6 damage.");
                        Console.ResetColor();
                        continue;
                    }
                    else {Console.WriteLine("No Strength Potion!"); continue;}
                }
                else{Console.WriteLine("Invalid input!"); continue;}

                int bossDamage = rand.Next(2,4);
                playerLife-=bossDamage;
                if(playerLife<=0)
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine("The Boss killed you!");
                    Console.ResetColor();
                    return false;
                }
                Console.WriteLine($"Boss attacks! You lose {bossDamage} life. Your life: {playerLife}");
            }
            Console.WriteLine("Press a key to continue...");
            Console.ReadKey(true);
            return true;
        }

        static bool AllMonstersDead(char[,] dungeon)
        {
            for(int i=0;i<dungeon.GetLength(0);i++)
                for(int j=0;j<dungeon.GetLength(1);j++)
                    if(dungeon[i,j]=='M') return false;
            return true;
        }

        static void DrawDungeon(char[,] dungeon, int playerX, int playerY, int endX, int endY)
        {
            for(int i=0;i<dungeon.GetLength(0);i++)
            {
                for(int j=0;j<dungeon.GetLength(1);j++)
                {
                    if(i==playerX && j==playerY)
                    {
                        Console.ForegroundColor=ConsoleColor.Cyan;
                        Console.Write("P");
                    }
                    else if(i==endX && j==endY)
                    {
                        Console.ForegroundColor=ConsoleColor.Red;
                        Console.Write("E");
                    }
                    else
                    {
                        switch(dungeon[i,j])
                        {
                            case '#': Console.ForegroundColor=ConsoleColor.Gray; break;
                            case '.': Console.ForegroundColor=ConsoleColor.White; break;
                            case 'F': Console.ForegroundColor=ConsoleColor.White; break;
                            case 'M': Console.ForegroundColor=ConsoleColor.Magenta; break;
                            case 'T': Console.ForegroundColor=ConsoleColor.Blue; break;
                            case 'B': Console.ForegroundColor=ConsoleColor.Yellow; break;
                        }
                        Console.Write(dungeon[i,j]=='F'?'.':dungeon[i,j]);
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}
