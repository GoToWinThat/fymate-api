using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base.Models;

namespace Domain.Entities
{
    public class Advert : IHasOwner
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
        public string OwnerID { get { return Profile.OwnerID; } }
    }
}
