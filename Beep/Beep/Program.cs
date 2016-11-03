using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Beep
{
    class Program
    {
        static readonly int BPM = 134;

        static readonly int[] C = new int[]
        {
            16, 32, 64, 130, 260, 522, 1046, 2092, 4186
        };

        static readonly int[] CS = new int[]
        {
            16, 34, 68, 138, 276, 554, 1108, 2216, 4434
        };

        static readonly int[] D = new int[]
        {
            18, 36, 72, 146, 292, 586, 1174, 2348, 4698
        };

        static readonly int[] DS = new int[]
        {
            18, 38, 78, 154, 310, 622, 1244, 2488, 4978
        };

        static readonly int[] E = new int[]
        {
            20, 40, 82, 164, 328, 658, 1318, 2636, 5274
        };

        static readonly int[] F = new int[]
        {
            20, 42, 86, 174, 350, 698, 1396, 2792, 5586
        };

        static readonly int[] FS = new int[]
        {
            22, 46, 92, 184, 368, 740, 1478, 2958, 5918
        };

        static readonly int[] G = new int[]
        {
            24, 48, 98, 196, 392, 782, 1566, 3134, 6270
        };

        static readonly int[] GS = new int[]
        {
            26, 52, 104, 206, 414, 830, 1660, 3322, 6644
        };

        static readonly int[] A = new int[]
        {
            26, 54, 110, 220, 440, 880, 1760, 3520, 7040
        };

        static readonly int[] AS = new int[]
        {
            28, 58, 116, 232, 466, 932, 1864, 3728, 7458
        };

        static readonly int[] B = new int[]
        {
            30, 60, 122, 246, 492, 986, 1974, 3950, 7902 
        };

        static void Main(string[] args)
        {

            int width = 80;
            int height = 30;

            while (true)
            {
                try
                {
                    Console.SetWindowSize(width, height);
                    Console.SetBufferSize(width, height);
                }
                catch (Exception e)
                {
                    width--;
                    height--;
                    continue;
                }

                break;
            }

            TimeSpan Offset = new TimeSpan(0, 0, 0, 0, 0);

            List<Lyrics> Text = LoadText();
            List<Note> Song = LoadSong();
            
            Note outNote = null;
            Lyrics outText = null;

            Stopwatch sync = Stopwatch.StartNew();

            Task[] waitForComplete = new Task[2];

            waitForComplete[0] = Task.Factory.StartNew(() =>
            {
                while (sync.ElapsedMilliseconds + Offset.TotalMilliseconds < Song.Last().Start.TotalMilliseconds + Song.Last().Duration)
                {
                    Note temp = Song.FirstOrDefault(note => note.Start.TotalMilliseconds <= sync.ElapsedMilliseconds + Offset.TotalMilliseconds && note.Start.TotalMilliseconds + note.Duration > sync.ElapsedMilliseconds + Offset.TotalMilliseconds);

                    if (!ReferenceEquals(temp, outNote))//temp != outNote)
                    {
                        outNote = temp;
                        Console.Beep(temp.Frequency, (int)(temp.Start.TotalMilliseconds + temp.Duration - sync.ElapsedMilliseconds - Offset.TotalMilliseconds));
                    }
                }
            });

            waitForComplete[1] = Task.Factory.StartNew(() =>
            {

                while (sync.ElapsedMilliseconds + Offset.TotalMilliseconds < Song.Last().Start.TotalMilliseconds + Song.Last().Duration)
                {
                    Lyrics temp2 = Text.FirstOrDefault(txt => txt.StartTime.TotalMilliseconds <= sync.ElapsedMilliseconds + Offset.TotalMilliseconds && txt.EndTime.TotalMilliseconds > sync.ElapsedMilliseconds + Offset.TotalMilliseconds);

                    if (temp2 != null)
                    {
                        if (!ReferenceEquals(temp2, outText))
                        {
                            Console.Clear();

                            int i = Text.IndexOf(temp2);

                            if(i > 1)
                            {
                                Console.CursorTop = height / 2 - 2;
                                Console.CursorLeft = (width - Text[i - 2].Text.Length) / 2;
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine(Text[i - 2].Text);
                            }

                            if(i > 0)
                            {
                                Console.CursorTop = height / 2 - 1;
                                Console.CursorLeft = (width - Text[i - 1].Text.Length) / 2;
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine(Text[i - 1].Text);
                            }

                            Console.CursorTop = height / 2;
                            Console.CursorLeft = (width - Text[i].Text.Length) / 2;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(temp2.Text);

                            if(i < Text.Count - 1)
                            {
                                Console.CursorTop = height / 2 + 1;
                                Console.CursorLeft = (width - Text[i + 1].Text.Length) / 2;
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine(Text[i + 1].Text);
                            }

                            if (i < Text.Count - 2)
                            {
                                Console.CursorTop = height / 2 + 2;
                                Console.CursorLeft = (width - Text[i + 2].Text.Length) / 2;
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine(Text[i + 2].Text);
                            }


                            outText = temp2;
                            
                        }
                    }

                    Console.CursorLeft = 2;
                    Console.CursorTop = height - 2;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(sync.Elapsed + Offset);

                    Thread.Sleep(1);
                }
            });

            Task.WaitAll(waitForComplete);

            sync.Stop();

            while (Console.KeyAvailable)
                Console.ReadKey();

            Console.Read();
        }

        private static List<Lyrics> LoadText()
        {
            List<Lyrics> toWrite = new List<Lyrics>();

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 0, 0),
                                   new TimeSpan(0, 0, 0, 1, 500),
                                   "Oh won't you tell me"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 1, 500),
                                   new TimeSpan(0, 0, 0, 3, 200),
                                   "Won't you tell me"));
            
            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 3, 200),
                                    new TimeSpan(0, 0, 0, 7, 0),
                                   "This thing I've come to be?"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 7, 0),
                                    new TimeSpan(0, 0, 0, 10, 200),
                                   "The monster that you see"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 10, 200),
                                    new TimeSpan(0, 0, 0, 14, 0),
                                   "Is it a part of me?"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 14, 0),
                                    new TimeSpan(0, 0, 0, 17, 200),
                                   "I'm breaking down and shaking"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 17, 200),
                                    new TimeSpan(0, 0, 0, 21, 0),
                                   "This world so helplessly"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 21, 0),
                                    new TimeSpan(0, 0, 0, 24, 200),
                                   "But you just laugh and grin"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 24, 200),
                                    new TimeSpan(0, 0, 0, 26, 0),
                                   "Completely blind within"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 41, 500),
                                    new TimeSpan(0, 0, 0, 45, 0),
                                   "There's no point now, broken anyway"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 45, 0), new TimeSpan(0, 0, 0, 48, 0),
                                   "I try to stop my breath"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 48, 0), new TimeSpan(0, 0, 0, 51, 200),
                                   "Even knowing the truth won't unravel"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 51, 200), new TimeSpan(0, 0, 0, 54, 0),
                                   "Me until my death"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 54, 300), new TimeSpan(0, 0, 0, 55, 0),
                                   "Freeze"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 55, 0), new TimeSpan(0, 0, 0, 57, 0),
                                   "So breakable, unbreakable"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 57, 0), new TimeSpan(0, 0, 0, 59, 0),
                                   "I'm shaking yet but unshakable"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 0, 59, 0), new TimeSpan(0, 0, 1, 2, 300),
                                   "The real you I've found at last"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 2, 300), new TimeSpan(0, 0, 1, 6, 800),
                                   "I'm standing alone in this world that keeps on changing"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 6, 800), new TimeSpan(0, 0, 1, 9, 500),
                                   "But hiding away, my true self is fading!"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 9, 500), new TimeSpan(0, 0, 1, 12, 0),
                                   "I hope you give up"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 12, 0), new TimeSpan(0, 0, 1, 15, 500),
                                   "There's nothing left to see..."));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 15, 500), new TimeSpan(0, 0, 1, 17, 600),
                                   "No, don't look at me"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 17, 600), new TimeSpan(0, 0, 1, 20, 600),
                                   "I'm standing in this world that someone imagined"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 20, 600), new TimeSpan(0, 0, 1, 23, 0),
                                   "I never want to hurt you, so until the end"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 23, 800), new TimeSpan(0, 0, 1, 30, 0),
                                   "I'm begging you, please, just to remember me..."));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 33, 500), new TimeSpan(0, 0, 1, 37, 0),
                                   "As clear as I used to be"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 42, 500), new TimeSpan(0, 0, 1, 46, 0),
                                   "The loneliness that wraps around keeps deepening until I drown"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 46, 0), new TimeSpan(0, 0, 1, 49, 0),
                                   "Fond memories we used to share pierce me 'til I no longer care"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 49, 0), new TimeSpan(0, 0, 1, 50, 0),
                                   "I cannot run"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 50,0), new TimeSpan(0, 0, 1, 51, 0),
                                   "I cannot hide"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 51, 0), new TimeSpan(0, 0, 1, 52, 0),
                                   "I cannot think"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 52, 0), new TimeSpan(0, 0, 1, 53, 0),
                                   "I cannot find"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 53, 0), new TimeSpan(0, 0, 1, 54, 0),
                                   "I cannot move"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 54, 0), new TimeSpan(0, 0, 1, 55, 0),
                                   "I cannot leave you!"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 1, 56, 0), new TimeSpan(0, 0, 2, 0, 0),
                                   "Unravelling the world!"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 12, 900), new TimeSpan(0, 0, 2, 14, 0),
                                   "A change so illogical"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 14, 0), new TimeSpan(0, 0, 2, 15, 500),
                                   "It shouldn't have been possible"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 15, 500), new TimeSpan(0, 0, 2, 17, 0),
                                   "But as our lives are intertwined"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 17, 0), new TimeSpan(0, 0, 2, 20, 0),
                                   "The two of us are left behind"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 41, 0), new TimeSpan(0, 0, 2, 44, 500),
                                   "I'm standing alone in this world that keeps on changing"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 44, 500), new TimeSpan(0, 0, 2, 48, 0),
                                   "But hiding away, my true self is fading!"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 48, 200), new TimeSpan(0, 0, 2, 50, 0),
                                   "I hope you give up"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 50, 0), new TimeSpan(0, 0, 2, 53, 0),
                                   "There's nothing left to see..."));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 53, 0), new TimeSpan(0, 0, 2, 55, 0),
                                   "No, don't look at me"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 55, 0), new TimeSpan(0, 0, 2, 58, 500),
                                   "I'm trapped within this world that someone planned ahead of us"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 2, 58, 500), new TimeSpan(0, 0, 3, 2, 0),
                                   "Before our fate and future starts crumbling back to dust"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 2, 0), new TimeSpan(0, 0, 3, 8, 0),
                                   "I'm begging you, please, just to remember me..."));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 9, 500), new TimeSpan(0, 0, 3, 13, 0),
                                   "As clear as I used to be..."));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 13, 0), new TimeSpan(0, 0, 3, 14, 600),
                                   "No, don't forget me!"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 14, 600), new TimeSpan(0, 0, 3, 16, 200),
                                   "No, don't forget me!"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 16, 200), new TimeSpan(0, 0, 3, 17, 800),
                                   "No, don't forget me!"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 17, 800), new TimeSpan(0, 0, 3, 19, 400),
                                   "No, don't forget me!"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 20, 0), new TimeSpan(0, 0, 3, 23, 500),
                                   "Shocked by how I was changed, I remain paralyzed"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 23, 500), new TimeSpan(0, 0, 3, 27, 0),
                                   "Until I have the chance to find my own paradise"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 27, 0), new TimeSpan(0, 0, 3, 30, 0),
                                   "I'm begging you, please, just to remember me"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 37, 500), new TimeSpan(0, 0, 3, 39, 0),
                                   "Oh won't you tell me..."));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 41, 500), new TimeSpan(0, 0, 3, 43, 0),
                                   "Won't you tell me, please..."));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 45, 0), new TimeSpan(0, 0, 3, 48, 0),
                                   "The monster that you see"));

            toWrite.Add(new Lyrics(new TimeSpan(0, 0, 3, 48, 500), new TimeSpan(0, 0, 3, 50, 0),
                                   "Is it a part of me?"));

            return toWrite;
        }

        private static List<Note> LoadSong()
        {
            string toPlay = "";

            toPlay += "C-6-4/AS-6-4/A-6-2/G-6-4/C-6-4/AS-6-4/A-6-4/G-6-4/G-6-2/F-6-6/DS-6-2/DS-6-3/F-6-3/D-6-14/P-2/D-6-2/";
            toPlay += "D-6-4/D-6-2/D-6-4/D-7-2/D-7-14/P-2/AS-6-2/A-6-4/A-6-2/A-6-4/AS-6-2/AS-6-14/P-4/";
            toPlay += "C-6-4/AS-6-4/A-6-2/G-6-4/C-6-4/AS-6-4/A-6-4/G-6-4/G-6-2/F-6-6/DS-6-2/DS-6-3/F-6-3/D-6-14/P-2/D-6-2/";
            toPlay += "D-6-4/D-6-2/D-6-4/D-7-2/D-7-14/P-2/AS-6-2/A-6-4/A-6-2/A-6-4/AS-6-2/AS-6-14/";
            toPlay += "G-4-3/G-4-3/G-4-2/G-4-3/G-4-1/D-4-4/";
            toPlay += "D-6-1/D-5-1/G-5-1/C-6-1/D-5-1/G-5-1/AS-5-1/D-5-1/D-6-1/D-5-1/G-5-1/C-6-1/D-5-1/G-5-1/AS-5-2/";
            toPlay += "G-4-3/G-4-3/G-4-2/G-4-3/G-4-1/G-4-4/";
            toPlay += "D-6-1/AS-6-2/D-6-1/G-6-1/D-6-1/AS-5-1/G-5-1/F-3-1/F-2-1/D-5-2/C-5-2/D-5-2/";
            toPlay += "D-6-1/AS-5-2/D-6-1/G-5-1/D-6-1/AS-5-1/G-5-1/D-3-1/D-3-1/D-5-2/AS-5-2/A-5-2/";
            toPlay += "D-6-1/AS-6-2/D-6-1/G-6-1/D-6-1/AS-5-1/G-5-1/F-3-1/F-2-1/D-5-2/C-5-2/D-5-2/";
            toPlay += "D-6-1/AS-5-2/D-6-1/G-5-1/D-6-1/AS-5-1/G-5-1/D-3-1/D-3-1/D-5-2/AS-5-2/A-5-2/";
            toPlay += "AS-4-3/G-4-2/P-1/C-5-2/AS-4-3/G-4-2/P-1/A-5-2/";
            toPlay += "AS-5-3/AS-5-3/AS-5-6/AS-5-2/D-6-2/D-6-3/C-6-3/C-6-8/AS-5-2/C-6-3/AS-5-3/A-5-4/F-5-4/D-5-12/P-2/A-5-2/";
            toPlay += "AS-5-2/AS-5-1/AS-5-7/AS-5-4/D-6-2/D-6-3/C-6-3/C-6-4/AS-5-6/";
            toPlay += "D-6-4/C-6-2/C-6-6/A-5-2/AS-5-8/D-5-2/F-4-4/A-3-2/F-4-2/";
            toPlay += "F-4-2/D-4-1/D-4-1/D-3-1/F-3-1/A-4-2/AS-4-2/A-4-1/A-4-1/D-3-1/G-3-1/F-4-2/";
            toPlay += "F-4-2/D-4-1/D-4-1/D-3-1/F-3-1/A-4-2/AS-4-2/A-4-1/A-4-1/D-3-1/G-3-1/F-4-2/";
            toPlay += "F-4-2/D-4-1/D-4-1/D-3-1/F-3-1/D-5-2/C-5-2/P-1/D-5-2/P-1/D-5-6/";
            toPlay += "P-2/G-3-1/F-4-1/D-6-4/C-6-4/D-6-3/C-6-3/C-6-2/C-6-3/AS-5-3/A-5-4/AS-5-4/A-5-4/F-5-6/";
            toPlay += "D-6-3/C-6-3/C-6-4/AS-5-4/AS-5-2/A-5-3/AS-5-3/F-6-4/A-5-4/AS-5-2/G-6-3/F-6-3/D-6-4/AS-5-4/AS-5-2/";
            toPlay += "AS-5-4/A-5-2/G-5-4/A-5-4/AS-5-14/D-3-1/G-2-1/AS-4-2/A-4-3/AS-4-3/A-4-4/F-4-4/D-5-2/";
            toPlay += "D-6-2/C-6-1/C-6-3/C-6-2/C-6-2/AS-5-1/AS-5-3/AS-5-2/A-5-3/AS-5-3/A-5-4/F-5-4/D-5-2/";
            toPlay += "D-6-2/C-6-1/C-6-3/C-6-2/C-6-2/AS-5-1/AS-5-3/AS-5-2/A-5-3/AS-5-3/F-6-4/A-5-4/AS-5-2/G-6-3/F-6-3/D-6-3/AS-5-6/AS-5-2/";
            toPlay += "AS-5-4/A-5-2/G-5-4/A-5-4/AS-5-18/D-5-1/D-4-1/F-4-1/AS-4-1/D-4-1/F-4-1/AS-4-1/D-4-1/D-5-1/D-4-1/F-4-1/AS-4-1/D-4-1/";
            toPlay += "C-5-1/C-4-1/F-4-1/AS-4-1/C-4-1/F-4-1/AS-4-1/C-4-1/C-5-1/C-4-1/F-4-1/AS-4-1/C-4-1/F-4-1/AS-5-2/";
            toPlay += "D-6-3/C-6-3/C-6-4/A-5-4/D-4-1/G-5-1/D-5-1/G-5-1/D-5-1/G-5-1/D-5-1/G-5-1/D-5-1/G-5-1/D-5-1/G-5-1/D-6-1/D-5-1/G-5-1/AS-5-1/D-5-1/G-5-1/AS-5-2/";
            toPlay += "D-6-1/D-5-1/F-5-1/AS-5-1/D-5-1/F-5-1/AS-5-1/D-6-1/D-5-1/F-5-1/AS-5-1/D-5-1/F-5-1/AS-5-1/D-5-1/";
            toPlay += "C-6-1/C-5-1/F-5-1/AS-5-1/C-5-1/F-5-1/AS-5-1/C-5-1/C-6-1/C-5-1/F-5-1/AS-5-1/C-5-1/F-5-1/AS-5-1/C-5-1/";
            toPlay += "A-5-1/D-5-1/AS-4-1/D-5-1/AS-4-1/D-5-1/AS-4-1/D-5-1/AS-5-1/G-5-2/AS-5-2/";
            toPlay += "C-6-2/AS-6-1/C-6-3/AS-6-2/C-6-2/AS-6-1/C-6-3/AS-6-2/";
            toPlay += "A-6-2/A-6-1/A-6-3/A-6-2/A-6-2/A-6-1/A-6-3/AS-6-2/";
            toPlay += "C-6-2/AS-6-1/C-6-3/AS-6-2/C-6-2/AS-6-1/C-6-3/AS-6-2/";
            toPlay += "A-6-2/A-6-1/A-6-3/A-6-2/A-6-2/A-6-1/A-6-3/AS-5-2/";
            toPlay += "C-6-2/AS-5-1/C-6-3/AS-6-2/G-6-2/AS-6-1/G-6-3/AS-5-2/";
            toPlay += "C-6-2/AS-5-1/C-6-3/AS-6-2/F-6-2/AS-6-1/F-6-3/AS-5-2/";
            toPlay += "C-6-2/AS-5-1/C-6-3/AS-6-2/F-6-3/AS-6-3/F-6-4/AS-6-6/G-6-4/G-6-4/D-4-2/";
            toPlay += "DS-1-1/D-3-1/G-3-1/A-3-1/AS-3-1/D-4-1/A-4-1/AS-4-1/D-5-1/A-4-1/AS-4-1/D-5-1/G-5-1/AS-5-1/D-6-1/A-6-1/";
            toPlay += "AS-6-1/C-6-1/AS-6-1/AS-5-1/C-6-1/AS-5-1/C-5-1/A-5-1/F-5-1/C-5-1/AS-4-1/F-4-1/C-4-1/AS-3-1/F-2-1/";
            toPlay += "G-1-2/G-3-1/A-3-1/AS-3-1/D-4-1/A-4-1/AS-4-1/D-5-1/G-4-1/A-4-1/D-5-1/G-5-1/AS-5-1/D-6-1/A-6-1/";
            toPlay += "C-6-1/D-6-1/AS-6-1/F-6-1/C-6-1/D-6-1/C-6-1/F-5-1/C-6-1/AS-5-1/A-5-1/F-5-1/C-5-1/A-4-1/F-4-1/C-4-1/";
            toPlay += "DS-1-2/D-3-1/A-3-1/AS-3-1/D-4-1/A-4-1/AS-4-1/D-5-1/G-4-1/A-4-1/D-5-1/G-5-1/AS-5-1/D-6-1/A-6-1/";
            toPlay += "AS-6-1/C-6-1/AS-6-1/F-6-1/AS-5-1/C-6-1/AS-5-1/F-5-1/A-5-1/F-5-1/C-5-1/AS-4-1/F-4-1/C-4-1/AS-3-1/A-3-1/";
            toPlay += "G-1-2/D-3-1/A-3-1/AS-3-1/D-4-1/A-4-1/AS-4-1/D-5-1/G-4-1/A-4-1/D-5-1/G-5-1/AS-5-1/D-6-1/A-6-1/";
            toPlay += "C-6-1/D-6-1/C-6-1/AS-6-1/C-6-1/D-6-1/C-6-1/F-5-1/C-6-1/AS-5-1/A-5-1/C-5-1/AS-4-1/A-4-1/F-4-1/C-4-1/";
            toPlay += "D-6-3/AS-5-3/G-5-2/D-6-3/AS-5-3/G-5-2/C-6-2/A-5-2/F-5-2/C-6-2/A-5-2/F-5-2/";
            toPlay += "AS-6-3/AS-6-3/AS-6-2/AS-6-3/A-6-3/F-6-2/";
            toPlay += "D-6-3/D-6-3/D-6-2/D-6-3/C-6-3/D-6-2/";
            toPlay += "AS-6-8/AS-6-1/DS-6-1/AS-5-1/DS-5-1/AS-4-1/DS-4-1/AS-3-1/DS-3-1/";
            toPlay += "AS-6-8/AS-6-1/DS-6-1/AS-5-1/DS-5-1/AS-4-1/DS-4-1/AS-3-1/DS-3-1/";
            toPlay += "AS-6-8/AS-6-1/DS-6-1/AS-5-1/DS-5-1/AS-4-1/DS-4-1/AS-3-1/DS-3-1/";
            toPlay += "C-6-8/C-6-1/AS-6-1/C-6-1/AS-5-1/D-5-1/AS-4-1/F-4-2/";
            toPlay += "F-4-2/D-4-1/D-4-1/D-3-1/F-3-1/A-4-2/AS-4-2/A-4-1/A-4-1/D-3-1/G-3-1/F-4-2/";
            toPlay += "F-4-2/D-4-1/D-4-1/D-3-1/F-3-1/A-4-2/AS-4-2/A-4-1/A-4-1/D-3-1/G-3-1/F-4-2/";
            toPlay += "F-4-2/D-4-1/D-4-1/D-3-1/F-3-1/A-4-2/AS-4-2/A-4-1/A-4-1/D-3-1/G-3-1/F-4-2/";
            toPlay += "F-4-2/D-4-1/D-4-1/D-3-1/F-3-1/A-4-2/AS-4-2/A-4-1/A-4-1/D-3-1/G-3-1/F-4-2/";
            toPlay += "D-5-1/A-4-1/AS-4-1/F-4-1/G-3-1/A-3-1/D-5-1/A-4-1/AS-4-1/F-4-1/D-4-1/D-4-1/D-5-1/A-4-1/AS-4-2/";
            toPlay += "D-5-1/A-4-1/AS-4-1/F-4-1/G-3-1/A-3-1/D-5-1/A-4-1/AS-4-1/F-4-1/D-4-1/D-4-1/D-5-1/A-4-1/AS-4-2/";
            toPlay += "D-5-1/A-4-1/AS-4-1/F-4-1/G-3-1/A-3-1/D-5-1/C-5-3/D-5-3/D-5-4/DS-6-2/D-6-4/D-6-4/C-6-4/";
            toPlay += "D-6-3/C-6-3/C-6-2/C-6-3/AS-6-3/AS-6-2/";
            toPlay += "A-6-3/AS-6-3/A-6-4/F-6-4/F-6-2/";
            toPlay += "D-6-3/C-6-3/C-6-2/C-6-3/AS-6-3/AS-6-2/";
            toPlay += "A-6-3/AS-6-3/F-6-4/A-6-6/G-6-3/F-6-3/D-6-4/AS-6-4/AS-6-2/";
            toPlay += "AS-6-4/A-6-2/G-6-4/A-6-4/AS-6-12/D-4-2/AS-5-2/";
            toPlay += "A-5-3/AS-5-3/A-5-4/F-5-4/D-5-2/";
            toPlay += "D-6-2/C-6-1/C-6-3/C-6-2/C-6-2/AS-5-1/AS-5-3/AS-5-2/";
            toPlay += "A-5-3/AS-5-3/A-5-4/F-5-4/D-5-2/";
            toPlay += "D-6-2/C-6-1/C-6-3/C-6-2/C-6-2/AS-5-1/AS-5-3/AS-5-2/";
            toPlay += "A-5-3/AS-5-3/F-6-4/A-5-4/AS-5-2/";
            toPlay += "G-6-3/F-6-3/D-6-4/AS-5-4/AS-5-2/";
            toPlay += "AS-5-4/A-5-2/G-5-4/A-5-4/AS-5-18/";
            toPlay += "C-5-1/C-4-1/F-4-1/AS-4-1/C-4-1/F-4-1/AS-4-1/C-4-1/C-5-1/C-4-1/F-4-1/AS-4-1/C-4-1/F-4-1/AS-5-1/";
            toPlay += "C-6-3/AS-5-3/C-6-4/AS-5-4/C-6-4/D-6-12/AS-5-2/";
            toPlay += "G-6-3/F-6-3/D-6-4/AS-5-4/AS-5-2/";
            toPlay += "G-6-3/F-6-3/D-6-4/AS-5-4/AS-5-2/";
            toPlay += "G-6-3/F-6-3/D-6-4/AS-5-4/AS-5-2/";
            toPlay += "G-6-3/F-6-3/D-6-4/AS-5-4/D-5-2/";
            toPlay += "D-6-3/C-6-3/C-6-2/C-6-3/AS-5-3/AS-5-2/";
            toPlay += "A-5-3/AS-5-3/A-5-4/F-5-4/D-5-2/";
            toPlay += "D-6-3/C-6-3/C-6-2/C-6-3/AS-5-3/AS-5-2/";
            toPlay += "A-5-3/AS-5-3/F-6-4/A-5-4/AS-5-2/";
            toPlay += "G-6-3/F-6-3/D-6-2/AS-5-6/AS-5-2/";
            toPlay += "AS-5-4/A-5-2/G-5-4/A-5-4/AS-5-18/";
            toPlay += "C-5-2/G-4-2/AS-4-2/C-5-2/G-4-2/AS-4-2/C-5-2/G-4-2/";
            toPlay += "C-5-2/F-4-2/A-4-2/AS-4-2/F-4-2/A-4-2/AS-4-2/F-4-2/";
            toPlay += "C-5-2/G-4-2/A-4-2/C-5-2/G-4-2/AS-4-2/D-5-2/AS-5-2/";
            toPlay += "C-6-4/AS-5-4/A-5-8/";
            toPlay += "F-4-2/C-5-2/D-5-2/F-4-2/C-5-2/D-5-2/F-4-2/AS-5-2/";
            toPlay += "C-6-4/AS-5-4/A-5-8/";
            toPlay += "F-4-2/A-4-2/AS-4-2/F-4-2/A-4-2/AS-4-2/F-4-2/D-5-2/";
            toPlay += "D-5-4/D-5-2/D-5-4/D-6-2/D-6-12/C-5-2/D-5-2/F-4-2/AS-5-2/";
            toPlay += "A-5-4/A-5-2/A-5-4/AS-5-2/AS-5-4/";
            toPlay += "D-6-1/G-6-1/D-6-1/C-6-1/G-6-1/D-6-1/AS-6-10/";
            toPlay += "D-6-1/G-6-1/D-6-1/C-6-1/G-6-1/D-6-1/AS-6-10/";
            toPlay += "AS-5-16";

            toPlay = toPlay.ToUpper();

            string[] arr = toPlay.Split('/');

            List<Note> notes = new List<Note>();

            for (int i = 0; i < arr.Length; i++)
            {
                int lTemp;
                int fTemp;

                TimeSpan S = new TimeSpan(0, 0, 0, 0, 0);

                if (i > 0)
                    S = notes[i - 1].Start + new TimeSpan(0, 0, 0, 0, notes[i - 1].Duration);

                if (Parse(arr[i], out lTemp, out fTemp))
                {
                    notes.Add(new Note(fTemp, lTemp, S));
                }
            }

            return notes;
        }

        private static bool Parse(string raw, out int lenght, out int freq)
        {
            if (raw == "")
            {
                lenght = 0;
                freq = 0;
                return false;
            }
            
            int[] selectedNote = null;

            string[] Pieces = raw.Split('-');

            double octave = double.Parse(Pieces[1]);
            //double octave = (Pieces[0].Trim() == "P" ? double.Parse(Pieces[1]) : double.Parse(Pieces[1]) - 1);

            if(Pieces.Length > 2)
                if (octave < 0 || octave > 8)
                    throw new ArgumentOutOfRangeException("OCTAVE", "Octave value exceeds range.");

            switch (Pieces[0].Trim())
            {
                case "A":
                    selectedNote = A;
                    break;

                case "AS":
                    selectedNote = AS;
                    break;

                case "B":
                    selectedNote = B;
                    break;

                case "C":
                    selectedNote = C;
                    break;

                case "CS":
                    selectedNote = CS;
                    break;

                case "D":
                    selectedNote = D;
                    break;

                case "DS":
                    selectedNote = DS;
                    break;

                case "E":
                    selectedNote = E;
                    break;

                case "F":
                    selectedNote = F;
                    break;

                case "FS":
                    selectedNote = FS;
                    break;

                case "G":
                    selectedNote = G;
                    break;

                case "GS":
                    selectedNote = GS;
                    break;

                case "P":
                    freq = 22000;
                    lenght = (int)(octave * (60000 / BPM / 4));
                    return true;

                default:
                    throw new Exception("NOT IMPLEMENTED    " + raw);
            }
            
            double duration = double.Parse(Pieces[2]);

            if (duration < 0)
                throw new Exception("Value is less then ZERO");
            

            double LenghtPerNote = (duration > 1 ? 60000 / BPM / 4 : 60000 / BPM / 4 + 10);

            freq = selectedNote[(int)octave];
            lenght = (int)(duration * LenghtPerNote);

            return true;
        }
    }
}
