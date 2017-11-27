using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TieFighter.Models.AccountViewModels
{
    public class UpdateUserThumbnailViewModel
    {
        public byte[] Image { get; set; }
        public string FileName { get; set; }
    }
}
