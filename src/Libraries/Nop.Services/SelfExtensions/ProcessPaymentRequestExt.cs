using System;

namespace Nop.Services.Payments
{
    public partial class ProcessPaymentRequest
    {
        public string SavedCardVault { get; set; }
        public bool IsNewCardSaveAllowed { get; set; }
    }
}
