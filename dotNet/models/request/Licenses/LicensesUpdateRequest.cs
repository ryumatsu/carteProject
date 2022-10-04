﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Licenses
{
    public class LicensesUpdateRequest : LicensesAddRequest, IModelIdentifier
    {
        [Required]
        public int Id { get; set; }

    }
}
