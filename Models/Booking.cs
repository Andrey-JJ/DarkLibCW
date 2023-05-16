﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DarkLibCW.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        [Display(Name = "Номер книги")]
        public Book? Book { get; set; }
        public int SubscriberId { get; set; }
        [Display(Name = "Абонент")]
        public Subscriber? Subscriber { get; set; }
    }
}
