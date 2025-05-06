using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MailBildirimUygulaması.Models;

public partial class Formlar
{
    public int FormID { get; set; }
    [Required]
    public string Uygulama { get; set; }
    [DisplayName("Açıklama")]
    public string Aciklama { get; set; }
    public string Not { get; set; }
    [DisplayName("Yayın Tarihi")]
    public DateOnly YayinTarihi { get; set; }
    public string? Onaylayan { get; set; }
    [DisplayName("Kontrol Eden")]
    public string? KontrolEden { get; set; }
    [DisplayName("Değişiklik Numarası")]
    public string DegisiklikNumarasi { get; set; }
    public string? Yayınlayan { get; set; }
    public string? Durum { get; set; }
    [DisplayName("Bölüm")]
    public string? Bolum { get; set; }
    public virtual ICollection<EskiYeniKodlar> EskiYeniKodlars { get; set; } = new List<EskiYeniKodlar>();
}
