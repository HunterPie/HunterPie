using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.UI.Architecture.Extensions;

public static class ObjectExtensions
{
    extension(object self)
    {
        /// <summary>
        /// Converts a boxed string into thickness
        /// </summary>
        /// <returns>Thickness</returns>
        public Thickness ToThickness()
        {
            if (self is string str)
            {
                double[] values = str.Split(',')
                    .Select(e => double.Parse(e))
                    .ToArray();

                return new Thickness(values[0], values[1], values[2], values[3]);
            }

            throw new ArgumentException("argument must be of type string");
        }
    }

    extension<T>(T obj) where T : class
    {
        /// <summary>
        /// Applies values to T
        /// </summary>
        /// <param name="block">Function that applies properties</param>
        public T Apply(Action<T> block)
        {
            block(obj);

            return obj;
        }

        public async Task<T> ApplyAsync(Func<T, Task> block)
        {
            await block(obj);

            return obj;
        }
    }
}