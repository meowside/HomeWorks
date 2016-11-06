using System;

namespace WannaBeSifrovani
{
    class Program
    {
        static void Main(string[] args)
        {
            int offsetKey = 0;

            string txt = "";

            do
            {
                Console.Write("Text k sifrovani:  ");
                txt = Console.ReadLine();
            } while (txt == "");

            while (true)
            {
                Console.Write("Posun: ");
                try
                {
                    int.TryParse(Console.ReadLine(), out offsetKey);
                }
                catch
                {
                    continue;
                }

                break;
            }

            string encryptedText = "";

            offsetKey = offsetKey % 26;

            foreach(char letter in txt)
            {
                int addedLetter;
                
                //AbCdEfGhIjKlMnOpQrStUvWxYz1

                if (char.IsLetter(letter))
                {
                    addedLetter = (letter + offsetKey);

                    if (char.IsLower(letter))
                    {
                        if (addedLetter > 'z')
                            addedLetter -= ('z' - 'a' + 1);

                        else if (addedLetter < 'a')
                            addedLetter += ('z' - 'a' + 1);
                    }

                    else
                    {
                        if (addedLetter > 'Z')
                            addedLetter -= ('Z' - 'A' + 1);

                        else if (addedLetter < 'A')
                            addedLetter += ('Z' - 'A' + 1);
                    }
                }

                else
                    addedLetter = letter;

                encryptedText += (char)addedLetter;

            }


            Console.WriteLine($"Sifrovany text:  {encryptedText}");

            Console.ReadLine();
        }
    }
}
