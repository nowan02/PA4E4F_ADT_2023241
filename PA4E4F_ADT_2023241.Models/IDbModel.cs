using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA4E4F_ADT_2023241.Models
{
    public interface IDbModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
