using System;

namespace TieFighter.Models
{
    public class Volume
    {
        private double _Value;

        private double Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                if (value < 0 && value > 100)
                {
                    throw new ArgumentOutOfRangeException("The value must " +
                        "be greater than or equal to zero and less than or " +
                        "equal to 100.");
                }
                else
                {
                    this._Value = value;
                }
            }
        }

        public static implicit operator double(Volume volume)
        {
            return volume.Value;
        }

        public static implicit operator Volume(double value)
        {
            return new Volume()
            {
                Value = value
            };
        }
    }
}
