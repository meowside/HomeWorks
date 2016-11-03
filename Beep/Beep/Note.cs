using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beep
{
    class Note
    {
        public int Frequency { get; set; }
        public int Duration { get; set; }

        public TimeSpan Start { get; set; }

        public Note(int Freq, int Dur, TimeSpan Start)
        {
            this.Frequency = Freq;
            this.Duration = Dur;

            this.Start = Start;
        }

        public override string ToString()
        {
            return string.Format($"{Frequency}\n{Duration}\n{Start}\n\n");
        }
    }
}
