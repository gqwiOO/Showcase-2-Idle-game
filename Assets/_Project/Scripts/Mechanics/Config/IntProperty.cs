using System;
using UnityEngine;

namespace Mechanics.Config
{
    [Serializable]
    public enum IntPropertyMode
    {
        Constant,
        RandomBetweenConstants
    }

    [Serializable]
    public struct IntProperty
    {
        private static readonly System.Random _random = new System.Random();

        [SerializeField] private IntPropertyMode _mode;
        [SerializeField] private int _constantValue;
        [SerializeField] private int _minValue;
        [SerializeField] private int _maxValue;

        public IntPropertyMode Mode => _mode;
        public int ConstantValue => _constantValue;
        public int MinValue => _minValue;
        public int MaxValue => _maxValue;

        public int Value
        {
            get
            {
                switch (_mode)
                {
                    case IntPropertyMode.Constant:
                        return _constantValue;
                    case IntPropertyMode.RandomBetweenConstants:
                        int min = Math.Min(_minValue, _maxValue);
                        int max = Math.Max(_minValue, _maxValue);
                        return _random.Next(min, max + 1);
                    default:
                        return _constantValue;
                }
            }
        }

        public static IntProperty Constant(int value)
        {
            return new IntProperty
            {
                _mode = IntPropertyMode.Constant,
                _constantValue = value,
                _minValue = value,
                _maxValue = value
            };
        }

        public static IntProperty RandomBetween(int min, int max)
        {
            return new IntProperty
            {
                _mode = IntPropertyMode.RandomBetweenConstants,
                _constantValue = min,
                _minValue = min,
                _maxValue = max
            };
        }
    }
}
