using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using diosmio.Models;


namespace diosmio
{
    public partial class Form1 : Form
    {

        db_lp3_G1Entities db = new db_lp3_G1Entities(); 

        Empleados objEmpleados = new Empleados();

        int idEmp;

        public Form1()
        {
            InitializeComponent();
            MostrarContenido();
        }

        private void MostrarContenido(string busqueda = null)
        {
            dataGridView1.AutoGenerateColumns = false;
            


            if(busqueda != null && !busqueda.Equals(""))
            {
                var Empleadosfiltrados = db.Empleados.Where(b => b.Nombre == busqueda).ToList();

                if(Empleadosfiltrados.Count > 0)
                {
                    dataGridView1.DataSource = Empleadosfiltrados;
                }
                else
                {
                    MessageBox.Show("nO se encontro nombre", "aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }

               



            }
            else
            {
                dataGridView1.DataSource = db.Empleados.ToList();

               

            }


        }

        private void btBusqueda_Click(object sender, EventArgs e)
        {
            if (!txtBuscar.Text.Equals(""))
            {
                MostrarContenido(txtBuscar.Text.Trim());
            }
            else
            {
                
                MostrarContenido();
                MessageBox.Show("Ingrese nombre","aviso",MessageBoxButtons.OK,MessageBoxIcon.Hand);
            }
        }

        private void Actualizar_O_Guardar_Dato()
        {
            string dni = txtDni.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string sueldo = txtSueldo.Text.Trim();

            try
            {

                if (dni.Length >= 8 && nombre.Length > 0 && decimal.Parse(sueldo) > 0)
                {
                    objEmpleados.Dni = dni;
                    objEmpleados.Nombre = nombre;
                    objEmpleados.Sueldo = Convert.ToDecimal(sueldo);
                    objEmpleados.Distrito = txtDistrito.Text.Trim();
                    objEmpleados.Direccion = txtDireccion.Text.Trim();
                    if (idEmp > 0 )
                    {
                        db.Entry(objEmpleados).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        db.Empleados.Add(objEmpleados);
                    }

                    db.SaveChanges();
                    MostrarContenido();
                    Limpiar();

                }
                else
                {
                    throw new Exception("Falta agregar datos obligatorios.... puto");
                }

            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

        }


        private void Eliminar()
        {
            try
            {
                var respuesta = MessageBox.Show("seguro querés eliminar a este pibe?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if(respuesta == DialogResult.Yes)
                {
                    db.Empleados.Remove(objEmpleados); //Lo removemos del contexto
                    db.SaveChanges();

                    MostrarContenido();
                    Limpiar();

                }
                else
                {
                    MessageBox.Show("Cuidando mano, para la proxima publico tus datos personales :v");
                }


            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Limpiar()
        {
            txtDireccion.Clear();
            txtDistrito.Clear();
            txtNombre.Clear();
            txtDni.Clear();
            txtSueldo.Clear();
            btBorrar.Enabled = false;
            idEmp = 0;
            txtDni.Focus();

        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MostrarContenido();
            Limpiar();
            
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView1.CurrentCell.RowIndex != -1)
                {

                    idEmp = Convert.ToInt32(dataGridView1.CurrentRow.Cells["IdEmpleado"].Value);
                    objEmpleados = db.Empleados.Where(x => x.idEmpleado == idEmp).FirstOrDefault();
                    txtDni.Text = objEmpleados.Dni;
                    txtNombre.Text = objEmpleados.Nombre;
                    txtSueldo.Text = objEmpleados.Sueldo.ToString();
                    txtDistrito.Text = objEmpleados.Distrito;
                    txtDireccion.Text = objEmpleados.Direccion;

                    btBorrar.Enabled = true;

                }
                

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btGuardar_Click(object sender, EventArgs e)
        {
            Actualizar_O_Guardar_Dato();
        }

        private void btBorrar_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
