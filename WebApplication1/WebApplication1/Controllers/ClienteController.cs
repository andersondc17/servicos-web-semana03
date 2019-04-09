using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.Controllers
{
    public class ClienteController : Controller
    {
        SqlConnection cn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["negocios"].ConnectionString);
        IEnumerable<clsCliente> listado(string nombre)
        {
            List<clsCliente> temporal = new List<clsCliente>();
            if (nombre == "") return temporal;
            else
            {
                SqlCommand cmd = new SqlCommand("sp_cliente_filtro",cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    clsCliente reg = new clsCliente()
                    {
                        idcliente = dr.GetString(0),
                        nombrecia = dr.GetString(1),
                        direccion = dr.GetString(2),
                        nombrepais = dr.GetString(3),
                        telefono = dr.GetString(4)
                    };
                    temporal.Add(reg);
                }
                dr.Close();
                cn.Close();
            }
            return temporal;              
        }

        IEnumerable<clsCliente> dropClientes()
        {
            List<clsCliente> temporal = new List<clsCliente>();
                SqlCommand cmd = new SqlCommand("sp_cliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    clsCliente reg = new clsCliente()
                    {
                        idcliente = dr.GetString(0),
                        nombrecia = dr.GetString(1),
                    };
                    temporal.Add(reg);
                }
                dr.Close();
                cn.Close();
            
            return temporal;
        }

        IEnumerable<clsPedido> pedidos_cliete(string codigo)
        {
            List<clsPedido> temporal = new List<clsPedido>();
            SqlCommand cmd = new SqlCommand("sp_pedidos_cliente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", codigo);
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                clsPedido reg = new clsPedido()
                {
                    IdPedido = dr.GetInt32(0),
                    FechaPedido = dr.GetDateTime(1),
                    DireccionDestinatario = dr.GetString(2),
                    CiudadDestinatario = dr.GetString(3)
                };
                temporal.Add(reg);
            }
            dr.Close();
            cn.Close();
            return temporal;
        }

        // GET: Cliente
        public ActionResult clientes_nombre(string nombre="",int p=0)
        {
            IEnumerable<clsCliente> temporal = listado(nombre);

            int c = temporal.Count();
            int nreg = 5;
            int pags = c < nreg ? 0 : (c % nreg > 0 ? c / nreg + 1 : c / nreg);
            ViewBag.pags = pags;
            ViewBag.nombre = nombre;
            return View(temporal.Skip(p*nreg).Take(nreg));
        }


        public ActionResult pedidos_empleado(string codigo = "", int p = 0)
        {
            //ejecuto el metodo
            IEnumerable<clsPedido> temporal = pedidos_cliete(codigo);
            int c = temporal.Count();
            int nreg = 10;
            int pags = c % nreg > 0 ? c / nreg + 1 : c / nreg;
            ViewBag.pags = pags;
            ViewBag.codigo = codigo;
            ViewBag.clientes = new SelectList(dropClientes(), "idcliente", "nombrecia", codigo);

            return View(temporal.Skip(p*nreg).Take(nreg));
        }
    }
}