using System;

namespace DefaultNamespace
{
    public static class LemmingUtils
    {
        public static LemmingRunRecord Crossover(this LemmingRunRecord record1, LemmingRunRecord record2)
        {
            var random = new Random();
            var size = Math.Max(record1.Size, record2.Size);
            var result = new LemmingMovementDirection[size];
            for (var i = 0; i < size; i++)
                result[i] = random.Next(0, 1) == 0 ? record1[i] : record2[i];
            return new LemmingRunRecord(result);
        }

        public static void Mutate(this LemmingRunRecord record, double mutationRatio = 0.1)
        {
            var numberOfMutations = (int) (record.Size * mutationRatio);
            var random = new Random();
            for (var i = 0; i < numberOfMutations; i++)
                record[(int) (GetRandom(random) * record.Size)] = GenerateNextDirection();
        }

        public static double GetRandom(Random random)
        {
            var num = random.NextDouble();
            return 1 - num * num;
        }

        public static LemmingMovementDirection GenerateNextDirection()
        {
            var random = new Random();
            return (LemmingMovementDirection)random.Next(0, 8);
        }
    }
}