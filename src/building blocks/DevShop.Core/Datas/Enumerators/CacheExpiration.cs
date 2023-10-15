using System.ComponentModel.DataAnnotations;

namespace DevShop.Core.Datas.Enumerators;

public enum CacheExpiration
{
    [Display(Name = "1Minutos", ShortName = "1", Description = "1 Minuto")]
    OneMinute = 1,

    [Display(Name = "2Minutos", ShortName = "2", Description = "2 Minutos")]
    TwoMinutes = 2,

    [Display(Name = "5Minutos", ShortName = "5", Description = "5 Minutos")]
    FiveMinutes = 5,

    [Display(Name = "10Minutos", ShortName = "10", Description = "10 Minutos")]
    TenMinutes = 10,

    [Display(Name = "30Minutos", ShortName = "30", Description = "30 Minutos")]
    ThirtyMinutes = 30,

    [Display(Name = "60Minutos", ShortName = "60", Description = "60 Minutos")]
    OneHour = 60,

    [Display(Name = "90Minutos", ShortName = "90", Description = "90 Minutos")]
    OneMiddleHours = 90,

    [Display(Name = "120Minutos", ShortName = "120", Description = "120 Minutos")]
    TwoHours = 120,

    [Display(Name = "150Minutos", ShortName = "150", Description = "150 Minutos")]
    TwoMiddleHours = 150,

    [Display(Name = "180Minutos", ShortName = "180", Description = "180 Minutos")]
    ThreeHours = 180,

    [Display(Name = "210Minutos", ShortName = "210", Description = "210 Minutos")]
    ThreeMiddleHours = 210,

    [Display(Name = "240Minutos", ShortName = "240", Description = "240 Minutos")]
    FourHours = 240,

    [Display(Name = "300Minutos", ShortName = "300", Description = "300 Minutos")]
    FiveHours = 300,

    [Display(Name = "360Minutos", ShortName = "360", Description = "360 Minutos")]
    SixHours = 360,

    [Display(Name = "420Minutos", ShortName = "420", Description = "420 Minutos")]
    SevenHours = 420,

    [Display(Name = "480Minutos", ShortName = "480", Description = "480 Minutos")]
    EightHours = 480,

    [Display(Name = "540Minutos", ShortName = "540", Description = "540 Minutos")]
    NineHours = 540,

    [Display(Name = "600Minutos", ShortName = "600", Description = "600 Minutos")]
    TenHours = 600,

    [Display(Name = "660Minutos", ShortName = "660", Description = "660 Minutos")]
    ElevenHours = 660,

    [Display(Name = "1200Minutos", ShortName = "1200", Description = "1200 Minutos")]
    TwelveHours = 1200,

    [Display(Name = "1440Minutos", ShortName = "1440", Description = "1440 Minutos")]
    TwelveForHours = 1440,
}