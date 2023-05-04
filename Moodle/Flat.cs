using Server.Moodle.DAL;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Server.Moodle
{
    public class Flat
    {
        public int UserId { get; private set; }
        public int Id { get; private set; }
        public string City { get; private set; }
        public string Neighbourhood { get; private set; }
        private double price;

        public Flat(int userId,int id, string neighbourhood, double price, int bedrooms, string picture_url, string description, string name, float review_scores_rating)
        {
            UserId = userId;
            Id = id;
            City = "Amsterdam";
            Neighbourhood = neighbourhood;
            Price = price;
            Bedrooms = bedrooms;
            Picture_url = picture_url;
            Description = description;
            Name = name;
            Review_scores_rating = review_scores_rating;
        }

        public double Price
        {
            get { return price; }
            private set
            {
                if (value > 100 && Bedrooms > 1)
                    price = value * 0.9; // apply 10% discount
                else
                    price = value;
            }
        }
        public int Bedrooms { get; private set; }
        public string Picture_url { get; private set; }
        public string Description { get; private set; }
        public string Name { get; private set; }
        public float Review_scores_rating { get; private set; }


        //constructor



        public bool Insert()
        {
            DBservices dBservices = new DBservices();
            return dBservices.InserFlatToDB(this);
        }
        // return the flat list
        public static List<Flat> Read(int id)
        {
            DBservices dBservices = new DBservices();
            return dBservices.GetAllFlatsFromDB(id);
        }
        
        public static Flat? ReadByFlatId(int id)
        {
            DBservices dBservices = new DBservices();
            return dBservices.getFlatById(id);
        }
        
        public static List<Flat> GetByPrice(double price,int id)
        {
            DBservices dBservices = new DBservices();
            return dBservices.GetFlatsLessThanEqualThePrice(price, id);
        }

        public static List<Flat> GetByCityRating(float rating, int id)
        {
            DBservices dBservices = new DBservices();
            return dBservices.GetFlatsGreaterThanEqualTheRating(rating,id);
        }

        public static bool DeleteById(int id)
        {
            DBservices dBservices = new DBservices();
            return dBservices.DeleteFlatById(id);
           
        }

        // give discount of 10%
        public double Discount(double price)
        {
            return price * 0.9;
        }

    }
}
