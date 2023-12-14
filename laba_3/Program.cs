using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba_3
{

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Добро пожаловать в игру!");
            Player player1 = new Player(Colors.green);
            Player player2 = new Player(Colors.red);
            GameBoard gameBoard = new GameBoard();
            GameEngine gameEngine = new GameEngine(gameBoard, player1, player2);

            player1.AddUnit(new Warrior(player1.Color) { CurrentPosition = new Position { X = 1, Y = 1 } });
            player1.AddUnit(new Warrior(player1.Color) { CurrentPosition = new Position { X = 3, Y = 1 } });
            player2.AddUnit(new Warrior(player2.Color) { CurrentPosition = new Position { X = 4, Y = 3 } });
            player2.AddUnit(new Warrior(player2.Color) { CurrentPosition = new Position { X = 8, Y = 8 } });
            gameEngine.StartGame();
        }
    }
}
