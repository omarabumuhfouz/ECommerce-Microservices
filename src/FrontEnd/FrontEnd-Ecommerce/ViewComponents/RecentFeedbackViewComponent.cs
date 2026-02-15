using FrontEnd_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

    public class RecentFeedbackViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var feedbacks = new List<FeedbackViewModel>
            {
                new FeedbackViewModel { Customer="John Smith", Stars=4, Comment="The product was good but delivery was delayed by 2 days. Otherwise happy with the purchase.", OrderId="#ORD-7842", Date="October 15, 2023" },
                new FeedbackViewModel { Customer="Emma Johnson", Stars=5, Comment="Excellent product quality and fast shipping. Will definitely shop again from this store!", OrderId="#ORD-7841", Date="October 15, 2023" }
            };

            return View(feedbacks);
        }
    }

    