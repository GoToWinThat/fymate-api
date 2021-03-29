using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base.Models;

namespace Domain.Entities
{
    public class Profile : IHasOwner
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<Advert> Adverts { get; set; }
        public string OwnerID { get; set; }
    }
}
