using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models;

public class Customer
{
    public Customer()
    {
        Sales = new HashSet<Sale>();
    }

    [Key]
    public int CustomerId { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(100)]
    [Column(TypeName = "VARCHAR")]
    public string Email { get; set; }

    public string CreditCardNumber {  get; set; }

    public virtual ICollection<Sale> Sales { get; set; }

}
