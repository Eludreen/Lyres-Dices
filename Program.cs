using System.Linq;
namespace Dice_of_lies
{
    // Define a class named Dice
    internal class Dice
    {
        // Private variables to store the number of sides of the dice and the current face value
        private int sides;
        private int face;
        
        // Create a new instance of Random to generate random numbers
        Random random = new Random();
        
        // Constructor that takes the number of sides as a parameter and initializes the dice with a random face value
        public Dice(int Sides)
        {       
            // Set the number of sides of the dice
            sides = Sides;
            
            // Generate a random face value between 1 and the number of sides
            face = random.Next(1, sides+1);
        }
        
        // Method to roll the dice and generate a new random face value
        public void Roll()
        {
            // Generate a new random face value between 1 and the number of sides
            face = random.Next(1, sides + 1);
        }
       
        // Method to get the current face value of the dice
        public int getValue()
        {
            // Return the current face value
            return face;
        }
    }

    // Define a class named Cup
    internal class Cup
    {
        // Private variable to store a list of Dice objects
        private List<Dice> contents = new List<Dice>();
      
        // Constructor that initializes the cup with a specified number of dice and sides for each die
        public Cup(int DiceNum, int sides)
        {
            // Adds dice with 'sides' number of sides
            for (int i = 0; i < DiceNum; i++)
            {
                contents.Add(new Dice(sides));
            }
        }

        // Method to roll all the dice in the cup
        public void roll(int num)
        {
            // Iterate through all dice in the cup and roll each one
            for (int i = 0; i < num; i++)
            {
                contents[i].Roll();
            }
        }

        // Method to get the face values of all dice in the cup
        public int[] getValues()
        {
            // Create an array to store the face values of all dice
            int[] values = new int[contents.Count];

            // Iterate through all dice in the cup and store their face values
            for (int i = 0; i < contents.Count; i++)
            {
                values[i] = contents[i].getValue();

            }
            // Return the array of face values
            return values;
        }
    }

    // Defines a class named coeff
    internal class coeff
    {

        // Private  variables to store various coefficients and values
        private double K;
        private double N;
        private double FactK;
        private double FactN;
        private double Coefficient = 10.0;
        private double Desired;

        // Constructor to initialize coefficients and set the name + desired coefficient based on the style
        public coeff(int k, int playercount, int style, int diceNum, ref string name)
        {
            // Initialize K from the provided parameter
            K = double.Parse(k.ToString());
            // Calculate N based on player count and number of dice
            N = (playercount-1)*diceNum;
            // Set Desired coefficient and name based on the specified style
            switch (style)
            {
                case (1):
                    {
                        Desired = 0.7;
                        Console.WriteLine("Ye better count yer luck against me.. RedBeard");
                        name = "RedBeard";
                        break;
                    }
                case (2):
                    {
                        Desired = 0.5;
                        Console.WriteLine("Ye better count yer luck against me.. GreyBeard");
                        name = "GreyBeard";
                        break;
                    }
                case (3):
                    {
                        Desired = 0.2;
                        Console.WriteLine("Ye better count yer luck against me.. WhiteBeard");
                        name = "WhiteBeard";
                        break;
                    }
                case (4):
                    {
                        Desired = 0.1;
                        Console.WriteLine("Ye better count yer luck against me.. BlackBeard");
                        name = "BlackBeard";
                        break;
                    }
            }
            Thread.Sleep(1000);
        }

        // Method to iterate and calculate until Coefficient is less than or equal to Desired
        public double cutoff()
        {
            while (Coefficient > Desired)
            {
                // Calculate factorial and increment K
                factorialEr();
                K += 1;
            }
            // Return the calculated K value that is closest to the desired vaue
            return K;
        }
        public void factorialEr()
        {
                FactK = getFactorial(K);
                FactN = getFactorial(N);
                const double chance1 = 0.1666666666666666666666666666666;
                const double chance2 = 0.8333333333333333333333333333333;
                double Power1 = getPower(chance1, K);
                double Power2 = getPower(chance2, N-K);
                double braket = getFactorial(N - K);
                double denominator = braket * FactK;
                Coefficient = (FactN / denominator) * Power1 * Power2;
        }
        //Calculates the factorial of a given number
        public double getFactorial(double FactorialMe)
        {
            double carrier = 1;
            for (double x = FactorialMe; x > 0; x--)
            {
                carrier = carrier * x;
            }
            return(carrier);
        } 
        //Calculates the Power of a number from taking the number and power as inputs
        public double getPower(double powerMe, double power)
        {
            double outer = 1.0;
            for(int i = 0; i < power; i++)
            {
                outer = powerMe * outer;
            }
            return outer;
        }
    }
    
