using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public class Analysis
    {
        public int LinesOfCode
        {
            get
            {
                int lines = 0;
                foreach (Statement statement in _statements)
                {
                    lines += statement.GetLines();
                }
                return lines;
            }
        }

        private readonly List<Statement> _statements;
        public Analysis(List<Statement> statements)
        {
            _statements = statements;
        }
    }
}
