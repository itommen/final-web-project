﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Models
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Food Category")]
        public string Name { get; set; }

        public virtual List<Recipe> Recipes { get; set; }
    }
}