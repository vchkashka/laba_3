using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba_3
{
    public enum Colors
    {
        red,
        green
    }
    [Serializable]
    public class Player
    {
        private List<UnitBase> units;//Список юнитов игрока

        public List<UnitBase> Units { get { return units; } }

        private Colors color;//Цвет игрока

        private int coins;//Количество монет игрока

        public int Coins
        {
            get
            {
                return coins;
            }
            set
            {
                coins = Math.Max(value, 1);
            }
        }


        public Colors Color { get { return color; } }

        public Player(Colors color)
        {
            coins = 10; // Изначальное количество монет у игрока
            this.color = color; // Устанавливаем цвет игрока
            units = new List<UnitBase>();
        }
        // Метод для добавления юнита в армию игрока (покупка)
        public void AddUnit(UnitBase unit) 
        {
            // Проверка, достаточно ли монет для покупки
            if (coins < unit.Cost)
            {
                Console.WriteLine("Нехватает монет, чтобы купить юнита");
            }
            else
            {
                coins -= unit.Cost; // Уменьшаем количество монет на стоимость юнита
                units.Add(unit);
                Console.WriteLine($"Юнит добавлен в армию игрока. Всего монет: {coins}");
            }

        }
        // Метод для удаления юнита из армии игрока (продажа)
        public void RemoveUnit(UnitBase unit) // продать
        {
            coins += unit.Cost / 2; // Получаем половину стоимости юнита при продаже
            units.Remove(unit);
            Console.WriteLine($"Юнит удален из армии игрока. Всего юнитов: {units.Count}");

        }

        // Метод для удаления юнита по индексу из армии игрока (продажа)
        public void RemoveUnitIndex(int i)
        {
            if (i >= 1 && i <= units.Count)
            {
                coins += units[i-1].Cost / 2;
                units.RemoveAt(i - 1);
            }
            else
            {
                Console.WriteLine("Некорректный индекс. Введите допустимый индекс для удаления юнита.");

            }

        }

    }
}
