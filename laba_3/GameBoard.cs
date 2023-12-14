using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba_3
{
    [Serializable]
    public class GameBoard
    {

        private Dictionary<Position, UnitBase> board;

        private const int X = 9;
        private const int Y = 9;

        public Dictionary<Position, UnitBase> Board { get { return board; } }

        public GameBoard()
        {
            board = new Dictionary<Position, UnitBase>();
        }

        // Метод для размещения юнита на доске
        public void PlaceUnit(UnitBase unit, Position position)
        {
            // Проверка, занята ли уже позиция
            if (board.ContainsKey(position))
            {
                Console.WriteLine($"Невозможно разместить юнита на позиции ({position.X}, {position.Y}). Позиция уже занята.");
            }
            // Проверка, находится ли позиция в пределах поля
            else if (unit.CurrentPosition.X > X && unit.CurrentPosition.Y > Y)
            {
                Console.WriteLine($"Невозможно разместить юнита на позиции ({position.X}, {position.Y}). Позиция вне поля.");
            }
            // Размещение юнита на доске
            else
            {
                board[position] = unit;
                unit.CurrentPosition = position;
                Console.WriteLine($"Юнит размещен на позиции ({position.X}, {position.Y}) на игровой доске.");
            }
        }

        // Метод для удаления юнита с доски
        public void RemoveUnit(UnitBase unit)
        {
            board.Remove(unit.CurrentPosition);
        }


        // Метод для перемещения юнита на доске
        public void MoveUnit(Position position, UnitBase unit)
        {
            unit.CurrentPosition = position;
        }


        // Метод для отображения текущего состояния игровой доски
        public void DisplayBoard()
        {
            Console.WriteLine("Текущая игровая доска:");

            // Выводим метки для оси X
            Console.Write("   ");
            for (int x = 1; x <= X; x++)
            {
                Console.Write($"  {x}  ");
            }
            Console.WriteLine();

            for (int y = 1; y <= Y; y++)
            {
                // Выводим метку для оси Y
                Console.Write($" {y} ");

                for (int x = 1; x <= 9; x++)
                {
                    Position currentPosition = new Position { X = x, Y = y };

                    UnitBase unit = GetUnitAtPosition(currentPosition);

                    unit = board.ContainsKey(currentPosition) ? board[currentPosition] : unit;

                    if (unit != null && unit.State && unit.CurrentPosition != null)
                    {
                        if (unit.Color == Colors.red)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        if (unit.Color == Colors.green)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.Write($" [{unit.GetType().Name[0]}] ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(" [ ] ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        // Метод для получения юнита на указанной позиции
        private UnitBase GetUnitAtPosition(Position position)
        {
            foreach (var kvp in board)
            {
                if (kvp.Key.X == position.X && kvp.Key.Y == position.Y)
                {
                    return kvp.Value;
                }
            }

            return null;
        }
    }
}