using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<Advert> Adverts { get; set; }
    }
}
