using Application.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels
{
    public class TransactionViewModel
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public List<TransactionBuyRequest> TransactionBuyRequests { get; set; }
    }
}