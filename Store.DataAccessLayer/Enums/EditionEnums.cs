﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Enums
{
    public partial class Enums
    {
        public class EditionEnums
        {
            public enum PrintingEditionType
            {
                None = 0,
                Book = 1,
                Journal = 2,
                Newspaper = 3
            }
            public enum Currency
            {
                None = 0,
                USD = 1,
                EUR = 2,
                GBP = 3,
                CHF = 4,
                JPY = 5,
                UAH = 6
            }
        }
    }
}