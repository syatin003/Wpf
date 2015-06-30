using System;
using System.Collections.Generic;
using System.Linq;
using EventManagementSystem.CommonObjects.Comparers;
using EventManagementSystem.Models;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Enums.Membership;

namespace EventManagementSystem.Services
{
    public class LoggingService
    {
        public static List<EventUpdate> FindDifference(EventModel currentEvent, EventModel nextEvent, bool IsNewEvent = false)
        {
            //var differences = new List<string>();
            var eventUpdates = new List<EventUpdate>();
            try
            {
                if (!IsNewEvent)
                {
                    // Name
                    if (currentEvent.Name != nextEvent.Name)
                    {
                        //differences.Add(string.Format("Edited: Name changed from {0} to {1}", currentEvent.Name, nextEvent.Name));
                        var message = string.Format("Edited: Name changed from {0} to {1}", currentEvent.Name, nextEvent.Name);
                        var update = ProcessUpdate(nextEvent, message, currentEvent.Name, nextEvent.Name, nextEvent.Event.ID, "Event", "Name", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }
                    // Date
                    if (currentEvent.Date != nextEvent.Date)
                    {
                        //differences.Add(string.Format("Edited: Date changed from {0} to {1}", currentEvent.Date.ToString("d"), nextEvent.Date.ToString("d")));
                        var message = string.Format("Edited: Date changed from {0} to {1}", currentEvent.Date.ToString("d"), nextEvent.Date.ToString("d"));
                        var update = ProcessUpdate(nextEvent, message, currentEvent.Date.ToString("d"), nextEvent.Date.ToString("d"), nextEvent.Event.ID, "Event", "Date", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    // Places
                    if (currentEvent.Places != nextEvent.Places)
                    {
                        //differences.Add(string.Format("Edited: Number of people changed from {0} to {1}", currentEvent.Places, nextEvent.Places));
                        var message = string.Format("Edited: Number of people changed from {0} to {1}", currentEvent.Places, nextEvent.Places);
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentEvent.Places), Convert.ToString(nextEvent.Places), nextEvent.Event.ID, "Event", "Number of people", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    // Type
                    if (currentEvent.EventType.ID != nextEvent.EventType.ID)
                    {
                        //differences.Add(string.Format("Edited: Type changed from {0} to {1}", currentEvent.EventType.Name, nextEvent.EventType.Name));
                        var message = string.Format("Edited: Type changed from {0} to {1}", currentEvent.EventType.Name, nextEvent.EventType.Name);
                        var update = ProcessUpdate(nextEvent, message, currentEvent.EventType.Name, nextEvent.EventType.Name, nextEvent.Event.ID, "Event", "Type", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    // Status
                    if (currentEvent.EventStatus.ID != nextEvent.EventStatus.ID)
                    {
                        //differences.Add(string.Format("Edited: Status changed from {0} to {1}", currentEvent.EventStatus.Name, nextEvent.EventStatus.Name));
                        var message = string.Format("Edited: Status changed from {0} to {1}", currentEvent.EventStatus.Name, nextEvent.EventStatus.Name);
                        var update = ProcessUpdate(nextEvent, message, currentEvent.EventStatus.Name, nextEvent.EventStatus.Name, nextEvent.Event.ID, "Event", "Status", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    // Contact
                    if (nextEvent.PrimaryContact != null && currentEvent.PrimaryContact == null)
                    {
                        //differences.Add(string.Format("Added: A new contact was associated with event"));
                        var message = string.Format("Added: A new contact was associated with event");
                        var update = ProcessUpdate(nextEvent, message, null, nextEvent.PrimaryContact.Contact.FirstName, nextEvent.Event.ID, "Event", "Primary Contact", UpdateAction.Added);
                        eventUpdates.Add(update);
                    }
                    else if (nextEvent.PrimaryContact == null && currentEvent.PrimaryContact != null)
                    {
                        //differences.Add(string.Format("Removed: A primary contact was removed"));
                        var message = string.Format("Removed: A primary contact was removed");
                        var update = ProcessUpdate(nextEvent, message, currentEvent.PrimaryContact.Contact.FirstName, null, nextEvent.Event.ID, "Event", "Primary Contact", UpdateAction.Removed);
                        eventUpdates.Add(update);
                    }
                    else if (nextEvent.PrimaryContact != null && currentEvent.PrimaryContact != null)
                        if (currentEvent.PrimaryContact.Contact.ID != nextEvent.PrimaryContact.Contact.ID)
                        {
                            //differences.Add(string.Format("Edited: Primary contact changed from {0} to {1}", currentEvent.PrimaryContact.Contact.FirstName, nextEvent.PrimaryContact.Contact.FirstName));
                            var message = string.Format("Edited: Primary contact changed from {0} to {1}", currentEvent.PrimaryContact.Contact.FirstName, nextEvent.PrimaryContact.Contact.FirstName);
                            var update = ProcessUpdate(nextEvent, message, currentEvent.PrimaryContact.Contact.FirstName, nextEvent.PrimaryContact.Contact.FirstName, nextEvent.Event.ID, "Event", "Primary Contact", UpdateAction.Edited);
                            eventUpdates.Add(update);
                        }

                    // Invoice Address
                    if (currentEvent.Event.InvoiceAddress != nextEvent.Event.InvoiceAddress)
                    {
                        //differences.Add(string.Format("Edited: Invoice address changed from {0} to {1}", currentEvent.Event.InvoiceAddress, nextEvent.Event.InvoiceAddress));
                        var message = string.Format("Edited: Invoice address changed from {0} to {1}", currentEvent.Event.InvoiceAddress, nextEvent.Event.InvoiceAddress);
                        var update = ProcessUpdate(nextEvent, message, currentEvent.Event.InvoiceAddress, nextEvent.Event.InvoiceAddress, nextEvent.Event.ID, "Event", "Invoice Address", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    // ShowInForwardBook
                    if (currentEvent.Event.ShowInForwardBook != nextEvent.Event.ShowInForwardBook)
                    {
                        //differences.Add(string.Format("Edited: Property Show In Forward Book was changed from {0} to {1}", currentEvent.Event.ShowInForwardBook, nextEvent.Event.ShowInForwardBook));

                        var message = string.Format("Edited: Property Show In Forward Book was changed from {0} to {1}", currentEvent.Event.ShowInForwardBook, nextEvent.Event.ShowInForwardBook);
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentEvent.Event.ShowInForwardBook), Convert.ToString(nextEvent.Event.ShowInForwardBook), nextEvent.Event.ID, "Event", "Show In Forward Book", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    // ShowOnCalendar
                    if (currentEvent.Event.ShowOnCalendar != nextEvent.Event.ShowOnCalendar)
                    {
                        //differences.Add(string.Format("Edited: Property Show On Calendar was changed from {0} to {1}", currentEvent.Event.ShowOnCalendar, nextEvent.Event.ShowOnCalendar));

                        var message = string.Format("Edited: Property Show On Calendar was changed from {0} to {1}", currentEvent.Event.ShowOnCalendar, nextEvent.Event.ShowOnCalendar);
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentEvent.Event.ShowOnCalendar), Convert.ToString(nextEvent.Event.ShowOnCalendar), nextEvent.Event.ID, "Event", "Show On Calendar", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    // UsedAsTemplate
                    if (currentEvent.Event.UsedAsTemplate != nextEvent.Event.UsedAsTemplate)
                    {
                        //differences.Add(string.Format("Edited: Property Used As Template was changed from {0} to {1}", currentEvent.Event.UsedAsTemplate, nextEvent.Event.UsedAsTemplate));
                        var message = string.Format("Edited: Property Used As Template was changed from {0} to {1}", currentEvent.Event.UsedAsTemplate, nextEvent.Event.UsedAsTemplate);
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentEvent.Event.UsedAsTemplate), Convert.ToString(nextEvent.Event.UsedAsTemplate), nextEvent.Event.ID, "Event", "Used As Template", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }
                }
                // Alternative contacts
                if (currentEvent.EventContacts.Count != nextEvent.EventContacts.Count)
                {
                    if (nextEvent.EventContacts.Count > currentEvent.EventContacts.Count)
                    {
                        var addedContacts = nextEvent.EventContacts.Except(currentEvent.EventContacts, new EventContactComparer()).ToList();
                        //differences.AddRange(addedContacts.Select(contact => string.Format("Added: Event contact {0} {1}", contact.Contact.FirstName, contact.Contact.LastName)));

                        addedContacts.ForEach(contact =>
                           {
                               var message = string.Format("Added: Event contact {0} {1}", contact.Contact.FirstName, contact.Contact.LastName);
                               var update = ProcessUpdate(nextEvent, message, null, string.Format("{0} {1}", contact.Contact.FirstName, contact.Contact.LastName), nextEvent.Event.ID, "Event", "Alternative Contact", UpdateAction.Added);
                               eventUpdates.Add(update);
                           });
                    }
                    else
                    {
                        var removedContacts = currentEvent.EventContacts.Except(nextEvent.EventContacts, new EventContactComparer()).ToList();
                        //differences.AddRange(removedContacts.Select(contact => string.Format("Removed: Event contact {0} {1}", contact.Contact.FirstName, contact.Contact.LastName)));
                        removedContacts.ForEach(contact =>
                        {
                            var message = string.Format("Removed: Event contact {0} {1}", contact.Contact.FirstName, contact.Contact.LastName);
                            var update = ProcessUpdate(nextEvent, message, string.Format("{0} {1}", contact.Contact.FirstName, contact.Contact.LastName), null, nextEvent.Event.ID, "Event", "Alternative Contact", UpdateAction.Removed);
                            eventUpdates.Add(update);
                        });
                    }
                }

                // Charges
                if (currentEvent.EventCharges.Count != nextEvent.EventCharges.Count)
                {
                    if (nextEvent.EventCharges.Count > currentEvent.EventCharges.Count)
                    {
                        var addedCharges = nextEvent.EventCharges.Except(currentEvent.EventCharges, new EventChargeComparer()).ToList();
                        //differences.AddRange(addedCharges.Select(eventChargeModel => string.Format("Added: Event charge {0:C2}", eventChargeModel.EventCharge.Price)));
                        addedCharges.ForEach(eventCharge =>
                        {
                            var message = string.Format("Added: Event charge {0:C2}", eventCharge.EventCharge.Price);
                            var update = ProcessUpdate(nextEvent, message, null, string.Format("{0:C2}", eventCharge.EventCharge.Price), eventCharge.EventCharge.ID, "Event Charge", "Charge", UpdateAction.Added);
                            eventUpdates.Add(update);
                        });
                    }
                    else
                    {
                        var removedCharges = currentEvent.EventCharges.Except(nextEvent.EventCharges, new EventChargeComparer()).ToList();
                        //differences.AddRange(removedCharges.Select(eventChargeModel => string.Format("Removed: Event charge {0:C2}", eventChargeModel.EventCharge.Price)));
                        removedCharges.ForEach(eventCharge =>
                        {
                            var message = string.Format("Removed: Event charge {0:C2}", eventCharge.EventCharge.Price);
                            var update = ProcessUpdate(nextEvent, message, string.Format("{0:C2}", eventCharge.EventCharge.Price), null, eventCharge.EventCharge.ID, "Event Charge", "Charge", UpdateAction.Removed);
                            eventUpdates.Add(update);
                        });
                    }
                }

                foreach (var newCharge in nextEvent.EventCharges)
                {
                    var currentCharge = currentEvent.EventCharges.FirstOrDefault(x => x.EventCharge.ID == newCharge.EventCharge.ID);

                    if (currentCharge == null) continue;

                    if (currentCharge.Quantity != newCharge.Quantity)
                    {
                        //differences.Add(string.Format("Edited: Charge quantity changed from {0} to {1}", currentCharge.Quantity, newCharge.Quantity));
                        var message = string.Format("Edited: Charge quantity changed from {0} to {1}", currentCharge.Quantity, newCharge.Quantity);
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentCharge.Quantity), Convert.ToString(newCharge.Quantity), newCharge.EventCharge.ID, "Event Charge", "Quantity", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentCharge.Product.ID != newCharge.Product.ID)
                    {
                        //differences.Add(string.Format("Edited: Charge product changed from {0} to {1}", currentCharge.Product.Name, newCharge.Product.Name));
                        var message = string.Format("Edited: Charge product changed from {0} to {1}", currentCharge.Product.Name, newCharge.Product.Name);
                        var update = ProcessUpdate(nextEvent, message, currentCharge.Product.Name, newCharge.Product.Name, newCharge.EventCharge.ID, "Event Charge", "Product Name", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentCharge.EventCharge.Notes != newCharge.EventCharge.Notes)
                    {
                        //differences.Add(string.Format("Edited: Charge notes was changed"));
                        var message = string.Format("Edited: Charge notes was changed");
                        var update = currentCharge.EventCharge.Notes != null ? ProcessUpdate(nextEvent, message, currentCharge.EventCharge.Notes, newCharge.EventCharge.Notes, newCharge.EventCharge.ID, "Event Charge", "Notes", UpdateAction.Edited) : ProcessUpdate(nextEvent, message, null, newCharge.EventCharge.Notes, newCharge.EventCharge.ID, "Event Charge", "Notes", UpdateAction.Added);
                        eventUpdates.Add(update);
                    }
                    if (currentCharge.IsCommited != newCharge.IsCommited)
                    {
                        //differences.Add(newCharge.IsCommited ? string.Format("Edited: Charge was commited for Product {0}", newCharge.Product.Name) : string.Format("Edited: Charge comit was reverted"));
                        var message = newCharge.IsCommited ? string.Format("Edited: Charge was commited for Product {0}", newCharge.Product.Name) : string.Format("Edited: Charge comit was reverted");
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentCharge.IsCommited), Convert.ToString(newCharge.IsCommited), newCharge.EventCharge.ID, "Event Charge", "IsCommited", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }
                }

                // Notes
                if (currentEvent.EventNotes.Count != nextEvent.EventNotes.Count)
                {
                    if (nextEvent.EventNotes.Count > currentEvent.EventNotes.Count)
                    {
                        var addedNotes = nextEvent.EventNotes.Except(currentEvent.EventNotes, new EventNoteComparer()).ToList();
                        // differences.AddRange(addedNotes.Select(eventNote => string.Format("Added: Event note {0}", eventNote.Note)));
                        addedNotes.ForEach(eventNote =>
                            {
                                var message = string.Format("Added: Event note {0}", eventNote.Note);
                                var update = ProcessUpdate(nextEvent, message, null, eventNote.Note, eventNote.EventNote.ID, "Event Note", "Notes", UpdateAction.Added);
                                eventUpdates.Add(update);
                            });
                    }
                    else
                    {
                        var removedNotes = currentEvent.EventNotes.Except(nextEvent.EventNotes, new EventNoteComparer()).ToList();
                        //differences.AddRange(removedNotes.Select(eventNote => string.Format("Removed: Event note {0}", eventNote.Note)));
                        removedNotes.ForEach(eventNote =>
                        {
                            var message = string.Format("Removed: Event note {0}", eventNote.Note);
                            var update = ProcessUpdate(nextEvent, message, eventNote.Note, null, eventNote.EventNote.ID, "Event Note", "Notes", UpdateAction.Removed);
                            eventUpdates.Add(update);
                        });
                    }
                }

                foreach (var newNote in nextEvent.EventNotes)
                {
                    var currentNote = currentEvent.EventNotes.FirstOrDefault(x => x.EventNote.ID == newNote.EventNote.ID);

                    if (currentNote == null) continue;

                    if (currentNote.Note != newNote.Note)
                    {
                        //differences.Add(string.Format("Edited: Note was changed from {0} to {1}", currentNote.Note, newNote.Note));
                        var message = string.Format("Edited: Note was changed from {0} to {1}", currentNote.Note, newNote.Note);
                        var update = currentNote.Note != null ? ProcessUpdate(nextEvent, message, currentNote.Note, newNote.Note, newNote.EventNote.ID, "Event Note", "Notes", UpdateAction.Edited) : ProcessUpdate(nextEvent, message, null, newNote.Note, newNote.EventNote.ID, "Event Note", "Notes", UpdateAction.Added);
                        eventUpdates.Add(update);
                    }

                    if (currentNote.NoteType.ID != newNote.NoteType.ID)
                    {
                        //differences.Add(string.Format("Edited: Note type was changed from {0} to {1}", currentNote.NoteType.Type, newNote.NoteType.Type));
                        var message = string.Format("Edited: Note type was changed from {0} to {1}", currentNote.NoteType.Type, newNote.NoteType.Type);
                        var update = ProcessUpdate(nextEvent, message, currentNote.NoteType.Type, newNote.NoteType.Type, newNote.EventNote.ID, "Event Note", "Type", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }
                }

                // Payments
                if (currentEvent.EventPayments.Count != nextEvent.EventPayments.Count)
                {
                    if (nextEvent.EventPayments.Count > currentEvent.EventPayments.Count)
                    {
                        var addedPayments = nextEvent.EventPayments.Except(currentEvent.EventPayments, new EventPaymentComparer()).ToList();
                        //differences.AddRange(addedPayments.Select(eventPayment => string.Format("Added: Payment {0:C0}", eventPayment.Amount)));
                        addedPayments.ForEach(eventPayment =>
                        {
                            var message = string.Format("Added: Payment {0:C0}", eventPayment.Amount);
                            var update = ProcessUpdate(nextEvent, message, null, string.Format("{0:C0}", eventPayment.Amount), eventPayment.EventPayment.ID, "Event Payment", "Amount", UpdateAction.Added);
                            eventUpdates.Add(update);
                        });
                    }
                    else
                    {
                        var removedPayments = currentEvent.EventPayments.Except(nextEvent.EventPayments, new EventPaymentComparer()).ToList();
                        // differences.AddRange(removedPayments.Select(eventPayment => string.Format("Removed: Payment {0:C0}", eventPayment.Amount)));
                        removedPayments.ForEach(eventPayment =>
                        {
                            var message = string.Format("Removed: Payment {0:C0}", eventPayment.Amount);
                            var update = ProcessUpdate(nextEvent, message, string.Format("{0:C0}", eventPayment.Amount), null, eventPayment.EventPayment.ID, "Event Payment", "Amount", UpdateAction.Removed);
                            eventUpdates.Add(update);
                        });
                    }
                }

                foreach (var newPayment in nextEvent.EventPayments)
                {
                    var currentPayment = currentEvent.EventPayments.FirstOrDefault(x => x.EventPayment.ID == newPayment.EventPayment.ID);

                    if (currentPayment == null) continue;

                    if (currentPayment.Date != newPayment.Date)
                    {
                        // differences.Add(string.Format("Edited: Payment date was changed from {0} to {1}", currentPayment.Date.ToString("d"), newPayment.Date.ToString("d")));
                        var message = string.Format("Edited: Payment date was changed from {0} to {1}", currentPayment.Date.ToString("d"), newPayment.Date.ToString("d"));
                        var update = ProcessUpdate(nextEvent, message, currentPayment.Date.ToString("d"), newPayment.Date.ToString("d"), newPayment.EventPayment.ID, "Event Payment", "Date", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentPayment.Amount != newPayment.Amount)
                    {
                        // differences.Add(string.Format("Edited: Payment amount was changed from {0:C2} to {1:C2}", currentPayment.Amount, newPayment.Amount));
                        var message = string.Format("Edited: Payment amount was changed from {0:C2} to {1:C2}", currentPayment.Amount, newPayment.Amount);
                        var update = ProcessUpdate(nextEvent, message, string.Format("{0:C2}", currentPayment.Amount), string.Format("{0:C2}", newPayment.Amount), newPayment.EventPayment.ID, "Event Payment", "Amount", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentPayment.User.ID != newPayment.User.ID)
                    {
                        // differences.Add(string.Format("Edited: Payment user was changed from {0} {1} to {2} {3}", currentPayment.User.FirstName, currentPayment.User.LastName, newPayment.User.FirstName, newPayment.User.LastName));
                        var message = string.Format("Edited: Payment user was changed from {0} {1} to {2} {3}", currentPayment.User.FirstName, currentPayment.User.LastName, newPayment.User.FirstName, newPayment.User.LastName);
                        var update = ProcessUpdate(nextEvent, message, string.Format("{0} {1}", currentPayment.User.FirstName, currentPayment.User.LastName), string.Format("{0} {1}", newPayment.User.FirstName, newPayment.User.LastName), newPayment.EventPayment.ID, "Event Payment", "User", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentPayment.PaymentMethod.ID != newPayment.PaymentMethod.ID)
                    {
                        // differences.Add(string.Format("Edited: Payment method was changed from {0} to {1}", currentPayment.PaymentMethod.Method, newPayment.PaymentMethod.Method));
                        var message = string.Format("Edited: Payment method was changed from {0} to {1}", currentPayment.PaymentMethod.Method, newPayment.PaymentMethod.Method);
                        var update = ProcessUpdate(nextEvent, message, currentPayment.PaymentMethod.Method, newPayment.PaymentMethod.Method, newPayment.EventPayment.ID, "Event Payment", "Payment Method", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentPayment.IsDeposit != newPayment.IsDeposit)
                    {
                        //  differences.Add(newPayment.IsDeposit ? string.Format("Edited: Payment type was changed to deposit") : string.Format("Edited: Payment type was changed to single"));
                        var message = string.Format(newPayment.IsDeposit ? string.Format("Edited: Payment type was changed to deposit") : string.Format("Edited: Payment type was changed to single"));
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentPayment.IsDeposit), Convert.ToString(newPayment.IsDeposit), newPayment.EventPayment.ID, "Event Payment", "Type", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentPayment.Notes != newPayment.Notes)
                    {
                        // differences.Add(string.Format("Edited: Payment note was changed"));
                        var message = string.Format("Edited: Payment note was changed");
                        var update = currentPayment.Notes != null ? ProcessUpdate(nextEvent, message, currentPayment.Notes, newPayment.Notes, newPayment.EventPayment.ID, "Event Payment", "Notes", UpdateAction.Edited) : ProcessUpdate(nextEvent, message, null, newPayment.Notes, newPayment.EventPayment.ID, "Event Payment", "Notes", UpdateAction.Added);
                        eventUpdates.Add(update);
                    }
                }

                // Caterings
                if (currentEvent.EventCaterings.Count != nextEvent.EventCaterings.Count)
                {
                    if (nextEvent.EventCaterings.Count > currentEvent.EventCaterings.Count)
                    {
                        var addedCaterings = nextEvent.EventCaterings.Except(currentEvent.EventCaterings, new EventCateringComparer()).ToList();
                        // differences.AddRange(addedCaterings.Select(eventCatering => string.Format("Added: Catering from {0} to {1}", eventCatering.StartTimeEx, eventCatering.EndTimeEx)));
                        addedCaterings.ForEach(eventCatering =>
                            {
                                var message = string.Format("Added: Catering from {0} to {1}", eventCatering.StartTimeEx, eventCatering.EndTimeEx);
                                var update = ProcessUpdate(nextEvent, message, null, string.Format("{0} to {1}", eventCatering.StartTimeEx, eventCatering.EndTimeEx), eventCatering.EventCatering.ID, "Event Catering", "Catering", UpdateAction.Added);
                                eventUpdates.Add(update);
                            });
                    }
                    else
                    {
                        var removedCaterings = currentEvent.EventCaterings.Except(nextEvent.EventCaterings, new EventCateringComparer()).ToList();
                        // differences.AddRange(removedCaterings.Select(eventCatering => string.Format("Removed: Catering from {0} to {1}", eventCatering.StartTimeEx, eventCatering.EndTimeEx)));
                        removedCaterings.ForEach(eventCatering =>
                               {
                                   var message = string.Format("Removed: Catering from {0} to {1}", eventCatering.StartTimeEx, eventCatering.EndTimeEx);
                                   var update = ProcessUpdate(nextEvent, message, string.Format("{0} to {1}", eventCatering.StartTimeEx, eventCatering.EndTimeEx), null, eventCatering.EventCatering.ID, "Event Catering", "Catering", UpdateAction.Removed);
                                   eventUpdates.Add(update);
                               });
                    }
                }

                foreach (var newCatering in nextEvent.EventCaterings)
                {
                    var currentCatering = currentEvent.EventCaterings.FirstOrDefault(x => x.EventCatering.ID == newCatering.EventCatering.ID);

                    if (currentCatering == null) continue;

                    if (currentCatering.Time != newCatering.Time)
                    {
                        //differences.Add(string.Format("Edited: Catering time was changed from {0} to {1}", currentCatering.StartTimeEx.ToString("t"), newCatering.StartTimeEx.ToString("t")));
                        var message = string.Format("Edited: Catering time was changed from {0} to {1}", currentCatering.StartTimeEx.ToString("t"), newCatering.StartTimeEx.ToString("t"));
                        var update = ProcessUpdate(nextEvent, message, currentCatering.StartTimeEx.ToString("t"), newCatering.StartTimeEx.ToString("t"), newCatering.EventCatering.ID, "Event Catering", "Time", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentCatering.Room.ID != newCatering.Room.ID)
                    {
                        //  differences.Add(string.Format("Edited: Catering room was changed from {0} to {1}", currentCatering.Room.Name, newCatering.Room.Name));
                        var message = string.Format("Edited: Catering room was changed from {0} to {1}", currentCatering.Room.Name, newCatering.Room.Name);
                        var update = ProcessUpdate(nextEvent, message, currentCatering.Room.Name, newCatering.Room.Name, newCatering.EventCatering.ID, "Event Catering", "Room", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentCatering.StartTime != newCatering.StartTime)
                    {
                        // differences.Add(string.Format("Edited: Catering start time was changed from {0} to {1}", currentCatering.StartTimeEx.ToString("t"), newCatering.StartTimeEx.ToString("t")));
                        var message = string.Format("Edited: Catering start time was changed from {0} to {1}", currentCatering.StartTimeEx.ToString("t"), newCatering.StartTimeEx.ToString("t"));
                        var update = ProcessUpdate(nextEvent, message, currentCatering.StartTimeEx.ToString("t"), newCatering.StartTimeEx.ToString("t"), newCatering.EventCatering.ID, "Event Catering", "Start Time", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentCatering.EndTime != newCatering.EndTime)
                    {
                        // differences.Add(string.Format("Edited: Catering end time was changed from {0} to {1}", currentCatering.EndTimeEx.ToString("t"), newCatering.EndTimeEx.ToString("t")));
                        var message = string.Format("Edited: Catering end time was changed from {0} to {1}", currentCatering.EndTimeEx.ToString("t"), newCatering.EndTimeEx.ToString("t"));
                        var update = ProcessUpdate(nextEvent, message, currentCatering.EndTimeEx.ToString("t"), newCatering.EndTimeEx.ToString("t"), newCatering.EventCatering.ID, "Event Catering", "End Time", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentCatering.EventCatering.Notes != newCatering.EventCatering.Notes)
                    {
                        // differences.Add(string.Format("Edited: Catering notes was changed"));
                        var message = string.Format("Edited: Catering notes was changed");
                        var update = currentCatering.EventCatering.Notes != null ? ProcessUpdate(nextEvent, message, currentCatering.EventCatering.Notes, newCatering.EventCatering.Notes, newCatering.EventCatering.ID, "Event Catering", "Notes", UpdateAction.Edited) : ProcessUpdate(nextEvent, message, null, newCatering.EventCatering.Notes, newCatering.EventCatering.ID, "Event Catering", "Notes", UpdateAction.Added);
                        eventUpdates.Add(update);
                    }
                }

                // Golfs
                if (currentEvent.EventGolfs.Count != nextEvent.EventGolfs.Count)
                {
                    if (nextEvent.EventGolfs.Count > currentEvent.EventGolfs.Count)
                    {
                        var addedGolfs = nextEvent.EventGolfs.Except(currentEvent.EventGolfs, new EventGolfComparer()).ToList();
                        // differences.AddRange(addedGolfs.Select(x => string.Format("Added: Golf option with time: {0}, tee {1}, holes {2}", x.TimeEx, x.Golf.Name, x.GolfHole.Hole)));
                        addedGolfs.ForEach(eventGolf =>
                            {
                                var message = string.Format("Added: Golf option with time: {0}, tee {1}, holes {2}", eventGolf.TimeEx, eventGolf.Golf.Name, eventGolf.GolfHole.Hole);
                                var update = ProcessUpdate(nextEvent, message, null, string.Format("time {0}, tee {1}, holes {2}", eventGolf.TimeEx, eventGolf.Golf.Name, eventGolf.GolfHole.Hole), eventGolf.EventGolf.ID, "Event Golf", "Golf", UpdateAction.Added);
                                eventUpdates.Add(update);
                            });
                    }
                    else
                    {
                        var removedGolfs = currentEvent.EventGolfs.Except(nextEvent.EventGolfs, new EventGolfComparer()).ToList();
                        // differences.AddRange(removedGolfs.Select(x => string.Format("Removed: Golf option with time: {0}, tee {1}, holes {2}", x.TimeEx, x.Golf.Name, x.GolfHole.Hole)));
                        removedGolfs.ForEach(eventGolf =>
                            {
                                var message = string.Format("Removed: Golf option with time: {0}, tee {1}, holes {2}", eventGolf.TimeEx, eventGolf.Golf.Name, eventGolf.GolfHole.Hole);
                                var update = ProcessUpdate(nextEvent, message, string.Format("time {0}, tee {1}, holes {2}", eventGolf.TimeEx, eventGolf.Golf.Name, eventGolf.GolfHole.Hole), null, eventGolf.EventGolf.ID, "Event Golf", "Golf", UpdateAction.Removed);
                                eventUpdates.Add(update);
                            });
                    }
                }

                foreach (var newGolf in nextEvent.EventGolfs)
                {
                    var currentGolf = currentEvent.EventGolfs.FirstOrDefault(x => x.EventGolf.ID == newGolf.EventGolf.ID);

                    if (currentGolf == null) continue;

                    if (currentGolf.Time != newGolf.Time)
                    {
                        //  differences.Add(string.Format("Edited: Golf time was changed from {0} to {1}", currentGolf.TimeEx.ToString("t"), newGolf.TimeEx.ToString("t")));
                        var message = string.Format("Edited: Golf time was changed from {0} to {1}", currentGolf.TimeEx.ToString("t"), newGolf.TimeEx.ToString("t"));
                        var update = ProcessUpdate(nextEvent, message, currentGolf.TimeEx.ToString("t"), newGolf.TimeEx.ToString("t"), newGolf.EventGolf.ID, "Event Golf", "Time", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentGolf.Golf.ID != newGolf.Golf.ID)
                    {
                        // differences.Add(string.Format("Edited: Golf tee was changed from {0} to {1}", currentGolf.Golf.Name, newGolf.Golf.Name));
                        var message = string.Format("Edited: Golf tee was changed from {0} to {1}", currentGolf.Golf.Name, newGolf.Golf.Name);
                        var update = ProcessUpdate(nextEvent, message, currentGolf.Golf.Name, newGolf.Golf.Name, newGolf.EventGolf.ID, "Event Golf", "Tee", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentGolf.GolfHole.ID != newGolf.GolfHole.ID)
                    {
                        //  differences.Add(string.Format("Edited: Golf holes was changed from {0} to {1}", currentGolf.GolfHole.Hole, newGolf.GolfHole.Hole));
                        var message = string.Format("Edited: Golf holes was changed from {0} to {1}", currentGolf.GolfHole.Hole, newGolf.GolfHole.Hole);
                        var update = ProcessUpdate(nextEvent, message, currentGolf.GolfHole.Hole, newGolf.GolfHole.Hole, newGolf.EventGolf.ID, "Event Golf", "Holes", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentGolf.EventGolf.Slots != newGolf.EventGolf.Slots)
                    {
                        //  differences.Add(string.Format("Edited: Golf slots was changed from {0} to {1}", currentGolf.EventGolf.Slots, newGolf.EventGolf.Slots));
                        var message = string.Format("Edited: Golf slots was changed from {0} to {1}", currentGolf.EventGolf.Slots, newGolf.EventGolf.Slots);
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentGolf.EventGolf.Slots), Convert.ToString(newGolf.EventGolf.Slots), newGolf.EventGolf.ID, "Event Golf", "Slots", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentGolf.EventGolf.Notes != newGolf.EventGolf.Notes)
                    {
                        //  differences.Add(string.Format("Edited: Golf notes was changed"));
                        var message = string.Format("Edited: Golf notes was changed");
                        var update = currentGolf.EventGolf.Notes != null ? ProcessUpdate(nextEvent, message, currentGolf.EventGolf.Notes, newGolf.EventGolf.Notes, newGolf.EventGolf.ID, "Event Golf", "Notes", UpdateAction.Edited) : ProcessUpdate(nextEvent, message, null, newGolf.EventGolf.Notes, newGolf.EventGolf.ID, "Event Golf", "Notes", UpdateAction.Added);
                        eventUpdates.Add(update);
                    }
                }

                // Rooms
                if (currentEvent.EventRooms.Count != nextEvent.EventRooms.Count)
                {
                    if (nextEvent.EventRooms.Count > currentEvent.EventRooms.Count)
                    {
                        var addedRooms = nextEvent.EventRooms.Except(currentEvent.EventRooms, new EventRoomComparer()).ToList();
                        //differences.AddRange(addedRooms.Select(x => string.Format("Added: Room option from {0} to {1}", x.StartTimeEx, x.EndTimeEx)));
                        addedRooms.ForEach(eventRoom =>
                            {
                                var message = string.Format("Added: Room option from {0} to {1}", eventRoom.StartTimeEx, eventRoom.EndTimeEx);
                                var update = ProcessUpdate(nextEvent, message, null, string.Format("{0} to {1}", eventRoom.StartTimeEx, eventRoom.EndTimeEx), eventRoom.EventRoom.ID, "Event Room", "Room", UpdateAction.Added);
                                eventUpdates.Add(update);
                            });
                    }
                    else
                    {
                        var removedRooms = currentEvent.EventRooms.Except(nextEvent.EventRooms, new EventRoomComparer()).ToList();
                        //  differences.AddRange(removedRooms.Select(x => string.Format("Removed: Room option from {0} to {1}", x.StartTimeEx, x.EndTimeEx)));
                        removedRooms.ForEach(eventRoom =>
                        {
                            var message = string.Format("Removed: Room option from {0} to {1}", eventRoom.StartTimeEx, eventRoom.EndTimeEx);
                            var update = ProcessUpdate(nextEvent, message, string.Format("{0} to {1}", eventRoom.StartTimeEx, eventRoom.EndTimeEx), null, eventRoom.EventRoom.ID, "Event Room", "Room", UpdateAction.Removed);
                            eventUpdates.Add(update);
                        });
                    }
                }

                foreach (var newRoom in nextEvent.EventRooms)
                {
                    var currentRoom = currentEvent.EventRooms.FirstOrDefault(x => x.EventRoom.ID == newRoom.EventRoom.ID);

                    if (currentRoom == null) continue;

                    if (currentRoom.Room.ID != newRoom.Room.ID)
                    {
                        // differences.Add(string.Format("Edited: Room was changed from {0} to {1}", currentRoom.Room.Name, newRoom.Room.Name));
                        var message = string.Format("Edited: Room was changed from {0} to {1}", currentRoom.Room.Name, newRoom.Room.Name);
                        var update = ProcessUpdate(nextEvent, message, currentRoom.Room.Name, newRoom.Room.Name, newRoom.EventRoom.ID, "Event Room", "Name", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentRoom.StartTime != newRoom.StartTime)
                    {
                        // differences.Add(string.Format("Edited: Room start time was changed from {0} to {1}", currentRoom.StartTimeEx.ToString("t"), newRoom.StartTimeEx.ToString("t")));
                        var message = string.Format("Edited: Room start time was changed from {0} to {1}", currentRoom.StartTimeEx.ToString("t"), newRoom.StartTimeEx.ToString("t"));
                        var update = ProcessUpdate(nextEvent, message, currentRoom.StartTimeEx.ToString("t"), newRoom.StartTimeEx.ToString("t"), newRoom.EventRoom.ID, "Event Room", "Start Time", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }
                    if (currentRoom.EndTime != newRoom.EndTime)
                    {
                        //differences.Add(string.Format("Edited: Room end time was changed from {0} to {1}", currentRoom.EndTimeEx.ToString("t"), newRoom.EndTimeEx.ToString("t")));
                        var message = string.Format("Edited: Room end time was changed from {0} to {1}", currentRoom.EndTimeEx.ToString("t"), newRoom.EndTimeEx.ToString("t"));
                        var update = ProcessUpdate(nextEvent, message, currentRoom.StartTimeEx.ToString("t"), newRoom.StartTimeEx.ToString("t"), newRoom.EventRoom.ID, "Event Room", "End Time", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentRoom.EventRoom.Notes != newRoom.EventRoom.Notes)
                    {
                        // differences.Add(string.Format("Edited: Room notes was changed"));
                        var message = string.Format("Edited: Room notes was changed");
                        var update = currentRoom.EventRoom.Notes != null ? ProcessUpdate(nextEvent, message, currentRoom.EventRoom.Notes, newRoom.EventRoom.Notes, newRoom.EventRoom.ID, "Event Room", "Notes", UpdateAction.Edited) : ProcessUpdate(nextEvent, message, null, newRoom.EventRoom.Notes, newRoom.EventRoom.ID, "Event Room", "Notes", UpdateAction.Added);
                        eventUpdates.Add(update);
                    }
                }


                // Invoices
                if (currentEvent.EventInvoices.Count != nextEvent.EventInvoices.Count)
                {
                    if (nextEvent.EventInvoices.Count > currentEvent.EventInvoices.Count)
                    {
                        var addedInvoices = nextEvent.EventInvoices.Except(currentEvent.EventInvoices, new EventInvoiceComparer()).ToList();
                        // differences.AddRange(addedInvoices.Select(x => string.Format("Added: Invoice option with {0} products", x.EventBookedProducts.Count)));
                        addedInvoices.ForEach(eventInvoice =>
                            {
                                var message = string.Format("Added: Invoice option with {0} products", eventInvoice.EventBookedProducts.Count);
                                var update = ProcessUpdate(nextEvent, message, null, string.Format("{0} products", eventInvoice.EventBookedProducts.Count), eventInvoice.EventInvoice.ID, "Event Invoice", "Invoice", UpdateAction.Added);
                                eventUpdates.Add(update);
                            });
                    }
                    else
                    {
                        var removedInvoices = currentEvent.EventInvoices.Except(nextEvent.EventInvoices, new EventInvoiceComparer()).ToList();
                        //  differences.AddRange(removedInvoices.Select(x => string.Format("Removed: Invoice option with {0} products state:{1}", x.EventBookedProducts.Count, x.EventInvoice.EntityState)));
                        removedInvoices.ForEach(eventInvoice =>
                              {
                                  var message = string.Format("Removed: Invoice option with {0} products", eventInvoice.EventBookedProducts.Count);
                                  var update = ProcessUpdate(nextEvent, message, string.Format("{0} products", eventInvoice.EventBookedProducts.Count), null, eventInvoice.EventInvoice.ID, "Event Invoice", "Invoice", UpdateAction.Removed);
                                  eventUpdates.Add(update);
                              });
                    }
                }

                foreach (var newInvoice in nextEvent.EventInvoices)
                {
                    var currentInvoice = currentEvent.EventInvoices.FirstOrDefault(x => x.EventInvoice.ID == newInvoice.EventInvoice.ID);

                    if (currentInvoice == null) continue;

                    if (currentInvoice.EventInvoice.Notes != newInvoice.EventInvoice.Notes)
                    {
                        //   differences.Add(string.Format("Edited: Invoice notes was changed"));
                        var message = string.Format("Edited: Invoice notes was changed");
                        var update = currentInvoice.EventInvoice.Notes != null ? ProcessUpdate(nextEvent, message, currentInvoice.EventInvoice.Notes, newInvoice.EventInvoice.Notes, newInvoice.EventInvoice.ID, "Event Invoice", "Notes", UpdateAction.Edited) : ProcessUpdate(nextEvent, message, null, newInvoice.EventInvoice.Notes, newInvoice.EventInvoice.ID, "Event Invoice", "Notes", UpdateAction.Added);
                        eventUpdates.Add(update);
                    }
                }

                // Booked products
                if (currentEvent.EventBookedProducts.Count != nextEvent.EventBookedProducts.Count)
                {
                    if (nextEvent.EventBookedProducts.Count > currentEvent.EventBookedProducts.Count)
                    {
                        var addedProducts = nextEvent.EventBookedProducts.Except(currentEvent.EventBookedProducts, new EventBookedProductComparer()).ToList();
                        //differences.AddRange(addedProducts.Select(x => string.Format("Added: {0} x {1}", x.Quantity, x.Product.Name)));
                        addedProducts.ForEach(eventBookedProduct =>
                             {
                                 var message = string.Format("Added: {0} x {1}", eventBookedProduct.Quantity, eventBookedProduct.Product.Name);
                                 var update = ProcessUpdate(nextEvent, message, null, string.Format("{0} x {1}", eventBookedProduct.Quantity, eventBookedProduct.Product.Name), eventBookedProduct.EventBookedProduct.ID, "Event Booked Product", "Product", UpdateAction.Added);
                                 eventUpdates.Add(update);
                             });
                    }
                    else
                    {
                        var removedProducts = currentEvent.EventBookedProducts.Except(nextEvent.EventBookedProducts, new EventBookedProductComparer()).ToList();
                        //differences.AddRange(removedProducts.Select(x => string.Format("Removed: {0} x {1}", x.Quantity, x.Product.Name)));
                        removedProducts.ForEach(eventBookedProduct =>
                             {
                                 var message = string.Format("Removed: {0} x {1}", eventBookedProduct.Quantity, eventBookedProduct.Product.Name);
                                 var update = ProcessUpdate(nextEvent, message, string.Format("{0} x {1}", eventBookedProduct.Quantity, eventBookedProduct.Product.Name), null, eventBookedProduct.EventBookedProduct.ID, "Event Booked Product", "Product", UpdateAction.Removed);
                                 eventUpdates.Add(update);
                             });
                    }
                }

                foreach (var newProduct in nextEvent.EventBookedProducts)
                {
                    var currentProduct = currentEvent.EventBookedProducts.FirstOrDefault(x => x.EventBookedProduct.ID == newProduct.EventBookedProduct.ID);

                    if (currentProduct == null) continue;

                    if (currentProduct.Product.ID != newProduct.Product.ID)
                    {
                        //differences.Add(string.Format("Edited: Product was changed from {0} to {1}", currentProduct.Product.Name, newProduct.Product.Name));
                        var message = string.Format("Edited: Product was changed from {0} to {1}", currentProduct.Product.Name, newProduct.Product.Name);
                        var update = ProcessUpdate(nextEvent, message, currentProduct.Product.Name, newProduct.Product.Name, newProduct.EventBookedProduct.ID, "Event Booked Product", "Product Name", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentProduct.Quantity != newProduct.Quantity)
                    {
                        //differences.Add(string.Format("Edited: {0} quantity was changed from {1} to {2}", currentProduct.Product.Name, currentProduct.Quantity, newProduct.Quantity));
                        var message = string.Format("Edited: {0} quantity was changed from {1} to {2}", currentProduct.Product.Name, currentProduct.Quantity, newProduct.Quantity);
                        var update = ProcessUpdate(nextEvent, message, Convert.ToString(currentProduct.Quantity), Convert.ToString(newProduct.Quantity), newProduct.EventBookedProduct.ID, "Event Booked Product", "Quantity", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentProduct.Price != newProduct.Price)
                    {
                        //differences.Add(string.Format("Edited: {0} price was changed from {1:C2} to {2:C2}", currentProduct.Product.Name, currentProduct.Price, newProduct.Price));
                        var message = string.Format("Edited: {0} price was changed from {1:C2} to {2:C2}", currentProduct.Product.Name, currentProduct.Price, newProduct.Price);
                        var update = ProcessUpdate(nextEvent, message, string.Format("{0:C2}", currentProduct.Price), string.Format("{0:C2}", newProduct.Price), newProduct.EventBookedProduct.ID, "Event Booked Product", "Price", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }
                }

                // Event Reminders
                var addedReminders = nextEvent.EventReminders.Except(currentEvent.EventReminders, new EventReminderComparer()).ToList();
                if (addedReminders.Any())
                {
                    {
                        //differences.AddRange(addedReminders.Select(eventReminder => string.Format("Added: Event-Reminder {0}", eventReminder.WhatToDo)));
                        addedReminders.ForEach(eventReminder =>
                             {
                                 var message = string.Format("Added: Event-Reminder {0}", eventReminder.WhatToDo);
                                 var update = ProcessUpdate(nextEvent, message, null, eventReminder.WhatToDo, eventReminder.EventReminder.ID, "Event Reminder", "WhatToDo", UpdateAction.Added);
                                 eventUpdates.Add(update);
                             });
                    }
                }

                var removedReminders = currentEvent.EventReminders.Except(nextEvent.EventReminders, new EventReminderComparer()).ToList();
                if (removedReminders.Any())
                {
                    //differences.AddRange(removedReminders.Select(eventReminder => string.Format("Removed: Event-Reminder {0}", eventReminder.WhatToDo)));
                    removedReminders.ForEach(eventReminder =>
                             {
                                 var message = string.Format("Removed: Event-Reminder {0}", eventReminder.WhatToDo);
                                 var update = ProcessUpdate(nextEvent, message, null, eventReminder.WhatToDo, eventReminder.EventReminder.ID, "Event Reminder", "WhatToDo", UpdateAction.Removed);
                                 eventUpdates.Add(update);
                             });

                }

                foreach (var newReminder in nextEvent.EventReminders)
                {
                    var currentReminder = currentEvent.EventReminders.FirstOrDefault(x => x.EventReminder.ID == newReminder.EventReminder.ID);

                    if (currentReminder == null) continue;

                    if (newReminder.AssignedToUser == null) continue;

                    if (currentReminder.DateDue != newReminder.DateDue)
                    {
                        // differences.Add(string.Format("Edited: Event-Reminder date due was changed from {0} to {1}", currentReminder.DateDue.ToString("d"), newReminder.DateDue.ToString("d")));
                        var message = string.Format("Edited: Event-Reminder date due was changed from {0} to {1}", currentReminder.DateDue.ToString("d"), newReminder.DateDue.ToString("d"));
                        var update = ProcessUpdate(nextEvent, message, currentReminder.DateDue.ToString("d"), newReminder.DateDue.ToString("d"), newReminder.EventReminder.ID, "Event Reminder", "DateDue", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentReminder.WhatToDo != newReminder.WhatToDo)
                    {
                        //differences.Add(string.Format("Edited: Event-Reminder WhatToDo was changed from {0} to {1}", currentReminder.WhatToDo, newReminder.WhatToDo));
                        var message = string.Format("Edited: Event-Reminder WhatToDo was changed from {0} to {1}", currentReminder.WhatToDo, newReminder.WhatToDo);
                        var update = ProcessUpdate(nextEvent, message, currentReminder.WhatToDo, newReminder.WhatToDo, newReminder.EventReminder.ID, "Event Reminder", "WhatToDo", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                    if (currentReminder.AssignedToUser.ID != newReminder.AssignedToUser.ID)
                    {
                        //differences.Add(string.Format("Edited: Event-Reminder Assigned To User was changed from {0} {1} to {2} {3}", currentReminder.AssignedToUser.FirstName, currentReminder.AssignedToUser.LastName, newReminder.AssignedToUser.FirstName, newReminder.AssignedToUser.LastName));
                        var message = string.Format("Edited: Event-Reminder Assigned To User was changed from {0} {1} to {2} {3}", currentReminder.AssignedToUser.FirstName, currentReminder.AssignedToUser.LastName, newReminder.AssignedToUser.FirstName, newReminder.AssignedToUser.LastName);
                        var update = ProcessUpdate(nextEvent, message, string.Format("{0} {1}", currentReminder.AssignedToUser.FirstName, currentReminder.AssignedToUser.LastName), string.Format("{0} {1}", newReminder.AssignedToUser.FirstName, newReminder.AssignedToUser.LastName), newReminder.EventReminder.ID, "Event Reminder", "Assigned To User", UpdateAction.Edited);
                        eventUpdates.Add(update);
                    }

                }
            }
            catch (Exception ex)
            {

            }

            currentEvent = null;

            return eventUpdates;
        }

        private static EventUpdate ProcessUpdate(EventModel eventModel, string message, string oldValue, string newValue, Guid itemID, string itemType, string field, UpdateAction action)
        {
            var update = new EventUpdate()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                Event = eventModel.Event,
                UserID = AccessService.Current.User.ID,
                Message = message,
                OldValue = oldValue,
                NewValue = newValue,
                ItemId = itemID,
                ItemType = itemType,
                Field = field,
                Action = action
            };
            return update;
        }

        public static IEnumerable<string> FindDifference(EnquiryModel currentEnquiry, EnquiryModel nextEnquiry, out bool activityChanged, out bool followUpsChanged)
        {
            var differences = new List<string>();

            activityChanged = false;
            followUpsChanged = false;

            try
            {
                // Name
                if (currentEnquiry.Name != nextEnquiry.Name)
                    differences.Add(string.Format("Edited: Name changed from {0} to {1}", currentEnquiry.Name, nextEnquiry.Name));

                // Date
                if (currentEnquiry.Date != nextEnquiry.Date)
                    differences.Add(string.Format("Edited: Date changed from {0} to {1}", currentEnquiry.Date, nextEnquiry.Date));

                // Places
                if (currentEnquiry.Places != nextEnquiry.Places)
                    differences.Add(string.Format("Edited: Number of people changed from {0} to {1}", currentEnquiry.Places, nextEnquiry.Places));

                // Type
                if (currentEnquiry.EventType.ID != nextEnquiry.EventType.ID)
                    differences.Add(string.Format("Edited: Type changed from {0} to {1}", currentEnquiry.EventType.Name, nextEnquiry.EventType.Name));

                // Status
                if (currentEnquiry.EventStatus.ID != nextEnquiry.EventStatus.ID)
                    differences.Add(string.Format("Edited: Status changed from {0} to {1}", currentEnquiry.EventStatus.Name, nextEnquiry.EventStatus.Name));

                // Contact
                if (currentEnquiry.PrimaryContact != null && currentEnquiry.PrimaryContact == null)
                    differences.Add(string.Format("Added: A new contact was associated with event"));
                else if (currentEnquiry.PrimaryContact == null && currentEnquiry.PrimaryContact != null)
                    differences.Add(string.Format("Removed: A primary contact was removed"));
                else if (nextEnquiry.PrimaryContact != null && currentEnquiry.PrimaryContact != null)
                    if (currentEnquiry.PrimaryContact.Contact.ID != nextEnquiry.PrimaryContact.Contact.ID)
                        differences.Add(string.Format("Edited: Primary contact changed from {0} to {1}", currentEnquiry.PrimaryContact.Contact.FirstName, nextEnquiry.PrimaryContact.Contact.FirstName));

                //Enquiry Detail Assigned To User
                if (currentEnquiry.AssignedToUser.ID != nextEnquiry.AssignedToUser.ID)
                    differences.Add(string.Format("Edited: Assigned To User changed from {0} to {1}", currentEnquiry.AssignedToUser.FirstName, nextEnquiry.AssignedToUser.FirstName));

                //Enquiry Detail EnquiryStatus
                if (currentEnquiry.EnquiryStatus.ID != nextEnquiry.EnquiryStatus.ID)
                    differences.Add(string.Format("Edited: Enquiry Status changed from {0} to {1}", currentEnquiry.EnquiryStatus.Status, nextEnquiry.EnquiryStatus.Status));

                //Enquiry Detail ReceivedMethod
                if (currentEnquiry.ReceivedMethod.ID != nextEnquiry.ReceivedMethod.ID)
                    differences.Add(string.Format("Edited: ReceivedMethod changed from {0} to {1}", currentEnquiry.ReceivedMethod.ReceiveMethod, nextEnquiry.ReceivedMethod.ReceiveMethod));

                //Enquiry Detail Value
                if (currentEnquiry.Value != nextEnquiry.Value)
                    differences.Add(string.Format("Edited: Value changed from {0} to {1}", currentEnquiry.Value, nextEnquiry.Value));

                //Enquiry Detail Likelihood
                if (currentEnquiry.Likelihood != nextEnquiry.Likelihood)
                    differences.Add(string.Format("Edited: Likelihood changed from {0} to {1}", currentEnquiry.Likelihood, nextEnquiry.Likelihood));

                //Enquiry Detail Campaign
                if (nextEnquiry.Campaign != null)
                {
                    if (currentEnquiry.Campaign != null && currentEnquiry.Campaign.ID != nextEnquiry.Campaign.ID)
                        differences.Add(string.Format("Edited: Campaign changed from {0} to {1}", currentEnquiry.Campaign.Name, nextEnquiry.Campaign.Name));
                    else if (currentEnquiry.Campaign == null)
                    {
                        differences.Add(string.Format("Edited: Campaign changed to {0}", nextEnquiry.Campaign.Name));
                    }
                }


                // Activities
                if (currentEnquiry.Activities.Count != nextEnquiry.Activities.Count)
                {
                    activityChanged = true;
                    if (nextEnquiry.Activities.Count > currentEnquiry.Activities.Count)
                    {
                        var addedActivities = nextEnquiry.Activities.Except(currentEnquiry.Activities, new EnquiryActivityComparer()).ToList();
                        differences.AddRange(addedActivities.Select(activityModel => string.Format("Added: Activity {0}", activityModel.Details)));
                    }
                    else
                    {
                        var removedActivities = currentEnquiry.Activities.Except(nextEnquiry.Activities, new EnquiryActivityComparer()).ToList();
                        differences.AddRange(removedActivities.Select(activityModel => string.Format("Removed: Event charge {0}", activityModel.Details)));
                    }
                }

                foreach (var newActivity in nextEnquiry.Activities)
                {
                    var currentActivity = currentEnquiry.Activities.FirstOrDefault(x => x.Activity.ID == newActivity.Activity.ID);

                    if (currentActivity == null) continue;

                    if (currentActivity.ActivityType.ID != newActivity.ActivityType.ID)
                        differences.Add(string.Format("Edited: Activity Type changed from {0} to {1}", currentActivity.ActivityType.Name, newActivity.ActivityType.Name));

                    if (currentActivity.Details != newActivity.Details)
                        differences.Add(string.Format("Edited: Activity Details changed from {0} to {1}", currentActivity.Activity.Details, newActivity.Activity.Details));

                    if (currentActivity.Direction != newActivity.Direction)
                        differences.Add(string.Format("Edited: Activity Direction changed from {0} to {1}", currentActivity.Direction, newActivity.Direction));

                    if (currentActivity.Length != newActivity.Length)
                        differences.Add(string.Format("Edited: Activity Length changed from {0} to {1}", currentActivity.Length, newActivity.Length));
                }

                // Notes
                if (currentEnquiry.EnquiryNotes.Count != nextEnquiry.EnquiryNotes.Count)
                {
                    if (nextEnquiry.EnquiryNotes.Count > currentEnquiry.EnquiryNotes.Count)
                    {
                        var addedNotes = nextEnquiry.EnquiryNotes.Except(currentEnquiry.EnquiryNotes, new EnquiryNoteComparer()).ToList();
                        differences.AddRange(addedNotes.Select(enquiryNote => string.Format("Added: Enquiry note {0}", enquiryNote.Note)));
                    }
                    else
                    {
                        var removedNotes = currentEnquiry.EnquiryNotes.Except(nextEnquiry.EnquiryNotes, new EnquiryNoteComparer()).ToList();
                        differences.AddRange(removedNotes.Select(enquiryNote => string.Format("Removed: Enquiry note {0}", enquiryNote.Note)));
                    }
                }

                foreach (var newNote in nextEnquiry.EnquiryNotes)
                {
                    var currentNote = currentEnquiry.EnquiryNotes.FirstOrDefault(x => x.EnquiryNote.ID == newNote.EnquiryNote.ID);

                    if (currentNote == null) continue;

                    if (currentNote.Note != newNote.Note)
                        differences.Add(string.Format("Edited: Note was changed from {0} to {1}", currentNote.Note, newNote.Note));

                }

                // FollowUps
                var addedFollowUps = nextEnquiry.FollowUps.Except(currentEnquiry.FollowUps, new EnquiryFollowUpComparer()).ToList();
                if (addedFollowUps.Any())
                {
                    followUpsChanged = true;
                    differences.AddRange(addedFollowUps.Select(enquiryFollowUp => string.Format("Added: Follow-Up {0}", enquiryFollowUp.WhatToDo)));
                }

                var removedFollowUps = currentEnquiry.FollowUps.Except(nextEnquiry.FollowUps, new EnquiryFollowUpComparer()).ToList();
                if (removedFollowUps.Any())
                {
                    followUpsChanged = true;
                    differences.AddRange(removedFollowUps.Select(enquiryFollowUp => string.Format("Removed: Follow-Up {0}", enquiryFollowUp.WhatToDo)));
                }

                foreach (var newFollowUp in nextEnquiry.FollowUps)
                {
                    var currentFollowUp = currentEnquiry.FollowUps.FirstOrDefault(x => x.FollowUp.ID == newFollowUp.FollowUp.ID);

                    if (currentFollowUp == null) continue;

                    if (newFollowUp.AssignedToUser == null) continue;

                    if (currentFollowUp.DateDue != newFollowUp.DateDue)
                        differences.Add(string.Format("Edited: Follow-Up date due was changed from {0} to {1}", currentFollowUp.DateDue.ToString("d"), newFollowUp.DateDue.ToString("d")));

                    if (currentFollowUp.WhatToDo != newFollowUp.WhatToDo)
                        differences.Add(string.Format("Edited: Follow-Up WhatToDo was changed from {0} to {1}", currentFollowUp.WhatToDo, newFollowUp.WhatToDo));

                    if (currentFollowUp.AssignedToUser.ID != newFollowUp.AssignedToUser.ID)
                        differences.Add(string.Format("Edited: Follow-Up Assigned To User was changed from {0} {1} to {2} {3}", currentFollowUp.AssignedToUser.FirstName, currentFollowUp.AssignedToUser.LastName, newFollowUp.AssignedToUser.FirstName, newFollowUp.AssignedToUser.LastName));

                }

            }
            catch (Exception ex)
            {
                //                throw ex;
            }

            return differences;
        }

        public static List<MembershipUpdate> FindDifference(MemberModel currentMember, MemberModel nextMember, string type, bool isNewMembership = false)
        {
            var membershipUpdates = new List<MembershipUpdate>();
            try
            {
                if (type == "Member")
                {
                    if (isNewMembership)
                    {
                        // Member
                        if (string.IsNullOrEmpty(currentMember.Contact.LastName) && !string.IsNullOrEmpty(nextMember.Contact.LastName))
                        {
                            var message = string.Format("Added: Member {0}", nextMember.Contact.ContactName);
                            var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.ContactName, nextMember.Member.ID, "Member", "Name", UpdateAction.Added);
                            membershipUpdates.Add(update);
                        }

                        // Category
                        if (currentMember.Category == null && nextMember.Category != null)
                        {
                            var message = string.Format("Added: Category {0}", nextMember.Category.Name);
                            var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Category.Name, nextMember.Member.ID, "Member", "Category", UpdateAction.Added);
                            membershipUpdates.Add(update);
                        }

                        // Start Date
                        if (currentMember.StartDate == null && nextMember.StartDate != null)
                        {
                            var message = string.Format("Added: Start Date {0}", Convert.ToDateTime(nextMember.StartDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, null, Convert.ToDateTime(nextMember.StartDate).ToString("d"), nextMember.Member.ID, "Member", "StartDate", UpdateAction.Added);
                            membershipUpdates.Add(update);
                        }

                        // Renewal Date
                        if (currentMember.RenewalDate == null && nextMember.RenewalDate != null)
                        {
                            var message = string.Format("Added: Renewal Date {0}", Convert.ToDateTime(nextMember.RenewalDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, null, Convert.ToDateTime(nextMember.RenewalDate).ToString("d"), nextMember.Member.ID, "Member", "RenewalDate", UpdateAction.Added);
                            membershipUpdates.Add(update);
                        }
                        // Status
                        if (currentMember.Status == null && nextMember.Status != null)
                        {
                            var message = string.Format("Added: Status {0}", Enum.GetName(typeof(Status), nextMember.Status));
                            var update = ProcessMembershipUpdate(nextMember, message, null, Enum.GetName(typeof(Status), nextMember.Status), nextMember.Member.ID, "Member", "Status", UpdateAction.Added);
                            membershipUpdates.Add(update);
                        }

                    }
                    else
                    {
                        // Category
                        if (currentMember.Category.ID != nextMember.Category.ID)
                        {
                            var message = string.Format("Edited: Category changed from {0} to {1}", currentMember.Category.Name, nextMember.Category.Name);
                            var update = ProcessMembershipUpdate(nextMember, message, currentMember.Category.Name, nextMember.Category.Name, nextMember.Member.ID, "Member", "Category", UpdateAction.Edited);
                            membershipUpdates.Add(update);
                        }

                        // Start Date
                        if (currentMember.StartDate != nextMember.StartDate)
                        {
                            var message = string.Format("Edited: Start Date changed from {0} to {1}", Convert.ToDateTime(currentMember.StartDate).ToString("d"), Convert.ToDateTime(nextMember.StartDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, Convert.ToDateTime(currentMember.StartDate).ToString("d"), Convert.ToDateTime(nextMember.StartDate).ToString("d"), nextMember.Member.ID, "Member", "StartDate", UpdateAction.Edited);
                            membershipUpdates.Add(update);
                        }

                        // Renewal Date
                        if (currentMember.RenewalDate != nextMember.RenewalDate)
                        {
                            var message = string.Format("Edited: Renewal Date changed from {0} to {1}", Convert.ToDateTime(currentMember.RenewalDate).ToString("d"), Convert.ToDateTime(nextMember.RenewalDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, Convert.ToDateTime(currentMember.RenewalDate).ToString("d"), Convert.ToDateTime(nextMember.RenewalDate).ToString("d"), nextMember.Member.ID, "Member", "RenewalDate", UpdateAction.Edited);
                            membershipUpdates.Add(update);
                        }
                        // Status
                        if (currentMember.Status != nextMember.Status)
                        {
                            var message = string.Format("Edited: Status changed from {0} to {1}", Enum.GetName(typeof(Status), currentMember.Status), Enum.GetName(typeof(Status), nextMember.Status));
                            var update = ProcessMembershipUpdate(nextMember, message, Enum.GetName(typeof(Status), currentMember.Status), Enum.GetName(typeof(Status), nextMember.Status), nextMember.Member.ID, "Member", "Status", UpdateAction.Edited);
                            membershipUpdates.Add(update);
                        }

                        // End Date
                        if (nextMember.Member.EndDate != null && currentMember.Member.EndDate == null)
                        {
                            var message = string.Format("Added: End date {0}", Convert.ToDateTime(nextMember.Member.EndDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, null, Convert.ToDateTime(nextMember.Member.EndDate).ToString("d"), nextMember.Member.ID, "Member", "EndDate", UpdateAction.Added);
                            membershipUpdates.Add(update);
                        }
                        else if (nextMember.Member.EndDate == null && currentMember.Member.EndDate != null)
                        {
                            var message = string.Format("Removed: End date {0} removed", Convert.ToDateTime(currentMember.Member.EndDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, Convert.ToDateTime(currentMember.Member.EndDate).ToString("d"), null, nextMember.Member.ID, "Member", "EndDate", UpdateAction.Removed);
                            membershipUpdates.Add(update);
                        }
                        else if (nextMember.Member.EndDate != null && currentMember.Member.EndDate != null && nextMember.Member.EndDate != currentMember.Member.EndDate)
                        {
                            var message = string.Format("Edited: End date changed from {0} to {1}", Convert.ToDateTime(currentMember.Member.EndDate).ToString("d"), Convert.ToDateTime(nextMember.Member.EndDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, Convert.ToDateTime(currentMember.Member.EndDate).ToString("d"), Convert.ToDateTime(nextMember.Member.EndDate).ToString("d"), nextMember.Member.ID, "Member", "EndDate", UpdateAction.Edited);
                            membershipUpdates.Add(update);
                        }

                        // Resign Date
                        if (nextMember.Member.ResignDate != null && currentMember.Member.ResignDate == null)
                        {
                            var message = string.Format("Added: Resign date {0}", Convert.ToDateTime(nextMember.Member.ResignDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, null, Convert.ToDateTime(nextMember.Member.ResignDate).ToString("d"), nextMember.Member.ID, "Member", "ResignDate", UpdateAction.Added);
                            membershipUpdates.Add(update);
                        }
                        else if (nextMember.Member.ResignDate == null && currentMember.Member.ResignDate != null)
                        {
                            var message = string.Format("Removed: Resign date {0} removed", Convert.ToDateTime(currentMember.Member.ResignDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, Convert.ToDateTime(currentMember.Member.ResignDate).ToString("d"), null, nextMember.Member.ID, "Member", "ResignDate", UpdateAction.Removed);
                            membershipUpdates.Add(update);
                        }
                        else if (nextMember.Member.ResignDate != null && currentMember.Member.ResignDate != null && nextMember.Member.ResignDate != currentMember.Member.ResignDate)
                        {
                            var message = string.Format("Edited: Resign date changed from {0} to {1}", Convert.ToDateTime(currentMember.Member.ResignDate).ToString("d"), Convert.ToDateTime(nextMember.Member.ResignDate).ToString("d"));
                            var update = ProcessMembershipUpdate(nextMember, message, Convert.ToDateTime(currentMember.Member.ResignDate).ToString("d"), Convert.ToDateTime(nextMember.Member.ResignDate).ToString("d"), nextMember.Member.ID, "Member", "ResignDate", UpdateAction.Edited);
                            membershipUpdates.Add(update);
                        }

                        // Contact
                        if (nextMember.Contact.Contact.ID == currentMember.Contact.Contact.ID)
                        {
                            ProcessMembershipContact(currentMember, nextMember, membershipUpdates);
                        }
                        else if (nextMember.Contact.Contact.ID != currentMember.Contact.Contact.ID)
                        {
                            var message = string.Format("Edited: Contact changed from {0} to {1}", currentMember.Contact.ContactName, nextMember.Contact.ContactName);
                            var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.ContactName, nextMember.Contact.ContactName, nextMember.Member.ID, "Member", "Contact", UpdateAction.Edited);
                            membershipUpdates.Add(update);

                            ProcessMembershipContact(currentMember, nextMember, membershipUpdates);
                        }
                    }
                }
                else if (type == "MemberNotes")
                {
                    // Notes
                    if (currentMember.MemberNotes.Count != nextMember.MemberNotes.Count)
                    {
                        if (nextMember.MemberNotes.Count > currentMember.MemberNotes.Count)
                        {
                            var addedNotes = nextMember.MemberNotes.Except(currentMember.MemberNotes, new MemberNoteComparer()).ToList();
                            addedNotes.ForEach(memberNote =>
                            {
                                var message = string.Format("Added: Note {0}", memberNote.Note);
                                var update = ProcessMembershipUpdate(nextMember, message, null, memberNote.Note, memberNote.MemberNote.ID, "Member Note", "Notes", UpdateAction.Added);
                                membershipUpdates.Add(update);
                            });
                        }
                        else
                        {
                            var removedNotes = currentMember.MemberNotes.Except(nextMember.MemberNotes, new MemberNoteComparer()).ToList();
                            removedNotes.ForEach(memberNote =>
                            {
                                var message = string.Format("Removed: Note {0}", memberNote.Note);
                                var update = ProcessMembershipUpdate(nextMember, message, memberNote.Note, null, memberNote.MemberNote.ID, "Member Note", "Notes", UpdateAction.Removed);
                                membershipUpdates.Add(update);
                            });
                        }
                    }
                    foreach (var newNote in nextMember.MemberNotes)
                    {
                        var currentNote = currentMember.MemberNotes.FirstOrDefault(x => x.MemberNote.ID == newNote.MemberNote.ID);

                        if (currentNote == null) continue;

                        if (currentNote.Note != newNote.Note)
                        {
                            var message = string.Format("Edited: Note was changed from {0} to {1}", currentNote.Note, newNote.Note);
                            var update = currentNote.Note != null ? ProcessMembershipUpdate(nextMember, message, currentNote.Note, newNote.Note, newNote.MemberNote.ID, "Member Note", "Notes", UpdateAction.Edited) : ProcessMembershipUpdate(nextMember, message, null, newNote.Note, newNote.MemberNote.ID, "Member Note", "Notes", UpdateAction.Added);
                            membershipUpdates.Add(update);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            currentMember = null;

            return membershipUpdates;
        }

        private static void ProcessMembershipContact(MemberModel currentMember, MemberModel nextMember, List<MembershipUpdate> membershipUpdates)
        {
            if (currentMember.Contact.Title == null && nextMember.Contact.Title != null)
            {
                var message = string.Format("Edited: Added Title {0}", nextMember.Contact.Title.Title);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.Title.Title, nextMember.Member.Contact.ID, "Contact", "Title", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (nextMember.Contact.Title != null && currentMember.Contact.Title != null && currentMember.Contact.Title.ID != nextMember.Contact.Title.ID)
            {
                var message = string.Format("Edited: Title changed from {0} to {1}", currentMember.Contact.Title.Title, nextMember.Contact.Title.Title);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Title.Title, nextMember.Contact.Title.Title, nextMember.Member.Contact.ID, "Contact", "Title", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (nextMember.Contact.FirstName != currentMember.Contact.FirstName)
            {
                var message = string.Format("Edited: First name changed from {0} to {1}", currentMember.Contact.FirstName, nextMember.Contact.FirstName);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.FirstName, nextMember.Contact.FirstName, nextMember.Member.Contact.ID, "Contact", "FirstName", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (nextMember.Contact.LastName != currentMember.Contact.LastName)
            {
                var message = string.Format("Edited: Last name changed from {0} to {1}", currentMember.Contact.LastName, nextMember.Contact.LastName);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.LastName, nextMember.Contact.LastName, nextMember.Member.Contact.ID, "Contact", "LastName", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.Email) && !string.IsNullOrEmpty(nextMember.Contact.Email))
            {
                var message = string.Format("Edited: Added Email {0}", nextMember.Contact.Email);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.Email, nextMember.Member.Contact.ID, "Contact", "Email", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.Email) && string.IsNullOrEmpty(nextMember.Contact.Email))
            {
                var message = string.Format("Edited: Email removed {0}", currentMember.Contact.Email);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Email, null, nextMember.Member.Contact.ID, "Contact", "Email", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.Email) && !string.IsNullOrEmpty(currentMember.Contact.Email) && nextMember.Contact.Email != currentMember.Contact.Email)
            {
                var message = string.Format("Edited: Email changed from {0} to {1}", currentMember.Contact.Email, nextMember.Contact.Email);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Email, nextMember.Contact.Email, nextMember.Member.Contact.ID, "Contact", "Email", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (nextMember.Contact.Gender != currentMember.Contact.Gender)
            {
                var message = string.Format("Edited: Gender changed from {0} to {1}", Enum.GetName(typeof(Gender), currentMember.Contact.Gender), nextMember.Contact.Gender);
                var update = ProcessMembershipUpdate(nextMember, message, Enum.GetName(typeof(Gender), currentMember.Contact.Gender), Enum.GetName(typeof(Gender), nextMember.Contact.Gender), nextMember.Member.Contact.ID, "Contact", "Gender", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.Address1) && !string.IsNullOrEmpty(nextMember.Contact.Address1))
            {
                var message = string.Format("Edited: Added Address1 {0}", nextMember.Contact.Address1);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.Address1, nextMember.Member.Contact.ID, "Contact", "Address1", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.Address1) && string.IsNullOrEmpty(nextMember.Contact.Address1))
            {
                var message = string.Format("Edited: Address1 removed {0}", currentMember.Contact.Address1);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Address1, null, nextMember.Member.Contact.ID, "Contact", "Address1", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.Address1) && !string.IsNullOrEmpty(currentMember.Contact.Address1) && nextMember.Contact.Address1 != currentMember.Contact.Address1)
            {
                var message = string.Format("Edited: Address1 changed from {0} to {1}", currentMember.Contact.Address1, nextMember.Contact.Address1);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Address1, nextMember.Contact.Address1, nextMember.Member.Contact.ID, "Contact", "Address1", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.Address2) && !string.IsNullOrEmpty(nextMember.Contact.Address2))
            {
                var message = string.Format("Edited: Added Address2 {0}", nextMember.Contact.Address2);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.Address2, nextMember.Member.Contact.ID, "Contact", "Address2", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.Address2) && string.IsNullOrEmpty(nextMember.Contact.Address2))
            {
                var message = string.Format("Edited: Address2 removed {0}", currentMember.Contact.Address2);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Address2, null, nextMember.Member.Contact.ID, "Contact", "Address2", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.Address2) && !string.IsNullOrEmpty(currentMember.Contact.Address2) && nextMember.Contact.Address2 != currentMember.Contact.Address2)
            {
                var message = string.Format("Edited: Address2 changed from {0} to {1}", currentMember.Contact.Address2, nextMember.Contact.Address2);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Address2, nextMember.Contact.Address2, nextMember.Member.Contact.ID, "Contact", "Address2", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.Address3) && !string.IsNullOrEmpty(nextMember.Contact.Address3))
            {
                var message = string.Format("Edited: Added Address3 {0}", nextMember.Contact.Address3);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.Address3, nextMember.Member.Contact.ID, "Contact", "Address3", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.Address3) && string.IsNullOrEmpty(nextMember.Contact.Address3))
            {
                var message = string.Format("Edited: Address3 removed {0}", currentMember.Contact.Address3);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Address3, null, nextMember.Member.Contact.ID, "Contact", "Address3", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.Address3) && !string.IsNullOrEmpty(currentMember.Contact.Address3) && nextMember.Contact.Address3 != currentMember.Contact.Address3)
            {
                var message = string.Format("Edited: Address3 changed from {0} to {1}", currentMember.Contact.Address3, nextMember.Contact.Address3);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Address3, nextMember.Contact.Address3, nextMember.Member.Contact.ID, "Contact", "Address3", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.CompanyName) && !string.IsNullOrEmpty(nextMember.Contact.CompanyName))
            {
                var message = string.Format("Edited: Added Company {0}", nextMember.Contact.CompanyName);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.CompanyName, nextMember.Member.Contact.ID, "Contact", "CompanyName", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.CompanyName) && string.IsNullOrEmpty(currentMember.Contact.CompanyName))
            {
                var message = string.Format("Edited: Company removed {0}", currentMember.Contact.CompanyName);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.CompanyName, null, nextMember.Member.Contact.ID, "Contact", "CompanyName", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.CompanyName) && !string.IsNullOrEmpty(currentMember.Contact.CompanyName) && nextMember.Contact.CompanyName != currentMember.Contact.CompanyName)
            {
                var message = string.Format("Edited: Company changed from {0} to {1}", currentMember.Contact.CompanyName, nextMember.Contact.CompanyName);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.CompanyName, nextMember.Contact.CompanyName, nextMember.Member.Contact.ID, "Contact", "CompanyName", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.City) && !string.IsNullOrEmpty(nextMember.Contact.City))
            {
                var message = string.Format("Edited: Added City {0}", nextMember.Contact.City);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.City, nextMember.Member.Contact.ID, "Contact", "City", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.City) && string.IsNullOrEmpty(nextMember.Contact.City))
            {
                var message = string.Format("Edited: City removed {0}", currentMember.Contact.City);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.City, null, nextMember.Member.Contact.ID, "Contact", "City", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.City) && !string.IsNullOrEmpty(currentMember.Contact.City) && nextMember.Contact.City != currentMember.Contact.City)
            {
                var message = string.Format("Edited: City changed from {0} to {1}", currentMember.Contact.City, nextMember.Contact.City);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.City, nextMember.Contact.City, nextMember.Member.Contact.ID, "Contact", "City", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.Country) && !string.IsNullOrEmpty(nextMember.Contact.Country))
            {
                var message = string.Format("Edited: Added Country {0}", nextMember.Contact.Country);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.Country, nextMember.Member.Contact.ID, "Contact", "Country", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.Country) && string.IsNullOrEmpty(nextMember.Contact.Country))
            {
                var message = string.Format("Edited: Country removed {0}", currentMember.Contact.Country);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Country, null, nextMember.Member.Contact.ID, "Contact", "Country", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.Country) && !string.IsNullOrEmpty(currentMember.Contact.Country) && nextMember.Contact.Country != currentMember.Contact.Country)
            {
                var message = string.Format("Edited: Country changed from {0} to {1}", currentMember.Contact.Country, nextMember.Contact.Country);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Country, nextMember.Contact.Country, nextMember.Member.Contact.ID, "Contact", "Country", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.PostCode) && !string.IsNullOrEmpty(nextMember.Contact.PostCode))
            {
                var message = string.Format("Edited: Added PostCode {0}", nextMember.Contact.PostCode);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.PostCode, nextMember.Member.Contact.ID, "Contact", "PostCode", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.PostCode) && string.IsNullOrEmpty(nextMember.Contact.PostCode))
            {
                var message = string.Format("Edited: PostCode removed {0}", currentMember.Contact.PostCode);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.PostCode, null, nextMember.Member.Contact.ID, "Contact", "PostCode", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.PostCode) && !string.IsNullOrEmpty(currentMember.Contact.PostCode) && nextMember.Contact.PostCode != currentMember.Contact.PostCode)
            {
                var message = string.Format("Edited: PostCode changed from {0} to {1}", currentMember.Contact.PostCode, nextMember.Contact.PostCode);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.PostCode, nextMember.Contact.PostCode, nextMember.Member.Contact.ID, "Contact", "PostCode", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.Phone1) && !string.IsNullOrEmpty(nextMember.Contact.Phone1))
            {
                var message = string.Format("Edited: Added Phone1 {0}", nextMember.Contact.Phone1);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.Phone1, nextMember.Member.Contact.ID, "Contact", "Phone1", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.Phone1) && string.IsNullOrEmpty(nextMember.Contact.Phone1))
            {
                var message = string.Format("Edited: Phone1 removed {0}", currentMember.Contact.Phone1);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Phone1, null, nextMember.Member.Contact.ID, "Contact", "Phone1", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.Phone1) && !string.IsNullOrEmpty(currentMember.Contact.Phone1) && nextMember.Contact.Phone1 != currentMember.Contact.Phone1)
            {
                var message = string.Format("Edited: Phone1 changed from {0} to {1}", currentMember.Contact.Phone1, nextMember.Contact.Phone1);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Phone1, nextMember.Contact.Phone1, nextMember.Member.Contact.ID, "Contact", "Phone1", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
            if (string.IsNullOrEmpty(currentMember.Contact.Phone2) && !string.IsNullOrEmpty(nextMember.Contact.Phone2))
            {
                var message = string.Format("Edited: Added Phone2 {0}", nextMember.Contact.Phone2);
                var update = ProcessMembershipUpdate(nextMember, message, null, nextMember.Contact.Phone2, nextMember.Member.Contact.ID, "Contact", "Phone2", UpdateAction.Added);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(currentMember.Contact.Phone2) && string.IsNullOrEmpty(nextMember.Contact.Phone2))
            {
                var message = string.Format("Edited: Phone2 removed {0}", currentMember.Contact.Phone2);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Phone2, null, nextMember.Member.Contact.ID, "Contact", "Phone2", UpdateAction.Removed);
                membershipUpdates.Add(update);
            }
            else if (!string.IsNullOrEmpty(nextMember.Contact.Phone2) && !string.IsNullOrEmpty(currentMember.Contact.Phone2) && nextMember.Contact.Phone2 != currentMember.Contact.Phone2)
            {
                var message = string.Format("Edited: Phone2 changed from {0} to {1}", currentMember.Contact.Phone2, nextMember.Contact.Phone2);
                var update = ProcessMembershipUpdate(nextMember, message, currentMember.Contact.Phone2, nextMember.Contact.Phone2, nextMember.Member.Contact.ID, "Contact", "Phone2", UpdateAction.Edited);
                membershipUpdates.Add(update);
            }
        }

        private static MembershipUpdate ProcessMembershipUpdate(MemberModel memberModel, string message, string oldValue, string newValue, Guid itemID, string itemType, string field, UpdateAction action)
        {
            var update = new MembershipUpdate()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                MemberID = memberModel.Member.ID,
                UserID = AccessService.Current.User.ID,
                Message = message,
                OldValue = oldValue,
                NewValue = newValue,
                ItemId = itemID,
                ItemType = itemType,
                Field = field,
                Action = Convert.ToInt32(action)
            };
            return update;
        }
    }
}