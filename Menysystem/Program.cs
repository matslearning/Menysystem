using System;
using System.Dynamic;
using System.Threading;

namespace Menysystem
{
    class Program
    {
        static string pressKeyText = "Tryck en tangent för att fortsätta!";
        static string illegalMenuChoiceText = "Felaktigt val!";
        static string illegalNrOfWordsText = "Alltför litet antal ord i den meningen!";
        static string askForAgeText = "Ange en ålder i siffror: ";
        static string unparsableInteger = "Felaktigt heltal!";
        static string goodbyeText = "Over and out :)" + Environment.NewLine;
        static string menuText =
                "Välkomna till huvudmenyn!@@" +
                "Följande menyval finns:@" +
                "1. Ungdom eller pensionär@" +
                "2. Upprepa tio gånger@" +
                "3. Det tredje ordet@" +
                "4. Pris för helt sällskap@" +
                "0. Avsluta@@" +
                "Skriv in en siffra för att testa en funktion: ";
        static ConsoleColor normalColor = ConsoleColor.White;
        static ConsoleColor errorColor = ConsoleColor.Red;
        static ConsoleColor logoffColor = ConsoleColor.Green;
        static ConsoleColor thirdWordColor = ConsoleColor.Yellow;
        static string newLine = Environment.NewLine;
        struct PriceInfo
        {
            public int price;
            public string priceRange;
        }

        /// <summary>
        /// Huvudslinga med huvudmeny. Låter användaren välja bland diverse funktioner.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            bool menuIsActive = true;
            while(menuIsActive)
            {
                SetForeground(normalColor);
                DisplayWelcomeMessage();
                string input = Console.ReadLine();
                switch(input)
                {
                    case "0":
                        WriteColorMessage(goodbyeText, logoffColor);
                        menuIsActive = false;
                        break;
                    case "1":
                        MenuChoiceOne();
                        break;
                    case "2":
                        MenuChoiceTwo();
                        break;
                    case "3":
                        MenuChoiceThree();
                        break;
                    case "4":
                        MenuChoiceFour();
                        break;
                    case "":
                        //ignorera tom sträng - detta case kan tas bort om man även vill räkna tom sträng som ett fel
                        break;
                    default:
                        DisplayErrorMessage(illegalMenuChoiceText);
                        break;
                }
            }
        }


        /// <summary>
        /// Sätter textfärg för konsolfönstret
        /// </summary>
        /// <param name="textcolor">Textfärgen</param>
        private static void SetForeground(ConsoleColor textcolor)
        {
            Console.ForegroundColor = textcolor;
        }

        /// <summary>
        /// Menyval nummer 1 - Ungdom eller pensionär
        /// Låter användaren ange en ålder och visar sedan prisklass och pris
        /// </summary>
        private static void MenuChoiceOne()
        {
            DisplayMenuChoiceHeader("Ungdom eller pensionär");

            int age = GetIntegerFromUser(askForAgeText);
            PriceInfo info = GetPriceInfo(age);
            Console.WriteLine(info.priceRange + ": " + info.price + "kr");

            WaitForKeyPress(pressKeyText);
        }

        /// <summary>
        /// Översätter ålder till prisinformation
        /// </summary>
        /// <returns>Struktur som innehåller pris och prisklass, t.ex. "Ungdomsspris"</returns>
        private static PriceInfo GetPriceInfo(int age)
        {
            PriceInfo info;
            int price;
            string priceRange;

            if (age < 20)
            {
                info.price = 80;
                info.priceRange = "Ungdomspris";
            }
            else if (age > 64)
            {
                info.price = 90;
                info.priceRange = "Pensionärspris";
            }
            else
            {
                info.price = 120;
                info.priceRange = "Standardpris";
            }
            return info;
        }

        /// <summary>
        /// Menyval nummer 2 - Upprepa tio gånger
        /// Låter användaren skriva in ett ord och visar sedan ordet tio gånger i rad
        /// </summary>
        private static void MenuChoiceTwo()
        {
            DisplayMenuChoiceHeader("Upprepa tio gånger");
            string arbitraryText = GetStringFromUser("Skriv in en text: ");

            for(int i=1; i<=10; i++)
            {
                Console.Write(i + "." + arbitraryText + ((i<10) ? ", " : ""));
            }

            WaitForKeyPress(newLine + pressKeyText);
        }

