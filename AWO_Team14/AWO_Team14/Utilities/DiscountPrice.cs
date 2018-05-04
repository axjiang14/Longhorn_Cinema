using AWO_Team14.DAL;
using AWO_Team14.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;


namespace AWO_Team14.Utilities
{
    public class DiscountPrice
    {
        public static Decimal GetTicketPrice(UserTicket ticket)
        {
            AppDbContext db = new AppDbContext();
            Decimal ticketPrice = -1;     
            // assign price for each ticket

            Boolean weekend = (int)ticket.Showing.ShowDate.DayOfWeek == 6 || (int)ticket.Showing.ShowDate.DayOfWeek == 0;
            Debug.WriteLine(weekend);
            // checks if showing is matinee
            if (ticket.Showing.StartHour < 12 && weekend == false)
            {
                var query = from c in db.Discounts
                            where c.DiscountName == "matinee"
                            select c;
                foreach (var result in query)
                {
                    // sets Current Price property
                    // $5.00
                    ticket.AppliedDiscounts = result.DiscountName;
                    //ticketPrice = ticket.Showing.ShowingPrice - result.DiscountValue;
                }
            }

            // checks if showing day is weekday and afternoon
            else if (ticket.Showing.StartHour >= 12 && ((int)ticket.Showing.ShowDate.DayOfWeek >= 1 && (int)ticket.Showing.ShowDate.DayOfWeek <= 4))
            {
                var query = from c in db.Discounts
                            where c.DiscountName == "weekday"
                            select c;
                foreach (var result in query)
                {
                    // sets Current Price property
                    // $10.00
                    ticket.AppliedDiscounts = result.DiscountName;
                    //ticketPrice = ticket.Showing.ShowingPrice - result.DiscountValue;
                }

            }
            // checks if showing day is after friday noontime
            else
            {
                var query = from c in db.Discounts
                            where c.DiscountName == "weekend"
                            select c;
                foreach (var result in query)
                {
                    // sets Current Price property
                    // $12.00
                    ticket.AppliedDiscounts = result.DiscountName;
                    //ticketPrice = ticket.Showing.ShowingPrice - result.DiscountValue;
                }
            }

            // additional discounts for Non-special showings
            if (ticket.Showing.Special == false)
            {
                 
                // tuesday discount
                if ((int)ticket.Showing.ShowDate.DayOfWeek == 2 && (ticket.Showing.StartHour >= 12 && ticket.Showing.StartHour < 17))
                {
                        var query = from c in db.Discounts
                                    where c.DiscountName == "tuesday"
                                    select c;
                        foreach (var result in query)
                        {
                        // sets Current Price property
                        // $8.00
                        ticket.AppliedDiscounts += ", " + result.DiscountName;
                        ticketPrice = result.DiscountValue;
                        }

                }

                //senior citizen discounts for 2 tickets in transcation
                var querySenior = (from u in db.UserTickets
                                  where u.Transaction.TransactionID == ticket.Transaction.TransactionID &&
                                  u.AppliedDiscounts.Contains("senior")
                                select u).Count();

                if (ticket.Transaction.User.Birthday.AddYears(60) <= ticket.Transaction.TransactionDate && querySenior < 2)
                {
                    var query = from c in db.Discounts
                                where c.DiscountName == "senior"
                                select c;
                    foreach (var result in query)
                    {
                        ticket.AppliedDiscounts += ", " + result.DiscountName;
                        ticketPrice = ticket.Showing.ShowingPrice - result.DiscountValue;
                        //ticketPrice -= result.DiscountValue;
                    }
                        
                }

                // early bird discount
                if ((ticket.Showing.ShowDate - ticket.Transaction.TransactionDate).TotalDays > 2)
                {
                    var query = from c in db.Discounts
                                where c.DiscountName == "earlybird"
                                select c;
                    foreach (var result in query)
                    {
                        ticket.AppliedDiscounts += ", " + result.DiscountName;
                        ticketPrice = ticket.Showing.ShowingPrice - result.DiscountValue;
                        //ticketPrice -= result.DiscountValue;
                    }
                }

                return ticketPrice;
            }

            else
            {
                return ticketPrice;
            }
        }

        public static Decimal GetBasePrice(Showing showing)
        {
            AppDbContext db = new AppDbContext();

            Decimal showingPrice = 0;

            // assign price for each ticket

            Boolean weekend = (int)showing.ShowDate.DayOfWeek == 6 || (int)showing.ShowDate.DayOfWeek == 0;
            Debug.WriteLine(weekend);
            // checks if showing is matinee
            if (showing.StartHour < 12 && weekend == false)
            {
                var query = from c in db.Discounts
                            where c.DiscountName == "matinee"
                            select c;
                foreach (var result in query)
                {
                    // sets Current Price property
                    // $5.00
                   showingPrice = result.DiscountValue;
                }
            }

            // checks if showing day is weekday and afternoon
            else if (showing.StartHour >= 12 && ((int)showing.ShowDate.DayOfWeek >= 1 && (int)showing.ShowDate.DayOfWeek <= 4))
            {
                var query = from c in db.Discounts
                            where c.DiscountName == "weekday"
                            select c;
                foreach (var result in query)
                {
                    // sets Current Price property
                    // $10.00
                   showingPrice = result.DiscountValue;
                }

            }
            // checks if showing day is after friday noontime
            else
            {
                var query = from c in db.Discounts
                            where c.DiscountName == "weekend"
                            select c;
                foreach (var result in query)
                {
                    // sets Current Price property
                    // $12.00
                  showingPrice = result.DiscountValue;
                }
            }

            return showingPrice;



        }

    }
}