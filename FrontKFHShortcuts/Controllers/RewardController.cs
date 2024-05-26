using Microsoft.AspNetCore.Mvc;
using FrontKFHShortcuts.Controllers;
using System.Collections.Generic;
using System.Linq;
using FrontKFHShortcuts.Models.Reward;
using System.Reflection.Metadata.Ecma335;

namespace FrontKFHShortcuts.Controllers
{
    public class RewardController : Controller
    {
        private static List<RewardResponse> rewards = new List<RewardResponse>
        {
            new RewardResponse { Id = 1, Title = "Organic Cream", RequieredPoints = 1234, Usage = 5, DueDate = DateTime.Now.AddMonths(1) },
            new RewardResponse { Id = 2, Title = "Home ", RequieredPoints = 14, Usage = 2, DueDate = DateTime.Now.AddMonths(1) }
,
            // Add more rewards as needed
        };

        public IActionResult Index()
        {
            return View(rewards);
        }

        // GET: Reward/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Reward/Create
        [HttpPost]
        public IActionResult Create(AddRewardRequest request)
        {
            if (ModelState.IsValid)
            {
                var newReward = new RewardResponse
                {
                    Id = rewards.Count + 1,
                    Title = request.Title,
                    RequieredPoints = request.RequieredPoints,
                    Usage = request.Usage,
                    DueDate = request.DueDate
                };

                rewards.Add(newReward);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public IActionResult EditReward(int id)
        {
            var reward = rewards.FirstOrDefault(r => r.Id == id);
            if (reward == null)
            {

                return NotFound();
            }
            var request = new AddRewardRequest
            {
                Title = reward.Title,
                RequieredPoints = reward.RequieredPoints,
                Usage = reward.Usage,
                DueDate = reward.DueDate
            };
            return View(request);
        }

        [HttpPost]
        public IActionResult EditReward(int id, AddRewardRequest request) {

            if (ModelState.IsValid)
            {
                var reward = rewards.FirstOrDefault(reward => reward.Id == id);
                if (reward != null)
                {

                    reward.Title = request.Title;
                    reward.RequieredPoints = request.RequieredPoints;
                    reward.Usage = request.Usage;
                    reward.DueDate = request.DueDate;

                    return RedirectToAction(nameof(Index));

                }
                return NotFound();
            }

            return View(request);
         
        
        }

        public IActionResult DeleteReward(int id) {

            var reward = rewards.FirstOrDefault(r => r.Id == id);
            if (reward != null) {

                rewards.Remove(reward);
                return RedirectToAction(nameof(Index));

            }

            return NotFound();
        }
    }
}