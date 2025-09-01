using CMCS_POE_ST10152431_PROG6212_Part1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;

namespace CMCS_POE_ST10152431_PROG6212_Part1.Controllers
{
    public class MunicipalController : Controller
    {
        [HttpGet]
        public IActionResult MainMenu()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ReportIssue()
        {
            var model = new ReportIssueViewModel
            {
                Categories = ReportIssueViewModel.GetDefaultCategories(),
                CurrentPoints = GetCurrentPoints()
            };
            ViewBag.PulseDisabled = IsPulseSubmittedThisWeek();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReportIssue(ReportIssueViewModel model)
        {
            model.Categories = ReportIssueViewModel.GetDefaultCategories();
            if (!ModelState.IsValid)
            {
                model.CurrentPoints = GetCurrentPoints();
                return View(model);
            }

            // Simulate generating a reference number
            var referenceNumber = $"MS-{DateTime.UtcNow:yyyyMMddHHmmss}";

            // Award +20 points on successful report
            AddPoints(20);

            TempData["SuccessMessage"] = $"Issue submitted successfully. Reference: {referenceNumber}";
            return RedirectToAction(nameof(MainMenu));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PulseSubmit(string pulseOption)
        {
            if (string.IsNullOrWhiteSpace(pulseOption))
            {
                TempData["PulseMessage"] = "Please select an option.";
                return RedirectToAction(nameof(ReportIssue));
            }

            // Only allow once per week
            if (IsPulseSubmittedThisWeek())
            {
                TempData["PulseMessage"] = "You have already submitted your pulse this week.";
                return RedirectToAction(nameof(ReportIssue));
            }

            var currentWeekKey = ISOWeek.GetYear(DateTime.UtcNow).ToString() + ":" + ISOWeek.GetWeekOfYear(DateTime.UtcNow).ToString();
            HttpContext.Session.SetString("LastPulseDate", currentWeekKey);
            AddPoints(5);
            TempData["PulseMessage"] = "Thanks—your ward’s priorities updated! +5 pts.";
            return RedirectToAction(nameof(ReportIssue));
        }

        private int GetCurrentPoints()
        {
            var points = HttpContext.Session.GetInt32("CivicPoints");
            return points ?? 120; // Start at 120 as per brief example
        }

        private void AddPoints(int amount)
        {
            var current = GetCurrentPoints();
            HttpContext.Session.SetInt32("CivicPoints", current + amount);
        }

        private bool IsPulseSubmittedThisWeek()
        {
            var lastPulseDate = HttpContext.Session.GetString("LastPulseDate");
            var currentWeekKey = ISOWeek.GetYear(DateTime.UtcNow).ToString() + ":" + ISOWeek.GetWeekOfYear(DateTime.UtcNow).ToString();
            return lastPulseDate == currentWeekKey;
        }
    }
}

