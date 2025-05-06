using System.Net.Mail;
using System.Net;
using MailBildirimUygulaması.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MailBildirimUygulaması.Models;

namespace MailBildirimUygulaması.Controllers
{
	public class FormController : Controller
	{
		private readonly DegisiklikFormuContext _db;
        public FormController(DegisiklikFormuContext db)
        {
			_db = db;
        }
        public IActionResult Index()
		{
			var formList = _db.Formlars.ToList();
			return View(formList);
		}
		
		public IActionResult FormDetail(int id)
		{
			var formDetail = _db.Formlars
				.Include(f => f.EskiYeniKodlars)
				.FirstOrDefault(f => f.FormID == id);
			return View(formDetail);
		}

        public IActionResult FormMail(int id)
		{
			var formMail = _db.Formlars
				.Include(f => f.EskiYeniKodlars)
				.FirstOrDefault(f => f.FormID == id);
            if (formMail == null)
            {
                return NotFound();
            }
            return View(formMail);
		}

		[HttpPost , ActionName("FormMail")]
		public IActionResult FormMailPOST(Formlar obj)
		{
			var formMailPOST = _db.Formlars.FirstOrDefault(f => f.FormID == obj.FormID);
			
			if(obj!= null)
			{
				formMailPOST.Onaylayan = obj.Onaylayan;
				_db.Update(formMailPOST);
				_db.SaveChanges();
			}
			return RedirectToAction("Index");
			
		}
    }
}
