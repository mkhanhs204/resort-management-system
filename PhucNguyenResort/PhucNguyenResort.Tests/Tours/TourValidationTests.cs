using System.ComponentModel.DataAnnotations;
using PhucNguyenResort.ViewModels.Tours;
using Xunit;

namespace PhucNguyenResort.Tests.Tours
{
    public class TourValidationTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(-1, false)]
        [InlineData(999999999, true)]
        [InlineData(1000000000, false)] // Exceeds upper boundary
        public void TourCreateVM_PriceValidation(decimal price, bool expectedValid)
        {
            var model = new TourCreateVM
            {
                TourName = "Test",
                MaxCapacity = 10,
                DurationHours = 10,
                Price = price
            };

            var results = ValidateModel(model);
            var isPriceValid = !results.Any(v => v.MemberNames.Contains("Price"));

            Assert.Equal(expectedValid, isPriceValid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(1000, true)]
        [InlineData(1001, false)]
        public void TourCreateVM_MaxCapacityValidation(int capacity, bool expectedValid)
        {
            var model = new TourCreateVM
            {
                TourName = "Test",
                Price = 10,
                DurationHours = 10,
                MaxCapacity = capacity
            };

            var results = ValidateModel(model);
            var isValid = !results.Any(v => v.MemberNames.Contains("MaxCapacity"));

            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(1000, true)]
        [InlineData(1001, false)]
        public void TourCreateVM_DurationHoursValidation(int hours, bool expectedValid)
        {
            var model = new TourCreateVM
            {
                TourName = "Test",
                Price = 10,
                MaxCapacity = 10,
                DurationHours = hours
            };

            var results = ValidateModel(model);
            var isValid = !results.Any(v => v.MemberNames.Contains("DurationHours"));

            Assert.Equal(expectedValid, isValid);
        }

        [Fact]
        public void TourCreateVM_TourNameValidation()
        {
            var validModel = new TourCreateVM { TourName = new string('a', 100), Price = 10, MaxCapacity = 10, DurationHours = 10 };
            var invalidModel = new TourCreateVM { TourName = new string('a', 101), Price = 10, MaxCapacity = 10, DurationHours = 10 };

            Assert.Empty(ValidateModel(validModel).Where(v => v.MemberNames.Contains("TourName")));
            Assert.NotEmpty(ValidateModel(invalidModel).Where(v => v.MemberNames.Contains("TourName")));
        }
    }
}
