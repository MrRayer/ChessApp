using System.Diagnostics;

namespace ChessApp.Models
{
    public class ChessMatch
    {
        public List<List<string>> Board { get; set; }
        public int Id { get; set; }
        public bool WhiteTurn = true;
        public string Victory = "";
        public ChessMatch(int _id)
        {
            Board = new();
            Board.Add(new List<string>() { "BR", "BKn", "BB", "BQ", "BK", "BB", "BKn", "BR" });
            Board.Add(new List<string>() { "BP", "BP", "BP", "BP", "BP", "BP", "BP", "BP" });
            Board.Add(new List<string>() { "", "", "", "", "", "", "", "" });
            Board.Add(new List<string>() { "", "", "", "", "", "", "", "" });
            Board.Add(new List<string>() { "", "", "", "", "", "", "", "" });
            Board.Add(new List<string>() { "", "", "", "", "", "", "", "" });
            Board.Add(new List<string>() { "WP", "WP", "WP", "WP", "WP", "WP", "WP", "WP" });
            Board.Add(new List<string>() { "WR", "WKn", "WB", "WQ", "WK", "WB", "WKn", "WR" });
            Id = _id;
        }
        public bool Move(Move move)
        {
            if (Victory != "") { Trace.WriteLine("match is over, go home"); return false; }
            if (!MoveIsValid(move)) { Trace.WriteLine("invalid movement"); return false; }
            Trace.WriteLine("valid movement");
            if (move.Piece[0] == "W"[0] && WhiteTurn == true || move.Piece[0] == "B"[0] && WhiteTurn == false)
            {
                Trace.WriteLine("valid turn");
                string piece = Board[move.StartY][move.StartX];
                Board[move.StartY][move.StartX] = "";
                Board[move.EndY][move.EndX] = piece;
                if (WhiteTurn) { WhiteTurn = false; }
                else { WhiteTurn = true; }
                return true;
            }
            else { Trace.WriteLine("invalid turn"); return false; }
        }
        private bool MoveIsValid(Move move)
        {
            Trace.WriteLine("------ Validating ------");
            Trace.WriteLine($"Match id:{Id} | Piece: {move.Piece}");
            Trace.WriteLine($"Start position: {move.StartX}:{move.StartY}");
            Trace.WriteLine($"End position: {move.EndX}:{move.EndY}");
            int Xdif = move.StartX - move.EndX;
            int Ydif = move.StartY - move.EndY;
            Trace.WriteLine($"Diffs: X:{Xdif}|Y:{Ydif}");
            try
            {
                if (Board[move.EndY][move.EndX][0] == move.Piece[0])
                {
                    Trace.WriteLine("ally on the way");
                    return false;
                }
            }
            catch { }
            switch (move.Piece)
            {
                case ("BP"):
                    Trace.WriteLine("BP detected");
                    if (Math.Abs(Xdif) == 0)
                    {
                        if (Ydif == -2 && move.StartY == 1 || Ydif == -1)
                        {
                            if (Board[move.EndY][move.EndX] == "") { if (KingAttacked(move.EndX,move.EndY)) Victory = "Black"; return true; }
                            return false;
                        }
                    }
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    {
                        if (Ydif == -2 && move.StartY == 1 || Ydif == -1)
                        {
                            if (Board[move.EndY][move.EndX] != "") { if (KingAttacked(move.EndX, move.EndY)) Victory = "Black"; return true; }
                            return false;
                        }
                    }
                    break;
                case ("BR"):
                    Trace.WriteLine("BR detected");
                    if (Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                    { // if it moves in X no move in Y or move in Y, no move in X
                        if (StraightCollitionCheck(move.StartX, move.StartY, move.EndX, move.EndY))
                        {
                            Trace.WriteLine("piece in the middle of movement");
                            return false;
                        }
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                        return true;
                    }
                    break;
                case ("BKn"):
                    Trace.WriteLine("BKn detected");
                    if (Math.Abs(Xdif) == 2 && Math.Abs(Ydif) == 1 || Math.Abs(Ydif) == 2 && Math.Abs(Xdif) == 1)
                    { // if difference in X = 2, difference in Y = 1 or difference in X = 1 and difference in Y = 2
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                        return true;
                    }
                    break;
                case ("BB"):
                    Trace.WriteLine("BB detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    { // Y movement = X movement
                        if (DiagonalCollitionCheck(move.StartX, move.StartY, move.EndX, move.EndY))
                        {
                            Trace.WriteLine("piece in the middle of movement");
                            return false;
                        }
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                        return true;
                    }
                    break;
                case ("BQ"):
                    Trace.WriteLine("BQ detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif) || Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                    { // Y movement = X movement or if it moves in X no move in Y or move in Y, no move in X
                        if (Math.Abs(Xdif) == Math.Abs(Ydif))
                        {
                            if (DiagonalCollitionCheck(move.StartX, move.StartY, move.EndX, move.EndY))
                            {
                                Trace.WriteLine("piece in the middle of movement");
                                return false;
                            }
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                            return true;
                        }
                        if (Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                        {
                            if (StraightCollitionCheck(move.StartX, move.StartY, move.EndX, move.EndY))
                            {
                                Trace.WriteLine("piece in the middle of movement");
                                return false;
                            }
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                            return true;
                        }
                    }
                    break;
                case ("BK"):
                    Trace.WriteLine("BK detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif) || Xdif == 0 || Ydif == 0)
                    { // Y movement = X movement
                        if (Math.Abs(Ydif) == 1 || Math.Abs(Xdif) == 1)
                        { // movement difference in X or Y = 1
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "Black";
                            return true;
                        }
                    }
                    break;
                case ("WP"):
                    Trace.WriteLine("WP detected");
                    if (Math.Abs(Xdif) == 0)
                    {
                        if (Ydif == 2 && move.StartY == 6 || Ydif == 1)
                        {
                            if (Board[move.EndY][move.EndX] == "") { if (KingAttacked(move.EndX, move.EndY)) Victory = "White"; return true; }
                            return false;
                        }
                    }
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    {
                        if (Ydif == 1)
                        {
                            if (Board[move.EndY][move.EndX] != "") { if (KingAttacked(move.EndX, move.EndY)) Victory = "White"; return true; }
                            return false;
                        }
                    }
                    break;
                case ("WR"):
                    Trace.WriteLine("WR detected");
                    if (Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                    { // if it moves in X no move in Y or move in Y, no move in X
                        if (StraightCollitionCheck(move.StartX, move.StartY, move.EndX, move.EndY))
                        {
                            Trace.WriteLine("piece in the middle of movement");
                            return false;
                        }
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                        return true;
                    }
                    break;
                case ("WKn"):
                    Trace.WriteLine("WKn detected");
                    if (Math.Abs(Xdif) == 2 && Math.Abs(Ydif) == 1 || Math.Abs(Ydif) == 2 && Math.Abs(Xdif) == 1)
                    { // if difference in X = 2, difference in Y = 1 or difference in X = 1 and difference in Y = 2
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                        return true;
                    }
                    break;
                case ("WB"):
                    Trace.WriteLine("WB detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    { // Y movement = X movement
                        if (DiagonalCollitionCheck(move.StartX, move.StartY, move.EndX, move.EndY))
                        {
                            Trace.WriteLine("piece in the middle of movement");
                            return false;
                        }
                        if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                        return true;
                    }
                    break;
                case ("WQ"):
                    Trace.WriteLine("WQ detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif) || Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                    { // Y movement = X movement or if it moves in X no move in Y or move in Y, no move in X
                        if (Math.Abs(Xdif) == Math.Abs(Ydif))
                        {
                            if (DiagonalCollitionCheck(move.StartX, move.StartY, move.EndX, move.EndY))
                            {
                                Trace.WriteLine("piece in the middle of movement");
                                return false;
                            }
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                            return true;
                        }
                        if (Xdif != 0 && Ydif == 0 || Xdif == 0 && Ydif != 0)
                        {
                            if (StraightCollitionCheck(move.StartX, move.StartY, move.EndX, move.EndY))
                            {
                                Trace.WriteLine("piece in the middle of movement");
                                return false;
                            }
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                            return true;
                        }
                    }
                    break;
                case ("WK"):
                    Trace.WriteLine("WK detected");
                    if (Math.Abs(Xdif) == Math.Abs(Ydif))
                    { // Y movement = X movement
                        if (Math.Abs(Ydif) == 1 || Math.Abs(Xdif) == 1)
                        { // movement difference in X or Y = 1
                            if (KingAttacked(move.EndX, move.EndY)) Victory = "White";
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }
        private bool KingAttacked(int X, int Y)
        {
            if (Board[Y][X] != "" && Board[Y][X][1] == "K"[0]) return true;
            else return false;
        }
        private bool StraightCollitionCheck(int startX,int startY,int endX,int endY)
        {
            bool collitionFound = false;
            if (startX == endX)
            {
                if (endY > startY) { endY--; }
                else endY++;
                while (endY != startY)
                {
                    if (Board[endY][endX] != "") { collitionFound = true; }
                    if (endY > startY) { endY--; }
                    else endY++;
                }
            }
            else
            {
                if (endX > startX) { endX--; }
                else endX++;
                while (endX != startX)
                {
                    if (Board[endY][endX] != "") { collitionFound = true; }
                    if (endX > startX) { endX--; }
                    else endX++;
                }
            }
            return collitionFound;
        }
        private bool DiagonalCollitionCheck(int startX, int startY, int endX, int endY)
        {
            bool collitionFound = false;
            if (endX < startX) endX++;
            else endX--;
            if (endY < startY) endY++;
            else endY--;
            while (endX != startX)
            {
                if (Board[endY][endX] != "") { collitionFound = true; }
                if (endX < startX) endX++;
                else endX--;
                if (endY < startY) endY++;
                else endY--;
            }
            return collitionFound;
        }
    }
}
