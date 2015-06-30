using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EventManagementSystem.Models;
using EventManagementSystem.Reporting.CommonObjects;
using EventManagementSystem.Reporting.Reports;
using Microsoft.Practices.ObjectBuilder2;
using Telerik.Reporting.Processing;
using Report = Telerik.Reporting.Report;

namespace EventManagementSystem.Services
{
    public static class ReportingService
    {
        public static void CreateInvoiceReport(InvoiceModel invoice, string exportPath)
        {
            var bag = PrepareInvoiceReportBag(invoice);

            var report = new InvoiceReport(bag);

            ExportToPDF(report, exportPath);

            Process.Start(exportPath);
        }

        public static void CreateQuoteReport(InvoiceModel invoice, string exportPath)
        {
            var bag = PrepareInvoiceReportBag(invoice);

            var report = new Quote(bag);

            ExportToPDF(report, exportPath);

            Process.Start(exportPath);
        }

        public static void CreateConfirmationReport(InvoiceModel invoice, string exportPath)
        {
            var bag = PrepareInvoiceReportBag(invoice);

            var report = new Confirmation(bag);

            ExportToPDF(report, exportPath);

            Process.Start(exportPath);
        }

        public static void CreateFunctionSheetReport(EventModel eventModel, string exportPath)
        {
            var bag = PrerateFunctionSheetReportBag(eventModel);

            var report = new FunctionSheet(bag);

            ExportToPDF(report, exportPath);

            Process.Start(exportPath);
        }

        private static FunctionSheetReportBag PrerateFunctionSheetReportBag(EventModel eventModel)
        {
            var bag = new FunctionSheetReportBag()
            {
                EventName = eventModel.Name,
                NumberOfPeople = eventModel.Places,
                EventDate = eventModel.Date.ToString("D"),
                Time = DateTime.Now.ToString("G")
            };

            if (eventModel.EventType != null)
                bag.EventType = eventModel.EventType.Name;

            if (eventModel.PrimaryContact != null)
            {
                bag.MainContact = eventModel.PrimaryContact.ContactName;
                bag.Telephone = eventModel.PrimaryContact.Contact.Phone1;
            }

            if (!string.IsNullOrWhiteSpace(eventModel.Event.InvoiceAddress))
                bag.CorrespondenceAddress = eventModel.Event.InvoiceAddress;
            else if (eventModel.PrimaryContact != null && !string.IsNullOrWhiteSpace(eventModel.PrimaryContact.FullAddressSepareted))
                bag.CorrespondenceAddress = eventModel.PrimaryContact.FullAddressSepareted;

            // Notes
            bag.Notes = new List<FunctionSheetEventNote>(eventModel.EventNotes.ToList().Select(x => new FunctionSheetEventNote()
            {
                Date = x.EventNote.Date.ToString("d"),
                Note = x.Note,
                Author = string.Format("by {0}", x.EventNote.User.FirstName)
            }));

            // Event items
            bag.Items = new List<FunctionSheetEventItem>();

            eventModel.EventCaterings.ForEach(x => bag.Items.Add(new FunctionSheetEventItem()
            {
                Time = x.Time,
                Type = "Catering Option",
                DisplayTime = x.Time.ToString("t"),
                Note = x.EventCatering.Notes,
                Products = x.EventBookedProducts.Select(y => string.Format("{0} x {1}", y.Quantity, y.Product.Name)).ToList()
            }));

            eventModel.EventRooms.ForEach(x => bag.Items.Add(new FunctionSheetEventItem()
            {
                Time = x.StartTime,
                Type = "Room Option",
                DisplayTime = x.StartTime.ToString("t"),
                Note = x.EventRoom.Notes,
                Products = x.EventBookedProducts.Select(y => string.Format("{0} x {1}", y.Quantity, y.Product.Name)).ToList()
            }));

            eventModel.EventGolfs.ForEach(x => bag.Items.Add(new FunctionSheetEventItem()
            {
                Time = x.Time,
                Type = "Golf Option",
                DisplayTime = x.Time.ToString("t"),
                Note = x.EventGolf.Notes,
                Products = x.EventBookedProducts.Select(y => string.Format("{0} x {1}", y.Quantity, y.Product.Name)).ToList()
            }));

            eventModel.EventInvoices.ForEach(x => bag.Items.Add(new FunctionSheetEventItem()
            {
                Type = "Special Option",
                Note = x.EventInvoice.Notes,
                Products = x.EventBookedProducts.Select(y => string.Format("{0} x {1}", y.Quantity, y.Product.Name)).ToList(),
            }));

            bag.Items = bag.Items.OrderBy(x => x.Time).ToList();

            var departmentItems = eventModel.EventBookedProducts.GroupBy(x => x.Product.ProductDepartment.Department)
                .Select(x => new { Department = x.Key, Value = x.Where(y => y.EventBookedProduct.Product.ProductOption.OptionName != "Invoice").Select(y => y.TotalPrice).Sum() }).ToList();

            bag.TotalItems = departmentItems.Select(x => new { Department = x.Department, Value = x.Value.ToString("C") }).ToList();

            bag.TotalExceptVAT = departmentItems.Select(x => x.Value).Sum();
            bag.LessDepositPaid = eventModel.EventPayments.Where(x => x.IsDeposit).Sum(x => x.Amount);
            bag.BalanceToPay = bag.TotalExceptVAT - bag.LessDepositPaid;

            return bag;
        }

