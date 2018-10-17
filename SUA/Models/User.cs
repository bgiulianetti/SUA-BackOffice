using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SUA.Models
{
    public class UserModel
    {
        public string UniqueId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int MailRecover { get; set; }
        public bool CrearFechas { get; set; }
        public bool CargaBordereaux { get; set; }
        public bool CargaSalas { get; set; }
        public bool VerBordereaux { get; set; }
        public List<string> ElencosAsignados { get; set; }
        public bool UserMaster { get; set; }
        public bool MustChangePasswordAtNextLogin { get; set; }
        public bool Blocked { get; set; }
    }
}