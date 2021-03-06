﻿using M17AB_TrabalhoModelo_1920.Classes;
using M17AB_TrabalhoModelo_1920_WIP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace M17AB_TrabalhoModelo_1920_WIP.Admin
{
    /// <summary>
    /// Summary description for Servicos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class Servicos : System.Web.Services.WebService
    {

        [WebMethod]
        public string ListaLivros()
        {
            Livro livro = new Livro();
            DataTable dados = livro.ListaTodosLivros();
            List<Livro> lLivros = new List<Livro>();
            for(int i = 0; i < dados.Rows.Count; i++)
            {
                Livro novo = new Livro();
                novo.nlivro = int.Parse(dados.Rows[i]["nlivro"].ToString());
                novo.nome = dados.Rows[i]["nome"].ToString();
                novo.preco = Decimal.Parse(dados.Rows[i]["preco"].ToString());
                lLivros.Add(novo);
            }
            return new JavaScriptSerializer().Serialize(lLivros);
        }
        [WebMethod(EnableSession = true)]
        public List<Dados> DadosUtilizadores()
        {
            if (Session["perfil"] == null ||
                Session["perfil"].ToString() != "0")
                return null;

            List<Dados> devolver = new List<Dados>();
            BaseDados bd = new BaseDados();
            DataTable dados = bd.devolveSQL(@"Select Count(*), case
                    when perfil=0 then 'Admin'
                    when perfil=1 then 'User'
                    end as [perfil]
                    from Utilizadores group by perfil");
            for (int i = 0; i < dados.Rows.Count; i++)
            {
                Dados registo = new Dados();
                registo.perfil = dados.Rows[i][1].ToString();
                registo.contagem = int.Parse(dados.Rows[i][0].ToString());
                devolver.Add(registo);
            }
            return devolver;
        }
        public class Dados
        {
            public string perfil;
            public int contagem;
        }
    }
}
