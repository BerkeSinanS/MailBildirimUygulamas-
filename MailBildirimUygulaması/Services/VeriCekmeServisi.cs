using System.Net.Mail;
using System.Net;
using MailBildirimUygulaması.Data;
using System.Text;


namespace MailBildirimUygulaması.Services
{
	public class VeriCekmeServisi : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public VeriCekmeServisi(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _scopeFactory.CreateScope())
				{
					var db = scope.ServiceProvider.GetRequiredService<DegisiklikFormuContext>();
                    var formlar = db.Formlars
                                    .Where(f => f.Durum == "Bekliyor")
                                    .ToList();
                    foreach (var form in formlar)
                    {
                        try
                        {
                            
                            string mailAdresi = form.Bolum switch
                            {
                                "Tasarım Bölümü" => "testmail6751@gmail.com",
                                "Arge Bölümü" => "argebolumu558@gmail.com",
                                "İnsan Kaynakları" => "insankaynaklari558@gmail.com",
                                _ => ""
                            };
                            string redCircle = "\U0001F534";  
         
                           
                            if (!string.IsNullOrEmpty(mailAdresi))
                            {
                             
                                string oncelikEtiketi;
                                MailPriority oncelikSeviyesi;

                                switch (form.Uygulama)
                                {
                                    case "Acil":
                                        oncelikEtiketi = $"{redCircle} [ACİL] ";
                                        oncelikSeviyesi = MailPriority.High;
                                        break;
                                    case "Normal":
                                        oncelikEtiketi = $" [NORMAL] ";
                                        oncelikSeviyesi = MailPriority.Normal;
                                        break;
                                    case "Düşük":
                                        oncelikEtiketi = $" [DÜŞÜK] ";
                                        oncelikSeviyesi = MailPriority.Low;
                                        break;
                                    default:
                                        oncelikEtiketi = "";
                                        oncelikSeviyesi = MailPriority.Normal;
                                        break;
                                }

                                var mailMessage = new MailMessage
                                {
                                    From = new MailAddress("gonderen@gmail.com"),

                                    Subject = $"{oncelikEtiketi}Form Durumu Güncellemesi", 
                                    Body = $@"
                                <html>
                                    <body>
                                        <p><strong>{form.Uygulama} öncelikli form işlenmiştir.</strong></p>
                                        <p>Daha fazla bilgi için <a href='https://localhost:7264/Form/FormMail/{form.FormID}'>bu linke</a> tıklayın.</p>
                                    </body>
                                </html>",
                                    IsBodyHtml = true,
                                    Priority = oncelikSeviyesi
                                };
                                mailMessage.To.Add(mailAdresi);
                                mailMessage.Priority = oncelikSeviyesi;
                                mailMessage.SubjectEncoding = Encoding.UTF8;

                                
                                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
								{
									smtpClient.Credentials = new NetworkCredential("testmail6751@gmail.com", "fjtd lefq pmfr pywo"); 
									smtpClient.EnableSsl = true;
									await smtpClient.SendMailAsync(mailMessage);
								}

								
								form.Durum = "İşlendi";
								db.Update(form);
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Mail gönderim hatası: {ex.Message}");
						}
					}

					await db.SaveChangesAsync();
				}
				await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
			}
		}
	}
}
