using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Pizza
{
    public partial class Form1 : Form
    {

        private Dictionary<int, string> pizzaNames = new Dictionary<int, string>()
        {
            { 1, "Margarinás" },
            { 2, "Sonku" },
            { 3, "Songo" },
            { 4, "Songoku" },
            { 5, "Paraszt" },
            { 6, "Ultra Paraszt" },
            { 7, "Tenger gyümi" },
            { 8, "Husi" },
            { 9, "Tonyás" },
            { 10, "Vega" },
            { 11, "Négysajtos" },
            { 12, "Hawaii" }
        };

        public class Order
        {
            public int PizzaID { get; set; }
            public int Quantity { get; set; }
            public int Price { get; set; }
        }

        public int Kos = 0;
        public List<Order> orders = new List<Order>();
        private string currentUser = "";
        private int privilegeLevel = 0;
        private readonly string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=PizzaData;Integrated Security=True;Pooling=False;Connect Timeout=30";

        public Form1()
        {
            InitializeComponent();
            SetupPieChart();
        }

        private void SetupPieChart()
        {
            comboBox2.Items.Add("Minden");
            comboBox2.Items.Add("Ma");
            comboBox2.Items.Add("Héten");
            comboBox2.Items.Add("Hónapban");
            comboBox2.SelectedIndex = 0;
            string selectedFilter = comboBox2.SelectedItem.ToString();

            var model = new PlotModel { Title = "Teljesített Rendelések" };
            var series = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelFormat = "{1}: {0}db"
            };
            string query = "";

            switch (selectedFilter)
            {
                case "Minden":
                    query = "SELECT PizzaID, COUNT(*) AS OrderCount FROM Rendelesek WHERE Allapot = 'Teljesített' GROUP BY PizzaID";
                    break;
                case "Ma":
                    query = "SELECT PizzaID, COUNT(*) AS OrderCount FROM Rendelesek WHERE OrderDate = CAST(GETDATE() AS DATE) AND Allapot = 'Teljesített' GROUP BY PizzaID";
                    break;
                case "Héten":
                    query = "SELECT PizzaID, COUNT(*) AS OrderCount FROM Rendelesek WHERE DATEPART(week, OrderDate) = DATEPART(week, GETDATE()) AND DATEPART(year, OrderDate) = DATEPART(year, GETDATE()) AND Allapot = 'Teljesített' GROUP BY PizzaID";
                    break;
                case "Hónapban":
                    query = "SELECT PizzaID, COUNT(*) AS OrderCount FROM Rendelesek WHERE MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE()) AND Allapot = 'Teljesített' GROUP BY PizzaID";
                    break;
            }

            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=PizzaData;Integrated Security=True;Pooling=False;Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Populate PieSeries
                while (reader.Read())
                {
                    int pizzaId = reader.GetInt32(reader.GetOrdinal("PizzaID"));
                    int orderCount = reader.GetInt32(reader.GetOrdinal("OrderCount"));

                    // Get pizza name from dictionary
                    string pizzaName = GetPizzaName(pizzaId);

                    // Add data to PieSeries
                    series.Slices.Add(new PieSlice($"{pizzaName}", orderCount));
                }

                reader.Close();
            }

            model.Series.Add(series);
            plotView1.Model = model;
        }

        private string GetPizzaName(int pizzaId)
        {
            if (pizzaNames.ContainsKey(pizzaId))
            {
                return pizzaNames[pizzaId];
            }
            else
            {
                return "Unknown";
            }
        }

        private void AddOrder(int pizzaID, int quantity, int price)
        {
            orders.Add(new Order() { PizzaID = pizzaID, Quantity = quantity, Price = price });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            flowLayoutPanelOrders.Visible = false;
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = false;
            flowLayoutPanel3.Visible = false;
            flowLayoutPanel4.Visible = false;
            flowLayoutPanel5.Visible = false;
            flowLayoutPanel6.Visible = false;
            PizzaPizza.Visible = false;
            Rolunklabel.Visible = false;
            Statisztika.Enabled = false;
            Kos1.Visible = false;
            Kos2.Visible = false;
            Kos3.Visible = false;
            Kos11.Visible = false;
            Kos22.Visible = false;
            Kos33.Visible = false;
            Kos111.Visible = false;
            Kos222.Visible = false;
            Kos333.Visible = false;
            Kos1111.Visible = false;
            Kos2222.Visible = false;
            Kos3333.Visible = false;
            Kos11111.Visible = false;
            Kos22222.Visible = false;
            Kos33333.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            Filterlbl.Visible = false;
            plotView1.Visible = false;
        }

        private void Megrendeles_Click(object sender, EventArgs e)
        {
            String ConString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=PizzaData;Integrated Security=True;Pooling=False;Connect Timeout=30";
            SqlConnection con = new SqlConnection(ConString);
            try
            {
                con.Open();
                SqlCommand sc;
                foreach (Order order in orders)
                {
                    String sql = "INSERT INTO Rendelesek (Nev,Cim,Email,Telefonszam,PizzaID,PizzaDB,Reszosszeg,Allapot) " +
                        "VALUES ('" + Nevtxt.Text + "','" + Szallitastxt.Text + "','" + Emailtxt.Text + "','" + Telefontxt.Text + "'," + order.PizzaID + "," + order.Quantity + "," + order.Price * order.Quantity + ",'Felvett'" + ")";
                    sc = new SqlCommand(sql, con);
                    sc.ExecuteNonQuery();
                    sc.Dispose();
                }
                MessageBox.Show("Sikeres megrendelés! A megrendelés megtekinthető a Rendelések menüpontban");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        private void Rolunk_Click(object sender, EventArgs e)
        {
            flowLayoutPanelOrders.Visible = false;
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = false;
            flowLayoutPanel3.Visible = false;
            flowLayoutPanel4.Visible = false;
            flowLayoutPanel5.Visible = false;
            flowLayoutPanel6.Visible = false;
            PizzaPizza.Visible = true;
            Rolunklabel.Visible = true;
            Loginbtn.Visible = true;
            Registerbtn.Visible = true;
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            Filterlbl.Visible = false;
            plotView1.Visible = false;
        }

        private void Etlap_Click(object sender, EventArgs e)
        {
            flowLayoutPanelOrders.Visible = false;
            flowLayoutPanel1.Visible = true;
            flowLayoutPanel2.Visible = false;
            flowLayoutPanel3.Visible = false;
            flowLayoutPanel4.Visible = false;
            flowLayoutPanel5.Visible = false;
            flowLayoutPanel6.Visible = false;
            PizzaPizza.Visible = false;
            Rolunklabel.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            Filterlbl.Visible = false;
            plotView1.Visible = false;
        }

        private void Kosar_Click(object sender, EventArgs e)
        {
            flowLayoutPanelOrders.Visible = false;
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = false;
            flowLayoutPanel3.Visible = true;
            flowLayoutPanel4.Visible = true;
            flowLayoutPanel5.Visible = true;
            flowLayoutPanel6.Visible = true;
            PizzaPizza.Visible = false;
            Rolunklabel.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            Filterlbl.Visible = false;
            plotView1.Visible = false;
        }

        private void Rendelesek_Click(object sender, EventArgs e)
        {
            flowLayoutPanelOrders.Visible = false;
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = false;
            flowLayoutPanel3.Visible = false;
            flowLayoutPanel4.Visible = false;
            flowLayoutPanel5.Visible = false;
            flowLayoutPanel6.Visible = false;
            PizzaPizza.Visible = false;
            Rolunklabel.Visible = false;
            plotView1.Visible = false;
            comboBox2.Visible = false;

            if (privilegeLevel == 1)
            {
                bool isAdmin = true;
                flowLayoutPanelOrders.Controls.Clear();
                comboBox1.Visible = true;
                Filterlbl.Visible = true;

                if (comboBox1.Items.Count == 0)
                {
                    comboBox1.Items.Add("Minden");
                    comboBox1.Items.Add("Felvett");
                    comboBox1.Items.Add("Kiszállítás alatt");
                    comboBox1.Items.Add("Teljesített");
                    comboBox1.Items.Add("Meghiúsult");
                }
                comboBox1.SelectedIndex = 0;
                flowLayoutPanelOrders.Visible = true;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query;
                    SqlCommand command;
                    string statusFilter = comboBox1.SelectedItem?.ToString(); // Get the selected filter option

                    if (!string.IsNullOrEmpty(statusFilter))
                    {
                        if (statusFilter == "Minden")
                        {
                            query = "SELECT * FROM Rendelesek"; // Fetch all orders
                            command = new SqlCommand(query, connection);
                        }
                        else
                        {
                            query = "SELECT * FROM Rendelesek WHERE Allapot = @StatusFilter"; // Modify the query based on the selected filter
                            command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@StatusFilter", statusFilter);
                        }
                    }
                    else
                    {
                        query = "SELECT * FROM Rendelesek"; // Fetch all orders
                        command = new SqlCommand(query, connection);
                    }

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                            string name = reader.GetString(reader.GetOrdinal("Nev"));
                            string address = reader.GetString(reader.GetOrdinal("Cim"));
                            string email = reader.GetString(reader.GetOrdinal("Email"));
                            string phoneNumber = reader.GetString(reader.GetOrdinal("Telefonszam"));
                            int pizzaId = reader.GetInt32(reader.GetOrdinal("PizzaID"));
                            int pizzaDb = reader.GetInt32(reader.GetOrdinal("PizzaDB"));
                            int totalPrice = reader.GetInt32(reader.GetOrdinal("Reszosszeg"));
                            string status = reader.GetString(reader.GetOrdinal("Allapot"));

                            OrderItemControl orderItem = new OrderItemControl(orderId, name, address, email, phoneNumber, pizzaId, pizzaDb, totalPrice, status, isAdmin);
                            flowLayoutPanelOrders.Controls.Add(orderItem);
                        }

                        reader.Close();

                        if (flowLayoutPanelOrders.Controls.Count == 0)
                        {
                            MessageBox.Show("Nem találtunk rendelést.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hiba történt a rendelések betöltése közben: " + ex.Message);
                    }
                }
            }
            else // Privilige level 0
            {
                RefreshOrderList();
            }
        }

        private void AddOrderAndUpdateLabels(int pizzaType, NumericUpDown numericUpDown, int price)
        {
            AddOrder(pizzaType, Convert.ToInt32(numericUpDown.Value), price);

            switch (Kos)
            {
                case 0:
                    Kos1.Text = $"{GetPizzaTypeName(pizzaType)} Pizza";
                    Kos2.Text = numericUpDown.Text;
                    Kos3.Text = $"{numericUpDown.Value * price} Ft";
                    Kos1.Visible = true;
                    Kos2.Visible = true;
                    Kos3.Visible = true;
                    break;
                case 1:
                    Kos11.Text = $"{GetPizzaTypeName(pizzaType)} Pizza";
                    Kos22.Text = numericUpDown.Text;
                    Kos33.Text = $"{numericUpDown.Value * price} Ft";
                    Kos11.Visible = true;
                    Kos22.Visible = true;
                    Kos33.Visible = true;
                    break;
                case 2:
                    Kos111.Text = $"{GetPizzaTypeName(pizzaType)} Pizza";
                    Kos222.Text = numericUpDown.Text;
                    Kos333.Text = $"{numericUpDown.Value * price} Ft";
                    Kos111.Visible = true;
                    Kos222.Visible = true;
                    Kos333.Visible = true;
                    break;
                case 3:
                    Kos1111.Text = $"{GetPizzaTypeName(pizzaType)} Pizza";
                    Kos2222.Text = numericUpDown.Text;
                    Kos3333.Text = $"{numericUpDown.Value * price} Ft";
                    Kos1111.Visible = true;
                    Kos2222.Visible = true;
                    Kos3333.Visible = true;
                    break;
                case 4:
                    Kos11111.Text = $"{GetPizzaTypeName(pizzaType)} Pizza";
                    Kos22222.Text = numericUpDown.Text;
                    Kos33333.Text = $"{numericUpDown.Value * price} Ft";
                    Kos11111.Visible = true;
                    Kos22222.Visible = true;
                    Kos33333.Visible = true;
                    break;
            }
            Kos++;
        }

        private string GetPizzaTypeName(int pizzaType)
        {
            switch (pizzaType)
            {
                case 1:
                    return "Margarinás";
                case 2:
                    return "Sonku";
                case 3:
                    return "Songo";
                case 4:
                    return "Songoku";
                case 5:
                    return "Paraszt";
                case 6:
                    return "Ultra Paraszt";
                case 7:
                    return "Tenger gyümi";
                case 8:
                    return "Husi";
                case 9:
                    return "Tonyás";
                case 10:
                    return "Vega";
                case 11:
                    return "Négysajtos";
                case 12:
                    return "Hawaii";
                default:
                    return "Unknown";
            }
        }

        private void Statisztika_Click(object sender, EventArgs e)
        {
            flowLayoutPanelOrders.Visible = false;
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = false;
            flowLayoutPanel3.Visible = false;
            flowLayoutPanel4.Visible = false;
            flowLayoutPanel5.Visible = false;
            flowLayoutPanel6.Visible = false;
            PizzaPizza.Visible = false;
            Rolunklabel.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = true;
            Filterlbl.Visible = true;
            plotView1.Visible = true;
        }

        private void Ertesitesek_Click(object sender, EventArgs e)
        {
            flowLayoutPanelOrders.Visible = false;
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = false;
            flowLayoutPanel3.Visible = false;
            flowLayoutPanel4.Visible = false;
            flowLayoutPanel5.Visible = false;
            flowLayoutPanel6.Visible = false;
            PizzaPizza.Visible = false;
            Rolunklabel.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            Filterlbl.Visible = false;
            plotView1.Visible = false;
        }

        private void Adataim_Click(object sender, EventArgs e)
        {
            flowLayoutPanelOrders.Visible = false;
            flowLayoutPanel1.Visible = false;
            flowLayoutPanel2.Visible = true;
            flowLayoutPanel3.Visible = false;
            flowLayoutPanel4.Visible = false;
            flowLayoutPanel5.Visible = false;
            flowLayoutPanel6.Visible = false;
            PizzaPizza.Visible = false;
            Rolunklabel.Visible = false;
            comboBox1.Visible = false;
            comboBox2.Visible = false;
            Filterlbl.Visible = false;
            plotView1.Visible = false;
            LoadUserInfo();
        }

        private void MargarinaKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(1, Margarinadb, 1800);
        }

        private void SonkuKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(2, Sonkudb, 2100);
        }

        private void SongoKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(3, Songodb, 2200);
        }

        private void SongokuKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(4, Songokudb, 2400);
        }

        private void ParasztKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(5, Parasztdb, 2600);
        }

        private void UParasztKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(6, Uparasztdb, 2900);
        }

        private void TengerKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(7, Tengerdb, 3000);
        }

        private void HusiKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(8, Husidb, 2900);
        }

        private void TonyasKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(9, Tonyasdb, 2500);
        }

        private void VegaKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(10, Vegadb, 2300);
        }

        private void NegysajtKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(11, Negysajtdb, 2400);
        }

        private void HawaiiKos_Click(object sender, EventArgs e)
        {
            AddOrderAndUpdateLabels(12, Hawaiidb, 6000);
        }

        private void Loginbtn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Felhasználónév és jelszó használata kötelező!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*), MAX(PrivilegeLevel) AS PrivilegeLevel FROM Felhasznalo WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        int count = (int)reader[0];
                        privilegeLevel = (int)reader["PrivilegeLevel"];
                        if (count > 0)
                        {
                            currentUser = username;
                            LoadUserInfo();
                            MessageBox.Show("Sikeres bejelentkezés!");
                            Loginbtn.Visible = false;
                            Registerbtn.Visible = false;
                            flowLayoutPanel7.Visible = false;
                            PizzaPizza.Visible = true;
                            Rolunklabel.Visible = true;

                            if (privilegeLevel == 1)
                            {
                                Statisztika.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Helytelen felhasználónév vagy jelszó.");
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bejelentkezési hiba: " + ex.Message);
                }
            }
        }

        private void Registerbtn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Felhasználónév és jelszó használata kötelező!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Felhasznalo (PrivilegeLevel, Username, Password) VALUES (0, @Username, @Password)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Sikeres regisztráció!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A regisztráció meghiúsult: " + ex.Message);
                }
            }
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            string name = Nevtxt.Text;
            string email = Emailtxt.Text;
            string phone = Telefontxt.Text;
            string address = Szallitastxt.Text;

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                MessageBox.Show("Elöször jelentkezz be.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Felhasznalo SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber, DeliveryAddress = @DeliveryAddress WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PhoneNumber", phone);
                command.Parameters.AddWithValue("@DeliveryAddress", address);
                command.Parameters.AddWithValue("@Username", currentUser);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Felhasználói információk sikeresen elmentve!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba felhasználói információk mentése közben: " + ex.Message);
                }
            }
        }

        private void LoadUserInfo()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Name, Email, PhoneNumber, DeliveryAddress FROM Felhasznalo WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", currentUser);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Nevtxt.Text = reader["Name"].ToString();
                        Emailtxt.Text = reader["Email"].ToString();
                        Telefontxt.Text = reader["PhoneNumber"].ToString();
                        Szallitastxt.Text = reader["DeliveryAddress"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba felhasználói betöltése közben: " + ex.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (privilegeLevel == 1)
            {
                bool isAdmin = true;
                flowLayoutPanelOrders.Controls.Clear();
                comboBox1.Visible = true;
                Filterlbl.Visible = true;

                if (comboBox1.Items.Count == 0)
                {
                    comboBox1.Items.Add("Minden");
                    comboBox1.Items.Add("Felvett");
                    comboBox1.Items.Add("Kiszállítás alatt");
                    comboBox1.Items.Add("Teljesített");
                    comboBox1.Items.Add("Meghiúsult");
                }
                flowLayoutPanelOrders.Visible = true;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query;
                    SqlCommand command;
                    string statusFilter = comboBox1.SelectedItem?.ToString(); // Get the selected filter option

                    if (!string.IsNullOrEmpty(statusFilter))
                    {
                        if (statusFilter == "Minden")
                        {
                            query = "SELECT * FROM Rendelesek"; // Fetch all orders
                            command = new SqlCommand(query, connection);
                        }
                        else
                        {
                            query = "SELECT * FROM Rendelesek WHERE Allapot = @StatusFilter"; // Modify the query based on the selected filter
                            command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@StatusFilter", statusFilter);
                        }
                    }
                    else
                    {
                        query = "SELECT * FROM Rendelesek"; // Fetch all orders
                        command = new SqlCommand(query, connection);
                    }

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                            string name = reader.GetString(reader.GetOrdinal("Nev"));
                            string address = reader.GetString(reader.GetOrdinal("Cim"));
                            string email = reader.GetString(reader.GetOrdinal("Email"));
                            string phoneNumber = reader.GetString(reader.GetOrdinal("Telefonszam"));
                            int pizzaId = reader.GetInt32(reader.GetOrdinal("PizzaID"));
                            int pizzaDb = reader.GetInt32(reader.GetOrdinal("PizzaDB"));
                            int totalPrice = reader.GetInt32(reader.GetOrdinal("Reszosszeg"));
                            string status = reader.GetString(reader.GetOrdinal("Allapot"));

                            OrderItemControl orderItem = new OrderItemControl(orderId, name, address, email, phoneNumber, pizzaId, pizzaDb, totalPrice, status, isAdmin);
                            flowLayoutPanelOrders.Controls.Add(orderItem);
                        }

                        reader.Close();

                        if (flowLayoutPanelOrders.Controls.Count == 0)
                        {
                            MessageBox.Show("Nem találtunk rendelést.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nem sikerült a rendelések betöltése: " + ex.Message);
                    }
                }
            }
        }

        public void RefreshOrderList()
        {
            flowLayoutPanelOrders.Controls.Clear();
            flowLayoutPanelOrders.Visible = true;
            bool isAdmin = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Rendelesek WHERE Allapot <> 'Meghiúsult' AND Email = @Email"; // Fetch orders for the current user's email
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", Emailtxt.Text);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                        string name = reader.GetString(reader.GetOrdinal("Nev"));
                        string address = reader.GetString(reader.GetOrdinal("Cim"));
                        string email = reader.GetString(reader.GetOrdinal("Email"));
                        string phoneNumber = reader.GetString(reader.GetOrdinal("Telefonszam"));
                        int pizzaId = reader.GetInt32(reader.GetOrdinal("PizzaID"));
                        int pizzaDb = reader.GetInt32(reader.GetOrdinal("PizzaDB"));
                        int totalPrice = reader.GetInt32(reader.GetOrdinal("Reszosszeg"));
                        string status = reader.GetString(reader.GetOrdinal("Allapot"));

                        OrderItemControl orderItem = new OrderItemControl(orderId, name, address, email, phoneNumber, pizzaId, pizzaDb, totalPrice, status, isAdmin);
                        flowLayoutPanelOrders.Controls.Add(orderItem);
                    }

                    reader.Close();

                    if (flowLayoutPanelOrders.Controls.Count == 0)
                    {
                        MessageBox.Show("Nincsenek rendeléseid.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nem sikerült a rendeléseid betölteni: " + ex.Message);
                }
            }
        }

        private void DelKos_Click(object sender, EventArgs e)
        {
            Kos1.Visible = false;
            Kos2.Visible = false;
            Kos3.Visible = false;
            Kos11.Visible = false;
            Kos22.Visible = false;
            Kos33.Visible = false;
            Kos111.Visible = false;
            Kos222.Visible = false;
            Kos333.Visible = false;
            Kos1111.Visible = false;
            Kos2222.Visible = false;
            Kos3333.Visible = false;
            Kos11111.Visible = false;
            Kos22222.Visible = false;
            Kos33333.Visible = false;

            Kos1.Text = "";
            Kos2.Text = "";
            Kos3.Text = "";
            Kos11.Text = "";
            Kos22.Text = "";
            Kos33.Text = "";
            Kos111.Text = "";
            Kos222.Text = "";
            Kos333.Text = "";
            Kos1111.Text = "";
            Kos2222.Text = "";
            Kos3333.Text = "";
            Kos11111.Text = "";
            Kos22222.Text = "";
            Kos33333.Text = "";

            Kos = 0;
            orders.Clear();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFilter = comboBox2.SelectedItem.ToString();

            var model = new PlotModel { Title = "Teljesített Rendelések" };
            var series = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0,
                InsideLabelFormat = "{1}: {0}db"
            };
            string query = "";

            switch (selectedFilter)
            {
                case "Minden":
                    query = "SELECT PizzaID, COUNT(*) AS OrderCount FROM Rendelesek WHERE Allapot = 'Teljesített' GROUP BY PizzaID";
                    break;
                case "Ma":
                    query = "SELECT PizzaID, COUNT(*) AS OrderCount FROM Rendelesek WHERE OrderDate = CAST(GETDATE() AS DATE) AND Allapot = 'Teljesített' GROUP BY PizzaID";
                    break;
                case "Héten":
                    query = "SELECT PizzaID, COUNT(*) AS OrderCount FROM Rendelesek WHERE DATEPART(week, OrderDate) = DATEPART(week, GETDATE()) AND DATEPART(year, OrderDate) = DATEPART(year, GETDATE()) AND Allapot = 'Teljesített' GROUP BY PizzaID";
                    break;
                case "Hónapban":
                    query = "SELECT PizzaID, COUNT(*) AS OrderCount FROM Rendelesek WHERE MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE()) AND Allapot = 'Teljesített' GROUP BY PizzaID";
                    break;
            }

            string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=PizzaData;Integrated Security=True;Pooling=False;Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Populate PieSeries
                while (reader.Read())
                {
                    int pizzaId = reader.GetInt32(reader.GetOrdinal("PizzaID"));
                    int orderCount = reader.GetInt32(reader.GetOrdinal("OrderCount"));

                    // Get pizza name from dictionary
                    string pizzaName = GetPizzaName(pizzaId);

                    // Add data to PieSeries
                    series.Slices.Add(new PieSlice($"{pizzaName}", orderCount));
                }

                reader.Close();
            }

            model.Series.Add(series);
            plotView1.Model = model;
        }
    }
}