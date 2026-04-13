using MediatR;

namespace Pricing.Impl.Features.Pricing
{
    public class CalculatePriceHandler : IRequestHandler<CalculatePriceQuery, double>
    {
        public Task<double> Handle(CalculatePriceQuery request, CancellationToken cancellationToken)
        {
            double price = 100; // giá cơ bản

            // Rule 1: tuổi > 40 tăng giá
            if (request.Age > 40)
            {
                price += 50;
            }

            // Rule 2: gói VIP
            if (request.Product.ToLower() == "vip")
            {
                price += 100;
            }

            return Task.FromResult(price);
        }
    }
}