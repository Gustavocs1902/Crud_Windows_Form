using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudWindowsForm.Modelos
{
    public class Funcionarios
    {
        public int ID {  get; set; }
        public string Nome { get; set; }
        public string Telefone {  get; set; }
        public string RG {  get; set; }
        public string Endereco { get; set; }
        public decimal Salario {  get; set; }
        public decimal HorasExtras { get; set; }

        public decimal SalarioTotal => Salario + (Salario * 0.05m * HorasExtras);

    }
}
