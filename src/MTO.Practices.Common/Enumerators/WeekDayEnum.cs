
namespace MTO.Practices.Common.Enumerators
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DateTimeEnum
    /// </summary>
    public enum WeekDayEnum
    {
        /// <summary>
        /// Desabilitado
        /// </summary>
        [Display(Name = "Desabilitar")]
        Desabilitado = -1,

        /// <summary>
        /// Domingo
        /// </summary>
        [Display(Name = "Domingo")]
        Sunday = 0,

        /// <summary>
        /// Segunda
        /// </summary>
        [Display(Name = "Segunda")]
        Monday = 1,

        /// <summary>
        /// Terca
        /// </summary>
        [Display(Name = "Terca")]
        Tuesday = 2,

        /// <summary>
        /// Quarta
        /// </summary>
        [Display(Name = "Quarta")]
        Wednesday = 3,

        /// <summary>
        /// Quinta
        /// </summary>
        [Display(Name = "Quinta")]
        Thursday = 4,

        /// <summary>
        /// Sexta
        /// </summary>
        [Display(Name = "Sexta")]
        Friday = 5,

        /// <summary>
        /// Sabado
        /// </summary>
        [Display(Name = "Sabado")]
        Saturday = 6
    }
}