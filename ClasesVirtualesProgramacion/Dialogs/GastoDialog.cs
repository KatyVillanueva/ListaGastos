using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClasesVirtualesProgramacion.Dialogs;

namespace ClasesVirtualesProgramacion.Dialogs
{
    public partial class GastoDialog : baseDialog
    {
        public GastoDialog()
        {
            InitializeComponent();
        }
        protected override bool ValidarEntrada()
        {
            erp.Clear();
            if (formapagoComboBox.SelectedIndex < 0)
                return NotificacionDeValidacion("Por favor seleccione una Forma de pago.", formapagoComboBox);

            if (categoriaComboBox.SelectedIndex < 0)
                return NotificacionDeValidacion("Por favor seleccione una Categoría.", categoriaComboBox);

            if (subcategoriaComboBox.SelectedIndex < 0)
                return NotificacionDeValidacion("Por favor seleccione una Subcategoría.", subcategoriaComboBox);

            if (descripcionTextBox.Text.Trim() == string.Empty)
                return NotificacionDeValidacion("Por favor escriba una descripción", descripcionTextBox);
            return true;
        }
    }
}
