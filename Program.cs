using System;

namespace HanoiP
{
    internal class Program
    {
        static void PrintPoles(int[,] poles)  // Метод для отрисовки поля.
        {
            int height = poles.GetLength(1);
            int width = poles.GetLength(0);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (poles[j, i] == 0)
                        Console.Write(string.Format("{0,"+ -(height+2) +"}", "|"));
                    else
                        Console.Write("|" + string.Format("{0,"+ -(height+1) +"}", String.Concat(Enumerable.Repeat("*", poles[j, i]))));
                }
                Console.WriteLine("");
            }
        }

        static int[,] GameStart() // Метод создающий двумерный массив, где i = шесты j = кол-во дисков
        {
            Console.WriteLine("Выберите количество дисков в башне");
            int diskAmount = 2;
            bool correctInput = false;
            while (!correctInput)
            {
                string input = Console.ReadLine();
                correctInput = Int32.TryParse(input, out diskAmount);
                if (!correctInput)
                    Console.WriteLine("Введите число!");
                else if (diskAmount < 2)
                {
                    Console.WriteLine("Минимальное количество дисков 2!");
                    correctInput = false;
                }
            }
            int[,] poles = new int[3,diskAmount];

            for (int j = 0; j < poles.GetLength(1); j++)
            {
                poles[0, j] = j + 1;
            }
            return poles;
        }

        static int[,] Step(int[,] poles) // Метод валидации хода
        {
            int firstPole;
            int secondPole;
            int selectedDisk = 0;
            int selectedDiskIndex = 0;
            Console.WriteLine("Выберите шест с диском:");

            //валидация
            if (!Int32.TryParse(Console.ReadLine(), out firstPole))
            {
                Console.WriteLine("Введите корректный номер шеста");
                PauseAfterError();
                return poles;
            }
            else if (firstPole < 1 || firstPole > 3)
            {
                Console.WriteLine("Введите корректный номер шеста");
                PauseAfterError();
                return poles;
            }
            firstPole--;
            //валидация

            for (int i = 0; i < poles.GetLength(1); i++)
            {
                if (poles[firstPole, i] != 0)
                {
                    selectedDisk = poles[firstPole, i];
                    selectedDiskIndex = i;
                    break;
                }
            }

            if (selectedDisk == 0)
            {
                Console.WriteLine("Выбран шест без диска");
                PauseAfterError();
                return poles;
            }
            Console.WriteLine("Выберите шест куда положить диск:");

            //валидация
            if (!Int32.TryParse(Console.ReadLine(), out secondPole))
            {
                Console.WriteLine("Введите корректный номер шеста");
                PauseAfterError();
                return poles;
            }
            else if (secondPole < 1 || secondPole > 3)
            {
                Console.WriteLine("Введите корректный номер шеста");
                PauseAfterError();
                return poles;
            }
            secondPole--;
            //валидация

            for (int i = 0; i < poles.GetLength(1); i++)
            {
                if (poles[secondPole, poles.GetLength(1) - 1] == 0)
                {
                    poles[secondPole, poles.GetLength(1) - 1] = selectedDisk;
                    poles[firstPole, selectedDiskIndex] = 0;
                    return poles;
                }

                else if (poles[secondPole, i] != 0 && poles[secondPole, i] < selectedDisk)
                {
                    Console.WriteLine("Нельзя класть более широкий диск на более узкий!");
                    PauseAfterError();
                    return poles;
                }

                else if (poles[secondPole, i] != 0 && poles[secondPole, i] > selectedDisk)
                {
                    poles[firstPole, selectedDiskIndex] = 0;
                    poles[secondPole, i - 1] = selectedDisk;
                    return poles;
                }
                
            }
            return poles;
        }

        static void PauseAfterError()
        {
            Thread.Sleep(1000);
        }

        static bool WinCheck(int player, int optimal, int winningScore, int[,] poles) // Проверка победных условий
        {
            int secondPoleScore = 0;
            int thirdPoleScore = 0;

            for (int i = 0; i < poles.GetLength(1); i++)
            {
                secondPoleScore += poles[1, i];
                thirdPoleScore += poles[2, i];
            }
            if (secondPoleScore == winningScore || thirdPoleScore == winningScore)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Ура! Головоломка решена!\n" +
                    "Ваше число ходов: " + player + "\n" +
                    "Наименьшее число ходов: " + optimal + "\n" +
                    "Добавьте ещё один диск и решите головоломку вновь!");
                Console.ReadKey();
                return true;
            }

            return false;
        }

        static void GamePlay(int[,] poles)
        {
            int playerMoves = 0;
            int optimalMoves = Convert.ToInt32(Math.Pow(2,poles.GetLength(1)) - 1);
            bool winningConditions = false;

            int winningScore = 0; // Благодаря валидации ходов, проверку выигрышных условий можно сделать по сумме элементов массива, т.к. их порядок всегда верный
            for (int i = 0; i < poles.GetLength(1); i++)
            {
                winningScore+=poles[0, i];
            }

            Console.Clear();
            while (!winningConditions)
            {
                Console.WriteLine("Ход игры:");
                PrintPoles(poles);
                poles = Step(poles);          
                Console.Clear();
                playerMoves++;
                if (playerMoves >= optimalMoves)
                {
                    winningConditions = WinCheck(playerMoves, optimalMoves, winningScore, poles);
                }
            }
        }

        static void Main()
        {
            Console.WriteLine("Ханойская башня");
            Console.ForegroundColor = ConsoleColor.Green;
            GamePlay(GameStart());      
        }
    }
}