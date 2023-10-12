using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VediciBot_Net.Persistence.Entity
{
    public class Roles
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public bool IsAsignable { get; set; }
    }
}
