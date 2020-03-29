using QITInterReg.Models;
using QITInterReg.Services;
using QITInterReg.ServicesImpl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QITInterReg.Controllers
{
    public class CustomerController : Controller
    {
        CustomerService customerService = new CustomerServiceImpl();
        // GET: Customer
        [Authorize]
        public ActionResult Index()
        {
            var list = customerService.getAll();
            return View(list);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            Debug.WriteLine("Inside controller");
            var customer = customerService.getByCustomerId(id);
            return View(customer);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Customer());
        }

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            customerService.createCustomer(customer);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var customer = customerService.getByCustomerId(Id);
            return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            customerService.update(customer);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var customer = customerService.getByCustomerId(Id);
            return View(customer);
        }

        [HttpPost]
        public ActionResult Delete(int Id, FormCollection collection)
        {
            customerService.delete(Id);
            return RedirectToAction("Index");
        }
    }
}