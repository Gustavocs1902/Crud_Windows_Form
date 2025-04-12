using CrudWindowsForm.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudWindowsForm
{
    public partial class Form1 : Form
    {
        private FuncRepositorio _repositorio;
        private int _funcSelecionado = 0;
        public Form1()
        {

            InitializeComponent();

            _repositorio = new FuncRepositorio();


            lst_Func.View = View.Details;
            lst_Func.LabelEdit = true;
            lst_Func.AllowColumnReorder = true;

            lst_Func.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lst_Func.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lst_Func.Columns.Add("Telefone", 100, HorizontalAlignment.Left);
            lst_Func.Columns.Add("RG", 100, HorizontalAlignment.Left);
            lst_Func.Columns.Add("Endereço", 150, HorizontalAlignment.Left);
            lst_Func.Columns.Add("Salario", 100, HorizontalAlignment.Left);
            lst_Func.Columns.Add("Horas Extras", 100, HorizontalAlignment.Left);
            lst_Func.Columns.Add("SalarioTotal", 100, HorizontalAlignment.Left);

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e) //Salvar
        {
            try
            {

                var funcionario = new Funcionarios
                {
                    Nome = txtName.Text,
                    Telefone = txtTelefone.Text,
                    RG = txtRG.Text,
                    Endereco = txtEndereco.Text,
                    Salario = decimal.Parse(txtSalario.Text),
                    HorasExtras = int.Parse(txtHorasExtras.Text),
                };
                if (_repositorio.Inserir(funcionario))
                {
                    MessageBox.Show("Dados inseridos com sucesso!");
                    button2_Click(null, null);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }
        private void button2_Click(object sender, EventArgs e) //Buscar
        {
            try
            {
                lst_Func.Items.Clear();

                var funcionario = _repositorio.Buscar(txtBuscar.Text);
                {
                    foreach (var func in funcionario)
                    {
                        var item = new ListViewItem(func.ID.ToString());
                        item.SubItems.Add(func.Nome);
                        item.SubItems.Add(func.Telefone);
                        item.SubItems.Add(func.RG);
                        item.SubItems.Add(func.Endereco);
                        item.SubItems.Add(func.Salario.ToString("C"));
                        item.SubItems.Add(func.HorasExtras.ToString());
                        item.SubItems.Add(func.SalarioTotal.ToString("C"));

                        //Guarda os valores originais
                        item.Tag = new
                        {
                            SalarioOriginal = func.Salario,
                            HorasOriginais = func.HorasExtras
                        };

                        lst_Func.Items.Add(item);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void lst_Func_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected && e.Item.Tag != null)
            {
                dynamic dados = e.Item.Tag;
                _funcSelecionado = int.Parse(e.Item.SubItems[0].Text);
                txtName.Text = e.Item.SubItems[1].Text;
                txtTelefone.Text = e.Item.SubItems[2].Text;
                txtRG.Text = e.Item.SubItems[3].Text;
                txtEndereco.Text = e.Item.SubItems[4].Text;
                txtSalario.Text = dados.SalarioOriginal.ToString();
                txtHorasExtras.Text = dados.HorasOriginais.ToString();
            }

        }

        private void button3_Click(object sender, EventArgs e) //Atualizar
        {
            try
            {
                if (_funcSelecionado == 0)
                {
                    MessageBox.Show("Selecione um funcionario clicando no ID!");
                    return;
                }
                if (!decimal.TryParse(txtSalario.Text, out decimal salario))
                {
                    MessageBox.Show("Formato de salário inválido! Use números com vírgula decimal.");
                    return;
                }

                if (!int.TryParse(txtHorasExtras.Text, out int HorasExtras))
                {
                    MessageBox.Show("HOras extras devem ser um número inteiro");
                    return;
                }
                var funcionario = new Funcionarios
                {
                    ID = _funcSelecionado,
                    Nome = txtName.Text,
                    Telefone = txtTelefone.Text,
                    RG = txtRG.Text,
                    Endereco = txtEndereco.Text,
                    Salario = decimal.Parse(txtSalario.Text),
                    HorasExtras = int.Parse(txtHorasExtras.Text)

                };

                if (_repositorio.Atualizar(funcionario))
                {
                    MessageBox.Show("Dados atualizados com sucesso!");
                    button2_Click(null, null);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                string nome = txtName.Text;

                DialogResult resposta = MessageBox.Show($"Tem certeza que quer deletar o funcionário '{nome}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resposta == DialogResult.Yes)
                {
                    //Busca o funcionario
                    var repositorio = new FuncRepositorio();

                    try
                    {
                        var funcionario = repositorio.Buscar(nome);

                        if (funcionario.Count == 1)
                        {
                            int id = funcionario[0].ID;

                            if (repositorio.Deletar(id))
                            {
                                MessageBox.Show("Funcionário excluido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtName.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Não foi possivel excluir o funcionario", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (funcionario.Count > 1)
                        {
                            MessageBox.Show("Há mais de um funcionario com essa nome. Selecionae na tabela.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Funcionario não encontrado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Errro ao excluir: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Selecione o do funcionario clicando no ID para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}

