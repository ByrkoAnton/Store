using System;

namespace Store.DataAccessLayer.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        
        public BaseEntity()
        {
            DateOfCreation = DateTime.Now;
        }
    }
}
