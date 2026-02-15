
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ProductService.Domain.Entities;
using ProductService.Domain.Exceptions;
using ProductService.Domain.ValueObjects;
using Xunit;

namespace ProductService.Tests.Domain.Entities
{
    public class ProductTests
    {
        private Product CreateTestProduct()
        {
            return ProductBuilder.CreateNew()
                .WithId(Guid.NewGuid())
                .WithCategoryId(Guid.NewGuid())
                .WithName("Test Product")
                .WithDescription("Test Description")
                .WithStockQuantity(10)
                .WithPrice(100, "USD")
                .WithMainImage("http://example.com/main.jpg", "main image")
                .Build();
        }

        // Discount Tests
        [Fact]
        public void EditDiscountOrThrow_ShouldAddDiscount_WhenNoDiscountExists()
        {
            var product = CreateTestProduct();
            product.EditDiscountOrThrow(10, DateTime.UtcNow.AddDays(1));
            product.Discount.Percentage.Should().Be(10);
        }

        [Fact]
        public void EditDiscountOrThrow_ShouldThrowInvalidOperationException_WhenDiscountExists()
        {
            var product = CreateTestProduct();
            product.EditDiscountOrThrow(10, DateTime.UtcNow.AddDays(1));
            Action act = () => product.EditDiscountOrThrow(20, DateTime.UtcNow.AddDays(1));
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void RemoveDiscount_ShouldSetDiscountToZero()
        {
            var product = CreateTestProduct();
            product.EditDiscountOrThrow(10, DateTime.UtcNow.AddDays(1));
            product.RemoveDiscount();
            product.Discount.Percentage.Should().Be(0);
        }

        // Stock Tests
        [Fact]
        public void IncreaseStockOrThrow_ShouldIncreaseStock()
        {
            var product = CreateTestProduct();
            var initialStock = product.StockQuantity;
            product = product.IncreaseStockOrThrow(5);
            product.StockQuantity.Should().Be(initialStock + 5);
        }
        
        [Fact]
        public void DecreaseStockOrThrow_ShouldDecreaseStock()
        {
            var product = CreateTestProduct();
            var initialStock = product.StockQuantity;
            product = product.DecreaseStockOrThrow(5);
            product.StockQuantity.Should().Be(initialStock - 5);
        }

        [Fact]
        public void DecreaseStockOrThrow_ShouldThrowInsufficientStockException_WhenStockIsInsufficient()
        {
            var product = CreateTestProduct();
            Action act = () => product.DecreaseStockOrThrow(product.StockQuantity + 1);
            act.Should().Throw<InsufficientStockException>();
        }

        // Image Tests
        [Fact]
        public void AddRelatedImagesOrThrow_ShouldAddImages_WhenUrlsAreUnique()
        {
            var product = CreateTestProduct();
            var images = new List<Image> { Image.Create("http://example.com/image1.jpg", "alt1") };
            product.AddRelatedImagesOrThrow(images);
            product.RelatedImages.Should().HaveCount(1);
        }

        [Fact]
        public void AddRelatedImagesOrThrow_ShouldThrowInvalidOperationException_WhenUrlIsNotUnique()
        {
            var product = CreateTestProduct();
            var images = new List<Image> { Image.Create("http://example.com/image1.jpg", "alt1") };
            product.AddRelatedImagesOrThrow(images);
            Action act = () => product.AddRelatedImagesOrThrow(images);
            act.Should().Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void RemoveRelatedImageOrThrow_ShouldRemoveImage_WhenImageExists()
        {
            var product = CreateTestProduct();
            var imageUrl = "http://example.com/image1.jpg";
            var images = new List<Image> { Image.Create(imageUrl, "alt1") };
            product.AddRelatedImagesOrThrow(images);
            product.RemoveRelatedImageOrThrow(imageUrl);
            product.RelatedImages.Should().BeEmpty();
        }

        [Fact]
        public void EditRelatedImageOrThrow_ShouldEditImage_WhenImageExists()
        {
            var product = CreateTestProduct();
            var oldUrl = "http://example.com/image1.jpg";
            var newUrl = "http://example.com/image2.jpg";
            var images = new List<Image> { Image.Create(oldUrl, "alt1") };
            product.AddRelatedImagesOrThrow(images);
            product.EditRelatedImageOrThrow(oldUrl, newUrl, "new alt");
            product.RelatedImages.First().Url.Should().Be(newUrl);
        }

        [Fact]
        public void ReplaceMainImageOrThrow_ShouldReplaceMainImage()
        {
            var product = CreateTestProduct();
            var newUrl = "http://example.com/new_main.jpg";
            product.ReplaceMainImageOrThrow(newUrl, "new alt");
            product.MainImage.Url.Should().Be(newUrl);
        }

        // Feature Tests
        [Fact]
        public void AddFeaturesOrThrow_ShouldAddFeatures_WhenNamesAreUnique()
        {
            var product = CreateTestProduct();
            var features = new List<Feature> { Feature.Create("Color", "Blue") };
            product.AddFeaturesOrThrow(features);
            product.Features.Should().HaveCount(1);
        }

        [Fact]
        public void AddFeaturesOrThrow_ShouldThrowInvalidOperationException_WhenNameIsNotUnique()
        {
            var product = CreateTestProduct();
            var features = new List<Feature> { Feature.Create("Color", "Blue") };
            product.AddFeaturesOrThrow(features);
            Action act = () => product.AddFeaturesOrThrow(features);
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void RemoveFeatureOrThrow_ShouldRemoveFeature_WhenFeatureExists()
        {
            var product = CreateTestProduct();
            var featureName = "Color";
            var features = new List<Feature> { Feature.Create(featureName, "Blue") };
            product.AddFeaturesOrThrow(features);
            product.RemoveFeatureOrThrow(featureName);
            product.Features.Should().BeEmpty();
        }

        [Fact]
        public void EditFeatureOrThrow_ShouldEditFeature_WhenFeatureExists()
        {
            var product = CreateTestProduct();
            var oldName = "Color";
            var newName = "Shade";
            var features = new List<Feature> { Feature.Create(oldName, "Blue") };
            product.AddFeaturesOrThrow(features);
            product.EditFeatureOrThrow(oldName, newName, "Dark Blue");
            product.Features.First().Name.Should().Be(newName);
        }

        // Tag Tests
        [Fact]
        public void AddTagOrThrow_ShouldAddTag_WhenTagIsNew()
        {
            var product = CreateTestProduct();
            var tag = Tag.Create("New Tag");
            product.AddTagOrThrow(tag);
            product.Tags.Should().HaveCount(1);
        }

        [Fact]
        public void AddTagOrThrow_ShouldThrowInvalidOperationException_WhenTagExists()
        {
            var product = CreateTestProduct();
            var tag = Tag.Create("New Tag");
            product.AddTagOrThrow(tag);
            Action act = () => product.AddTagOrThrow(tag);
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void RemoveTagOrThrow_ShouldRemoveTag_WhenTagExists()
        {
            var product = CreateTestProduct();
            var tag = Tag.Create("New Tag");
            product.AddTagOrThrow(tag);
            product.RemoveTagOrThrow(tag);
            product.Tags.Should().BeEmpty();
        }
    }
}