    // Define a class named player
    internal class player
    {
        // Private variables to store player information
        private string name;
        private bool Computer;
        private Cup cup;
        private int diceNum = 5;
        private int sides;
        private int style;
        private int[] Limits = new int[6];
        private int score = 0;

        // Constructor to initialize player attributes
        public player(int Sides, bool computer, int playerCount)
        {
            Computer = computer;
            sides = Sides;
            cup = new Cup(diceNum,sides);
            
            // If player is computer, choose style randomly and calculate limits
            if (Computer)
            {
                Random random = new Random();  
                style = random.Next(1,5);
                coeff playUpTo = new coeff(1, playerCount, style, diceNum, ref name);
                double limit = playUpTo.cutoff();
                double Celing = Math.Ceiling(limit);
                int MinGuess = int.Parse(Celing.ToString());
                int[] InCup = cup.getValues();
                limits(InCup, MinGuess);
            }
           
            else
            {
                // If player is human, prompt for name
                Console.WriteLine("What is yer name matey");
                name = Console.ReadLine();
            }
        }
        
        public void LoseDice()
        {
            diceNum--;
        }
        // Method to increment player's score
        public void raiseScore()
        {
            score = score + 1;
            Console.WriteLine(name + "'s score is now " + score);
        }
        
        // Method to get player's score
        public int getScore()
        {
            return score;
        }

        // Method to roll the dice
        public void roll()
        {
            cup.roll(diceNum);
        }

        // Method to get player's name
        public string Name()
        {
            return name;
        }

        // Method to calculate limits based on dice values and ceiling
        public void limits(int[] inCup, int celing)
        {
            for(int i = 0; i < 6; i++)
            {
                Limits[i] = (inCup.Count(x => x == (i + 1))) + celing;
            }
        }

        // Method to let the player choose to raise or call
        public void choose(ref string currentQuant, ref string currentQual, ref bool Call, int playerNum)
        {
            if (Computer)
            {
                raise(ref currentQuant, ref currentQual, ref Call, playerNum);
            }

            // For human player, prompt for choice
            else
            {
                Thread.Sleep(5000);
                Console.Clear();
                int[] dice = cup.getValues();
                Thread.Sleep(500);
                Console.WriteLine("Your dice: ");
                for (int i = 0; i < diceNum; i++)
                {
                    Console.Write(" " + dice[i]);
                }
                Console.WriteLine("");
                string RorC = "";
                bool valid = false;
                Thread.Sleep(500);
                while (valid == false)
                {
                    Console.WriteLine("Would you like to raise or call? (r/c)");
                    RorC = Console.ReadLine().ToUpper();
                    if (RorC == "R")
                    {
                        valid = true;
                        raise(ref currentQuant, ref currentQual, ref Call, playerNum);
                    }
                    else if(RorC == "C")
                    {
                        valid = true;
                        call(ref Call);
                    }
                    else
                    {

                    }
                }
 
            }
        }
        public int getNum()
        {

            return diceNum;
        }
        // Method to handle raising bet
        public void raise(ref string currentQuant, ref string currentQual, ref bool Call, int playerNum)
        {
            // For human player, prompt for raise quantity and quality
            int quantity = int.Parse(currentQuant);
            int quality = int.Parse(currentQual);
            int betting = 0;
            if (Computer == false)
            {
                Console.WriteLine("Raise to a quantity of value above the current bet - " + quantity + " " + quality + "'s");
                bool verif1 = false;
                bool verif2 = false;
                int newQuant = 0;
                int newQual = 0;
                while ((newQual < quality && newQuant < quantity))
                {
                    while (verif1 == false || verif2 == false || newQuant < 1 || newQual < 1 || newQual > 6 || newQuant > 5*playerNum)
                    {
                        Console.WriteLine("Enter the quantity you with to raise to");
                        verif1 = int.TryParse(Console.ReadLine(), out newQuant);
                        Console.WriteLine("Enter the value of the face you with to raise to");
                        verif2 = int.TryParse(Console.ReadLine(), out newQual);
                    }

                }
                if (newQual > quality || newQuant > quantity)
                {
                    Console.WriteLine("Eye the new bet is - " + newQuant + " " + newQual + "'s");
                    currentQuant = newQuant.ToString();
                    currentQual = newQual.ToString();
                }
      
            }
            
            // For computer player, choose the best possible raise based on limits
            else
            {
                for(int i = 0; i < 6; i++)
                {
                    if (Limits[i] > quantity || i > quality)
                    {
                        Console.WriteLine("I "+ name +" raise to - " + Limits[i] + " " + (i+1) + "'s");
                        betting = int.Parse((Limits[i].ToString()) + (i+1.ToString()));
                        currentQuant = Limits[i].ToString();
                        currentQual = (1+i).ToString();
                        break;
                    }
                    if (i == 5)
                    {
                        Console.WriteLine("I think ye are a liar");
                        call(ref Call);
                    }
                }
            }

        }

