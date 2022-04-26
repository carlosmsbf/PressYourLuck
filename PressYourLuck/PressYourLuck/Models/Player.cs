using System;
using System.ComponentModel.DataAnnotations;


namespace PressYourLuck.Models
{
    public class Player
    {

        private string name;
        private double balance;

        [Required(ErrorMessage = "Please, enter your name.")]
        public string Name { get => name; set => name = value; }

        [Required(ErrorMessage ="You can not leave empty. Please, enter a valur to add for your balance account. ")]
        [Range(0.01, 10000.00, ErrorMessage = "It has to be a balance between CA$ 0.01 to CA$ 10,000.00. ")]
        public double Balance { get => balance; set => balance = value; }

    }
}
