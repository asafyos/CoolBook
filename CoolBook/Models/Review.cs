﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBook.Models
{
    public class Review
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public String Body { get; set; }

        public int Rate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
