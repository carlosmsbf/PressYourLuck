using System;
using System.ComponentModel.DataAnnotations;

namespace PressYourLuck.Models
{
    public class PlayerBet
    {

        private double currentBet;

        [Required(ErrorMessage = "You can not leave empty. Please, enter a value to bet. ")]
        [Range(0.01, double.MaxValue, ErrorMessage = "It has to be more than $0.01. ")]
        public double CurrentBet { get => currentBet; set => currentBet = value; }

    }
}
