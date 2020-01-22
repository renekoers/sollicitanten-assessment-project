using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Exceptions
{
    class ConstructionException : Exception
    {
        public ConstructionException()
        {

        }

        public ConstructionException(string message) : base(message)
        {

        }

        public ConstructionException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
