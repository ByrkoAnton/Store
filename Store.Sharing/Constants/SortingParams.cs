using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Sharing.Constants
{
    public partial class Constants
    {
        public class SortingParams
        {
            public const string AUTHOR_DEF_SORT_PROP = "Name";
            public const string USER_DEF_SORT_PROP = "LastName";
            public const string EDITION_DEF_SORT_PROP = "Description";
            public const string ORDER_DEF_SORT_PROP = "id";
            public const string SORT_ASC_DIRECTION = "ASC";
            public const string SORT_DESC_DIRECTION = "DESC";
        }
    }
}
