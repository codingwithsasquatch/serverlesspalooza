using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sasquatch.Entities
{
    public class RestockRequest
    {
        #region Properties

        public string ProductId { get; set; }
            
        public int RestockQuantity { get; set; }

        #endregion
    }
}
