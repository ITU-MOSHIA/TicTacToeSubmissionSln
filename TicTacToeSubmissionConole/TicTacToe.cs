using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using TicTacToeRendererLib.Enums;
using TicTacToeRendererLib.Renderer;

namespace TicTacToeSubmissionConole
{
    public class TicTacToe
    {
        private TicTacToeConsoleRenderer _boardRenderer;// Object to display the board on the console
        public PlayerEnum[,] board; // 2D array to hold the game board state
        public PlayerEnum _currentPlayer;// Variable to track the current player (X or O)

        public enum PlayerEnum
        {
            None, // Represents an empty spot on the board
            X,    // Represents Player X
            O      // Represents Player O
        }

        public TicTacToe()
        {
            _boardRenderer = new TicTacToeConsoleRenderer(10, 6);  // Initializes the board renderer with size
            board = new PlayerEnum[3, 3]; // Creates a 3x3 grid for the game board
            _currentPlayer = PlayerEnum.X; // Sets the starting player as X
        }

        public void Run()
        {
            bool gameRunning = true; // Variable to keep the game loop running
            while (gameRunning)
            {
                Console.Clear();  // Clear the console for a fresh display
                _boardRenderer.Render();  // Redraw board after every move

                Console.SetCursorPosition(2, 19);
                Console.Write($"Player {_currentPlayer}, it is your turn");

                int row = -1, column = -1;
                bool validInput = false;

                // Handle input and validation
                while (!validInput)
                {
                    try
                    {
                        // User input for row
                        Console.SetCursorPosition(0, 20);
                        Console.Write(new string(' ', Console.WindowWidth));  // Clear line
                        Console.SetCursorPosition(2, 20);  // Reset cursor for row input
                        Console.Write("Please Enter Row (0, 1, 2): ");
                        row = int.Parse(Console.ReadLine());

                        // User input for column
                        Console.SetCursorPosition(0, 22);
                        Console.Write(new string(' ', Console.WindowWidth));  // Clear line
                        Console.SetCursorPosition(2, 22);  // Reset cursor for column input
                        Console.Write("Please Enter Column (0, 1, 2): ");
                        column = int.Parse(Console.ReadLine());

                        if (row >= 0 && row <= 2 && column >= 0 && column <= 2)
                        {
                            if (board[row, column] == PlayerEnum.None)
                            {
                                board[row, column] = _currentPlayer;
                                validInput = true;
                            }
                            else
                            {
                                Console.SetCursorPosition(2, 23);
                                Console.WriteLine("Invalid move, cell is already taken.");
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(2, 23);
                            Console.WriteLine("Invalid move, please enter values between 0 and 2.");
                        }
                    }
                    catch
                    {
                        Console.SetCursorPosition(2, 23);
                        Console.WriteLine("Invalid input, please enter a number.");
                    }
                }

                // Map your PlayerEnum to TicTacToeRendererLib.Enums.PlayerEnum
                TicTacToeRendererLib.Enums.PlayerEnum mappedPlayer = _currentPlayer == PlayerEnum.X
                    ? TicTacToeRendererLib.Enums.PlayerEnum.X
                    : TicTacToeRendererLib.Enums.PlayerEnum.O;

                _boardRenderer.AddMove(row, column, mappedPlayer, true);

                // Check for a win
                if (CheckWin(row, column))
                {
                    Console.SetCursorPosition(2, 24);
                    Console.WriteLine($"Player {_currentPlayer} wins!");
                    gameRunning = false; // Stops the game if there is a winner
                }
                else if (CheckDraw())
                {
                    Console.SetCursorPosition(2, 24);
                    Console.WriteLine("It's a draw!");
                    gameRunning = false; // Stops the game if it is a draw
                }
                else
                {
                    _currentPlayer = _currentPlayer == PlayerEnum.X ? PlayerEnum.O : PlayerEnum.X; // Switch players
                }
            }
        }

        public bool CheckWin(int row, int column)
        {
            // Check row
            if (board[row, 0] == _currentPlayer && board[row, 1] == _currentPlayer && board[row, 2] == _currentPlayer)
                return true;

            // Check column
            if (board[0, column] == _currentPlayer && board[1, column] == _currentPlayer && board[2, column] == _currentPlayer)
                return true;

            // Check diagonals
            if (board[0, 0] == _currentPlayer && board[1, 1] == _currentPlayer && board[2, 2] == _currentPlayer)
                return true;

            if (board[0, 2] == _currentPlayer && board[1, 1] == _currentPlayer && board[2, 0] == _currentPlayer)
                return true;

            return false;
        }

        public bool CheckDraw()
        {
            foreach (var cell in board)
            {
                if (cell == PlayerEnum.None)
                    return false; // Returns false if there is at least one empty cell
            }
            return true; // Returns true if all cells are filled
        }
    }
}