        private static InvoiceReportBag PrepareInvoiceReportBag(InvoiceModel invoice)
        {
            var bag = new InvoiceReportBag()
            {
                PreparedBy = new SafeUserModel(AccessService.Current.User).FirstName,
                InvoiceNumber = invoice.InvoiceNumber,
                EventName = invoice.EventModel.Name,
                EventDate = invoice.EventModel.Date.ToString("d"),
                QuoteDate = invoice.InvoiceDate.ToString("d"),
                Logo = (!string.IsNullOrWhiteSpace(Properties.Settings.Default.ClubInfoImageUrl)) ? Properties.Settings.Default.ClubInfoImageUrl : "http://www.catster.com/files/original.jpg",
                GolfClubHeaderInfo = Properties.Settings.Default.ClubInfoAddress,
                GolfClubLeftFooterInfo = Properties.Settings.Default.ClubInfoFooter,
                GolfClubRightFooterInfo = Properties.Settings.Default.ClubInfoBankAccount
            };

            if (invoice.EventModel.EventType != null)
            {
                var tokens = string.Concat(invoice.EventModel.EventType.Token1,
                invoice.EventModel.EventType.Token2,
                invoice.EventModel.EventType.Token3,
                invoice.EventModel.EventType.Token4,
                invoice.EventModel.EventType.Token5);

                bag.CorrespondenceTokens = tokens;
            }

            if (!string.IsNullOrWhiteSpace(invoice.EventModel.Event.InvoiceAddress))
                bag.ContactAddress = invoice.EventModel.Event.InvoiceAddress;
            else if (invoice.EventModel.PrimaryContact != null && !string.IsNullOrWhiteSpace(invoice.EventModel.PrimaryContact.FullAddressSepareted))
                bag.ContactAddress = invoice.EventModel.PrimaryContact.FullAddressSepareted;

            bag.InvoiceProductItems = new List<InvoiceProductItem>();

            // Charges which owners don't have ShowInInvoice flag.
            var bannedCharges = new List<EventChargeModel>();

            invoice.EventModel.EventCaterings.Where(x => !x.EventCatering.ShowInInvoice).ForEach(x => bannedCharges.AddRange(x.EventBookedProducts.Select(y => y.EventCharge)));
            invoice.EventModel.EventRooms.Where(x => !x.EventRoom.ShowInInvoice).ForEach(x => bannedCharges.AddRange(x.EventBookedProducts.Select(y => y.EventCharge)));
            invoice.EventModel.EventGolfs.Where(x => !x.EventGolf.ShowInInvoice).ForEach(x => bannedCharges.AddRange(x.EventBookedProducts.Select(y => y.EventCharge)));
            invoice.EventModel.EventInvoices.Where(x => !x.EventInvoice.ShowInInvoice).ForEach(x => bannedCharges.AddRange(x.EventBookedProducts.Select(y => y.EventCharge)));

            // Convert charges to InvoiceProductItems, but with checking banned charges
            bag.InvoiceProductItems.AddRange(invoice.EventModel.EventCharges.Where(x => !bannedCharges.Select(y => y.EventCharge.ID).Contains(x.EventCharge.ID)).Select(x => new InvoiceProductItem()
            {
                Quantity = x.Quantity,
                Description = x.Product.Name,
                Price = x.Price,
                TotalPrice = x.TotalPrice,
            }));

            bag.TotalWithVAT = bag.InvoiceProductItems.Sum(x => x.TotalPrice);
            bag.TotalExceptVAT = bag.TotalWithVAT / 120 * 100;
            bag.VAT = bag.TotalWithVAT - bag.TotalExceptVAT;
            bag.LessDepositPaid = invoice.EventModel.EventPayments.Where(x => x.IsDeposit).Sum(x => x.Amount);
            bag.BalanceToPay = bag.TotalWithVAT - bag.LessDepositPaid;

            invoice.InnerInvoice.Amount = bag.TotalWithVAT;

            return bag;
        }

        private static void ExportToPDF(Report report, string exportPath)
        {
            var reportProcessor = new ReportProcessor();
            var instanceReportSource = new Telerik.Reporting.InstanceReportSource();
            instanceReportSource.ReportDocument = report;
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

            using (var fs = new FileStream(exportPath, FileMode.Create))
            {
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            }
        }
    }
}