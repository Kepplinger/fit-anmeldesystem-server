using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Models
{
    public class FitController
    {
         IUnitOfWork _iuow;
        public FitController(IUnitOfWork iuow)
        {
           this. _iuow = iuow;
        }
    }
}