using MediatR;

namespace Pricing.Impl.Features.Pricing
{
    public record CalculatePriceQuery(int Age, string Product) : IRequest<double>;
}