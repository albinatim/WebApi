using System;
using System.Collections.Generic;

namespace Train
{
    public class Train
    {
        static Random rnd = new Random();
        LinkedList<Wagon> wagons = new LinkedList<Wagon>();
        int countWagons;
        LinkedListNode<Wagon> currentWagon;
        
        public int CountWagons { get => countWagons; set => countWagons = value; }
        int i = 0;
        int amount = 0;
        public Train()
        {
            CountWagons = rnd.Next(3,100);
            for(int k = 0; k < CountWagons; k++)
            {
                Wagon wagon = new Wagon();
                wagons.AddLast(wagon);
            }
            foreach (Wagon w in wagons)
            {
                w.TurnedOn = rnd.Next(0, 2) == 1 ? true : false;
            }
        }

        public void Enter()
        {
            currentWagon = wagons.First;
            currentWagon.Value.TurnedOn = true;
            
        }

        public void GoLeft()
        {
            if (currentWagon != wagons.First)
                currentWagon = currentWagon.Previous;
            else
                currentWagon = wagons.Last;
        }
        public void GoRight()
        {
            if (currentWagon != wagons.Last)
                currentWagon = currentWagon.Next;
            else
                currentWagon = wagons.First;
        }
        public bool Count()
        {
            amount = 0;
            do { GoLeft(); i++; amount++; }
            while (currentWagon.Value.TurnedOn != true);
            currentWagon.Value.TurnedOn = false;
            while (i > 0)
            {
                GoRight();
                i--;
            }
            if (currentWagon.Value.TurnedOn == false)
            {
                return true;
            }
            else
            {
                
                return false;
            }
        }
        public void Guess()
        {
            Enter();
            bool guessed = false;
            while (!guessed)
            {
                guessed = Count();
            }
            Console.WriteLine("поезд состоит из {0} вагонов", amount);
            Console.WriteLine("Ответ верный? {0}",CountWagons==amount);
        }
    }
    public class Wagon
    {
        bool turnedOn;

        public Wagon()
        {
        }

        public bool TurnedOn { get => turnedOn; set => turnedOn = value; }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Train train = new Train();
            train.Guess();
        }
    }
}
