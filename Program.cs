using System;
using System.Threading;

//Problem ucztujących filozofów

namespace Projekt2_MateuszKaliszuk
{
    class Forks
    {
        bool[] forks;

        public Forks()
        {
            forks = new bool[5];
        }

        public void getForks(int left, int right)
        {
            lock (this)
            {
                while (forks[left] || forks[right]) Monitor.Wait(this);
                forks[left] = true; forks[right] = true;
            }
        }

        public void putForks(int left, int right)
        {
            lock (this)
            {
                forks[left] = false;
                forks[right] = false;
                Monitor.PulseAll(this);
            }
        }
    }

    class Philosopher
    {
        private int n;
        private int leftFork, rightFork;
        private int thinkingTime;
        private int eatingTime;
        private Forks forks;

        private Philosopher(int n, int thinkingTime, int eatingTime, Forks forks)
        {
            this.n = n;
            leftFork = n == 0 ? 4 : n - 1;
            rightFork = n % 5;
            this.thinkingTime = thinkingTime;
            this.eatingTime = eatingTime;
            this.forks = forks;

            new Thread(new ThreadStart(Run)).Start();
        }

        private void Run()
        {
            Console.WriteLine("Philosopher " + n + " is thinking");
            Thread.Sleep(thinkingTime);
            forks.getForks(leftFork, rightFork);
            Console.WriteLine("Philosopher " + n + " is eating...");
            Thread.Sleep(eatingTime);
            forks.putForks(leftFork, rightFork);
            Console.WriteLine("Philosopher " + n + " has done eating");
        }


        public static void InitializePhilosophers()
        {
            Random random = new Random();
            Forks forks = new Forks();
            for (int i = 0; i < 5; i++)
            {
                new Philosopher(i, random.Next(0, 5000), random.Next(0, 5000), forks);
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Philosopher.InitializePhilosophers();
            Console.ReadKey();
        }
    }
}
