using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class LemmingRunRecord
    {
        [SerializeField]
        private List<LemmingMovementDirection> _data = new List<LemmingMovementDirection>();

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
            if(_data.Count <= currentStep)  
                _data.Add(LemmingUtils.GenerateNextDirection());

            return _data[currentStep];
        }

        public void AddMovement(LemmingMovementDirection direciton)
        {
            _data.Add(direciton);
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