        /// <summary>
        /// Menyval nummer 3 - Det tredje ordet
        /// Låter användaren skriva in ett antal ord och visar sedan det tredje av dessa ord
        /// </summary>
        private static void MenuChoiceThree()
        {
            DisplayMenuChoiceHeader("Det tredje ordet");
            string sentence;
            string[] array;
            do
            {
                sentence = GetStringFromUser("Skriv in en mening som innehåller minst tre ord: ");
                array = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if(array.Length < 3)
                {
                    WriteColorMessage(illegalNrOfWordsText + newLine, errorColor);
                }
            } while (array.Length < 3);

            Console.Write("Det tredje ordet är: ");
            WriteColorMessage(array[2], thirdWordColor);

            WaitForKeyPress(newLine + pressKeyText);
        }
        /// <summary>
        /// Låter användaren mata in åldern för helt sällskap - för att få information om totalpris
        /// </summary>
        private static void MenuChoiceFour()
        {
            DisplayMenuChoiceHeader("Pris för helt sällskap");

            int nrOfAttendees = GetIntegerFromUser("Ange hur många biobesökare sällskapet har: ");

            int total = 0;

            for (int i=1; i<=nrOfAttendees; i++)
            {
                int age = GetIntegerFromUser($"Ange ålder på besökare nummer {i}: ");
                PriceInfo info = GetPriceInfo(age);
                total += info.price;
            }

            Console.WriteLine();
            Console.WriteLine($"Antal personer: {nrOfAttendees}");
            Console.WriteLine($"Totalkostnad:   {total}");

            WaitForKeyPress(pressKeyText);
        }

        private static void DisplayWelcomeMessage()
        {
            Console.Clear();
            Console.Write(menuText.Replace("@", newLine));
        }
        /// <summary>
        /// Väntar tills användaren har tryckt på en tangent
        /// </summary>
        /// <param name="prompt">Eventuell text som visas för användaren</param>
        private static void WaitForKeyPress(string prompt)
        {
            Console.WriteLine();
            Console.WriteLine(prompt);
            Console.ReadKey();
        }

        /// <summary>
        /// Visar instruktionstext och låter användaren skriva in en textsträng
        /// </summary>
        /// <param name="prompt">Instruktionstext som visas för användaren</param>
        /// <returns>Textsträngen</returns>
        private static string GetStringFromUser(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        /// <summary>
        /// Visar instruktionstext och låter användaren skriva in ett heltal.
        /// OBS! Ger sig inte förrän talet är giltigt.
        /// </summary>
        /// <param name="prompt">Instruktionstext som visas för användaren</param>
        /// <returns>Heltalet</returns>
        private static int GetIntegerFromUser(string prompt)
        {
            int integer = 0;
            bool isInteger = false;
            while(isInteger == false)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if(int.TryParse(input, out integer))
                {
                    isInteger = true;
                }
                else
                {
                    WriteColorMessage(unparsableInteger + newLine, errorColor);
                }
            }
            return integer;
        }

        /// <summary>
        /// Visar felmeddelande och väntar sedan på tangenttryckning
        /// </summary>
        /// <param name="errorMessage">Själva felmeddelandet</param>
        private static void DisplayErrorMessage(string errorMessage)
        {
            WriteColorMessage(errorMessage + " " + pressKeyText, errorColor);
            WaitForKeyPress("");
        }

        /// <summary>
        /// Skriver ut textmeddelande i valfri textfärg
        /// </summary>
        /// <param name="message">Meddelande som skall visas</param>
        /// <param name="color">Textfärg på meddelandet</param>
        private static void WriteColorMessage(string message, ConsoleColor color)
        {
            SetForeground(color);
            Console.Write(message);
            SetForeground(normalColor);
        }
        /// <summary>
        /// Används vid utskrift av menyval med streckad rad ovanför
        /// </summary>
        /// <param name="header">Text som beskriver menyvalet</param>
        private static void DisplayMenuChoiceHeader(string header)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Menyval - " + header + newLine);
        }
    }
}
