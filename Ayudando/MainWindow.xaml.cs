using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Ayudando
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sql;

        public MainWindow()
        {
            InitializeComponent();

            string conexion = ConfigurationManager.ConnectionStrings["Ayudando.Properties.Settings.PracticasConnectionString"].ConnectionString;
            try
            {
                sql = new SqlConnection(conexion);
                MostrarCoches();
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        private void MostrarCoches()
        {
            string query = "SELECT *,CONCAT(marca, ' ', patente) AS INFO FROM COCHE";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sql);

            using (dataAdapter)
            {
                DataTable data = new DataTable();

                dataAdapter.Fill(data);

                listaCoches.DisplayMemberPath = "INFO";//Bind
                listaCoches.SelectedValuePath = "Id";//
                listaCoches.ItemsSource = data.DefaultView;
            }
        }

        private void Crear_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Marca.Text) || string.IsNullOrEmpty(Patente.Text)) return;

            CrearCoche();
            MostrarCoches();
        }

        private void CrearCoche()
        {
            string query = "INSERT INTO COCHE (marca,patente) VALUES (@marca,@patente)";

            SqlCommand command = new SqlCommand(query, sql);

            sql.Open();

            command.Parameters.AddWithValue("@marca", Marca.Text);
            command.Parameters.AddWithValue("@patente", Patente.Text);

            command.ExecuteNonQuery();

            sql.Close();

        }
    }
}
