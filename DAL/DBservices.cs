using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Server.Moodle;
using System.Globalization;

namespace Server.Moodle.DAL
{
    public class DBservices
    {

        public DBservices()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        //--------------------------------------------------------------------------------------------------
        // This method Inserts a student to the student table 
        //--------------------------------------------------------------------------------------------------
        public bool InserFlatToDB(Flat flat)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@FlatID", flat.Id);
            paramDic.Add("@City", flat.City);
            paramDic.Add("@Neighbourhood", flat.Neighbourhood);
            paramDic.Add("@Price", flat.Price);
            paramDic.Add("@BedRooms", flat.Bedrooms);
            paramDic.Add("@Picture_url", flat.Picture_url);
            paramDic.Add("@Name", flat.Name);
            paramDic.Add("@Description", flat.Description);
            paramDic.Add("@Review_scores_rating", flat.Review_scores_rating);
            paramDic.Add("@UserId", flat.UserId);

            cmd = CreateCommandWithStoredProcedure("SP_Insert_Flat", con, paramDic);             // create the command
            // Set up the output parameter
            SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit);
            isSuccessParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(isSuccessParam);


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                bool isSuccess = (bool)isSuccessParam.Value;
                return isSuccess;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        /* Read all the Flats that less than equal to the price*/
        public List<Flat> GetFlatsLessThanEqualThePrice(double price, int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Price", price);
            paramDic.Add("@UserId", id);


            cmd = CreateCommandWithStoredProcedure("SP_Get_Flat_By_Price", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            List<Flat> fLatsList = new List<Flat>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    
                    Flat f = new Flat(
                        Convert.ToInt32(dataReader["UserId"]),
                        Convert.ToInt32(dataReader["FlatID"]),
                        dataReader["Neighbourhood"].ToString(),
                        Convert.ToDouble(dataReader["Price"]),
                        Convert.ToInt32(dataReader["BedRooms"]),
                        dataReader["Picture_url"].ToString(),
                        dataReader["Description"].ToString(),
                        dataReader["Name"].ToString(),
                        float.Parse(dataReader["Review_scores_rating"].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    );
                    fLatsList.Add(f);
                }
                return fLatsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        /* Read all the Flats that less than equal to the price*/
        public List<Flat> GetFlatsGreaterThanEqualTheRating(float Review_scores_rating, int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Review_scores_rating", Review_scores_rating);
            paramDic.Add("@UserId", id);


            cmd = CreateCommandWithStoredProcedure("SP_Get_Flat_By_Rating", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            List<Flat> fLatsList = new List<Flat>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {

                    Flat f = new Flat(
                        Convert.ToInt32(dataReader["UserId"]),
                        Convert.ToInt32(dataReader["FlatID"]),
                        dataReader["Neighbourhood"].ToString(),
                        Convert.ToDouble(dataReader["Price"]),
                        Convert.ToInt32(dataReader["BedRooms"]),
                        dataReader["Picture_url"].ToString(),
                        dataReader["Description"].ToString(),
                        dataReader["Name"].ToString(),
                        float.Parse(dataReader["Review_scores_rating"].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    );
                    fLatsList.Add(f);
                }
                return fLatsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        /* Read all the Flats that less than equal to the price*/
        public List<Order> GetOrdersByUserID(int userID)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserID", userID);


            cmd = CreateCommandWithStoredProcedure("[SP_Get_Orders_for_UserID]", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            List<Order> ordersList = new List<Order>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Order o = new Order(
                        Convert.ToInt32(dataReader["OrderId"]),
                        Convert.ToInt32(dataReader["UserId"]),
                        Convert.ToInt32(dataReader["FlatId"]),
                        DateTime.Parse(dataReader["StartDate"].ToString()),
                        DateTime.Parse(dataReader["EndDate"].ToString()),
                        Convert.ToInt32(dataReader["PricePerNight"])
                    );
                    ordersList.Add(o);
                }
                return ordersList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }

        /* Read all the Flats that less than equal to the price*/
        public List<Order> getAllOrders()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("[SP_Get_All_Orders]", con, null);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            List<Order> ordersList = new List<Order>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Order o = new Order(
                        Convert.ToInt32(dataReader["OrderId"]),
                        Convert.ToInt32(dataReader["UserId"]),
                        Convert.ToInt32(dataReader["FlatId"]),
                        DateTime.Parse(dataReader["StartDate"].ToString()),
                        DateTime.Parse(dataReader["EndDate"].ToString()),
                        Convert.ToInt32(dataReader["PricePerNight"])
                    );
                    ordersList.Add(o);
                }
                return ordersList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        /* Read all the Flats that less than equal to the price*/
        public Order getOrderById(int orderId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@OrderId", orderId);


            cmd = CreateCommandWithStoredProcedure("[SP_Get_Order_By_Id]", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    Order o = new Order(
                       Convert.ToInt32(dataReader["OrderId"]),
                       Convert.ToInt32(dataReader["UserId"]),
                       Convert.ToInt32(dataReader["FlatId"]),
                       DateTime.Parse(dataReader["StartDate"].ToString()),
                       DateTime.Parse(dataReader["EndDate"].ToString()),
                       Convert.ToInt32(dataReader["PricePerNight"])
                   ); ;
                    return o;
                }
                throw new Exception("Order not found");

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        
        public Flat getFlatById(int flatId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@FLatId", flatId);


            cmd = CreateCommandWithStoredProcedure("[SP_Get_Flat_By_Id]", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;

            SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit);
            isSuccessParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(isSuccessParam);


            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    Flat f = new Flat(
                       Convert.ToInt32(dataReader["UserId"]),
                       Convert.ToInt32(dataReader["FlatID"]),
                       dataReader["Neighbourhood"].ToString(),
                       Convert.ToDouble(dataReader["Price"]),
                       Convert.ToInt32(dataReader["BedRooms"]),
                       dataReader["Picture_url"].ToString(),
                       dataReader["Description"].ToString(),
                       dataReader["Name"].ToString(),
                       float.Parse(dataReader["Review_scores_rating"].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                   );
                   return f;
                }
                throw new Exception("Flat not found");
                /*
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                bool isSuccess = (bool)isSuccessParam.Value;
                return isSuccess;
                */
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        
        /* Read all the Flats that less than equal to the price*/
        public List<Flat> GetAllFlatsFromDB(int id )
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", id);

            cmd = CreateCommandWithStoredProcedure("SP_Get_All_Flats", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            List<Flat> fLatsList = new List<Flat>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {

                    Flat f = new Flat(
                        Convert.ToInt32(dataReader["UserId"]),
                        Convert.ToInt32(dataReader["FlatID"]),
                        dataReader["Neighbourhood"].ToString(),
                        Convert.ToDouble(dataReader["Price"]),
                        Convert.ToInt32(dataReader["BedRooms"]),
                        dataReader["Picture_url"].ToString(),
                        dataReader["Description"].ToString(),
                        dataReader["Name"].ToString(),
                        float.Parse(dataReader["Review_scores_rating"].ToString(), CultureInfo.InvariantCulture.NumberFormat)
                    );
                    fLatsList.Add(f);
                }
                return fLatsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        public WebUser getUserById(string id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@UserId", Convert.ToInt32(id));


            cmd = CreateCommandWithStoredProcedure("[SP_Get_User_By_Id]", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;




            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    WebUser u = new WebUser(dataReader["First"].ToString(), dataReader["Last"].ToString(), dataReader["Id"].ToString(), dataReader["Country"].ToString(), dataReader["Email"].ToString(), dataReader["Password"].ToString(), dataReader["PhoneNumber"].ToString(), dataReader["ImgUrl"].ToString());
                    return u;
                }
                throw new Exception("User not found");

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method uses the return user
        //--------------------------------------------------------------------------------------------------
        public WebUser GetByemail(string email)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@email", email);


            cmd = CreateCommandWithStoredProcedure("SP_GetByemail", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    WebUser u = new WebUser(dataReader["First"].ToString(), dataReader["Last"].ToString(), dataReader["Id"].ToString(), dataReader["Country"].ToString(), dataReader["Email"].ToString(), dataReader["Password"].ToString(), dataReader["PhoneNumber"].ToString(), dataReader["ImgUrl"].ToString());
                    return u;
                }
                throw new Exception("User not found");

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method uses the return user with email and password
        //--------------------------------------------------------------------------------------------------
        public WebUser LogInPost(string email,string password)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@email", email);
            paramDic.Add("@password", password);


            cmd = CreateCommandWithStoredProcedure("SP_LogInPost", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;


            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    WebUser u = new WebUser(dataReader["First"].ToString(), dataReader["Last"].ToString(), dataReader["Id"].ToString(), dataReader["Country"].ToString(), dataReader["Email"].ToString(), dataReader["Password"].ToString(), dataReader["PhoneNumber"].ToString(), dataReader["ImgUrl"].ToString());
                    return u;
                }
                throw new Exception("User not found");

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method Reads all Flats
        //--------------------------------------------------------------------------------------------------
        public List<WebUser> GetAll()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            cmd = CreateCommandWithStoredProcedure("SP_GetAll", con, null);             // create the command


            List<WebUser> userList = new List<WebUser>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    string First = dataReader["First"].ToString();
                    string Last = dataReader["Last"].ToString();
                    string Id = dataReader["Id"].ToString();
                    string Country = dataReader["Country"].ToString();
                    string Email = dataReader["Email"].ToString();
                    string Password = dataReader["Password"].ToString();
                    string PhoneNumber = dataReader["PhoneNumber"].ToString();
                    string ImgUrl = dataReader["ImgUrl"].ToString();
                    WebUser u = new WebUser(First, Last, Id, Country, Email, Password, PhoneNumber,ImgUrl);
                    userList.Add(u);
                }
                return userList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method Inserts a student to the student table 
        //--------------------------------------------------------------------------------------------------
        public bool InsertUser(WebUser user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@First", user.First);
            paramDic.Add("@Last", user.Last);
            paramDic.Add("@Country", user.Country);
            paramDic.Add("@Email", user.Email);
            paramDic.Add("@Password", user.Password);
            paramDic.Add("@PhoneNumber", user.PhoneNumber);
            paramDic.Add("@ImgUrl", user.Profile_img);

            cmd = CreateCommandWithStoredProcedure("SP_InsertUser", con, paramDic);             // create the command
                                                                                                // Set up the output parameter
            SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit);
            isSuccessParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(isSuccessParam);


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                bool isSuccess = (bool)isSuccessParam.Value;
                return isSuccess;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method Inserts a student to the student table 
        //--------------------------------------------------------------------------------------------------
        public bool InsertOrderToDB(Order order)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@StartDate", order.StartDate);
            paramDic.Add("@EndDate", order.EndDate);
            paramDic.Add("@FlatId", order.FlatId);
            paramDic.Add("@UserId", order.UserId);
            paramDic.Add("@PricePerNight", order.PricePerNight);

            cmd = CreateCommandWithStoredProcedure("SP_Insert_Order", con, paramDic);             // create the command
            SqlParameter isSuccessParam = new SqlParameter("@IsSuccess", SqlDbType.Bit);
            isSuccessParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(isSuccessParam);


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                bool isSuccess = (bool)isSuccessParam.Value;
                return isSuccess;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method Delete a user by email 
        //--------------------------------------------------------------------------------------------------
        public bool DeleteUser(string email)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Email", email);



            cmd = CreateCommandWithStoredProcedure("SP_DeleteUser", con, paramDic);             // create the command


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return Convert.ToBoolean(numEffected) ? Convert.ToBoolean(numEffected) : throw new Exception("User Not found");
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method Delete a user by email 
        //--------------------------------------------------------------------------------------------------
        public bool DeleteFlatById(int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@FlatID", id);



            cmd = CreateCommandWithStoredProcedure("SP_Delete_Flat_By_Id", con, paramDic);             // create the command


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return Convert.ToBoolean(numEffected) ? Convert.ToBoolean(numEffected) : throw new Exception("Flat Not found");
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method update a user to the user table 
        //--------------------------------------------------------------------------------------------------
        public bool UpdateUser(WebUser user)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@First", user.First);
            paramDic.Add("@Last", user.Last);
            paramDic.Add("@Id", user.Id);
            paramDic.Add("@Country", user.Country);
            paramDic.Add("@Email", user.Email);
            paramDic.Add("@Password", user.Password);
            paramDic.Add("@PhoneNumber", user.PhoneNumber);
            paramDic.Add("@ImgUrl" , user.Profile_img);


            cmd = CreateCommandWithStoredProcedure("SP_UpdateUser", con, paramDic);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return Convert.ToBoolean(numEffected) ? Convert.ToBoolean(numEffected) : throw new Exception("User Not found");
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        //--------------------------------------------------------------------------------------------------
        // This method update a order to the orders table 
        //--------------------------------------------------------------------------------------------------

        public int UpdateOrder(Order order)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@StartDate", order.StartDate);
            paramDic.Add("@EndDate", order.EndDate);
            paramDic.Add("@OrderId", order.Id);
            paramDic.Add("@PricePerNight", order.PricePerNight);


            cmd = CreateCommandWithStoredProcedure("SP_UpdateOrder", con, paramDic);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method Delete a Order by id 
        //--------------------------------------------------------------------------------------------------
        public bool DeleteOrder(int id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@OrderId", id);



            cmd = CreateCommandWithStoredProcedure("SP_DeleteOrderById", con, paramDic);             // create the command


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return Convert.ToBoolean(numEffected) ? Convert.ToBoolean(numEffected) : throw new Exception("Order not found");
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        
        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }
        //--------------------------------------------------------------------------------------------------
        // this method check the key and the date 
        //--------------------------------------------------------------------------------------------------
        public WebUser checkIfKeyCorrect(string key, string id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@key", key);
            paramDic.Add("@date", DateTime.Now);
            paramDic.Add("@userId", int.Parse(id));


            cmd = CreateCommandWithStoredProcedure("SP_Reset_password_With_Key", con, paramDic);             // create the command
            var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

            returnParameter.Direction = ParameterDirection.ReturnValue;




            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    WebUser u = new WebUser(dataReader["First"].ToString(), dataReader["Last"].ToString(), dataReader["Id"].ToString(), dataReader["Country"].ToString(), dataReader["Email"].ToString(), dataReader["Password"].ToString(), dataReader["PhoneNumber"].ToString() , dataReader["ImgUrl"].ToString());
                    return u;
                }
                throw new Exception("Key is wrong or expired");

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
                // note that the return value appears only after closing the connection
                var result = returnParameter.Value;
            }

        }
        //--------------------------------------------------------------------------------------------------
        // This method Delete a Order by id 
        //--------------------------------------------------------------------------------------------------
        public bool SetKeyAndDate(string key , DateTime date, string id)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@key", key);
            paramDic.Add("@date", date);
            paramDic.Add("@UserId", int.Parse(id));



            cmd = CreateCommandWithStoredProcedure("SP_SetKeyAndExpiredDate", con, paramDic);             // create the command


            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return Convert.ToBoolean(numEffected) ? Convert.ToBoolean(numEffected) : throw new Exception("Something wrong");
                
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

    }
}
