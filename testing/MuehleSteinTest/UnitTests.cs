using System;
using NUnit.Framework;
using MuehleStein;
using System.Collections.Generic;

namespace MuehleSteinTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PlayGame3()
        {
            Game game = new Game();
            var to = new List<int>{ 20, 19, 13, 5, 9, 2, 17, 14, 1, 7, 12, 6, 16, 18, 11, 4};
            foreach (var t in to)
            {
                game.NextMove(0, t);
            }

            game.NextMove(0, 16);
        }
            


        [Test]
        public void PlayGame2()
        {
            Game game = new Game();
            game.AddStone(8);
            game.AddStone(7);
            game.AddStone(22);
            game.AddStone(3);
            game.AddStone(5);
            game.AddStone(20);
            game.AddStone(19);
            game.AddStone(9);

            game.AddStone(21);
            game.AddStone(12);
            game.AddStone(11);
            game.AddStone(6);
            game.AddStone(2);
            Console.WriteLine(game.GetBoardAsString());
            var s = game.GetPossibleMoves();
            game.RemoveStone(6);
            game.AddStone(15);
            game.AddStone(14);
            game.AddStone(4);   
        }


        [Test]
        public void PlayGame()
        {
            var board = new List<int>() { 0, 0, -1, -1, 1, 1, 1, -1, 1, -1, 1, -1, -1, 0, -1, 1, 0, 0, 0, 1, -1, 1, 1, 0, 0 };
            Game game = new Game(board.ToArray(), true, true, Game.Phase.Place, 9, 9);

            game.RemoveStone(2);
        }

        [Test]
        public void GetPossibleMovesTest()
        {
            Game game = new Game();
            
            var list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            
            CollectionAssert.AreEquivalent(game.GetPossibleMoves()[0], list);
            game.AddStone(1);

            list.Remove(1);
            CollectionAssert.AreEquivalent(game.GetPossibleMoves()[0], list);

            game.AddStone(4);
            list.Remove(4);
            CollectionAssert.AreEquivalent(game.GetPossibleMoves()[0], list);

            game.AddStone(2);
            list.Remove(2);
            CollectionAssert.AreEquivalent(game.GetPossibleMoves()[0], list);

            game.AddStone(5);
            list.Remove(5);
            CollectionAssert.AreEquivalent(game.GetPossibleMoves()[0], list);

            game.AddStone(3);
            list.Remove(3);
            CollectionAssert.AreEquivalent(game.GetPossibleMoves()[0], new List<int> { 4, 5 });

            game.RemoveStone(5);
            list.Add(5);
            CollectionAssert.AreEquivalent(game.GetPossibleMoves()[0], list);

            game.AddStone(5);
            list.Remove(5);
            CollectionAssert.AreEquivalent(game.GetPossibleMoves()[0], list);
        }

        [Test]
        public void IsMoveLegalMoveTest()
        {
            int[] board = new int[25];
            board[1] = 1;
            board[3] = 1;
            board[5] = 1;
            board[15] = 1;
            

            board[17] = -1;
            board[20] = -1;
            board[23] = -1;
            board[22] = -1;

            Game game = new Game(board, true, false, Game.Phase.Move, 4, 4);
            Console.WriteLine(game.GetBoardAsString());
            game.MoveStone(5, 2);
            game.RemoveStone(22);
            game.MoveStone(23, 7);
        }


        [Test]
        public void IsRemoveLegalMoveTest()
        {
            int[] board = new int[25];
            board[1] = 1;
            board[2] = 1;
            board[3] = 1;
            board[15] = 1;
            board[24] = 1;
            board[8] = 1;


            board[17] = -1;
            board[20] = -1;
            board[23] = -1;
            board[22] = -1;

            Game game = new Game(board, false, true, Game.Phase.Jump);
            Assert.IsFalse(game.IsRemoveLegalMove(1));
            Assert.IsFalse(game.IsRemoveLegalMove(2));
            Assert.IsFalse(game.IsRemoveLegalMove(3));
            Assert.IsFalse(game.IsRemoveLegalMove(15));


            Assert.IsTrue(game.IsRemoveLegalMove(8));
        }

        [Test]
        public void PlayAGame()
        {
            Game game = new Game();
            game.AddStone(1);
            game.AddStone(2);
            game.AddStone(3);
            game.AddStone(4);
            game.AddStone(5);
            game.AddStone(6);
            game.AddStone(7);
            game.AddStone(8);
            game.AddStone(9);
            game.AddStone(10);
            game.AddStone(11);
            game.AddStone(12);
            game.AddStone(13);
            game.AddStone(14);
            game.AddStone(15);
            game.AddStone(16);
            game.AddStone(17);
            Assert.IsFalse(game.IsInMoveState);
            game.AddStone(18);
            Assert.IsTrue(game.IsInMoveState);
            Assert.Throws<IllegalMoveException>(() => game.AddStone(19));

            Assert.Throws<IllegalMoveException>(() => game.MoveStone(1, 2));
            Assert.Throws<IllegalMoveException>(() => game.MoveStone(2, 5));
            Assert.Throws<IllegalMoveException>(() => game.MoveStone(10, 22));


            var gameBoard = "x-----o-----x\n" +
                "| o---x---o |\n" +
                "| | x o x | |\n" +
                "o x o   x o x\n" +
                "| | o x o | |\n" +
                "| 0---0---0 |\n" +
                "0-----0-----0";

            Assert.AreEqual(gameBoard, game.GetBoardAsString());

            game.MoveStone(17, 20);

            gameBoard =     "x-----o-----x\n" +
                            "| o---x---o |\n" +
                            "| | x o x | |\n" +
                            "o x o   x o x\n" +
                            "| | o 0 o | |\n" +
                            "| 0---x---0 |\n" +
                            "0-----0-----0";

            Assert.AreEqual(gameBoard, game.GetBoardAsString());

            Assert.Throws<IllegalMoveException>(() => game.MoveStone(1, 2));
            Assert.Throws<IllegalMoveException>(() => game.MoveStone(2, 5));
            Assert.Throws<IllegalMoveException>(() => game.MoveStone(17, 20));
            game.MoveStone(18, 17); //B
            game.MoveStone(20, 23);

            game.MoveStone(17, 18); //B

            Assert.IsFalse(game.IsInRemoveStoneState);

            game.MoveStone(23, 24);
            Assert.IsTrue(game.IsInRemoveStoneState);

            gameBoard =     "x-----o-----x\n" +
                            "| o---x---o |\n" +
                            "| | x o x | |\n" +
                            "o x o   x o x\n" +
                            "| | o 0 o | |\n" +
                            "| 0---0---0 |\n" +
                            "0-----0-----x";

            Assert.AreEqual(gameBoard, game.GetBoardAsString());

            Assert.Throws<IllegalMoveException>(() => game.MoveStone(18, 17));
            Assert.Throws<IllegalMoveException>(() => game.MoveStone(10, 22));
            Assert.Throws<IllegalMoveException>(() => game.AddStone(22));

            Assert.IsTrue(game.IsRemoveLegalMove(2));
            Assert.IsTrue(game.IsRemoveLegalMove(4));
            Assert.IsTrue(game.IsRemoveLegalMove(6));
            Assert.IsTrue(game.IsRemoveLegalMove(8));
            Assert.IsTrue(game.IsRemoveLegalMove(10));
            Assert.IsTrue(game.IsRemoveLegalMove(12));
            Assert.IsTrue(game.IsRemoveLegalMove(14));
            Assert.IsTrue(game.IsRemoveLegalMove(16));
            Assert.IsTrue(game.IsRemoveLegalMove(18));

            Assert.IsFalse(game.IsRemoveLegalMove(1));
            Assert.IsFalse(game.IsRemoveLegalMove(3));
            Assert.IsFalse(game.IsRemoveLegalMove(5));
            Assert.IsFalse(game.IsRemoveLegalMove(7));
            Assert.IsFalse(game.IsRemoveLegalMove(9));
            Assert.IsFalse(game.IsRemoveLegalMove(11));
            Assert.IsFalse(game.IsRemoveLegalMove(13));
            Assert.IsFalse(game.IsRemoveLegalMove(15));
            Assert.IsFalse(game.IsRemoveLegalMove(17));
            Assert.IsFalse(game.IsRemoveLegalMove(24));

            game.RemoveStone(2);

            Assert.IsFalse(game.IsInRemoveStoneState);

            gameBoard =
                            "x-----0-----x\n" +
                            "| o---x---o |\n" +
                            "| | x o x | |\n" +
                            "o x o   x o x\n" +
                            "| | o 0 o | |\n" +
                            "| 0---0---0 |\n" +
                            "0-----0-----x";

            Assert.AreEqual(gameBoard, game.GetBoardAsString());

            game.MoveStone(18, 17);

            gameBoard =
                "x-----0-----x\n" +
                "| o---x---o |\n" +
                "| | x o x | |\n" +
                "o x o   x o x\n" +
                "| | o o 0 | |\n" +
                "| 0---0---0 |\n" +
                "0-----0-----x";

            Assert.AreEqual(gameBoard, game.GetBoardAsString());
            game.MoveStone(5, 2);

            Assert.IsTrue(game.IsInRemoveStoneState);

            
              game.RemoveStone(4);

            Assert.IsFalse(game.IsInRemoveStoneState);

            game.MoveStone(17, 18);
            game.MoveStone(2, 5);
            game.MoveStone(18, 17);
            game.MoveStone(5, 2);
            game.RemoveStone(6);

            game.MoveStone(17, 18);
            game.MoveStone(2, 5);
            game.MoveStone(18, 17);
            game.MoveStone(5, 2);
            game.RemoveStone(8);

            game.MoveStone(17, 18);
            game.MoveStone(2, 5);
            game.MoveStone(18, 17);
            game.MoveStone(5, 2);
            game.RemoveStone(10);

            gameBoard = "x-----x-----x\n" +
                        "| 0---0---0 |\n" +
                        "| | x 0 x | |\n" +
                        "0 x o   x o x\n" +
                        "| | o o 0 | |\n" +
                        "| 0---0---0 |\n" +
                        "0-----0-----x";

            Assert.AreEqual(gameBoard, game.GetBoardAsString());

            game.MoveStone(17, 18);
            game.MoveStone(2, 5);
            game.MoveStone(18, 17);
            game.MoveStone(5, 2);
            Assert.IsTrue(game.IsInMoveState);
            game.RemoveStone(12);
            Assert.IsFalse(game.IsInMoveState);
            
            gameBoard = "x-----x-----x\n" +
                        "| 0---0---0 |\n" +
                        "| | x 0 x | |\n" +
                        "0 x 0   x o x\n" +
                        "| | o o 0 | |\n" +
                        "| 0---0---0 |\n" +
                        "0-----0-----x";

            Assert.AreEqual(gameBoard, game.GetBoardAsString());
            

            Assert.Throws<IllegalMoveException>(() => game.MoveStone(14, 16));
            Assert.Throws<IllegalMoveException>(() => game.MoveStone(24, 18));
            Assert.Throws<IllegalMoveException>(() => game.MoveStone(23, 22));

            Console.WriteLine("________");
            Console.WriteLine(game.GetBoardAsString());
            
            game.MoveStone(14, 23);
            Console.WriteLine("_____2___");
            Console.WriteLine(game.GetBoardAsString());
            game.MoveStone(15, 14);
            Assert.IsFalse(game.IsInRemoveStoneState);
            game.MoveStone(23, 18);
            Assert.IsTrue(game.IsInRemoveStoneState);
            game.RemoveStone(24);
            Assert.IsFalse(game.IsInRemoveStoneState);
            gameBoard = "x-----x-----x\n" +
                        "| 0---0---0 |\n" +
                        "| | x 0 x | |\n" +
                        "0 x 0   x x 0\n" +
                        "| | o o o | |\n" +
                        "| 0---0---0 |\n" +
                        "0-----0-----0";
            Assert.AreEqual(gameBoard, game.GetBoardAsString());

            game.MoveStone(2, 5);
            game.MoveStone(16, 22);

            game.MoveStone(5, 2);

            Assert.IsFalse(game.GameEnded);
            game.RemoveStone(22);

            Assert.IsTrue(game.GameEnded);
            Assert.Throws<IllegalMoveException>(() => game.MoveStone(16, 22));
            Assert.Throws<IllegalMoveException>(() => game.AddStone(19));
        }

        [Test]
        public void InitializeGameTest()
        {
            int[] board = new int[25];
            board[1] = 1;
            board[2] = 1;
            board[3] = -1;
            board[4] = 1;
            board[5] = -1;
            board[17] = -1;

            Game game2 = new Game(board, false, true, Game.Phase.Jump);

            Game game = new Game();
            game.AddStone(1);            
            game.AddStone(17);

            game.AddStone(2);
            game.AddStone(3);

            game.AddStone(4);
            game.AddStone(5);

            Assert.AreEqual(game.GetBoardAsString(), game2.GetBoardAsString());

            Console.WriteLine(game.GetBoardAsString());


            var gameBoard = "x-----x-----o\n" +
                            "| x---o---0 |\n" +
                            "| | 0 0 0 | |\n" +
                            "0 0 0   0 0 0\n" +
                            "| | 0 o 0 | |\n" +
                            "| 0---0---0 |\n" +
                            "0-----0-----0";

            Assert.AreEqual(gameBoard, game2.GetBoardAsString());

        }

        [Test]
        public void IllegalMoveAddStone1()
        {           
            Game game = new Game();
            game.AddStone(1);
            Assert.Throws<IllegalMoveException>(() =>game.AddStone(1));
        }

        [Test]
        public void IllegalMoveAddStone2()
        {
            Game game = new Game();
            game.AddStone(1);
            game.IsInRemoveStoneState = true;
            Assert.Throws<IllegalMoveException>(() => game.AddStone(2));
        }
    }
}