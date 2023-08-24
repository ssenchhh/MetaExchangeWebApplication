using MetaExchange.API.Controllers.Requests;
using FluentAssertions;
using MetaExchange.API.Validators;
using MetaExchange.API.Enums;

namespace MetaExchange.API.Tests.Validators
{
    public class ExchangeRequestValidatorTests
    {
        [Fact]
        public void Validate_Valid_IsValidTrue()
        {
            var request = CreateRequest();
            var validator = new ExchangeRequestValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_RequestTypeIsNotInEnum_IsValidFalse()
        {
            var request = CreateRequest();
            request.RequestType = (RequestType)10;
            var validator = new ExchangeRequestValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public void Validate_RequestType_ReturnsExpected(int type, bool expected)
        {
            var request = CreateRequest();
            request.RequestType = (RequestType)type;
            var validator = new ExchangeRequestValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(10, true)]
        public void Validate_Amount_ReturnsExpected(decimal amount, bool expected)
        {
            var request = CreateRequest();
            request.Amount = amount;
            var validator = new ExchangeRequestValidator();
            var result = validator.Validate(request);
            result.IsValid.Should().Be(expected);
        }

        private ExchangeRequest CreateRequest()
        {
            return new ExchangeRequest
            {
                RequestType = 0,
                Amount = 1
            };
        }
    }
}
