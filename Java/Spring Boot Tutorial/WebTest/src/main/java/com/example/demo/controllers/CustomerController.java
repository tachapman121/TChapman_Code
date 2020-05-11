package com.example.demo.controllers;

import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.model.Customer;
import com.example.demo.services.CustomerService;

@RestController
@RequestMapping(CustomerController.BASE_URL)
public class CustomerController {
	private final CustomerService customerService;
	public static final String BASE_URL = "/api/v1/customers";
	
	public CustomerController(CustomerService customerService) {
		this.customerService = customerService;
	}
	
	@GetMapping
	List<Customer> getAllCustomers() {
		return customerService.findAllCustomers();
	}
	
	@GetMapping("/{id}")
	public Customer getCustomerById(@PathVariable Long id) {
		return customerService.findCustomerById(id);
	}
	
	@PostMapping
	@ResponseStatus(HttpStatus.CREATED)
	// parses into JSON using Jackson, looks at RequestBody
	public Customer saveCustomer(@RequestBody Customer customer) {
		return customerService.saveCustomer(customer);
	}
}
