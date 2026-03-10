using FluentValidation;
using PaintMixer.Api.Dtos;

namespace PaintMixer.Api.Validators
{
    public sealed class SubmitPaintJobRequestValidator : AbstractValidator<SubmitPaintJobRequest>
    {
        private const int MinDyeValue = 0;
        private const int MaxDyeValue = 100;

        public SubmitPaintJobRequestValidator()
        {
            RuleFor(x => x.Red)
                .InclusiveBetween(MinDyeValue, MaxDyeValue)
                .WithMessage("Red dye must be between 0 and 100.");

            RuleFor(x => x.Blue)
                .InclusiveBetween(MinDyeValue, MaxDyeValue)
                .WithMessage("Blue dye must be between 0 and 100.");

            RuleFor(x => x.Yellow)
                .InclusiveBetween(MinDyeValue, MaxDyeValue)
                .WithMessage("Yellow dye must be between 0 and 100.");

            RuleFor(x => x.White)
                .InclusiveBetween(MinDyeValue, MaxDyeValue)
                .WithMessage("White dye must be between 0 and 100.");

            RuleFor(x => x.Black)
                .InclusiveBetween(MinDyeValue, MaxDyeValue)
                .WithMessage("Black dye must be between 0 and 100.");

            RuleFor(x => x.Green)
                .InclusiveBetween(MinDyeValue, MaxDyeValue)
                .WithMessage("Green dye must be between 0 and 100.");

            RuleFor(x => x)
                .Must(HaveValidTotal)
                .WithMessage("The total dye amount must not exceed 100.");
        }

        private static bool HaveValidTotal(SubmitPaintJobRequest request)
        {
            var total = request.Red + request.Blue + request.Yellow
                      + request.White + request.Black + request.Green;

            return total <= 100;
        }
    }
}
