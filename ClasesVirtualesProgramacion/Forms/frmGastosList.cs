using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClasesVirtualesProgramacion.Forms
{
    public partial class frmGastosList : Form
    {
        admConexion oConexion = new admConexion();
        public frmGastosList()
        {
            InitializeComponent();
        }

        private void frmGastosList_Load(object sender, EventArgs e)
        {
            dsClasesVirtuales.Gastos.Clear();
            string SelectSQL = "Select * from gastos";
            if (oConexion.SelectData(SelectSQL, dsClasesVirtuales.Gastos) != true)
                MessageBox.Show("No se ha podido cargar ningún dato de gastos, contacte al departamento de desarrollo técnico", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Dialogs.GastoDialog frmNuevo = new Dialogs.GastoDialog();
            frmNuevo.ShowDialog();
            if (frmNuevo.DialogResult == DialogResult.OK)
            {
                string sqlInsert = string.Format("Insert into gastos (fecha, categoria, subcategoria, descripcion, valor, formapago)values('{0}', '{1} ', '{2}', '{3}', '{4}', '{5}')", frmNuevo.fechaDateTimePicker.Value.ToString("yyyy-MM-dd"), frmNuevo.categoriaComboBox.Text, frmNuevo.subcategoriaComboBox.Text, frmNuevo.descripcionTextBox.Text.Trim(), frmNuevo.nudValor.Value.ToString(), frmNuevo.formapagoComboBox.Text.Trim());
                if (oConexion.AccionSQL(sqlInsert) == true)
                {
                    this.frmGastosList_Load(null, null);
                    MessageBox.Show("La información de gastos ha sido almacenada correctamente.", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gastosDataGridView.Focus();
                }
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Dialogs.GastoDialog frmEditar = new Dialogs.GastoDialog();
            DataGridViewRow Fila = gastosDataGridView.CurrentRow;
            Int16 ID = Int16.Parse(Fila.Cells[0].Value.ToString());
            frmEditar.fechaDateTimePicker.Value = Convert.ToDateTime(Fila.Cells[1].Value);
            frmEditar.categoriaComboBox.Text = Fila.Cells[2].Value.ToString();
            frmEditar.subcategoriaComboBox.Text = Fila.Cells[3].Value.ToString();
            frmEditar.descripcionTextBox.Text = Fila.Cells[4].Value.ToString();
            frmEditar.nudValor.Text = Fila.Cells[5].Value.ToString();
            frmEditar.formapagoComboBox.Text = Fila.Cells[6].Value.ToString();
            frmEditar.ShowDialog();
            if (frmEditar.DialogResult == DialogResult.OK)
            {
                string sqlUpdate = string.Format("update gastos set fecha = '{0}', categoria='{1}', subcategoria='{2}', descripcion='{3}', valor='{4}', formapago='{5}' where id= {6} ", frmEditar.fechaDateTimePicker.Value.ToString("yyyy-MM-dd"),
                  frmEditar.categoriaComboBox.Text,
                  frmEditar.subcategoriaComboBox.Text,
                  frmEditar.descripcionTextBox.Text.Trim(),
                  frmEditar.nudValor.Value.ToString(),
                  frmEditar.formapagoComboBox.Text.Trim(), ID);
                if (oConexion.AccionSQL(sqlUpdate) == true)
                {
                    this.frmGastosList_Load(null, null);
                    MessageBox.Show("La información de gastos ha sido actualizada correctamente. ", "Editar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gastosDataGridView.Focus();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (gastosBindingSource.Count > 0)
            {
                if (MessageBox.Show("Asegurese de querer eliminar la información de los gastos. Desea eliminar permanentemente este registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    DataGridViewRow Fila = gastosDataGridView.CurrentRow;
                    Int16 ID = Int16.Parse(Fila.Cells[0].Value.ToString());
                    string sqlDelete = string.Format("delete from gastos where id = {0}", ID);
                    if (oConexion.AccionSQL(sqlDelete) == true)
                    {
                        this.frmGastosList_Load(null, null);
                        MessageBox.Show("La información de gastos ha sido eliminada permanentemente.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gastosDataGridView.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay información para eliminar un registro.", "Eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (cmbBuscarpor.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar una de las opciones para buscar el gasto, ya sea por Fecha, por Categoría o Subcategoría", "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbBuscarpor.Focus();
                return;
            }
            else
            {
                if (txtCriterio.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Por favor escriba un criterio para realizar la búsqueda del gasto.", "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCriterio.Focus();
                    return;
                }
                else
                {
                    string sqlSelect = string.Empty;
                    switch (cmbBuscarpor.SelectedIndex)
                    {
                        case 0: // Categoría
                            sqlSelect = string.Format("Select * from gastos where categoria like'{0}%'", txtCriterio.Text.Trim());
                            break;
                        case 1: // Subcategoría
                            sqlSelect = string.Format("Select * from gastos where subcategoria like '{0}%'", txtCriterio.Text.Trim());
                            break;
                        default: // Descripción
                            sqlSelect = string.Format("Select * from gastos where descripcion like '{0}%'", txtCriterio.Text.Trim());
                            break;
                    }
                    dsClasesVirtuales.Gastos.Clear();
                    if (oConexion.SelectData(sqlSelect, dsClasesVirtuales.Gastos) == true)
                        gastosDataGridView.Focus();
                }
            }
        }
    }
}