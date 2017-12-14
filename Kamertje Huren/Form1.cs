using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kamertje_Huren
{
    public partial class Form1 : Form
    {
        int x = -1;
        int y = -1; //to memorise x and y positions of the mouse when clicking on panel

        
        int fieldSize = 3; //fieldsize not yet fully in use

        int playerTurn = 1;
        //1 is speler 1
        //2 is speler 2


        
        int[,] horzs = new int[2, 3]; //horizontal plays of both players
        int[,] verts = new int[3, 2]; //vertical plays of both players
        int[,] score = new int[2, 2]; //score of both players

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Resets all the values to 0, thus starting a new game
        /// </summary>
        public void NewGame()
        {
            //resets horizontal moves
            for (int i = 0; i < fieldSize - 1; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    horzs[i, j] = 0;
                }
            }

            //resets vertical moves
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize - 1; j++)
                {
                    verts[i, j] = 0;
                }
            }

            //resets score points
            for(int i = 0; i < fieldSize -1; i++)
            {
                for (int j = 0; j < fieldSize - 1; j++)
                {
                    score[i, j] = 0;
                }
            }

            
            playerTurn = 1;
            lbl1.Text = "Player 1";
            lbl2.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Refresh();
            NewGame();
        }

        /// <summary>
        /// Paints all the elements on the panel, moves and scores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //graphics init
            Graphics graphs = panel1.CreateGraphics();
            

            Rectangle rect = new Rectangle(0, 0, 50, 50); //rectangles for ellipses on the corners
            SolidBrush b = new SolidBrush(Color.Blue); //corner ellipses will be blue

            //draws all the corners
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rect.X = i * 100;
                    rect.Y = j * 100;
                    graphs.FillEllipse(b, rect);
                }
                
            }

            Rectangle block = new Rectangle();

            //draws all the points (in the color of the player)
            for (int i = 0; i < fieldSize - 1; i++)
            {
                for (int j = 0; j < fieldSize - 1; j++)
                {
                    SolidBrush b2 = new SolidBrush(Color.Transparent);
                    block = new Rectangle(i * 100 + 50, j * 100 + 50, 50, 50);

                    if (score[i, j] == 1)
                    {
                        b2 = new SolidBrush(Color.Green);
                    }
                    else if (score[i, j] == 2)
                    {
                        b2 = new SolidBrush(Color.Red);

                    }

                    graphs.FillRectangle(b2, block);
                }
            }
            
            //draws the player moves (in their own color)
            //horizontal moves
            for(int i = 0; i < fieldSize-1; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    SolidBrush b2 = new SolidBrush(Color.Black);
                    block = new Rectangle(i * 100 + 50, j * 100 + 15, 51, 20);

                    if (horzs[i,j] == 1)
                    {
                        b2 = new SolidBrush(Color.Green);
                    }
                    else if (horzs[i,j] == 2)
                    {
                        b2 = new SolidBrush(Color.Red);

                    }

                    graphs.FillRectangle(b2, block);
                }
            }
            //vertical moves
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize -1; j++)
                {
                    SolidBrush b2 = new SolidBrush(Color.Black);
                    block = new Rectangle(i * 100 + 15, j * 100 +50, 20, 51);

                    if (verts[i, j] == 1)
                    {
                        b2 = new SolidBrush(Color.Green);
                    }
                    else if (verts[i, j] == 2)
                    {
                        b2 = new SolidBrush(Color.Red);

                    }

                    graphs.FillRectangle(b2, block);
                }
            }

        }

        /// <summary>
        /// happens when panel is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            //sets mouse coordinates for further use
            x = e.X;
            y = e.Y;
            bool playerPlayed = false;
            bool playerHasScored = false;

            //checks is player clicked on playable field (also the ones on which already have been played)
            if(x < 250 && x > 0 && y < 250 && y > 0)
            {
                //checks if horizontal move is made
                if ((x / 50) % 2 == 1 && (y / 50) % 2 == 0 && horzs[x / 100, y / 100] == 0)
                {
                    //sets player move in matrix
                    horzs[x / 100, y / 100] = playerTurn;
                    playerPlayed = true;

                    //checks if not on bottom row
                    if(y/100 >= 0 && y/100 < fieldSize-1)
                    {
                        //checks if square is made
                        if (verts[x / 100, y / 100] != 0 && verts[x / 100 + 1, y / 100] != 0 && horzs[x / 100, y / 100 + 1] != 0)
                        {
                            Console.WriteLine("Square is made under me");
                            score[x / 100, y / 100] = playerTurn;
                            playerHasScored = true;
                        }
                    }

                    //checks if not in bottom row
                    if (y / 100 >= 1 && y / 100 < fieldSize)
                    {
                        //checks if square is made
                        if (verts[x / 100, y / 100-1] != 0 && verts[x / 100 + 1, y / 100 -1] != 0 && horzs[x / 100, y / 100 - 1] != 0)
                        {
                            Console.WriteLine("square is made above me");
                            score[x / 100, y / 100 - 1] = playerTurn;
                            playerHasScored = true;
                        }
                    }

                }

                //checks if vertical move is made
                if ((x / 50) % 2 == 0 && (y / 50) % 2 == 1 && verts[x / 100, y / 100] == 0)
                {
                    //sets player move in matrix
                    verts[x / 100, y / 100] = playerTurn;
                    playerPlayed = true;

                    //checks if not in right column
                    if (x / 100 >= 1 && x / 100 < fieldSize)
                    {
                        //checks if square can be made
                        if (horzs[x / 100 - 1, y / 100 + 1] != 0 && horzs[x / 100 - 1, y / 100] != 0 && verts[x / 100 - 1, y / 100] != 0)
                        {
                            Console.WriteLine("square on the left");
                            score[x / 100-1, y / 100] = playerTurn;
                            playerHasScored = true;
                        }
                    }
                    //checks if not in left column
                    if (x / 100 >= 0 && x / 100 < fieldSize - 1)
                    {
                        //checks if square can be made
                        if (horzs[x / 100, y / 100 + 1] != 0 && horzs[x / 100, y / 100] != 0 && verts[x / 100+1, y / 100] != 0)
                        {
                            Console.WriteLine("square on the right");
                            score[x / 100, y / 100] = playerTurn;
                            playerHasScored = true;
                        }
                    }
                }

                int playerPlayedScore = 0;
                bool playerHasWon = false;

                //checks if player has won!
                foreach(int i in score)
                {
                    if (i== playerTurn)
                    {
                        playerPlayedScore += 1;
                    }
                }

                //displas message on second label when a player has won
                if(playerPlayedScore > (((fieldSize - 1) * (fieldSize - 1) / 2)))
                {
                    lbl2.Text = "Player " + playerTurn.ToString() + " WOOONNNNNN!!!!!";
                    playerHasWon = true;
                }

                //changes players turn when other player has player and he has not scored
                if (playerPlayed && !playerHasScored)
                {
                    
                    if (playerTurn == 1)
                    {
                        playerTurn = 2;
                        lbl1.Text = "Speler 2";
                    }
                    else if(playerTurn == 2)
                    {
                        playerTurn = 1;
                        lbl1.Text = "Speler 1";
                    }
                }
            }
            
            //refreshes screen with tht new drawings
            Refresh();
        }

        //Knop voor nieuw spel
        private void btn1_Click(object sender, EventArgs e)
        {
            NewGame();
            Refresh();
        }
    }
}
