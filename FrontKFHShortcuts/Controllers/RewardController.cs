using FrontKFHShortcuts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using FrontKFHShortcuts.Models.Reward;
using System.Reflection.Metadata.Ecma335;

namespace FrontKFHShortcuts.Controllers
{
    public class RewardController : Controller
    {

        private readonly GlobalAppState MyState;

        public RewardController(GlobalAppState state)
        {
            MyState = state;
        }

        // GET: Reward
        public async Task<IActionResult> Index()
        {
            var client = MyState.createClient();
            var response = await client.GetAsync("Admin/GetReward");
            if (response.IsSuccessStatusCode)
            {
                var rewards = await response.Content.ReadFromJsonAsync<List<RewardResponse>>();
                return View(rewards);
            }
            return View(null);
        }

        // GET: Reward/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Reward/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RewardRequest reward)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = MyState.createClient();
                    var response = await client.PostAsJsonAsync("Admin/AddReward", reward);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View(reward);
            }
            catch
            {
                return View(reward);
            }
        }

        // GET: Reward/Edit/5
        public async Task<IActionResult> Edit(RewardResponse reward)
        {
            var rewardRequest = new RewardRequest
            {
                Title = reward.Title,
                RequiredPoints = reward.RequiredPoints,
                Usages = reward.Usages,
                DueDate = reward.DueDate
            };
            ViewBag.RewardId = reward.Id; // Store the ID to use in the form
            return View(rewardRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RewardRequest reward)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = MyState.createClient();
                    var response = await client.PatchAsJsonAsync($"Admin/EditReward?Id={id}", reward);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Reward/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(RewardResponse reward)
        {
            return View(reward);
        }

        // POST: Reward/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var client = MyState.createClient();
                var response = await client.DeleteAsync($"Admin/RemoveReward?Id={id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }
    }
}