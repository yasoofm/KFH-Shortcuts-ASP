﻿namespace FrontKFHShortcuts.Models.Request
{
    public class RequestResponse
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ClientNumber { get; set; }
        public string ClientName { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int NumberOfPoints { get; set; }
        public DateTime RequestDate { get; set; } // Ensure this property exists
    }
}
