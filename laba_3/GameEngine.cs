using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace laba_3
{
    [Serializable]
    public class SaveData
    {
        public GameBoard GameBoard { get; set; }
        public List<Player> Players { get; set; }
    }
    public class GameEngine
    {
        private GameBoard gameBoard; // Игровая доска
        private List<Player> players; // Список игроков

        public GameEngine(GameBoard gameBoard, params Player[] players)
        {
            this.gameBoard = gameBoard;
            this.players = new List<Player>(players);
        }

        public void StartGame()
        {
            Console.WriteLine("Игра началась!");
            for (int i = 0; i < 8; i++)
            {
                Menu();// вызываем начальное меню
                Place();
                GameLoop();
                Console.ReadLine();

            }
        }
        // Размещение юнитов на доске
        private void Place()
        {
            foreach (Player player in players)
            {
                foreach (UnitBase unit in player.Units)
                {
                    if (unit.CurrentPosition != null)
                    {
                        gameBoard.PlaceUnit(unit, unit.CurrentPosition);

                    }

                }
            }

        }
        // Игровой цикл для раунда
        private void GameLoop() // раунд
        {
            gameBoard.DisplayBoard();
            bool flag = true;
            while (flag)
            {
                foreach (Player player in players)
                {
                    foreach (UnitBase unit1 in gameBoard.Board.Values.ToList())
                    {
                        UnitBase target = GetNearestTarget(player, unit1);
                        // Атака цели
                        if (target != null)
                        {

                            unit1.Attack(target);
                            // Удаление юнита из игры, если он уничтожен
                            if (!unit1.State)
                            {
                                gameBoard.RemoveUnit(unit1);
                                player.RemoveUnit(unit1);
                            }

                        }
                    }

                }



                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                gameBoard.DisplayBoard();

                bool player1Alive = players[0].Units.Any(unit => unit.State);
                bool player2Alive = players[1].Units.Any(unit => unit.State);

                // Если у одного из игроков не осталось живых юнитов, завершаем игру
                if (!player1Alive || !player2Alive)
                {
                    Console.WriteLine("Игра завершена. Результат:");

                    if (player1Alive && !player2Alive)
                    {
                        flag = false;

                        Console.WriteLine("Игрок 1 победил!");
                        players[0].Coins += 2;
                        players[1].Coins += 1;
                        Console.WriteLine("Победитель получает 2 монеты, проигравший 1 монету");
                        Console.WriteLine($"Осталось юнитов у игрока 1: {gameBoard.Board.Count}");
                    }
                    else if (!player1Alive && player2Alive)
                    {
                        flag = false;
                        Console.WriteLine("Игрок 2 победил!");
                        Console.WriteLine("Победитель получает 2 монеты, проигравший 1 монету");
                        players[1].Coins += 2;
                        players[0].Coins += 1;
                        Console.WriteLine($"Осталось юнитов у игрока 2: {players[1].Units.Count}");
                    }
                    else
                    {
                        flag = false;
                        players[1].Coins += 1;
                        players[0].Coins += 1;
                        Console.WriteLine("Ничья! Оба игрока потеряли все юниты.");
                        Console.WriteLine("Оба игрока получили");
                    }


                }
            }
        }
        // Получение ближайшей цели для атаки
        private UnitBase GetNearestTarget(Player attackerPlayer, UnitBase attacker)
        {
            Player targetPlayer = players.Find(player => player != attackerPlayer);
            UnitBase nearestTarget = null;
            double nearestDistanceSquared = double.MaxValue;
            // Поиск ближайшей цели среди юнитов противника
            foreach (UnitBase target in targetPlayer.Units)
            {
                if (target.State && target.CurrentPosition != null && target.Color != attacker.Color)
                {
                    double distanceSquared = GetDistanceSquared(attacker.CurrentPosition, target.CurrentPosition);
                    // Обновление ближайшей цели, если найдена более близкая
                    if (distanceSquared < nearestDistanceSquared)
                    {
                        nearestDistanceSquared = distanceSquared;
                        nearestTarget = target;
                    }
                }
            }

            return nearestTarget;
        }
        // Вычисление квадрата расстояния между двумя позициями
        private double GetDistanceSquared(Position position1, Position position2)
        {
            int dx = position2.X - position1.X;
            int dy = position2.Y - position1.Y;
            return dx * dx + dy * dy;
        }
        private void Menu()
        {
            bool exitGame = false;

            while (!exitGame)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Меню Игрока 1");
                Console.WriteLine("2. Меню Игрока 2");
                Console.WriteLine("3. Начать игру");
                Console.WriteLine("4. Посмотреть поле");
                Console.WriteLine("5. Сохранить игру");
                Console.WriteLine("6. Загрузить игру");

                Console.Write("Выберите опцию (1-3): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Menu_player(players[0]);
                        break;
                    case "2":
                        Menu_player(players[1]);
                        break;
                    case "3":
                        if (UnitsAreSet(players[0], players[1]) && players[0].Units.Count > 0 && players[1].Units.Count > 0 && gameBoard.Board.Count > 0)
                        {
                            exitGame = true;
                        }
                        {
                            Console.WriteLine("Расставьте юнитов для обоих игроков перед началом игры!");
                        }
                        break;
                    case "4":
                        Place();
                        gameBoard.DisplayBoard();
                        break;
                    case "5":
                        Save();
                        break;
                    case "6":
                        Load();
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод. Пожалуйста, выберите от 1 до 3.");
                        break;
                }
            }
            Console.Clear();
        }
        private void Save()
        {
            SaveData data = new SaveData()
            {
                GameBoard = gameBoard,
                Players = players
            };
            BinaryFormatter formatter = new BinaryFormatter();
            // Сериализация объекта и сохранение в файл
            using (FileStream fs = new FileStream("game.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, data);
            }
            Console.WriteLine("Игра сохранена.");
        }
        private void Load()
        {
            // Десериализация объекта из файла
            using (FileStream fs = new FileStream("game.dat", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                SaveData saveData = (SaveData)formatter.Deserialize(fs);

                // Присвоение сохраненных значений текущим объектам
                gameBoard = saveData.GameBoard;
                players = saveData.Players;
            }

            Console.WriteLine("Игра загружена.");
        }
        //Показать юнитов
        private void Show_Unins(Player player)
        {
            Console.WriteLine("Ваши юниты:");
            for (int i = 0; i < player.Units.Count; i++)
            {
                Console.Write($"{i + 1}. {player.Units[i].GetType().Name} (Текущие здоровье - {player.Units[i].Health}) ");
                if (player.Units[i].CurrentPosition == null)
                {
                    Console.Write("юнит не стоит на поле\n");
                }
                else
                {
                    Console.Write($"позиция ({player.Units[i].CurrentPosition.X},{player.Units[i].CurrentPosition.Y})\n");
                }
            }
        }
        // меня для каждого игрока
        private void Menu_player(Player player)
        {
            Console.Clear();
            Console.WriteLine($"Меню Игрока {player.Color}...");
            Console.WriteLine($"Монеты: {player.Coins}");
            Console.WriteLine("Выберите опцию:");
            Console.WriteLine("1. Купить юнита");
            Console.WriteLine("2. Продать юнита");
            Console.WriteLine("3. Показать армию");
            Console.WriteLine("4. Расстановка");
            Console.Write("Введите номер выбранного типа (1-4): ");
            string unitChoice = Console.ReadLine();



            switch (unitChoice)
            {
                case "1":
                    Buy_menu(player);
                    break;
                case "2":
                    Sell_menu(player);
                    break;
                case "3":
                    Show_Unins(player);
                    break;
                case "4":
                    Col_menu(player);
                    break;
                default:
                    break;
            }



        }
        // Меня покупки
        private void Buy_menu(Player player)
        {
            Console.WriteLine("Магазин");
            Console.WriteLine("Выберите тип юнита:");
            Console.WriteLine("1. Воин (2 монеты)");
            Console.WriteLine("2. Лучник (4 монеты)");
            Console.Write("Введите (1-2): ");
            string unitChoice = Console.ReadLine();



            switch (unitChoice)
            {
                case "1":
                    player.AddUnit(new Warrior(player.Color));
                    break;
                case "2":
                    player.AddUnit(new Archer(player.Color));
                    break;
                default:
                    break;
            }
            Console.Clear();

        }
        // Меню для расстоновки 
        private void Col_menu(Player player)
        {
            Show_Unins(player);
            Console.WriteLine("Выберите юнита, которому вы хотите поменять расположение / добавить юнита на поле ");
            string c = Console.ReadLine();
            if (int.TryParse(c, out int index) && index >= 1 && index <= player.Units.Count)
            {
                Console.Write("Введите новые координаты для перемещения (X Y): ");
                string[] coordinates = Console.ReadLine().Split();

                if (coordinates.Length == 2 && int.TryParse(coordinates[0], out int newX) && int.TryParse(coordinates[1], out int newY))
                {
                    if (IsValidCoordinate(newX) && IsValidCoordinate(newY))
                    {
                        Position newPosition = new Position { X = newX, Y = newY };
                        if (player.Units[index - 1].CurrentPosition != null)
                        {
                            gameBoard.RemoveUnit(player.Units[index - 1]);
                        }

                        gameBoard.MoveUnit(newPosition, player.Units[index - 1]);
                        Console.WriteLine($"Юнит перемещен на позицию ({newPosition.X}, {newPosition.Y}).");
                    }
                    else
                    {
                        Console.WriteLine("Координаты должны быть в диапазоне от 1 до 9.");
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод координат.");
                }
            }
            else
            {
                Console.WriteLine($"Некорректный ввод. Введите целое число от 1 до {player.Units.Count}.");
            }
            Console.Clear();
        }

        private bool IsValidCoordinate(int coordinate)
        {
            return coordinate >= 1 && coordinate <= 9;
        }
        //Меню для продажи
        private void Sell_menu(Player player)
        {
            Console.WriteLine("При продаже вы получите меньше монет!!!!");
            Show_Unins(player);
            Console.WriteLine("Выберите юнита для продажи");
            string c = Console.ReadLine();
            if (int.TryParse(c, out int index))
            {
                
                gameBoard.RemoveUnit(player.Units[index - 1]);
                player.RemoveUnitIndex(index);
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Введите целое число.");
            }
            Console.Clear();

        }

        private bool UnitsAreSet(Player player1, Player player2)
        {
            // Проверяем, что у обоих игроков есть юниты
            return player1.Units.Count > 0 && player2.Units.Count > 0;
        }
    }



}


