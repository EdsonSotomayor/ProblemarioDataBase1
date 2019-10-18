using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace Problemario1_DataBase
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;
        public MainWindow()
        {
            InitializeComponent();
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\EQUIPOSLLA.mdb";
            MostrarDatos();
        }

        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Equipos";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();
            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible; 
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void LimpiarTodo()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            cbJuego.SelectedIndex = 0;
            txtPais.Text = "";
            btnNuevo.Content = "Nuevo";
            txtId.IsEnabled = true;
        }
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();

            cmd.Connection = con;

            if (txtId.Text != "")
            {
                if (txtId.IsEnabled == true)
                {
                    if (cbJuego.Text != "Selecciona Juego")
                    {
                        cmd.CommandText = "insert into Equipos(Id,Nombre,Juego,Pais,LdEquipo) " +
                           "Values(" + txtId.Text + ",'" + txtNombre.Text + "','" + cbJuego.Text + "','" + txtPais.Text + "','" + txtLider.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("Equipo Agregado Correctamente...");
                        LimpiarTodo();

                    }
                    else
                    {
                        MessageBox.Show("Favor de seleccionar el Juego....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Equipos set Nombre='" + txtNombre.Text + "',Juego='" + cbJuego.Text + "',Pais='" + txtPais.Text + "',LdEquipo='" + txtLider.Text + "'where Id=" + txtId.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Datos del Equipo Actualizados...");
                    LimpiarTodo();
                }

            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if(gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtId.Text = row["Id"].ToString();
                txtNombre.Text = row["Nombre"].ToString();
                cbJuego.Text = row["Juego"].ToString();
                txtPais.Text = row["Pais"].ToString();
                txtLider.Text = row["LdEquipo"].ToString();
                txtId.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un Equipo....");
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from Equipos where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Equipos eliminados correctamente...");
                LimpiarTodo();
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un Equipo...");
            }

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
