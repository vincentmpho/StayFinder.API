﻿using System.ComponentModel.DataAnnotations;

namespace StayFinder.API.Models.DTOs.Country
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
