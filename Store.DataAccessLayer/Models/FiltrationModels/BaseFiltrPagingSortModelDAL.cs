﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Models.FiltrationModels
{
   public class BaseFiltrPagingSortModelDAL
    {
        public long? Id { get; set; }
        public string PropertyForSort { get; set; }
        public bool IsAscending { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

    }
}
