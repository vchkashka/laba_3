using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using laba_3;
namespace Test
{
    [TestClass]
    public class Units_Test

    {
        [TestMethod]
        public void Attack_Test()
        {
            // исходные данные

            Warrior warrior = new Warrior(Colors.green);
            warrior.CurrentPosition = new Position { X = 1, Y = 2 };
            Archer archer = new Archer(Colors.red);
            archer.CurrentPosition = new Position { X = 1, Y = 1 };
            int expected = 30;


            warrior.Attack(archer);

            int actual = archer.Health;

            Assert.AreEqual(expected, actual);  

        }

        [TestMethod]
        public void Move_Test()
        {
            Warrior warrior = new Warrior(Colors.green);
            warrior.CurrentPosition = new Position { X = 1, Y = 1 };
            Archer archer = new Archer(Colors.red);
            archer.CurrentPosition = new Position { X = 1, Y = 3 };

            Position expected = new Position { X = 1, Y = 2 };
            warrior.Move(archer.CurrentPosition);


            Position actual = warrior.CurrentPosition;

            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class Player_Test
    {
        [TestMethod]

        public void AddUnit_Test()
        {
            Player player = new Player(Colors.green);
            Warrior warrior = new Warrior(player.Color);

            int expected = 1;

            player.AddUnit(warrior);

            int actual = player.Units.Count;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]

        public void RemoveUnit()
        {
            Player player = new Player(Colors.green);
            Warrior warrior = new Warrior(player.Color);
            Warrior warrior1 = new Warrior(player.Color);
            Warrior warrior2 = new Warrior(player.Color);
            int expected = 2;

            player.AddUnit(warrior);
            player.AddUnit(warrior1);
            player.AddUnit(warrior2);
            player.RemoveUnit(warrior);
            int actual = player.Units.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]

        public void RemoveUnitIndex_Test()
        {
            Player player = new Player(Colors.green);
            Warrior warrior = new Warrior(player.Color);
            Warrior warrior1 = new Warrior(player.Color);
            Warrior warrior2 = new Warrior(player.Color);
            int expected = 2;

            player.AddUnit(warrior);
            player.AddUnit(warrior1);
            player.AddUnit(warrior2);
            player.RemoveUnitIndex(1);
            int actual = player.Units.Count;

            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class GameBoard_Test
    {
        [TestMethod]

        public void PlaceUnit_Test()
        {
            Player player1 = new Player(Colors.green);
            Player player2 = new Player(Colors.red);
            Archer archer = new Archer(player1.Color)  { CurrentPosition = new Position { X = 1, Y = 2 } };
            Warrior warrior = new Warrior(player2.Color) { CurrentPosition = new Position { X = 4, Y = 9 } };

            player1.AddUnit(archer);
            player2.AddUnit(warrior);

            GameBoard gameBoard = new GameBoard();

            gameBoard.PlaceUnit(archer,archer.CurrentPosition);
            gameBoard.PlaceUnit(warrior, warrior.CurrentPosition);


            int expected = 2;

            int actual = gameBoard.Board.Count;

            Assert.AreEqual (expected, actual);

        }

        [TestMethod]
        public void RemoveUnit()
        {
            Player player1 = new Player(Colors.green);
            Player player2 = new Player(Colors.red);
            Archer archer = new Archer(player1.Color) { CurrentPosition = new Position { X = 1, Y = 2 } };
            Warrior warrior = new Warrior(player2.Color) { CurrentPosition = new Position { X = 4, Y = 9 } };

            player1.AddUnit(archer);
            player2.AddUnit(warrior);

            GameBoard gameBoard = new GameBoard();

            gameBoard.PlaceUnit(archer, archer.CurrentPosition);
            gameBoard.PlaceUnit(warrior, warrior.CurrentPosition);


            int expected = 1;

            gameBoard.Board.Remove(archer.CurrentPosition);

            int actual = gameBoard.Board.Count;

            Assert.AreEqual (expected, actual); 
        }

        [TestMethod]

        public void MoveUnit_Test()
        {
            Player player1 = new Player(Colors.green);
            Archer archer = new Archer(player1.Color) { CurrentPosition = new Position { X = 1, Y = 2 } };
            player1.AddUnit(archer);
            GameBoard gameBoard = new GameBoard();

            gameBoard.PlaceUnit(archer, archer.CurrentPosition);

            Position expected = new Position() { X = 1, Y = 5 };

            gameBoard.MoveUnit(new Position { X =1, Y = 5},archer);

            Position actual = archer.CurrentPosition;

            Assert.AreEqual(expected, actual);

        }
    }
}
