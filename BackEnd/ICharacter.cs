﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd
{
    public interface ICharacter
    {
        void RunCommand(Command command);
    }
}
