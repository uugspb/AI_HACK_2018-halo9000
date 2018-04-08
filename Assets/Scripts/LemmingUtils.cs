using System;
using System.Linq;

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
                result[i] = random.Next(0, 2) == 0 ? record1[i] : record2[i];
            return new LemmingRunRecord(result);
        }

        public static double GetRandom(Random random)
        {
            var num = random.NextDouble();
            return 1 - num * num;
        }

        public static LemmingMovementDirection GenerateNextDirection()
        {
            var random = new Random();
            return (random.Next(0, 6) == 0 ? LemmingMovementDirection.Jump : LemmingMovementDirection.None) |
                   (random.Next(0, 2) == 0 ? LemmingMovementDirection.Right : LemmingMovementDirection.None);
        }

        private static int _minimumFrameDistanceToForceMutation = 100;

        public static void Mutate(this LemmingRunRecord record, int frameId, int previousFrameId)
        {
            record.MutateLastActions(frameId);            
            if (WinnersTable.WinnersData.Any())
            {
                var data = WinnersTable.WinnersData[0].Data;
                Array.Resize(ref data, (int) (0.75 * data.Length));
                record = new LemmingRunRecord(data);
            }
            if (Math.Abs(frameId - previousFrameId) < _minimumFrameDistanceToForceMutation)
                record.Mutate(0.2);
        }
    }
}