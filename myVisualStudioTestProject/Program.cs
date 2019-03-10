using System;
using System.Threading;

namespace myVisualStudioTestProject
{
    class Program
    {
        static variableLock successLock;
        static variableLock exceptionLock;

        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("Started Main Thread");
                successLock = new variableLock();
                exceptionLock = new variableLock();

                Thread myThread1 = new Thread(() => taskWrapper(1));
                Thread myThread2 = new Thread(() => taskWrapper(2));
                Thread myThread3 = new Thread(() => taskWrapper(3));

                Thread heartBeat = new Thread(heartBeating);

                myThread1.Start();
                myThread2.Start();
                myThread3.Start();
                heartBeat.Start();

                myThread1.Join();
                myThread2.Join();
                myThread3.Join();

                Console.WriteLine("Code reached place where threads have joined.\n\n\n\n\n");
                Console.WriteLine("RESTARTING RESTARTING RESTARTING \n\n\n\n\n");
            }
        }

        private static void taskWrapper(int i)
        {
            Random random = new Random();
            var simulationTime = random.Next(10*1000, 40*1000);
            var timePeriod = random.Next(10*1000, 40*1000);
            task(i, simulationTime, timePeriod);
        }

        private static void task(int i, int simulationTime, int timePeriod)
        {
            int exceptionCount = 0;
            var exceptionOccured = false;

            while(true)
            {
                Console.WriteLine("Task " + i + " Running");

                Thread.Sleep(simulationTime); // Simulating time to run Task i
                exceptionOccured = !randomTrueFalse();
                if (exceptionOccured)
                {
                    Console.WriteLine("Task " + i + " had exception");
                    exceptionCount += 1;
                    successLock.mergeLockValue(false);
                } else
                {
                    Console.WriteLine("Task " + i + " Completed");
                }
                

                if (exceptionCount > 2)
                {
                    // Task failing for more than 2 times, we need to restart the entire process so setting
                    // the exceptionLock here. So every task will abort itself and we'll restart our main loop.
                    exceptionLock.mergeLockValue(false);
                    break;
                }

                if (!exceptionLock.getLockValue())
                {
                    // An exception occured in some other thread, we break out of current task
                    break;
                }

                Thread.Sleep(timePeriod); // Simulating time to re-run the task. i.e. time period of this task
            }
        }

        private static void heartBeating()
        {
            while(true)
            {
                Console.WriteLine("HeartBeat: " + successLock.getLockValue());
                Thread.Sleep(5 * 1000);
            }
        }

        private static bool randomTrueFalse()
        {
            Random random = new Random();
            var value = random.Next(1000);

            if (value > 500)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
