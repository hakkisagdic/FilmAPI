﻿using FilmAPI.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FilmAPI.Common.DTOs.FilmPerson
{
    public class BaseFilmPersonDto : IBaseDto
    {
        public BaseFilmPersonDto(string title,
                                 short year,
                                 string lastName,
                                 string birthdate,
                                 string role,
                                 short length = 0,
                                 string firstMidName = "")
        {
            Title = title;
            Year = year;
            LastName = lastName;
            Birthdate = birthdate;
            Role = role;
            Length = length;
            FirstMidName = firstMidName;
        }
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(1850, 2050)]
        public short Year { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Birthdate { get; set; }
        [Required]
        public string Role { get; set; }
        public short Length { get; set; }
        public string FirstMidName { get; set; }
    }
}