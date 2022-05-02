/***************************************************************************************************
* Copyright 2006 (c) Daniel Sevitti <https://github.com/ibprogrammin>
*
* Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
* associated documentation files (the "Software"), to deal in the Software without restriction,
* including without limitation the rights to use, copy, modify, merge, publish, distribute,
* sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all copies or
* substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
* NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
* DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
**************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WaterMangoApp
{
    /// <summary>
    /// the WaterMango.Plant object for storing the plant data
    /// </summary>
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastWatered { get; set; }
        public int SecondsWatered { get; set; }
        public bool Watered { 
            get { return IsPlantWatered(); }
            set { }
        }
        public bool Watering { get; set; }

        public Plant(int id, string name, DateTime lastWatered, int secondsWatered)
        {
            Id = id;
            Name = name;
            LastWatered = lastWatered;
            SecondsWatered = secondsWatered;
            Watering = false;
        }

        /// <summary>
        /// Checks that 6 hours have passed since the plant was watered and return true or false 
        /// </summary>
        /// <returns>a boolean indicating if the plant has been watered or not</returns>
        public bool IsPlantWatered()
        {
            // computes the number of ticks that have elapsed from now until the last time the plant was watered
            long elapsedTicks = DateTime.Now.Ticks - LastWatered.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            // Check if 6 hours have passed
            if (elapsedSpan.TotalHours >= 6)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Checks that 30 seconds have passed since the last time the plant was watered
        /// </summary>
        /// <param name="lastWatered"></param>
        /// <returns></returns>
        public static bool CanPlantBeWatered(DateTime lastWatered)
        {
            //Check the elapsed time between now and the last time the plant was watered
            long elapsedTicks = DateTime.Now.Ticks - lastWatered.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            //Make sure that 30 seconds have passed
            if (elapsedSpan.TotalSeconds > 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
