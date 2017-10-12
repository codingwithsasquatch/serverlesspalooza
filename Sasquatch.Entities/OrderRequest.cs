using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sasquatch.Entities
{
    public class OrderRequest
    {
        #region Properties

        public int CustomerId { get; set; }

        public string MailingAddress { get; set; }

        public CartItem Item { get; set; }   

        #endregion
    }
}
