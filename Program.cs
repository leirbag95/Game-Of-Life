using System;
using System.IO;
using System.Collections.Generic;

namespace Jeu_de_la_vie
{
    class MainClass
    {

        //MARK:const variables
        const int nb_row = 5;
        const int nb_col = 5;

        //MARK: public variables
        public static int populationCounter = 0;

        public static void displayGrid(int[,] grid)
        {
            for (int y = 0; y < nb_col; y++)
            {
                for (int x = 0; x < nb_row; x++)
                {                  
                    switch (grid[y, x])
                    {
                        case 0:
                            //Is Dead
                            Console.Write(".");
                            break;
                        case 1:
                        //Is alive
                            Console.Write("#");
                            break;
                        case -1:
                        //Will be die
                            Console.Write("*");
                            break;
                        case 2:
                            //Will be born
                            Console.Write("-");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        public static int[,] checkAround(int[,] grid, int y, int x, bool withVisual = true)
        {
            int nb_life = 0;
            /*Setup of grid2 with grid values*/
            int[,] grid2 = new int[nb_col, nb_row];
            for (int i = 0; i < nb_col; i++)
            {
                for (int j = 0; j < nb_row; j++)
                {
                    grid2[i, j] = grid[i, j];
                }
            }
            /*Then we do a cercle checking */
            for (int i = y - 1; i < y + 2; i++)
            {
                for (int j = x - 1; j < x + 2;j++)
                {

                    int tmp_i = i;
                    int tmp_j = j;
                    if (i < 0) {
                        tmp_i = 4;
                    }
                    if (j < 0)
                    {
                        tmp_j = 4;
                    }
                    if (i > 4)
                    {
                        tmp_i = 0;
                    }
                    if (j > 4)
                    {
                        tmp_j = 0;
                    }
                    if (grid2[tmp_i, tmp_j] == 1 && (tmp_j != x || tmp_i != y))
                    {
                        nb_life += 1;
                    }
                }
            }
            //If the mode is with Visual or not
            if (withVisual) 
            {
                if (grid2[y, x] == 0 && nb_life == 3)
                {
                    grid2[y, x] = 2;
                }
                else if (grid2[y, x] == 1 && (nb_life < 2 || nb_life > 3))
                {
                    grid2[y, x] = -1;
                } 
            } else {
                if (grid2[y, x] == 0 && nb_life == 3)
                {
                    grid2[y, x] = 1;
                    populationCounter++;
                }
                else if (grid2[y, x] == 1 && (nb_life < 2 || nb_life > 3))
                {
                    grid2[y, x] = 0;
                    populationCounter--;
                }
            }
                return grid2;
        }

        public static void UpdateGrid(int[,] grid) 
        {
            for (int i = 0; i < nb_col; i++)
            {
                for (int j = 0; j < nb_row; j++)
                {
                    if (grid[i, j] == 2)
                    {
                        grid[i, j] = 1;
                        populationCounter++;
                    }
                    else if (grid[i, j] == -1)
                    {
                        grid[i, j] = 0;
                        populationCounter--;
                    }
                }
            }
        }


        public static void gamePlay(int[,] grid, bool withVisual) {
            int generationCount = 0;
            bool isInChange = false;

            while (true)
            {
                /*On initialise grid2 avec les meme valeur que grid*/
                int[,] grid2 = new int[nb_row, nb_col];
                for (int i = 0; i < nb_col; i++)
                {
                    for (int j = 0; j < nb_row; j++)
                    {
                        grid2[i, j] = grid[i, j];
                    }
                }

                if (isInChange) {
                    UpdateGrid(grid2);
                    isInChange = false;
                } else {
                    for (int y = 0; y < nb_col; y++)
                    {
                        for (int x = 0; x < nb_row; x++)
                        {
                            /*
                             * si une cellule vivante est entourée de moins de 2 cellules vivantes alors
                             * elle meurt à la génération suivante (cas de sous-population).
                            */
                            if (withVisual) isInChange = true;
                            int[,] tmp_grid = checkAround(grid, y, x, withVisual);
                            /*On met à jour le tmp_grid*/
                            for (int i = 0; i < nb_col; i++)
                            {
                                for (int j = 0; j < nb_row; j++)
                                {
                                    if (tmp_grid[i, j] != grid[i, j])
                                    {
                                        grid2[i, j] = tmp_grid[i, j];
                                    }
                                }
                            }
                        }
                    }
                    generationCount++;
                }
                /*On initialise grid avec les meme valeur que grid2*/
                for (int i = 0; i < nb_col; i++)
                {
                    for (int j = 0; j < nb_row; j++)
                    {
                        grid[i, j] = grid2[i, j];
                    }
                }

                displayGrid(grid);

                Console.WriteLine("Generation N°:" + generationCount);
                Console.WriteLine("People in life:" + populationCounter );
                Console.WriteLine("Press Enter to continue, if not press ONLY : \'N\' ");
                string res = Console.ReadLine();
                if (res == "N") {
                    break;
                }
            }

        }

        public static void gamePlayWithVisual()
        {

        }

        public static void Main(string[] args)
        {
            //MARK: var

            int[,] grid = 
            {
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0}
            };

            populationCounter = 4;
           

            Console.WriteLine("Hi! Welcome in the Game Life.");
            Console.WriteLine("I am J.H. Conway and I created this Game.");
            Console.WriteLine("Before starting, choose your mode:");
            Console.WriteLine("\t 1.Classic Game Life with visualization of each evolution steps");
            Console.WriteLine("\t 2.Classic Game Life without visualization of each evolution steps");

            displayGrid(grid);
        /*
         * User choose a mode
        */
        tryagain:
            string res = Console.ReadLine();
            if (res == "1")
            {
                gamePlay(grid, true);
            } else if (res == "2") {
                gamePlay(grid, false);
            } else {
                Console.WriteLine("I did not understand, try again by pressing only 1 or 2.");
                goto tryagain;
                       
            }
            Console.WriteLine("Thank you for playing, See you soon ! ;-).");
        }

    }
}
