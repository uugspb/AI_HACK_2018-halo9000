using System;
using System.Collections.Generic;
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

//        public static void Mutate(this LemmingRunRecord record, int frameId, int previousFrameId)
//        {
//            record.MutateLastActions(frameId);            
//            if (WinnersTable.WinnersData.Any())
//            {
//                var data = WinnersTable.WinnersData[0].Data;
//                Array.Resize(ref data, (int) (0.75 * data.Length));
//                record = new LemmingRunRecord(data);
//            }
//            if (Math.Abs(frameId - previousFrameId) < _minimumFrameDistanceToForceMutation)
//                record.Mutate(0.2);
//
//            var dict = new SortedDictionary<int, int>();
//        }
        
        private static TopRecordsCollection _topRecordsCollection = new TopRecordsCollection(5);
        
        public static void ModifyRecord(LemmingRunRecord record, float distanceToExit)
        {
            _topRecordsCollection.TryPut(distanceToExit, record);
            record.Data = _topRecordsCollection.GetRandomSequence();
            record.Mutate();
        }
        
    }

    class TopRecordsCollection
    {
        private readonly int _limit;
        private readonly SortedDictionary<float, LemmingMovementDirection[]> _data;

        public TopRecordsCollection(int limit)
        {
            _limit = limit;
            _data = new SortedDictionary<float, LemmingMovementDirection[]>();
        }

        public void TryPut(float distance, LemmingRunRecord record)
        {
            if (_data.Count > 0 && _data.Keys.All(key => key < distance) || _data.ContainsKey(distance))
                return;
            _data[distance] = record.Data.ToArray();
            if (_data.Count > _limit)
                _data.Remove(_data.Keys.Last());
        }

        public LemmingMovementDirection[] GetRandomSequence()
        {
            var random = new Random();
            var keyCollection = _data.Keys.ToArray();
            return _data[keyCollection[random.Next(0, keyCollection.Length)]];
        }
    }
}