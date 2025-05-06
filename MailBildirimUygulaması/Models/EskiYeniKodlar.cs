using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MailBildirimUygulaması.Models;

public partial class EskiYeniKodlar
{
    public int KodId { get; set; }
    public int FormId { get; set; }
    [DisplayName("Eski Kod")]
    public string EskiKod { get; set; }
    [DisplayName("Yeni Kod")]
    public string YeniKod { get; set; }
    public int Adet { get; set; }
    public Formlar? Form { get; set; }

}
