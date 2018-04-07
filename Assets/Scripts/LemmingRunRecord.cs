using System.Collections.Generic;

namespace DefaultNamespace
{
    public class LemmingRunRecord
    {
        private List<LemmingMovementDirection> _data;

        public LemmingRunRecord()
        {
            _data = new List<LemmingMovementDirection>();
        }

        public LemmingRunRecord(LemmingMovementDirection[] data)
        {
            _data = new List<LemmingMovementDirection>(data);
        }

        public LemmingMovementDirection GetOrGenerateNextMovement(int currentStep)
        {
            return _data.Count > currentStep ? _data[currentStep] : LemmingUtils.GenerateNextDirection();
        }

        public int Size
        {
            get { return _data.Count; }
        }

        public LemmingMovementDirection this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }
    }
}