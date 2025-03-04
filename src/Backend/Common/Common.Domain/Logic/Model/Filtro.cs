using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Logic.Model
{
    public class Filtro
    {
        public int? pageNo { get; set; } = 1;
        public int? pageSize { get; set; } = 20;
        public string? OrderByColumn { get; set; }
        public bool? IsAsc { get; set; } = true;
    }
}
