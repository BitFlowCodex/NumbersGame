namespace NumbersGame;

internal class Program
{
    static void Main(string[] args)
    {
        // Stores different console responses for each type of guess
        Dictionary<string, string[]> responses = new()
        {
            ["high"] =
                [
                    "Åh nej, du gissade för högt!",
                    "Tyvärr, du gissade för högt!",
                    "bra försök, men du gissade för högt",
                    "haha inte den siffra, Du gissade för högt!"
                ],
            ["low"] =
                [
                    "Åh nej, du gissade för lågt!",
                    "Tyvärr, du gissade för lågt!",
                    "bra försök, men du gissade för lågt",
                    "haha inte den siffra, Du gissade för lågt!"
                ],
            ["hot"] =
                [
                    "Det bränns!",
                    "Du är nära!",
                    "Riktig nära!"
                ]
        };

        // Selects a random response for each type of guess
        (string, string, string) Response(Dictionary<string, string[]> dict)
        {
            Random random = new Random();
            string high = "", low = "", hot = "";

            foreach (var response in dict)
            {
                int randomIndex = random.Next(0, response.Value.Length);

                if (response.Key == "high")
                {
                    high = response.Value[randomIndex];
                }
                else if (response.Key == "low")
                {
                    low = response.Value[randomIndex];
                }
                else
                {
                    hot = response.Value[randomIndex];
                }
            }

            return (high, low, hot);
        }

        // Sets the overall difficulty of the game.
        (int, int) Difficulty()
        {
            Random random = new Random();

            Console.Write("Välj svårighetsgraden (1-3): ");
            string? userInput = Console.ReadLine();

            if (!int.TryParse(userInput, out int difficultyLevel))
            {
                Console.WriteLine("Du måste skriva en siffra!");
                return Difficulty();
            }

            switch (difficultyLevel)
            {
                case 1: return (10, random.Next(1, 15));

                case 2: return (5, random.Next(1, 25));

                case 3: return (3, random.Next(1, 40));

                default:
                    Console.WriteLine("Välj mellan 1 och 3");
                    return Difficulty();
            }
        }

        // Manages the game state, whether the player wants to play again or not
        bool GameState()
        {
            Console.Write("Vill du spela igen? (ja/nej): ");
            string? userInput = Console.ReadLine();

            // checks the input and ignores capatilization
            if (userInput?.Equals("ja", StringComparison.OrdinalIgnoreCase) == true) return true;
            if (userInput?.Equals("nej", StringComparison.OrdinalIgnoreCase) == true) return false;

            Console.WriteLine("Skriv ja eller nej");
            return GameState();
        }

        // Checks the guess and updates the player's remaining chances
        int CheckGuess(int userGuess, int number, int userChance)
        {
            (string highResp, string lowResp, string hotResp) = Response(responses);

            if (Math.Abs(userGuess - number) == 1)
            {
                Console.WriteLine(hotResp);
                userChance--;
            }

            else if (userGuess > number)
            {
                Console.WriteLine(highResp);
                userChance--;
            }

            else if (userGuess < number)
            {
                Console.WriteLine(lowResp);
                userChance--;
            }

            else
            {
                Console.WriteLine("Wohoo! Du klarade det!");
            }

            return userChance;
        }

        // Our main game loop
        void Game()
        {
            (int chance, int number) = Difficulty();
            int oldChance = chance;
            Console.WriteLine($"Välkommen! Jag tänker på ett nummer. Kan du gissa vilket? Du får {oldChance} försök. {number}");

            while (chance > 0)
            {
                Console.Write("Gissa: ");
                string? playerInput = Console.ReadLine();

                if (int.TryParse(playerInput, out int playerGuess))
                {
                    int updatedUserChance = CheckGuess(playerGuess, number, chance);
                    chance = updatedUserChance;

                    if (playerGuess == number)
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Du måste skriva något!");
                }

                if (chance <= 0)
                {
                    Console.WriteLine($"Tyvärr, du lyckades inte gissa talet på {oldChance} försök!");
                }
            }
        }

        // Controls whether the game continues or not
        while (true)
        {
            Game();

            if (!GameState()) break;
        }
    }
}