document.querySelectorAll(".alert").forEach((alert) => {
  window.setTimeout(() => {
    alert.classList.add("fade");
  }, 3500);
});

document.querySelectorAll(".purchase-form").forEach((form) => {
  const paymentMethodSelect = form.querySelector(".payment-method-select");
  const stripeTestPanel = form.querySelector(".stripe-test-panel");

  if (!paymentMethodSelect || !stripeTestPanel) {
    return;
  }

  const toggleStripePanel = () => {
    const isStripeTest = paymentMethodSelect.value === "Stripe Test";
    stripeTestPanel.style.display = isStripeTest ? "block" : "none";
  };

  paymentMethodSelect.addEventListener("change", toggleStripePanel);
  toggleStripePanel();
});
