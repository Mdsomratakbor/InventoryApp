﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReadWriteFramework.Interfaces
{
    public interface IDatabaseConnection
    {
        string ConnectionString { get; set; }
    }
}
