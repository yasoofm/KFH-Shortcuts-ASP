using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FrontKFHShortcuts.Models.Request;

namespace FrontKFHShortcuts.Controllers
{
    public class RequestController : Controller
    {
        private static List<RequestResponse> requests = new List<RequestResponse>
        {
            new RequestResponse
            {
                Id = 1,
                EmployeeId = 1,
                EmployeeName = "NAME NAME",
                ClientNumber = "+965 XXXXXXXX",
                ClientName = "NAME NAME",
                ProductId = 1,
                ProductTitle = "Organic Cream",
                NumberOfPoints = 1234
            }
            // Add more sample data as needed
        };

        public IActionResult Index()
        {
            return View(requests);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RequestResponse request)
        {
            if (ModelState.IsValid)
            {
                request.Id = requests.Count > 0 ? requests[^1].Id + 1 : 1;
                requests.Add(request);
                return RedirectToAction("Index");
            }
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = requests.FirstOrDefault(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        [HttpPost]
        public IActionResult Edit(RequestResponse request)
        {
            if (ModelState.IsValid)
            {
                var existingRequest = requests.FirstOrDefault(r => r.Id == request.Id);
                if (existingRequest == null)
                {
                    return NotFound();
                }
                existingRequest.EmployeeId = request.EmployeeId;
                existingRequest.EmployeeName = request.EmployeeName;
                existingRequest.ClientName = request.ClientName;
                existingRequest.ClientNumber = request.ClientNumber;
                existingRequest.ProductId = request.ProductId;
                existingRequest.ProductTitle = request.ProductTitle;
                existingRequest.NumberOfPoints = request.NumberOfPoints;

                return RedirectToAction("Index");
            }
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var request = requests.FirstOrDefault(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var request = requests.FirstOrDefault(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }
            requests.Remove(request);
            return RedirectToAction("Index");
        }
    }
}
