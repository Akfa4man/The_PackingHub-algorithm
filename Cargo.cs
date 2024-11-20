using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_PackingHub___algorithm
{
    internal class Cargo
    {
        public string Name { get; set; }
        public float Length { get; }
        public float Width { get; }
        public float Height { get; }
        public float Weight { get; }
        public ManipulativeSigns Signs { get; }

        public Cargo(float length, float width, float height, float weight, ManipulativeSigns signs)
        {
            Length = length;
            Width = width;
            Height = height;
            Weight = weight;
            Signs = signs;
        }

        public IEnumerable<(float, float, float)> GetOrientations()
        {
            yield return (Length, Width, Height);
            yield return (Length, Height, Width);
            yield return (Width, Length, Height);
            yield return (Width, Height, Length);
            yield return (Height, Length, Width);
            yield return (Height, Width, Length);
        }
    }
}