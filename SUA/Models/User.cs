using SUA.Utilities;
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
        public string MailRecover { get; set; }
        public string CrearFechas { get; set; }
        public string CargaBordereaux { get; set; }
        public string CargaSalas { get; set; }
        public string VeBordereaux { get; set; }
        public List<Show> ShowsAsignados { get; set; }
        public string UserMaster { get; set; }
        public string MustChangePasswordAtNextLogin { get; set; }
        public string Blocked { get; set; }
        public DateTime LastLogin { get; set; }

        public void SetId()
        {
            UniqueId = UtilitiesAndStuff.GenerateUniqueId();
        }
    }
}