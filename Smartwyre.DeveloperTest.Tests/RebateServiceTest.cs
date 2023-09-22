using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using NSubstitute;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTest
{
    private readonly IFixture _fixture;
    private readonly RebateService _rebateService;
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IRebateCalculatorFactory _calculatorFactory;
    private readonly IRebateCalculator _rebateCalculator;

    public RebateServiceTest()
    {
        _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        _rebateDataStore = _fixture.Freeze<IRebateDataStore>();
        _productDataStore = _fixture.Freeze<IProductDataStore>();
        _calculatorFactory = _fixture.Freeze<IRebateCalculatorFactory>();
        _rebateCalculator = _fixture.Create<IRebateCalculator>();
        _rebateService = _fixture.Create<RebateService>();
    }

    [Fact]
    public void Calculate_NullRebate_ReturnsFailure()
    {
        var request = new CalculateRebateRequest()
        {
            RebateIdentifier = "rebate",
            ProductIdentifier = "product",
            Volume = 1m
        };
        _rebateDataStore.GetRebate(Arg.Is(request.RebateIdentifier)).Returns((Rebate)null);

        var result = _rebateService.Calculate(request);

        result.Success.Should().BeFalse();
        result.Reason.Should().NotBeEmpty();
        _rebateDataStore.Received().GetRebate(Arg.Is(request.RebateIdentifier));
        _productDataStore.DidNotReceive().GetProduct(Arg.Any<string>());
        _calculatorFactory.DidNotReceive().Build(Arg.Any<IncentiveType>());
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }

    [Fact]
    public void Calculate_NullProduct_ReturnsFailure()
    {
        var request = new CalculateRebateRequest()
        {
            RebateIdentifier = "rebate",
            ProductIdentifier = "product",
            Volume = 1m
        };
        _rebateDataStore.GetRebate(Arg.Is(request.RebateIdentifier)).Returns(new Rebate());
        _productDataStore.GetProduct(Arg.Is(request.ProductIdentifier)).Returns((Product)null);

        var result = _rebateService.Calculate(request);

        result.Success.Should().BeFalse();
        result.Reason.Should().NotBeEmpty();
        _rebateDataStore.Received().GetRebate(Arg.Is(request.RebateIdentifier));
        _productDataStore.Received().GetProduct(Arg.Is(request.ProductIdentifier));
        _calculatorFactory.DidNotReceive().Build(Arg.Any<IncentiveType>());
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }

    [Fact]
    public void Calculate_NullRebateCalculator_ReturnsFailure()
    {
        var request = new CalculateRebateRequest()
        {
            RebateIdentifier = "rebate",
            ProductIdentifier = "product",
            Volume = 1m
        };
        _rebateDataStore.GetRebate(Arg.Is(request.RebateIdentifier)).Returns(new Rebate());
        _productDataStore.GetProduct(Arg.Is(request.ProductIdentifier)).Returns(new Product());
        _calculatorFactory.Build(Arg.Any<IncentiveType>()).Returns((IRebateCalculator)null);

        var result = _rebateService.Calculate(request);

        result.Success.Should().BeFalse();
        result.Reason.Should().NotBeEmpty();
        _rebateDataStore.Received().GetRebate(Arg.Is(request.RebateIdentifier));
        _productDataStore.Received().GetProduct(Arg.Is(request.ProductIdentifier));
        _calculatorFactory.Received().Build(Arg.Any<IncentiveType>());
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }

    [Fact]
    public void Calculate_UnsupportedRebateType_ReturnsFailure()
    {
        var request = new CalculateRebateRequest()
        {
            RebateIdentifier = "rebate",
            ProductIdentifier = "product",
            Volume = 1m
        };
        var rebate = new Rebate();
        var product = new Product();
        _rebateDataStore.GetRebate(Arg.Is(request.RebateIdentifier)).Returns(rebate);
        _productDataStore.GetProduct(Arg.Is(request.ProductIdentifier)).Returns(product);
        _calculatorFactory.Build(Arg.Is(rebate.Incentive)).Returns(_rebateCalculator);
        _rebateCalculator.IncentiveTypeSupported(Arg.Is<Product>(p => product.Identifier == p.Identifier))
            .Returns(false);

        var result = _rebateService.Calculate(request);

        result.Success.Should().BeFalse();
        result.Reason.Should().NotBeEmpty();
        _rebateDataStore.Received().GetRebate(Arg.Is(request.RebateIdentifier));
        _productDataStore.Received().GetProduct(Arg.Is(request.ProductIdentifier));
        _calculatorFactory.Received().Build(Arg.Is(rebate.Incentive));
        _rebateCalculator.Received().IncentiveTypeSupported(
            Arg.Is<Product>(p => product.Identifier == p.Identifier));
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }

    [Fact]
    public void Calculate_InvalidRebate_ReturnsFailure()
    {
        var request = new CalculateRebateRequest()
        {
            RebateIdentifier = "rebate",
            ProductIdentifier = "product",
            Volume = 1m
        };
        var rebate = new Rebate();
        var product = new Product();
        _rebateDataStore.GetRebate(Arg.Is(request.RebateIdentifier)).Returns(rebate);
        _productDataStore.GetProduct(Arg.Is(request.ProductIdentifier)).Returns(product);
        _calculatorFactory.Build(Arg.Is(rebate.Incentive)).Returns(_rebateCalculator);
        _rebateCalculator.IncentiveTypeSupported(Arg.Is<Product>(p => product.Identifier == p.Identifier))
            .Returns(true);
        _rebateCalculator.RebateIsValid(
            Arg.Is<Rebate>(r => rebate.Identifier == r.Identifier),
            Arg.Is<Product>(p => product.Identifier == p.Identifier),
            Arg.Is(request.Volume)).Returns(false);

        var result = _rebateService.Calculate(request);

        result.Success.Should().BeFalse();
        result.Reason.Should().NotBeEmpty();
        _rebateDataStore.Received().GetRebate(Arg.Is(request.RebateIdentifier));
        _productDataStore.Received().GetProduct(Arg.Is(request.ProductIdentifier));
        _calculatorFactory.Received().Build(Arg.Is(rebate.Incentive));
        _rebateCalculator.Received().IncentiveTypeSupported(
            Arg.Is<Product>(p => product.Identifier == p.Identifier));
        _rebateCalculator.Received().RebateIsValid(
            Arg.Is<Rebate>(r => rebate.Identifier == r.Identifier),
            Arg.Is<Product>(p => product.Identifier == p.Identifier),
            Arg.Is(request.Volume));
        _rebateDataStore.DidNotReceive().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }

    [Fact]
    public void Calculate_Calculated_ReturnsSuccess()
    {
        var request = new CalculateRebateRequest()
        {
            RebateIdentifier = "rebate",
            ProductIdentifier = "product",
            Volume = 1m
        };
        var rebate = new Rebate();
        var product = new Product();
        _rebateDataStore.GetRebate(Arg.Is(request.RebateIdentifier)).Returns(rebate);
        _productDataStore.GetProduct(Arg.Is(request.ProductIdentifier)).Returns(product);
        _calculatorFactory.Build(Arg.Is(rebate.Incentive)).Returns(_rebateCalculator);
        _rebateCalculator.IncentiveTypeSupported(Arg.Is<Product>(p => product.Identifier == p.Identifier))
            .Returns(true);
        _rebateCalculator.RebateIsValid(
            Arg.Is<Rebate>(r => rebate.Identifier == r.Identifier),
            Arg.Is<Product>(p => product.Identifier == p.Identifier),
            Arg.Is(request.Volume)).Returns(true);
        _rebateCalculator.CalculateAmount(
            Arg.Is<Rebate>(r => rebate.Identifier == r.Identifier),
            Arg.Is<Product>(p => product.Identifier == p.Identifier),
            Arg.Is(request.Volume)).Returns(1m);

        var result = _rebateService.Calculate(request);

        result.Success.Should().BeTrue();
        result.Reason.Should().BeNull();
        _rebateDataStore.Received().GetRebate(Arg.Is(request.RebateIdentifier));
        _productDataStore.Received().GetProduct(Arg.Is(request.ProductIdentifier));
        _calculatorFactory.Received().Build(Arg.Is(rebate.Incentive));
        _rebateCalculator.Received().IncentiveTypeSupported(
            Arg.Is<Product>(p => product.Identifier == p.Identifier));
        _rebateCalculator.Received().RebateIsValid(
            Arg.Is<Rebate>(r => rebate.Identifier == r.Identifier),
            Arg.Is<Product>(p => product.Identifier == p.Identifier),
            Arg.Is(request.Volume));
        _rebateCalculator.Received().CalculateAmount(
            Arg.Is<Rebate>(r => rebate.Identifier == r.Identifier),
            Arg.Is<Product>(p => product.Identifier == p.Identifier),
            Arg.Is(request.Volume));
        _rebateDataStore.Received().StoreCalculationResult(Arg.Is<Rebate>(r => rebate.Identifier == r.Identifier), Arg.Is(1m));
    }
}