using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beep
{
    class Lyrics
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Text { get; set; }

        public Lyrics(TimeSpan Start, TimeSpan End, string Text)
        {
            this.Text = Text;
            this.StartTime = Start;
            this.EndTime = End;
        }
    }
}
