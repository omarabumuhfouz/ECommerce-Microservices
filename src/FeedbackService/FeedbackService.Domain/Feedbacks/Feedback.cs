using FeedbackService.Domain.Errors;
using MediatR;
using SharedKernel.Primitives.Result;
using SharedKernel.Shared;

namespace FeedbackService.Domain.Feedbacks;

public record Feedback : AggregateRoot
{
    private Feedback() { }
    private Feedback(
        Guid id,
        Guid customerId,
        Guid productId,
        string customerName,
        string productName,
        Rating rating,
        Comment comment
    ) : base(id)
    {
        CustomerId = customerId;
        ProductId = productId;
        CustomerName = customerName;
        ProductName = productName;
        Rating = rating;
        Comment = comment;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Rating Rating { get; private set; }
    public Comment Comment { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }


    public static Result<Feedback> Create(
        Guid customerId,
        Guid productId,
        string customerName,
        string productName,
        int ratingValue,
        string commentValue)
    {
        if (string.IsNullOrWhiteSpace(customerName)) return DomainErrors.Customer.NameRequired;

        if (string.IsNullOrWhiteSpace(productName)) return DomainErrors.Product.NameRequired;

        var ratingResult = Rating.Create(ratingValue);
        if (ratingResult.IsFailure) return ratingResult.TopError;

        var commentResult = Comment.Create(commentValue);
        if (commentResult.IsFailure) return commentResult.TopError;

        return new Feedback(
            Guid.NewGuid(),
            customerId,
            productId,
            customerName,
            productName,
            ratingResult.Value,
            commentResult.Value
        );
    }

    public Result<Unit> Update(int newRating, string? newComment)
    {
        var ratingResult = Rating.Create(newRating);
        if (ratingResult.IsFailure) return ratingResult.TopError;

        Comment? commentObj = null;
        if (!string.IsNullOrWhiteSpace(newComment))
        {
            var commentResult = Comment.Create(newComment);
            if (commentResult.IsFailure) return commentResult.TopError;

            commentObj = commentResult.Value;
        }

        Rating = ratingResult.Value;
        Comment = commentObj;
        UpdatedAt = DateTime.UtcNow;

        return Unit.Value;
    }

    public void SyncCustomerName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName) || CustomerName == newName) return;

        CustomerName = newName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SyncProductName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName) || CustomerName == newName) return;

        ProductName = newName;
        UpdatedAt = DateTime.UtcNow;
    }

}
