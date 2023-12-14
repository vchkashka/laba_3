using laba_3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba_3
{

    // Абстрактный базовый класс для всех типов юнитов
    [Serializable]
    public abstract class UnitBase
    {

        public int Health { get; set; }//Здоровье юнита
        public int AttackDamage { get; set; }//Урон от атаки юнита
        public int MaxAttackRange { get; set; }//Максимальная дальность атаки
        public bool State { get; set; }//Состояние юнита (жив/мертв)
        public Colors Color { get; set; }//Цвет юнита

        public int Cost { get; set; }//Стоимость юнита в монетах
        public Position CurrentPosition { get; set; }//Текущая позиция юнита

        public abstract void Attack(UnitBase target);
        public abstract void Move(Position Enemy);

    }
    [Serializable]
    public class Warrior : UnitBase
    {
        
        public Warrior(Colors color)
        {
            Health = 100;
            AttackDamage = 20;
            MaxAttackRange = 1;
            Color = color;
            Cost = 2;
            State = true;
        }

        // Реализация метода атаки для Воина
        public override void Attack(UnitBase target)
        {
            // Проверка, что цель не пуста и цвет цели отличается от цвета Воина
            if (target != null && !target.Color.Equals(Color))
            {
                double distance = CalculateDistance(CurrentPosition, target.CurrentPosition);
                // Проверка, что цель в пределах дальности атаки
                if (distance <= MaxAttackRange)
                {
                    Console.WriteLine($"Воин({CurrentPosition.X},{CurrentPosition.Y}) {Color} атакует цель на позиции {target.Color} ({target.CurrentPosition.X}, {target.CurrentPosition.Y})!");
                    target.Health -= AttackDamage;
                    Console.WriteLine($"Здоровье цели: {target.Health}");
                    // Если здоровье цели меньше или равно нулю, убиваем цель
                    if (target.Health <= 0)
                    {
                        target.State = false;
                        Console.WriteLine("Цель уничтожена!");
                    }
                }
                else
                {
                    Move(target.CurrentPosition);
                    Console.WriteLine("У воина нет цели для атаки.");
                }
            }
        }

        // Реализация метода перемещения для Воина
        public override void Move(Position Enemy)
        {
            int deltaX = Enemy.X - CurrentPosition.X;
            int deltaY = Enemy.Y - CurrentPosition.Y;

            int newX = CurrentPosition.X + Math.Sign(deltaX);
            int newY = CurrentPosition.Y + Math.Sign(deltaY);

            Console.WriteLine($"Воин перемещается с позиции ({CurrentPosition.X}, {CurrentPosition.Y}) на позицию ({newX}, {newY})");
            CurrentPosition.X = newX;
            CurrentPosition.Y = newY;
        }
        // Метод для вычисления расстояния между двумя позициями
        private double CalculateDistance(Position position1, Position position2)
        {
            int deltaX = position1.X - position2.X;
            int deltaY = position1.Y - position2.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY) - 0.5;
        }
    }

    [Serializable]
    public class Archer : UnitBase
    {


        public Archer(Colors color)
        {
            Health = 50;
            AttackDamage = 50;
            MaxAttackRange = 4;
            this.Color = color;
            Cost = 4;
            State = true;
        }
        public override void Attack(UnitBase target)
        {
            // Проверка, что цель не пуста и цвет цели отличается от цвета Лучник
            if (target != null && !target.Color.Equals(Color))
            {
                double distance = CalculateDistance(CurrentPosition, target.CurrentPosition);
                // Проверка, что цель в пределах дальности атаки
                if (distance <= MaxAttackRange)
                {
                    Console.WriteLine($"Лучник({CurrentPosition.X},{CurrentPosition.Y}) {Color} атакует цель на позиции {target.Color} ({target.CurrentPosition.X}, {target.CurrentPosition.Y})!");
                    target.Health -= AttackDamage;
                    Console.WriteLine($"Здоровье цели: {target.Health}");
                    // Если здоровье цели меньше или равно нулю, убиваем цель
                    if (target.Health <= 0)
                    {
                        target.State = false;
                        Console.WriteLine("Цель уничтожена!");
                    }
                }
                else
                {
                    Move(target.CurrentPosition);
                    Console.WriteLine("У воина нет цели для атаки.");
                }
            }
        }
        public override void Move(Position Enemy)
        {
            int deltaX = Enemy.X - CurrentPosition.X;
            int deltaY = Enemy.Y - CurrentPosition.Y;

            int newX = CurrentPosition.X + Math.Sign(deltaX);
            int newY = CurrentPosition.Y + Math.Sign(deltaY);

            Console.WriteLine($"Лучник перемещается с позиции ({CurrentPosition.X}, {CurrentPosition.Y}) на позицию ({newX}, {newY})");
            CurrentPosition.X = newX;
            CurrentPosition.Y = newY;
        }

        private double CalculateDistance(Position position1, Position position2)
        {
            int deltaX = position1.X - position2.X;
            int deltaY = position1.Y - position2.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY) - 0.5;
        }
    }
}






