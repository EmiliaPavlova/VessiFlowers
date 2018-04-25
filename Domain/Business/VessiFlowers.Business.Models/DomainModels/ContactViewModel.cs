namespace VessiFlowers.Business.Models.DomainModels
{
    using System.ComponentModel.DataAnnotations;

    public class ContactViewModel
    {
        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Как мога да ти бъда полезна?")]
        [DataType(DataType.MultilineText)]
        public string HowCanIHelpYou { get; set; }

        [Display(Name = "За какъв повод търсиш цветя?")]
        [DataType(DataType.MultilineText)]
        public string WhatIsYourParty { get; set; }

        [Display(Name = "Кога е твоят повод?")]
        [DataType(DataType.MultilineText)]
        public string WhenIsYourParty { get; set; }

        [Display(Name = "За какъв човек?")]
        [DataType(DataType.MultilineText)]
        public string WhatTypeIsTheMan { get; set; }

        [Display(Name = "Кои са нейните/неговите любими цветя? А цветове?")]
        [DataType(DataType.MultilineText)]
        public string HisHerFlowersColors { get; set; }

        [Display(Name = "Каква емоция искаш да събудиш у нея/у него?")]
        [DataType(DataType.MultilineText)]
        public string WhatAreYouGoingToDo { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Display(Name = "Твоят е-мейл адрес е: ")]
        public string Email { get; set; }

        [Display(Name = "Има ли нещо друго, което съм пропуснала да те попитам, а е важно?")]
        [DataType(DataType.MultilineText)]
        public string AmIMissingSomething { get; set; }
    }
}