using System.ComponentModel.DataAnnotations;

namespace CarSales.Web.Models.ViewModels;

public class PurchaseRequestViewModel : IValidatableObject
{
    public int CarId { get; set; }

    [Range(0, 100000000)]
    [Display(Name = "Deposit amount")]
    public decimal AmountPaid { get; set; }

    [Required, StringLength(60)]
    [Display(Name = "Payment method")]
    public string PaymentMethod { get; set; } = "Stripe Test";

    [StringLength(500)]
    [Display(Name = "Proof of payment reference / URL")]
    public string? ProofOfPayment { get; set; }

    [EmailAddress]
    [Display(Name = "Purchase confirmation email")]
    public string? ConfirmationEmail { get; set; }

    [StringLength(19)]
    [Display(Name = "Card number")]
    public string? CardNumber { get; set; }

    [StringLength(5)]
    [Display(Name = "Expiry (MM/YY)")]
    public string? ExpiryDate { get; set; }

    [StringLength(4)]
    [Display(Name = "CVC")]
    public string? Cvc { get; set; }

    [StringLength(100)]
    [Display(Name = "Name on card")]
    public string? CardholderName { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.Equals(PaymentMethod, "Stripe Test", StringComparison.OrdinalIgnoreCase))
        {
            yield break;
        }

        var normalizedCard = (CardNumber ?? string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);
        if (normalizedCard != "4242424242424242")
        {
            yield return new ValidationResult(
                "Use the Stripe test card number 4242 4242 4242 4242 for this demo.",
                [nameof(CardNumber)]);
        }

        if (string.IsNullOrWhiteSpace(ExpiryDate))
        {
            yield return new ValidationResult("Expiry date is required for Stripe test payments.", [nameof(ExpiryDate)]);
        }

        if (string.IsNullOrWhiteSpace(Cvc))
        {
            yield return new ValidationResult("CVC is required for Stripe test payments.", [nameof(Cvc)]);
        }

        if (string.IsNullOrWhiteSpace(CardholderName))
        {
            yield return new ValidationResult("Cardholder name is required for Stripe test payments.", [nameof(CardholderName)]);
        }
    }
}
