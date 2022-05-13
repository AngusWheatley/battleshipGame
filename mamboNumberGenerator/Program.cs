using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mamboNumberGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string name = "";
            string playAgain = "";

            int gamesPlayed = 0;
            int gamesWon = 0;
            int gamesLost = 0;
            int gamesTied = 0;

            RulesAndInstructions();
            name = Console.ReadLine();

            string path = "GameStats.txt";
            string pathTemp = "LastPlayedGame.txt";
            File.Delete(pathTemp);
            File.Delete(path);
            FileStream fs = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);



            for (int i = 0; i < 1; i++)         //Repeats the game if player wants to
            {

                int xCo = 0;                    //coordinate variables for each array
                int yCo = 0;
                int enemyXCo = 0;
                int enemyYCo = 0;
                int enemyXCoFake = 0;
                int enemyYCoFake = 0;
                string enemyYCoFakeLetter = " ";

                int humanWin = 0;               //Counts how many boats have been hit by each player
                int enemyWin = 0;

                int turnCounter = 0;            //Counts how many turns have been played

                int dirBoat = 0;                //Extends boat for each 
                int enemyDirBoat = 0;

                int findBoat = 0;               //Enemy ai to find boat

                int endMessage = 0;



                Random random = new Random();


                string[,] friendlyBoard = new string[10, 10];           //Initialsing friendly board
                string[,] enemyBoardHidden = new string[10, 10];        //Initialsing enemy board that is not displayed
                string[,] enemyBoardShown = new string[10, 10];         //Initialsing enemy board that is displayed


                SpaceMaking(friendlyBoard);                             //Friendly space making for board

                BoatMaking(ref xCo, ref yCo, ref dirBoat, friendlyBoard, random);       //Friendly boat making



                Console.WriteLine();        //Seperates title from board
                Console.WriteLine();



                EnemySpaceMaking(enemyBoardHidden);                                            //Enemy space making for board that is not displayed

                EnemyBoatMaking(ref enemyXCo, ref enemyYCo, ref enemyDirBoat, enemyBoardHidden);       //Enemy boat making that is not displayed

                EnemyBoatMakingFake(ref enemyXCoFake, ref enemyYCoFake, enemyBoardShown);                       //Enemy boat making that is displayed

                EnemySpaceMakingFake(enemyBoardShown);

                BoardMaking(friendlyBoard, enemyBoardShown);                                                    //Board making


                GamePlayer(name, ref gamesWon, ref gamesLost, ref gamesTied, sw, ref xCo, ref yCo, ref enemyXCo, ref enemyYCo, ref enemyXCoFake, ref enemyYCoFake, ref enemyYCoFakeLetter, ref humanWin, ref enemyWin, ref turnCounter, ref findBoat, random, friendlyBoard, enemyBoardHidden, enemyBoardShown);

                gamesPlayed++;

                playAgain = PlayAgainQuery(gamesPlayed, gamesWon, gamesLost, ref i, sw, fs, path, pathTemp, ref endMessage, random);
            }


            sw.Close();
            fs.Close();
            File.Copy(path, pathTemp);
        }

        private static void RulesAndInstructions()
        {
            Console.ForegroundColor = ConsoleColor.White;           //changes writing to white
            Console.WriteLine();
            Console.WriteLine("     =========================");
            Console.WriteLine("     ||Welcome to Battleship||");             //title of first screen
            Console.WriteLine("     =========================");
            Console.WriteLine();
            Console.WriteLine("     Rules");                                                                                                 //Rules for battleship
            Console.WriteLine("     1. All letter coordinates must be in capital letters (playing with caps lock on is recommended).");
            Console.WriteLine("     2. Only enter letters between A and J.");
            Console.WriteLine("     3. Only enter numbers between 0 and 9.");
            Console.WriteLine("     4. Type in values only when prompted to.");
            Console.WriteLine("     5. Only enter one value only.");
            Console.WriteLine("     6. Have fun!");
            Console.WriteLine();
            Console.WriteLine("     (Play in fullscreen for a better experience)");
            Console.WriteLine("     (Enter your name and press enter to continue after you have read the rules)");
        }

        private static void SpaceMaking(string[,] friendlyBoard)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    friendlyBoard[i, j] = " ";
                }
            }
        }               //friendly space making for board

        private static void BoatMaking(ref int xCo, ref int yCo, ref int dirBoat, string[,] friendlyBoard, Random random)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int i = 0; i < 4; i++)                             //Counter for placing boats
            {
                xCo = random.Next(10);                              //Randomises boat coordinates
                yCo = random.Next(10);

                if (friendlyBoard[xCo, yCo] == "0")                 //Checks for boat
                {
                    i--;                                        //Minuses one from counter to attempt another boat placement
                }

                if (friendlyBoard[xCo, yCo] == " ")                 //Checks for boat
                {
                    friendlyBoard[xCo, yCo] = "0";                  //Places first segment of the boat
                    dirBoat = random.Next(1, 4);                    //Randomises boat extension direction


                    if (dirBoat == 1)
                    {
                        if (yCo <= 7 && yCo >= 0)
                        {
                            yCo++;
                            if (friendlyBoard[xCo, yCo] == " ")
                            {
                                yCo++;
                                if (friendlyBoard[xCo, yCo] == " ")
                                {
                                    friendlyBoard[xCo, yCo] = "0";
                                    yCo--;
                                    friendlyBoard[xCo, yCo] = "0";
                                }
                                else if (friendlyBoard[xCo, yCo] == "0")
                                {
                                    yCo--;
                                    yCo--;
                                    friendlyBoard[xCo, yCo] = " ";
                                    i--;
                                }
                            }
                            else if (friendlyBoard[xCo, yCo] == "0")
                            {
                                yCo--;
                                friendlyBoard[xCo, yCo] = " ";
                                i--;
                            }
                        }

                        else if (yCo >= 8 && yCo <= 9)
                        {
                            yCo--;
                            if (friendlyBoard[xCo, yCo] == " ")
                            {
                                yCo--;
                                if (friendlyBoard[xCo, yCo] == " ")
                                {
                                    friendlyBoard[xCo, yCo] = "0";
                                    yCo++;
                                    friendlyBoard[xCo, yCo] = "0";
                                }
                                else if (friendlyBoard[xCo, yCo] == "0")
                                {
                                    yCo++;
                                    yCo++;
                                    friendlyBoard[xCo, yCo] = " ";
                                    i--;
                                }
                            }
                            else if (friendlyBoard[xCo, yCo] == "0")
                            {
                                yCo++;
                                friendlyBoard[xCo, yCo] = " ";
                                i--;
                            }
                        }

                    }

                    else if (dirBoat == 2)
                    {
                        if (xCo < 8 && xCo >= 0)
                        {
                            xCo++;
                            if (friendlyBoard[xCo, yCo] == " ")
                            {
                                xCo++;
                                if (friendlyBoard[xCo, yCo] == " ")
                                {
                                    friendlyBoard[xCo, yCo] = "0";
                                    xCo--;
                                    friendlyBoard[xCo, yCo] = "0";
                                }
                                else if (friendlyBoard[xCo, yCo] == "0")
                                {
                                    xCo--;
                                    xCo--;
                                    friendlyBoard[xCo, yCo] = " ";
                                    i--;
                                }
                            }
                            else if (friendlyBoard[xCo, yCo] == "0")
                            {
                                xCo--;
                                friendlyBoard[xCo, yCo] = " ";
                                i--;
                            }
                        }

                        else if (xCo > 7 && xCo <= 9)
                        {
                            xCo--;
                            if (friendlyBoard[xCo, yCo] == " ")
                            {
                                xCo--;
                                if (friendlyBoard[xCo, yCo] == " ")
                                {
                                    friendlyBoard[xCo, yCo] = "0";
                                    xCo++;
                                    friendlyBoard[xCo, yCo] = "0";
                                }
                                else if (friendlyBoard[xCo, yCo] == "0")
                                {
                                    yCo++;
                                    yCo++;
                                    friendlyBoard[xCo, yCo] = " ";
                                    i--;
                                }
                            }
                            else if (friendlyBoard[xCo, yCo] == "0")
                            {
                                xCo++;
                                friendlyBoard[xCo, yCo] = " ";
                                i--;
                            }
                        }
                    }

                    else if (dirBoat == 3)
                    {
                        if (yCo >= 0 && yCo <= 7)
                        {
                            yCo++;
                            if (friendlyBoard[xCo, yCo] == " ")
                            {
                                yCo++;
                                if (friendlyBoard[xCo, yCo] == " ")
                                {
                                    friendlyBoard[xCo, yCo] = "0";
                                    yCo--;
                                    friendlyBoard[xCo, yCo] = "0";
                                }
                                else if (friendlyBoard[xCo, yCo] == "0")
                                {
                                    yCo--;
                                    yCo--;
                                    friendlyBoard[xCo, yCo] = " ";
                                    i--;
                                }
                            }
                            else if (friendlyBoard[xCo, yCo] == "0")
                            {
                                yCo--;
                                friendlyBoard[xCo, yCo] = " ";
                                i--;
                            }
                        }

                        else if (yCo >= 8 && yCo <= 9)
                        {
                            yCo--;
                            if (friendlyBoard[xCo, yCo] == " ")
                            {
                                yCo--;
                                if (friendlyBoard[xCo, yCo] == " ")
                                {
                                    friendlyBoard[xCo, yCo] = "0";
                                    yCo++;
                                    friendlyBoard[xCo, yCo] = "0";
                                }
                                else if (friendlyBoard[xCo, yCo] == "0")
                                {
                                    yCo++;
                                    yCo++;
                                    friendlyBoard[xCo, yCo] = " ";
                                    i--;
                                }
                            }
                            else if (friendlyBoard[xCo, yCo] == "0")
                            {
                                yCo++;
                                friendlyBoard[xCo, yCo] = " ";
                                i--;
                            }
                        }
                    }

                    else if (dirBoat == 4)
                    {
                        if (xCo >= 0 && xCo < 8)
                        {
                            xCo++;
                            if (friendlyBoard[xCo, yCo] == " ")
                            {
                                xCo++;
                                if (friendlyBoard[xCo, yCo] == " ")
                                {
                                    friendlyBoard[xCo, yCo] = "0";
                                    xCo--;
                                    friendlyBoard[xCo, yCo] = "0";
                                }
                                else if (friendlyBoard[xCo, yCo] == "0")
                                {
                                    xCo--;
                                    xCo--;
                                    friendlyBoard[xCo, yCo] = " ";
                                    i--;
                                }
                            }
                            else if (friendlyBoard[xCo, yCo] == "0")
                            {
                                xCo--;
                                friendlyBoard[xCo, yCo] = " ";
                                i--;
                            }
                        }
                        else if (xCo > 7 && xCo <= 9)
                        {
                            xCo--;
                            if (friendlyBoard[xCo, yCo] == " ")
                            {
                                xCo--;
                                if (friendlyBoard[xCo, yCo] == " ")
                                {
                                    friendlyBoard[xCo, yCo] = "0";
                                    xCo++;
                                    friendlyBoard[xCo, yCo] = "0";
                                }
                                else if (friendlyBoard[xCo, yCo] == "0")
                                {
                                    xCo++;
                                    xCo++;
                                    friendlyBoard[xCo, yCo] = " ";
                                    i--;
                                }
                            }
                            else if (friendlyBoard[xCo, yCo] == "0")
                            {
                                xCo++;
                                friendlyBoard[xCo, yCo] = " ";
                                i--;
                            }
                        }
                    }

                }
            }
        }       //friendly boat making

        private static void EnemySpaceMaking(string[,] enemyBoardHidden)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    enemyBoardHidden[i, j] = " ";
                }

            }
        }                                       //enemy space making for board that is not displayed

        private static void EnemyBoatMaking(ref int enemyXCo, ref int enemyYCo, ref int enemyDirBoat, string[,] enemyBoardHidden)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Random random2 = new Random();
            for (int i = 0; i < 4; i++)
            {
                enemyXCo = random2.Next(10);
                enemyYCo = random2.Next(10);

                if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                {
                    enemyXCo = random2.Next(10);
                    enemyYCo = random2.Next(10);
                    if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                    {
                        i--;
                    }
                }

                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                {
                    enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                    enemyDirBoat = random2.Next(1, 5);

                    if (enemyDirBoat > 0)
                    {

                        if (enemyDirBoat == 1)
                        {
                            if (enemyYCo <= 7 && enemyYCo >= 0)
                            {
                                enemyYCo++;
                                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                {
                                    enemyYCo++;
                                    if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                    {
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                        enemyYCo--;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                    }
                                    else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                    {
                                        enemyYCo--;
                                        enemyYCo--;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                        i--;
                                    }
                                }
                                else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                {
                                    enemyYCo--;
                                    enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                    i--;
                                }
                            }

                            else if (enemyYCo >= 8 && enemyYCo <= 9)
                            {
                                enemyYCo--;
                                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                {
                                    enemyYCo--;
                                    if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                    {
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                        enemyYCo++;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                    }
                                    else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                    {
                                        enemyYCo++;
                                        enemyYCo++;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                        i--;
                                    }
                                }
                                else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                {
                                    enemyYCo++;
                                    enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                    i--;
                                }
                            }

                        }


                        else if (enemyDirBoat == 2)
                        {
                            if (enemyXCo < 8 && enemyXCo >= 0)
                            {
                                enemyXCo++;
                                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                {
                                    enemyXCo++;
                                    if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                    {
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                        enemyXCo--;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                    }
                                    else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                    {
                                        enemyXCo--;
                                        enemyXCo--;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                        i--;
                                    }
                                }
                                else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                {
                                    enemyXCo--;
                                    enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                    i--;
                                }
                            }

                            else if (enemyXCo > 7 && enemyXCo <= 9)
                            {
                                enemyXCo--;
                                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                {
                                    enemyXCo--;
                                    if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                    {
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                        enemyXCo++;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                    }
                                    else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                    {
                                        enemyYCo++;
                                        enemyYCo++;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                        i--;
                                    }
                                }
                                else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                {
                                    enemyXCo++;
                                    enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                    i--;
                                }
                            }
                        }


                        else if (enemyDirBoat == 3)
                        {
                            if (enemyYCo >= 0 && enemyYCo <= 7)
                            {
                                enemyYCo++;
                                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                {
                                    enemyYCo++;
                                    if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                    {
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                        enemyYCo--;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                    }
                                    else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                    {
                                        enemyYCo--;
                                        enemyYCo--;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                        i--;
                                    }
                                }
                                else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                {
                                    enemyYCo--;
                                    enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                    i--;
                                }
                            }

                            else if (enemyYCo >= 8 && enemyYCo <= 9)
                            {
                                enemyYCo--;
                                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                {
                                    enemyYCo--;
                                    if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                    {
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                        enemyYCo++;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                    }
                                    else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                    {
                                        enemyYCo++;
                                        enemyYCo++;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                        i--;
                                    }
                                }
                                else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                {
                                    enemyYCo++;
                                    enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                    i--;
                                }
                            }
                        }


                        else if (enemyDirBoat == 4)
                        {
                            if (enemyXCo >= 0 && enemyXCo < 8)
                            {
                                enemyXCo++;
                                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                {
                                    enemyXCo++;
                                    if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                    {
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                        enemyXCo--;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                    }
                                    else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                    {
                                        enemyXCo--;
                                        enemyXCo--;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                        i--;
                                    }
                                }
                                else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                {
                                    enemyXCo--;
                                    enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                    i--;
                                }
                            }
                            else if (enemyXCo > 7 && enemyXCo <= 9)
                            {
                                enemyXCo--;
                                if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                {
                                    enemyXCo--;
                                    if (enemyBoardHidden[enemyXCo, enemyYCo] == " ")
                                    {
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                        enemyXCo++;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = "0";
                                    }
                                    else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                    {
                                        enemyXCo++;
                                        enemyXCo++;
                                        enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                        i--;
                                    }
                                }
                                else if (enemyBoardHidden[enemyXCo, enemyYCo] == "0")
                                {
                                    enemyXCo++;
                                    enemyBoardHidden[enemyXCo, enemyYCo] = " ";
                                    i--;
                                }
                            }
                        }
                    }
                }
            }

        }       //enemy boat making that is not displayed

        private static void EnemyBoatMakingFake(ref int enemyXCoFake, ref int enemyYCoFake, string[,] enemyBoardShown)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    enemyBoardShown[i, j] = " ";
                }
            }
        }    //enemy boat making that is displayed

        private static void EnemySpaceMakingFake(string[,] enemyBoardShown)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    enemyBoardShown[i, j] = " ";                //puts spaces in array
                }

            }
        }

        private static void BoardMaking(string[,] friendlyBoard, string[,] enemyBoardShown)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.Write("                                      Your board");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("                                                                          Enemy board");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("           A      B      C      D      E      F      G      H      I      J");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("                     A      B      C      D      E      F      G      H      I      J");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("       ========================================================================");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("             ========================================================================");
            for (int i = 0; i < 10; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("     " + i + " ||  ");

                Console.Write(friendlyBoard[i, 0] + "  ||  " + friendlyBoard[i, 1] + "  ||  " + friendlyBoard[i, 2] + "  ||  " + friendlyBoard[i, 3] + "  ||  " + friendlyBoard[i, 4] + "  ||  " + friendlyBoard[i, 5] + "  ||  " + friendlyBoard[i, 6] + "  ||  " + friendlyBoard[i, 7] + "  ||  " + friendlyBoard[i, 8] + "  ||  " + friendlyBoard[i, 9] + "  ||           ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(i + " ||  " + enemyBoardShown[i, 0] + "  ||  " + enemyBoardShown[i, 1] + "  ||  " + enemyBoardShown[i, 2] + "  ||  " + enemyBoardShown[i, 3] + "  ||  " + enemyBoardShown[i, 4] + "  ||  " + enemyBoardShown[i, 5] + "  ||  " + enemyBoardShown[i, 6] + "  ||  " + enemyBoardShown[i, 7] + "  ||  " + enemyBoardShown[i, 8] + "  ||  " + enemyBoardShown[i, 9] + "  ||");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine();
                Console.Write("       ========================================================================");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("             ========================================================================");
            }
        }                 //friendly board making

        private static void GamePlayer(string name, ref int gamesWon, ref int gamesLost, ref int gamesTied, StreamWriter sw, ref int xCo, ref int yCo, ref int enemyXCo, ref int enemyYCo, ref int enemyXCoFake, ref int enemyYCoFake, ref string enemyYCoFakeLetter, ref int humanWin, ref int enemyWin, ref int turnCounter, ref int findBoat, Random random, string[,] friendlyBoard, string[,] enemyBoardHidden, string[,] enemyBoardShown)
        {
            for (int j = 0; j < 1000; j++)                                  //Keeps game playing
            {
                if (humanWin < 12 && enemyWin < 12)                         //Checks that no one has won
                {
                    BoatAttacks(out xCo, out yCo, ref enemyXCoFake, ref enemyYCoFake, ref enemyYCoFakeLetter, ref enemyXCo, ref enemyYCo, ref humanWin, ref enemyWin, ref turnCounter, ref findBoat, friendlyBoard, enemyBoardHidden, enemyBoardShown, random);      //Boat attacks for both players
                }
                if (humanWin == 12)                                         //Checks if human has won
                {
                    gamesWon++;
                    Console.Clear();
                    BoardMaking(friendlyBoard, enemyBoardShown);
                    Console.WriteLine("     You won after " + turnCounter + " turns");
                    sw.WriteLine(name + " won after " + turnCounter + " turns");
                    DateTime now = DateTime.Now;
                    sw.WriteLine("This game was played on " + now.ToString("F"));
                    sw.WriteLine("Session score is " + name + " " + gamesWon + "-" + gamesLost + " Enemy");
                    sw.WriteLine("----------------------------------------------------");
                    j = 1000;                                               //Ends game when someone wins
                }
                if (enemyWin == 12)                                         //Checks if enemy has won
                {
                    gamesLost++;
                    Console.Clear();
                    BoardMaking(friendlyBoard, enemyBoardShown);
                    Console.WriteLine("     You lose after " + turnCounter + " turns");
                    sw.WriteLine(name + " lost after " + turnCounter + " turns");
                    DateTime now = DateTime.Now;
                    sw.WriteLine("This game was played on " + now.ToString("F"));
                    sw.WriteLine("Session score is " + name + " " + gamesWon + "-" + gamesLost + " Enemy");
                    sw.WriteLine("----------------------------------------------------");
                    j = 1000;                                               //Ends game when someone wins
                }
            }
        }

        private static string PlayAgainQuery(int gamesPlayed, int gamesWon, int gamesLost, ref int i, StreamWriter sw, FileStream fs, string path, string pathTemp, ref int endMessage, Random random)
        {
            string playAgain;
            Console.WriteLine("     Would you like to play again?");
            Console.WriteLine("     (Type 'y' or 'Y' to play again)");
            playAgain = Console.ReadLine();                                 //Checks if player wants to play again

            if (playAgain == "y" || playAgain == "Y")
            {
                Console.Clear();
                i--;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("     Stats for this session");
                Console.WriteLine("     ======================");
                Console.WriteLine();
                Console.WriteLine("     You played " + gamesPlayed + " games this session");
                Console.WriteLine("     You won " + gamesWon + " games and lost " + gamesLost + " this session");
                sw.WriteLine("----------------------------------------------------");

                endMessage = random.Next(0, 3);

                if (endMessage == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("     Goodbye.");
                }
                else if (endMessage == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("     Have a good day.");
                }
                else if (endMessage == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("     See ya later.");
                }
                else if (endMessage == 3)
                {
                    Console.WriteLine();
                    Console.WriteLine("     Bye-bye :)");
                }

            }

            return playAgain;
        }           //Asks if player want to play again

        private static void BoatAttacks(out int xCo, out int yCo, ref int enemyXCoFake, ref int enemyYCoFake, ref string enemyYCoFakeLetter, ref int enemyXCo, ref int enemyYCo, ref int humanWin, ref int enemyWin, ref int turnCounter, ref int findBoat, string[,] friendlyBoard, string[,] enemyBoardHidden, string[,] enemyBoardShown, Random random)
        {
            int yCoHuman = 0;                   //y-coordinate for human attack

            xCo = random.Next(10);
            yCo = random.Next(10);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.Write("     Please enter your letter coordinate  ");
            enemyYCoFakeLetter = Console.ReadLine();

            switch (enemyYCoFakeLetter)         //Changes the coordinate number to a letter
            {
                case "A":
                    {
                        yCoHuman = 0;
                        break;
                    }
                case "B":
                    {
                        yCoHuman = 1;
                        break;
                    }
                case "C":
                    {
                        yCoHuman = 2;
                        break;
                    }
                case "D":
                    {
                        yCoHuman = 3;
                        break;
                    }
                case "E":
                    {
                        yCoHuman = 4;
                        break;
                    }
                case "F":
                    {
                        yCoHuman = 5;
                        break;
                    }
                case "G":
                    {
                        yCoHuman = 6;
                        break;
                    }
                case "H":
                    {
                        yCoHuman = 7;
                        break;
                    }
                case "I":
                    {
                        yCoHuman = 8;
                        break;
                    }
                case "J":
                    {
                        yCoHuman = 9;
                        break;
                    }

                default:
                    break;
            }

            Console.Write("     Please enter your number coordinate  ");
            enemyXCo = enemyXCoFake = Convert.ToInt32(Console.ReadLine());

            if (enemyBoardHidden[enemyXCo, yCoHuman] == "0")
            {
                enemyBoardShown[enemyXCoFake, yCoHuman] = "X";
                humanWin++;
                turnCounter++;
            }

            else if (enemyBoardHidden[enemyXCo, yCoHuman] == " ")
            {
                enemyBoardShown[enemyXCoFake, yCoHuman] = "F";
                turnCounter++;
            }

            Console.Clear();
            for (int j = 0; j < 1; j++)
            {
                if (friendlyBoard[xCo, yCo] == "0" || friendlyBoard[xCo, yCo] == " ")
                {
                    if (friendlyBoard[xCo, yCo] == "0")
                    {
                        friendlyBoard[xCo, yCo] = "X";
                        enemyWin++;
                        findBoat = random.Next(1, 4);
                        if (findBoat == 1)
                        {
                            xCo++;
                        }
                        if (findBoat == 2)
                        {
                            yCo++;
                        }
                        if (findBoat == 3)
                        {
                            xCo--;
                        }
                        if (findBoat == 4)
                        {
                            yCo--;
                        }
                    }
                    else if (friendlyBoard[xCo, yCo] == " ")
                    {
                        friendlyBoard[xCo, yCo] = "F";
                        xCo = random.Next(10);
                        yCo = random.Next(10);
                    }
                }
                else if (friendlyBoard[xCo, yCo] == "X" || friendlyBoard[xCo, yCo] == "F")
                {
                    xCo = random.Next(10);
                    yCo = random.Next(10);
                    j--;
                }
            }

            BoardMaking(friendlyBoard, enemyBoardShown);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            if (enemyBoardHidden[enemyXCo, yCoHuman] == "0")
            {
                Console.WriteLine("     Hit!");
                Console.WriteLine("     ========");
            }

            else if (enemyBoardHidden[enemyXCo, yCoHuman] == " ")
            {
                Console.WriteLine("     Miss");
                Console.WriteLine("     ========");
            }
        }       //boat attacks for both players

    }
}
