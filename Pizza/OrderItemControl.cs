using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pizza.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.Common;
using System.Runtime.Remoting.Contexts;

namespace Pizza
{
    public partial class OrderItemControl : UserControl
    {
        private int orderId;
        private bool isAdmin;

        public OrderItemControl(int orderId, string name, string address, string email, string phoneNumber, int pizzaId, int pizzaDb, int totalPrice, string status, bool isAdmin)
        {
            this.orderId = orderId;
            this.isAdmin = isAdmin;

            InitializeComponent();
            lblOrderID.Text = $"ID: {orderId}";
            lblName.Text = $"Név: {name}";
            lblAddress.Text = $"Cím: {address}";
            lblEmail.Text = $"Email: {email}";
            lblPhoneNumber.Text = $"Telefonszám: {phoneNumber}";
            lblPizzaID.Text = $"Pizza ID: {pizzaId}";
            lblQuantity.Text = $"Mennyiség: {pizzaDb}";
            lblTotalPrice.Text = $"Részösszeg: {totalPrice} Ft";
            lblStatus.Text = $"Állapot: {status}";

            if (isAdmin)
            {
                comboBox1.Visible = true;
            }
            else
            {
                comboBox1.Visible = false;
            }

            if (!isAdmin)
            {
                System.Windows.Forms.Button cancelBtn = new System.Windows.Forms.Button();
                cancelBtn.Text = "Törlés";
                cancelBtn.Click += CancelOrder_Click;
                flowLayoutPanel1.Controls.Add(cancelBtn);
            }
        }

        private void OrderItemControl_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Felvett");
            comboBox1.Items.Add("Kiszállítás alatt");
            comboBox1.Items.Add("Teljesített");
            comboBox1.Items.Add("Meghiúsult");
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string newStatus = comboBox1.SelectedItem.ToString();
                lblStatus.Text = $"Állapot: {newStatus}";
                UpdateOrderStatus(orderId, newStatus);
            }
        }

        private void UpdateOrderStatus(int orderId, string newStatus)
        {
            string connectionString = "Data Source = (localdb)\\ProjectModels; Initial Catalog = PizzaData; Integrated Security = True; Pooling = False; Connect Timeout = 30";
            string query = "UPDATE Rendelesek SET Allapot = @Allapot WHERE OrderId = @OrderId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Allapot", newStatus);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hiba lépett fel: {ex.Message}");
                    }
                }
            }
        }
        private void CancelOrder_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Biztosan törölni szeretnéd a rendelésed?", "Rendelés törlése", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string connectionString = "Data Source = (localdb)\\ProjectModels; Initial Catalog = PizzaData; Integrated Security = True; Pooling = False; Connect Timeout = 30";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Rendelesek SET Allapot = @NewStatus WHERE OrderId = @OrderId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NewStatus", "Meghiúsult");
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Rendelés sikeresen törölve.");
                            (this.ParentForm as Form1)?.RefreshOrderList();
                        }
                        else
                        {
                            MessageBox.Show("Rendelés törlése sikertelen.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Rendelés törlése sikertelen: " + ex.Message);
                    }
                }
            }
        }
    }
}