        // Method to get dice values
        public int[] getValues()
        {
            return cup.getValues();
        }

        // Method to set Call flag to true
        public void call(ref bool Call)
        {
            Call = true;  
        }
    }

    // Define a class named Game
    internal class Game
    {

        // Private variables to store game information
        private List<player> Players = new List<player>();
        private int playerCount;
        private string CurrentQant = "1";
        private string CurrentQual = "1";

        // Constructor to initialize the game with the specified number of players and sides
        public Game(int PlayerCount, int sides, int humans)
        {
            playerCount = PlayerCount;
            for(int i = 0; i < humans; i++)
            {
                Players.Add(new player(sides, false, PlayerCount)); // Add human player
            }
            for (int i = 0; i < (PlayerCount - humans); i++)
            {
                Players.Add(new player(sides,true,PlayerCount)); // Adds computer players
            }
        }

        // Method to play a round of the game
        public void round()
        {
            bool call = false;
            while (call == false)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    // Players take turns to choose whether to raise or call
                    Players[i].choose(ref CurrentQant, ref CurrentQual, ref call, playerCount);
                    if (call)
                    {
                        // If a player calls, resolve the bet
                        Thread.Sleep(500);
                        if (i - 1 >= 0)
                        {
                            Console.WriteLine(Players[i].Name() + " accuses " + Players[i - 1].Name() + " of being a LIAR");
                        }
                        else
                        {
                            Console.WriteLine(Players[i].Name() + " accuses " + Players[Players.Count - 1].Name() + " of being a LIAR");
                        }

                        // Gather all dice values
                        List<int> Dice = new List<int>();
                        for (int x = 0; x < playerCount; x++)
                        {
                            int[] transferer = Players[x].getValues();
                            for(int z = 0; z < Players[x].getNum(); z++)
                            {
                                Dice.Add(transferer[z]);
                            }
                        }

                        // Count the occurrences of each dice value
                        List<int> FinalDice = new List<int>();
                        for (int z = 0; z < 6; z++)
                        {
                           FinalDice.Add(Dice.Count(g => g == (z+1)));
                        }
                        Thread.Sleep(500);

                        // Compare bet with actual dice values and update scores
                        if (FinalDice[(int.Parse(CurrentQual))-1] < int.Parse(CurrentQant))
                        {
                            if (i - 1 >= 0)
                            {
                                Console.WriteLine(Players[i - 1].Name() + " is a lying scallywag and was discovered by " + Players[i].Name());
                                Players[i - 1].LoseDice();
                            }
                            else
                            {
                                Console.WriteLine(Players[Players.Count-1].Name() + " is a lying scallywag and was discovered by " + Players[i].Name());
                                Players[Players.Count - 1].LoseDice();
                            }
                            Players[i].raiseScore();
                        }
                        else
                        {
                            if (i - 1 >= 0)
                            {
                                Console.WriteLine(Players[i].Name() + " is a traitorous sea dog who has falsely accused " + Players[i - 1].Name());
                                Players[i - 1].raiseScore();
                            }
                            else
                            {
                                Console.WriteLine(Players[i].Name() + " is a traitorous sea dog who has falsely accused " + Players[Players.Count - 1].Name());
                                Players[Players.Count - 1].raiseScore();
                            }
                            Players[i].LoseDice();
                        }
                        Thread.Sleep(500);
                        break;
                    }
                }
            }
        }

        // Method to run the game
        public void Gamering()
        {
            bool bazinga = true;
            while (bazinga)
            {
                for(int i = 0; i < playerCount; i++)
                {
                    Thread.Sleep(500);
                    // Check if any player has won
                    if (Players[i].getScore() == 5)
                    {
                        Console.WriteLine(Players[i].Name() + " has won");
                        Thread.Sleep(500);
                        if(i != 0)
                        {
                            Console.WriteLine("See young scallywag ye are yet a match for me " + Players[i].Name() + " ruler of the seas");
                            Thread.Sleep(500);
                        }
                        bazinga = false; // End the game
                    }
                }
                if (bazinga)
                {
                    round(); // If no winner yet, proceed to the next round
                }
                roll(); // Roll the dice for the next round
            }
        }

        // Method to roll the dice for all players
        public void roll()
        {
            CurrentQant = "1";
            CurrentQual = "1";
            for (int i = 0; i < playerCount; i++)
            {
                Players[i].roll();
            }
        }
    }


    internal class Program
    {
        // Method to display the menu options and handle user input
        static void Menu()
        {
            Console.WriteLine("Please select an option from this list:");
            Thread.Sleep(500);
            Console.WriteLine("Instructions for those who need them - 1");
            Thread.Sleep(500);
            Console.WriteLine("Play.. if ye dare - 2");
            Thread.Sleep(500);
            Console.WriteLine("Flee like the coward ye be!- 3");
            string opt = Console.ReadLine();
            switch (opt)
            {
                case ("1"):
                    {
                        // Display instructions to the user
                        Console.WriteLine("I see ye are a landlubber");
                        Thread.Sleep(2000);
                        Console.WriteLine("Each scallywag will have 5 dice which only they will see");
                        Thread.Sleep(2000);
                        Console.WriteLine("Then each player will take turns to either raise or call");
                        Thread.Sleep(2000);
                        Console.WriteLine("When making a bet you declare a quantity of dice that will have a certain value face up e.g 2 4s");
                        Thread.Sleep(2000);
                        Console.WriteLine("This bet means you believe there will be 2 dice with the value 4 facing up when all dice are revelaed");
                        Thread.Sleep(2000);
                        Console.WriteLine("To raise you increase the current bet by increasing the number or face of the dice");
                        Thread.Sleep(2000);
                        Console.WriteLine("Calling will allow you to accuse The most recent better of being a LIAR");
                        Thread.Sleep(1800);
                        Console.WriteLine("After someone is accused the dice are revealed. If The accuser is right they gain a point but if accused is right they gain one");
                        Console.WriteLine();
                        break;
                    }
                case ("2"):
                    {
                        // Prompt for the number of players and start the game
                        int players = 7; // Default value to enter the loop
                        bool bazingaa = false;   // Flag to ensure valid input
                        while (players < 2 || players > 5 || bazingaa == false)
                        {
                            Console.WriteLine("How many players?  2-5");
                            bazingaa = int.TryParse(Console.ReadLine(), out players); // TryParse used to parse user input
                        }
                        bazingaa = false;
                        int humans = 0;
                        while (humans < 1 || humans > players || bazingaa == false)
                        {
                            Console.WriteLine("How many of the " + players + " players are human?");
                            bazingaa = int.TryParse(Console.ReadLine(), out humans); // TryParse used to parse user input
                        }


                        int sides = 6; // Default number of sides for each die
                        Game TheGame = new Game(players, sides, humans); // Create a new game object
                        TheGame.Gamering(); // Start the game
                        break;
                    }
                case ("3"):
                    {
                        // Exit the program
                        Console.WriteLine("Begone while ye still can");
                        Thread.Sleep(2000);
                        Environment.Exit(42);
                        break;
                    }
                default:
                    {
                        //Tells the user they have inputted an incorrect value
                        Console.WriteLine("That is not an option matey");
                        break;
                    }
            }
        }
        // Main method, the entry point of the program
        static void Main(string[] args)
        {
            Console.WriteLine("Ahoy matey welcome aboard me humble ship");
            Thread.Sleep(500);
            while (true)
            {
                // Display menu and handle user input in a loop
                Menu();
            }
        }
    }
}