using System;

namespace BackEnd
{
    public class Puzzle
    {
        public Puzzle(string level)
        {
        }
        public string[][] GetState() { return new string[1][]; }
        public ICharacter GetCharacter() { return new TestNotReal(); }
        public bool IsFinished() { return true; }
        public int GetParLines() { return 8; }

    }
}
