using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataModels.CustomModels
{
    public class StockModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Stock { get; set; }

        [Required(ErrorMessage ="*Product Stock is required")]
        public int? NewStock { get; set; }
        public string CategoryName { get; set; }
    }
}
