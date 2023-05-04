using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Server.Moodle.DAL;
namespace Server.Moodle
{
    public class Order
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int FlatId { get; private set; }
        //[DisplayFormat(DataFormatString = "{yyyy/MM/dd}", ApplyFormatInEditMode = true)]

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int PricePerNight { get; private set; }
        // constructor
        public Order(int id, int userId, int flatId, DateTime startDate, DateTime endDate, int pricePerNight)
        {
            this.Id = id;
            this.UserId = userId;
            this.FlatId = flatId;

            // Convert the StartDate and EndDate properties to "year-month-day" strings
            // and assign the resulting strings back to the properties
            //this.StartDate = DateTime.Parse(startDate.ToString("yyyy-MM-dd"));
            //this.EndDate = DateTime.Parse(endDate.ToString("yyyy-MM-dd"));
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.PricePerNight = pricePerNight;
        }
        // insert the order to order list
        public bool Insert()
        {
            DBservices dBservices = new DBservices();
            return dBservices.InsertOrderToDB(this);

        }
        public static List<Order> readByUser(int userId)
        {
            DBservices dbServices = new DBservices(); 
            return dbServices.GetOrdersByUserID(userId);
        }
        public static List<Order> Read()
        {
            DBservices dbServices = new DBservices();
            return dbServices.getAllOrders();
        }
        public static Order? Read(int id)
        {
            DBservices dbServices = new DBservices();
            return dbServices.getOrderById(id);
        }
        public static int Update(Order order)
        {
            DBservices dBservices = new DBservices();
            return dBservices.UpdateOrder(order);
        }
        public static bool Delete(int id)
        {
            DBservices dBservices = new DBservices();
            return dBservices.DeleteOrder(id);
        }
    }
}
